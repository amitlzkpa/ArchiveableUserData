using System;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.DocObjects.Custom;
using Newtonsoft.Json;

namespace ArchivableUserData
{
    [System.Runtime.InteropServices.Guid("39ca2675-f4b7-44bf-bc18-880b02d9bdd1")]
    public class LoadData : Command
    {
        static LoadData _instance;
        public LoadData()
        {
            _instance = this;
        }

        ///<summary>The only instance of the LoadData command.</summary>
        public static LoadData Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "LoadData"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoObject r = doc.Objects.FindByObjectType(ObjectType.AnyObject)[0];

            DynLoadTestClass cls = new DynLoadTestClass(false, 63, 1.414, "hello");
            cls.Method1("foo");
            RhinoApp.WriteLine(cls.Method2(6).ToString());
            string asmStr = HotLoadingUserData.GenerateCSharpCode(cls);
            //RhinoApp.WriteLine(asmStr);
            r.Geometry.UserDictionary.Set("_assembly", asmStr);

            string insStr = JsonConvert.SerializeObject(cls);
            r.Geometry.UserDictionary.Set("_vals", insStr);

            r.Geometry.UserDictionary.Set("_type", cls.GetType().FullName);
            return Result.Success;
        }
    }
}
