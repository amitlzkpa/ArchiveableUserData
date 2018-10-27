using System;
using System.Collections.Generic;

using Rhino;
using System.Runtime.InteropServices;
using System.Collections;

namespace ArchivableUserData
{


    public class TestClassA
    {
        public bool booly = true;
        public byte bytey = 39;
        public sbyte sbytey = 6;
        public char chary = 'c';
        public int inty = 1;
        public short shorty = 6;
        public ushort ushorty = 9;
        public uint uinty = 27;
        public long longy = 4200;
        public ulong ulongy = 3600;
        public decimal decimaly = 1.618M;
        public double doubley = 2.414;
        public float floaty = 3.302f;
        public object objecty = new object();
        public string stringy = "mercury";


        private int foo = 36;
    }


    public class TestClassB
    {
        public List<int> listy = new List<int>() { 1, 1, 2, 3, 5, 8, 13, 21, 34 };
        public HashSet<int> sety = new HashSet<int>() { 1, 1, 2, 3, 5, 8, 13, 21, 34 };
        public Dictionary<string, string> dicty = new Dictionary<string, string>() {
                                                                                { "john", "lennon" },
                                                                                { "paul", "mccartney" },
                                                                                { "ringo", "starr" },
                                                                                { "george", "harrison" }
                                                                             };
    }


    public class TestClassC : TestClassA
    {
        public TestClassB nestedB = new TestClassB();
    }


    public class TestClassD : IEnumerable
    {
        private List<int> l = new List<int>() { 1, 2, 5, 12, 29, 70 };

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)l).GetEnumerator();
        }
    }


    public class TestClassE
    {
        public string boo = "Boo";
        private string yeah = "yeah";

        public void Run()
        {
            Console.WriteLine(string.Format("{0}-{1}!", boo, yeah));
        }
    }







    [Guid("1158A37F-7D33-4704-9243-A850498A8813")]
    public class CustomDataClass : Rhino.DocObjects.Custom.UserData
    {
        public int alpha { get; set; }
        public double beta { get; set; }
        //public object[] nestedData = new object[20];
        public TestClassA classA = new TestClassA();
        public TestClassB classB = new TestClassB();
        public TestClassC classC = new TestClassC();
        public TestClassD classD = new TestClassD();
        public TestClassE classE = new TestClassE();


        public CustomDataClass() { }

        public CustomDataClass(int a, double b, object[] testClasses = null)
        {
            alpha = a;
            beta = b;
            //if(testClasses != null)
            //{
            //    for(int i=0; i<testClasses.Length; i++)
            //    {
            //        nestedData[i] = testClasses[i];
            //    }
            //}
        }

        public override string Description
        {
            get { return "Class to test serialization"; }
        }

        public override string ToString()
        {
            string ret = string.Format("alpha={0}, beta={1} | ", alpha, beta);
            //for (int i = 0; i < nestedData.Length; i++)
            //{
            //    ret += (nestedData[i] != null) ? nestedData[i].GetType().Name + ", " : "";
            //}
            return ret;
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
            if (dict.ContainsKey("Weight") && dict.ContainsKey("Density"))
            {
                alpha = (int)dict["Weight"];
                beta = (double)dict["Density"];
            }
            return true;
        }

        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            Rhino.Collections.ArchivableDictionary dict = new Rhino.Collections.ArchivableDictionary(1, "CustomData");
            dict.Set("Weight", alpha);
            dict.Set("Density", beta);
            archive.WriteDictionary(dict);
            return true;
        }
    }
}
