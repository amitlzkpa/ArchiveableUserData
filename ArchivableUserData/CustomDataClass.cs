using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections;

using Rhino;

using Newtonsoft.Json;

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



    // ----------------------------------------------------



    [Guid("1158A37F-7D33-4704-9243-A850498A8813")]
    public class CustomDataClass : Rhino.DocObjects.Custom.UserData
    {
        public int alpha { get; set; }
        public double beta { get; set; }

        public TestClassA classA = new TestClassA();
        public TestClassB classB = new TestClassB();
        public TestClassC classC = new TestClassC();
        public TestClassD classD = new TestClassD();
        public TestClassE classE = new TestClassE();


        public CustomDataClass() { }

        public CustomDataClass(int a, double b)
        {
            alpha = a;
            beta = b;
        }

        public override string Description
        {
            get { return "Class to test serialization"; }
        }

        public override string ToString()
        {
            string ret = string.Format("alpha={0}, beta={1}", alpha, beta);
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
            if (dict.ContainsKey("alpha")) alpha = (int)dict["alpha"];
            if (dict.ContainsKey("beta")) beta = (double)dict["beta"];
            if (dict.ContainsKey("classA")) classA = JsonConvert.DeserializeObject<TestClassA>(dict.GetString("classA"));
            if (dict.ContainsKey("classB")) classB = JsonConvert.DeserializeObject<TestClassB>(dict.GetString("classB"));
            if (dict.ContainsKey("classC")) classC = JsonConvert.DeserializeObject<TestClassC>(dict.GetString("classC"));
            if (dict.ContainsKey("classD")) classD = JsonConvert.DeserializeObject<TestClassD>(dict.GetString("classD"));
            if (dict.ContainsKey("classE")) classE = JsonConvert.DeserializeObject<TestClassE>(dict.GetString("classE"));
            return true;
        }

        protected override bool Write(Rhino.FileIO.BinaryArchiveWriter archive)
        {
            Rhino.Collections.ArchivableDictionary dict = new Rhino.Collections.ArchivableDictionary(1, "CustomData");
            dict.Set("alpha", alpha);
            dict.Set("beta", beta);
            dict.Set("classA", JsonConvert.SerializeObject(classA, Formatting.Indented));
            dict.Set("classB", JsonConvert.SerializeObject(classB, Formatting.Indented));
            dict.Set("classC", JsonConvert.SerializeObject(classC, Formatting.Indented));
            dict.Set("classD", JsonConvert.SerializeObject(classD, Formatting.Indented));
            dict.Set("classE", JsonConvert.SerializeObject(classE, Formatting.Indented));
            archive.WriteDictionary(dict);
            return true;
        }
    }
}
