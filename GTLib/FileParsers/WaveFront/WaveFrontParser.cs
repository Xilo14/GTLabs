using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using GTLib.Elements;
using GTLib.Primitives;

namespace GTLib
{
    public class WaveFrontParser
    {
        public ObjModel Parse(string filename)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            string[] s = File.ReadAllLines(filename);
            var model = new ObjModel();
            foreach (var str in s)
            {
                if (str.StartsWith("v "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });
                    words = words.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();


                    model.vertices.Add(new Dot3D(
                        Convert.ToDouble(words[1], provider),
                        Convert.ToDouble(words[2], provider),
                        Convert.ToDouble(words[3], provider)));
                }
                else if (str.StartsWith("vn "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });
                    words = words.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    model.verticesNormal.Add(new Vector3(
                        Convert.ToSingle(words[1], provider),
                        Convert.ToSingle(words[2], provider),
                        Convert.ToSingle(words[3], provider)));
                }
                else if (str.StartsWith("vt "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });
                    words = words.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    model.verticesTexture.Add(new Vector2(
                        Convert.ToSingle(words[1], provider),
                        Convert.ToSingle(words[2], provider)));
                }
                else if (str.StartsWith("f "))
                {
                    List<Vector3> f = new List<Vector3>();


                    string[] face = str.TrimStart('f').Trim(' ').Split(new char[] { ' ' });
                    face = face.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                    foreach (var vert in face)
                    {
                        string[] nums = vert.Split(new char[] {'/'});
                        //nums = nums.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

                        float vertNum = Convert.ToSingle(nums[0], provider);
                        float vertTextNum = 0;
                        float vertNormNum = 0;

                        if (nums.Length > 1 && !string.IsNullOrWhiteSpace(nums[1]))
                            vertTextNum = Convert.ToSingle(nums[1], provider);
                        if (nums.Length > 2 && !string.IsNullOrWhiteSpace(nums[2]))
                            vertNormNum = Convert.ToSingle(nums[2], provider);

                        f.Add(new Vector3(
                            vertNum,
                            vertTextNum,
                            vertNormNum
                            ));
                    }
                    model.faces.Add(f);
                }
            }
            return model;
        }

    }
}
