﻿using System;
using System.Collections.Generic;
using System.IO;

using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;

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
            get { return "ArchivableUserData"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {


            RhinoObject r = doc.Objects.Find(new Guid("8abfe660-98a6-4e68-ae81-995fd2860d38"));
            Rhino.DocObjects.Custom.UserData ud = null;


            // remove
            //ud = r.Geometry.UserData.Find(typeof(CustomDataClass)) as CustomDataClass;
            //if (ud != null)
            //{
            //    bool p = r.Geometry.UserData.Remove(ud);
            //}


            // add
            //ud = GetCustomData();
            //bool p = r.Geometry.UserData.Add(ud);


            // serialize
            ud = r.Geometry.UserData.Find(typeof(CustomDataClass)) as CustomDataClass;
            string outFilePath = "G:/00    CURRENT/Rhino/ArchivableUserData/ArchivableUserData/files/out.json";
            using (StreamWriter sw = new StreamWriter(outFilePath))
                sw.Write(JsonConvert.SerializeObject(ud, Formatting.Indented, new UserDataSerializer()));


            RhinoApp.WriteLine((ud != null) ? ud.ToString() : "");

            return Result.Success;
        }



        private CustomDataClass GetCustomData()
        {
            object[] nd = new object[20];
            nd[0] = new TestClassA();
            nd[1] = new TestClassB();
            nd[2] = new TestClassC();
            nd[3] = new TestClassD();
            nd[4] = new TestClassE();
            CustomDataClass d = new CustomDataClass(42, 12.34, nd);
            return d;
        }


    }
}