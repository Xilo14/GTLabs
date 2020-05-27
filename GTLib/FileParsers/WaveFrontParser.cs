using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using GTLib.Elements;
using GTLib.Primitives;

namespace GTLib
{
    public class WaveFrontParser
    {
        public Model Parse(string filename)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            string[] s = File.ReadAllLines(filename);
            var model = new Model();
            foreach (var str in s)
            {
                if (str.StartsWith("v "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });

                    model.v.Add(new Dot3D(
                        Convert.ToDouble(words[1], provider),
                        Convert.ToDouble(words[2], provider),
                        Convert.ToDouble(words[3], provider)));
                }
                else if (str.StartsWith("vn "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });

                    model.vn.Add(new Vector3(
                        Convert.ToSingle(words[2], provider),
                        Convert.ToSingle(words[3], provider),
                        Convert.ToSingle(words[4], provider)));
                }
                else if (str.StartsWith("vt "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });

                    model.vt.Add(new Vector2(
                        Convert.ToSingle(words[2], provider),
                        Convert.ToSingle(words[3], provider)));
                }
                else if (str.StartsWith("f "))
                {
                    List<Vector3> f = new List<Vector3>();
                    

                    string[] words = str.TrimStart('f').Split(new char[]{'/', ' '});

                    f.Add(new Vector3(
                        Convert.ToSingle(words[1], provider),
                        Convert.ToSingle(words[2], provider),
                        Convert.ToSingle(words[3], provider)));

                    f.Add(new Vector3(
                        Convert.ToSingle(words[4], provider),
                        Convert.ToSingle(words[5], provider),
                        Convert.ToSingle(words[6], provider)));

                    f.Add(new Vector3(
                        Convert.ToSingle(words[7], provider),
                        Convert.ToSingle(words[8], provider),
                        Convert.ToSingle(words[9], provider)));

                    model.f.Add(f);
                }
            }


            return model;
        }
    }
}
