using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

using Newtonsoft.Json;

namespace ArchivableUserData
{
    class UserDataSerializer : JsonConverter
    {





        public static string GenerateCSharpCode(Type type)
        {
            string className = type.Name;


            CSharpCodeProvider provider = new CSharpCodeProvider();


            // Build the file name.
            string sourceFileName;
            if (provider.FileExtension[0] == '.')
            {
                sourceFileName = className + provider.FileExtension;
            }
            else
            {
                sourceFileName = className + "." + provider.FileExtension;
            }


            // Build the parameters for source compilation.
            CompilerParameters cp = new CompilerParameters();
            cp.IncludeDebugInformation = true;
            
            // Add assembly references.
            // ugly hard-refs
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.dll");
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.Core.dll");
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.Drawing.dll");
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Rhinoceros 5/System/rhinocommon.dll");
            cp.ReferencedAssemblies.Add("G:/00    CURRENT/Rhino/ArchivableUserData/ArchivableUserData/packages/Newtonsoft.Json.11.0.2/lib/net40/Newtonsoft.Json.dll");

            cp.GenerateExecutable = false;
            cp.GenerateInMemory = false;

            // ugly hard-link
            FileInfo sourceFilePath = new FileInfo("../" + sourceFileName);
            CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFilePath.FullName);
            string encodedAssembly = "";

            if (cr.Errors.Count > 0)
            {
                // Display compilation errors.
                RhinoApp.WriteLine("Errors building {0} into {1}",
                    sourceFileName, cr.PathToAssembly);
                foreach (CompilerError ce in cr.Errors)
                {
                    RhinoApp.WriteLine("  {0}", ce.ToString());
                    RhinoApp.WriteLine();
                }
            }
            else
            {
                encodedAssembly = Convert.ToBase64String(File.ReadAllBytes(cp.OutputAssembly));
            }

            return encodedAssembly;
        }







        public override bool CanConvert(Type objectType)
        {
            return typeof(Rhino.DocObjects.Custom.UserData).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Binary");
            string ass = GenerateCSharpCode(value.GetType());
            writer.WriteValue(ass);

            writer.WritePropertyName("TypeInfo");
            writer.WriteValue(value.GetType().ToString());
            writer.WritePropertyName("Data");
            writer.WriteRawValue(JsonConvert.SerializeObject(value, Formatting.Indented));
            writer.WritePropertyName("Methods");
            writer.WriteStartArray();
            foreach (System.Reflection.MemberInfo m in value.GetType().GetMethods())
                if(!m.Name.StartsWith("get_") && !m.Name.StartsWith("set_")) writer.WriteValue(m.ToString());
            writer.WriteEndObject();
        }
    }
}
