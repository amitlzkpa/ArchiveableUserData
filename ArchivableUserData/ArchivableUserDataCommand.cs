using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.DocObjects.Custom;

using Newtonsoft.Json;

namespace ArchivableUserData
{
    [System.Runtime.InteropServices.Guid("b64e39e0-c3fd-4a50-bed9-0880c13e138c")]
    public class ArchivableUserDataCommand : Command
    {
        public ArchivableUserDataCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static ArchivableUserDataCommand Instance
        {
            get; private set;
        }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        public override string EnglishName
        {
            get { return "ArchivableUserDataCommand"; }
        }

        


        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {


            RhinoObject r = doc.Objects.Find(new Guid("4e3c26e5-5554-4676-8606-b09bc7bac7e9"));
            Rhino.DocObjects.Custom.UserData ud = null;
            bool p = false;






            //DynLoadTestClass cls = new DynLoadTestClass();
            //string assStr = HotLoadingUserData.GenerateCSharpCode(cls);
            //RhinoApp.WriteLine(assStr);
            //r.Geometry.UserDictionary.Set("_assembly", assStr);

            //string insStr = JsonConvert.SerializeObject(cls);
            //RhinoApp.WriteLine(insStr);
            //r.Geometry.UserDictionary.Set("_vals", insStr);

            //r.Geometry.UserDictionary.Set("_type", cls.GetType().Name);






            string s = r.Geometry.UserDictionary.GetString("_assembly");
            string j = r.Geometry.UserDictionary.GetString("_vals");




            byte[] assBytes = Convert.FromBase64String(s);
            string t = Path.GetTempFileName();
            File.WriteAllBytes(t, assBytes);
            Assembly assembly = Assembly.LoadFrom(t);

            Type type = assembly.GetType("ArchivableUserData.DynLoadTestClass");

            object instanceOfMyType = Activator.CreateInstance(type);







            //RhinoApp.WriteLine((ud != null) ? ud.ToString() : "");

            return Result.Success;
        }



        private CustomDataClass GetCustomData()
        {
            CustomDataClass d = new CustomDataClass(42, 12.34);
            return d;
        }


    }
}
