using System;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.DocObjects.Custom;
using System.IO;
using System.Reflection;

namespace ArchivableUserData
{
    [System.Runtime.InteropServices.Guid("1e1a9ccb-a139-4fab-ba33-093abe551030")]
    public class ReadData : Command
    {
        static ReadData _instance;
        public ReadData()
        {
            _instance = this;
        }

        ///<summary>The only instance of the ReadData command.</summary>
        public static ReadData Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "ReadData"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            RhinoObject r = doc.Objects.FindByObjectType(ObjectType.AnyObject)[0];

            string s = r.Geometry.UserDictionary.GetString("_assembly");
            string j = r.Geometry.UserDictionary.GetString("_vals");
            string t = r.Geometry.UserDictionary.GetString("_type");

            byte[] asmBytes = Convert.FromBase64String(s);
            string temp = Path.GetTempFileName();
            File.WriteAllBytes(temp, asmBytes);

            Assembly assembly = Assembly.LoadFrom(temp);
            Type type = assembly.GetType(t);
            object instanceOfMyType = Activator.CreateInstance(type);

            return Result.Success;
        }
    }
}
