using System.Numerics;
using GTLib.Elements;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GTLib.Tests.Elements
{
    [TestClass]
    public class Element3D_Test
    {
        private readonly InheritorElement3D el;
        public Element3D_Test()
        {
            el = new InheritorElement3D();
            el.Forward = new Vector3(-1,0,0);
        }
        [TestMethod]
        public void TestRadz()
        {
            Assert.IsTrue(el.RadX == 0);
            Assert.IsTrue(el.RadY == 0);
            Assert.IsTrue(el.RadZ == 0);
        }
    }

    public class InheritorElement3D : Element3D
    {

    }
}