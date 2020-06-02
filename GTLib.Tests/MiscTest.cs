using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace GTLib.Tests
{
    [TestClass]
    public class MiscTest
    {
        public MiscTest()
        {

        }
        [TestMethod]
        public void MatrixTest()
        {
            Matrix4x4 Tc = new Matrix4x4(
                1.0f, 0.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f, 0.0f,
                0.0f, 0.0f, 1.0f, 0.0f,
                -30, -30, -30, 1.0f);

            Vector4 v = Vector4.One;
            Vector3 v3 = Vector3.One;
            v3 = Vector3.Transform(v3,Tc);
            v = Vector4.Transform(v, Tc);
        }
    }
}
