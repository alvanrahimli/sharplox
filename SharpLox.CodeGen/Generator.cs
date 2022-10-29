using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLox.CodeGen
{
    internal static class Generator
    {
        [Obsolete(message: "This old implementation create classes. Use record generator")]
        public static void DefineAst(string outputDir, string baseName, List<string> types)
        {
            var outputFilePath = Path.Combine(outputDir, $"{baseName}.cs");
            var file = new StreamWriter(outputFilePath);

            file.WriteLine($"abstract class {baseName}");
            file.WriteLine("{");

            file.WriteLine("}");
            file.WriteLine();

            foreach (var type in types)
            {
                var splitted = type.Split(":");
                var className = splitted[0].Trim();
                var fields = splitted[1].Split(",");

                file.WriteLine($"static class {className} : {baseName}");
                file.WriteLine("{");

                // Fields
                foreach (var field in fields)
                {
                    var fieldSplt = field.Trim().Split(" ");
                    file.WriteLine($"\tpublic {fieldSplt[0]} {Capitalize(fieldSplt[1])} {{ get; set; }}");
                }
                
                file.WriteLine();

                // Constructor
                file.WriteLine($"\tpublic {className}({splitted[1].Trim()})");
                file.WriteLine("\t{");
                foreach (var field in fields)
                {
                    var fieldSplt = field.Trim().Split(" ");
                    file.WriteLine($"\t\tthis.{Capitalize(fieldSplt[1])} = {fieldSplt[1]}");
                }
                file.WriteLine("\t}");

                file.WriteLine("}");
                file.WriteLine();
            }

            file.Close();
        }

        public static void DefineAst2(string outputDir, string baseName, List<string> types)
        {
            var outputFilePath = Path.Combine(outputDir, $"{baseName}.cs");
            var writer = new StreamWriter(outputFilePath);

            writer.WriteLine("namespace SharpLox;");

            writer.WriteLine($"abstract record {baseName}");
            writer.WriteLine("{");

            writer.WriteLine("\tpublic abstract T Accept<T>(Visitor<T> visitor);");
            writer.WriteLine();
            DefineFields(writer, baseName, types);
            writer.WriteLine();
            DefineVisitor(writer, baseName, types);
            
            writer.WriteLine("}");

            writer.Close();
        }

        private static void DefineVisitor(StreamWriter writer, string baseName, List<string> types)
        {
            writer.WriteLine("\tpublic interface Visitor<T>");
            writer.WriteLine("\t{");

            foreach (var type in types)
            {
                var splitted = type.Split(":");
                var className = splitted[0].Trim();
                var fieldsList = splitted[1].Trim();

                writer.WriteLine($"\t\tT Visit{className}{baseName}({className} {baseName.ToLower()});");
            }

            writer.WriteLine("\t}");
        }

        private static void DefineFields(StreamWriter writer, string baseName, List<string> types)
        {
            foreach (var type in types)
            {
                var splitted = type.Split(":");
                var className = splitted[0].Trim();

                writer.WriteLine($"\tinternal record {className}({splitted[1].Trim()}) : {baseName}");
                writer.WriteLine("\t{");

                writer.WriteLine("\t\tpublic override T Accept<T>(Visitor<T> visitor)");
                writer.WriteLine("\t\t{");
                writer.WriteLine($"\t\t\treturn visitor.Visit{className}{baseName}(this);");
                writer.WriteLine("\t\t}");

                writer.WriteLine("\t}");
                writer.WriteLine();
            }
        }

        private static string Capitalize(string inp) => $"{char.ToUpper(inp[0])}{inp[1..]}";
    }
}
