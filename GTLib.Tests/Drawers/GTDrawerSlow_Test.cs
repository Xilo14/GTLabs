using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using GTLib.Drawers;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GTLib.Tests.Drawers
{   
    [TestClass]
    public class GTDrawerSlow_Test
    {
        private readonly GTDrawerSlow _drawer;

        public GTDrawerSlow_Test()
        {
            _drawer = new GTDrawerSlow();
        }

        [TestMethod]
        public void AreCross_ReturnNull()
        {
            Vector2 v11 = new Vector2(0,0);
            Vector2 v12 = new Vector2(0,1);
            Vector2 v21 = new Vector2(1,0);
            Vector2 v22 = new Vector2(1,1);


            var result = _drawer.AreCross(v11, v12, v21, v22);

            Assert.IsNull(result, "Lines (0,0:0,1) and (1,0:1,1) now crossing...Oops");
        }
        [TestMethod]
        public void AreCross_ReturnVek()
        {
            Vector2 v11 = new Vector2(0, 0);
            Vector2 v12 = new Vector2(2, 2);
            Vector2 v21 = new Vector2(1, 1);
            Vector2 v22 = new Vector2(2, 0);


            var result = _drawer.AreCross(v11, v12, v21, v22);

            Assert.IsTrue(result?.X == 1 && result?.Y == 1, "Lines (0,0:2,2) and (1,1:2,0) now not crossing...Oops");
        }
        
    }
}
