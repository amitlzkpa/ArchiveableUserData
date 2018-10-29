using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;

namespace ArchivableUserData
{
    [Guid("4FDDDFAC-937B-4B1D-A952-08272D84D658")]
    class IgnorantCustomClass : HotLoadingUserData
    {

        public override string Description
        {
            get { return "Class to test deserialization"; }
        }

        protected override void OnDuplicate(Rhino.DocObjects.Custom.UserData source)
        {
            throw new NotImplementedException();
        }

        public override bool ShouldWrite
        {
            get { return true; }
        }

        protected override bool Read(Rhino.FileIO.BinaryArchiveReader archive)
        {
            Rhino.Collections.ArchivableDictionary dict = archive.ReadDictionary();
            object ho = base.ReadHotLoadData(dict);

            return true;
        }

        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            Rhino.Collections.ArchivableDictionary dict = new Rhino.Collections.ArchivableDictionary(1, "CustomData");
            base.WriteHotLoadData(dict);

            return true;
        }
    }
}
