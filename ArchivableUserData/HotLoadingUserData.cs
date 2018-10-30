using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;

using System.Reflection;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

using Rhino;

namespace ArchivableUserData
{
    [Guid("7150A248-FBFF-49AF-832A-3F9A76676A61")]
    public class HotLoadingUserData : Rhino.DocObjects.Custom.UserData
    {

        protected object ReadHotLoadData(Rhino.Collections.ArchivableDictionary dict)
        {
            string encAss = "";
            string type = "";
            if (dict.ContainsKey("EncodedAssembly")) encAss = dict.GetString("EncodedAssembly");
            if (dict.ContainsKey("Type")) type = dict.GetString("Type");
            byte[] assByt = Convert.FromBase64String(encAss);
            string tempPath = Path.GetTempFileName();
            File.WriteAllBytes(tempPath, assByt);
            Assembly assembly = Assembly.LoadFrom(tempPath);
            Type t = assembly.GetType(type);
            object instanceOfMyType = Activator.CreateInstance(t);
            return instanceOfMyType;
        }

        protected bool WriteHotLoadData(Rhino.Collections.ArchivableDictionary dict)
        {
            string encAss = GenerateCSharpCode(this.GetType());
            dict.Set("EncodedAssembly", encAss);
            dict.Set("Type", this.GetType().FullName);

            return true;
        }






        public virtual string[] GetReferencedAssemblies()
        {
            string[] refs = new string[]
                                {
                                    "System",
                                    "System.Collections.Generic",
                                    "System.Runtime.InteropServices",
                                    "System.Collections",
                                    "Rhino",
                                    "Newtonsoft.Json"
                                };
            return refs;
        }


        public virtual string GetSourceFilePath()
        {
            return "G:/00    CURRENT/Rhino/ArchivableUserData/ArchivableUserData/ArchivableUserData/CustomDataClass.cs";
        }








        public static string GenerateCSharpCode(object o)
        {
            Type type = o.GetType();
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
            //cp.ReferencedAssemblies.Add("System.dll");
            //cp.ReferencedAssemblies.Add("System.Collections.dll");
            //cp.ReferencedAssemblies.Add("System.Runtime.InteropServices.dll");
            //cp.ReferencedAssemblies.Add("HotLoadingUserData.dll");
            // ugly hard-refs
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.dll");
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.Core.dll");
            //cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.Drawing.dll");
            //cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Reference Assemblies/Microsoft/Framework/.NETFramework/v4.0/System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("C:/Program Files (x86)/Rhinoceros 5/System/rhinocommon.dll");
            //cp.ReferencedAssemblies.Add("G:/00    CURRENT/Rhino/ArchivableUserData/ArchivableUserData/packages/Newtonsoft.Json.11.0.2/lib/net40/Newtonsoft.Json.dll");

            cp.GenerateExecutable = false;
            cp.GenerateInMemory = false;

            // Invoke compilation.
            //CodeCompileUnit cu = new CodeCompileUnit();
            //provider.CompileAssemblyFromDom(cp, cu);

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


    }
}
