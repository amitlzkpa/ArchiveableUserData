using Rhino;

namespace ArchivableUserData
{
    class DynLoadTestClass
    {

        public string alpha = "alpha";
        private int beta = 42;

        public void Method1(string gamma)
        {
            RhinoApp.WriteLine($"{alpha}-{gamma}");
        }

        public int Method2(int add)
        {
            return beta + add;
        }

    }
}
