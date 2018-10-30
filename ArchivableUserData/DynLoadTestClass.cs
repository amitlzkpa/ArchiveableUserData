using Rhino;

namespace ArchivableUserData
{
    class DynLoadTestClass
    {
        private bool booly = true;
        private int inty = 42;
        private double doubley = 2.414;
        private string stringy = "mercury";

        public string alpha = "alpha";
        private int beta = 42;

        public void Method1(string gamma)
        {
            RhinoApp.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", alpha, beta, booly, inty, doubley, stringy));
        }

        public int Method2(int add)
        {
            return beta + add;
        }

        public DynLoadTestClass(bool b, int i, double d, string s)
        {
            booly = b;
            inty = i;
            doubley = d;
            stringy = s;
        }

        public DynLoadTestClass()
        {
        }

    }
}
