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
                    string[] swords = new string[10];
                    int i = 0; int j = 0;
                    while (i < 3)
                    {
                        if (!String.IsNullOrEmpty(words[j]))
                        {
                            swords[i] = words[j];
                            i++;
                        }
                        j++;
                    }
                    model.v.Add(new Dot3D(
                        Convert.ToDouble(swords[1], provider),
                        Convert.ToDouble(swords[2], provider),
                        Convert.ToDouble(swords[3], provider)));
                }
                else if (str.StartsWith("vn "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });
                    string[] swords = new string[10];
                    int i = 0; int j = 0;
                    while (i < 3)
                    {
                        if (!String.IsNullOrEmpty(words[j]))
                        {
                            swords[i] = words[j];
                            i++;
                        }
                        j++;
                    }
                    model.vn.Add(new Vector3(
                        Convert.ToSingle(swords[1], provider),
                        Convert.ToSingle(swords[2], provider),
                        Convert.ToSingle(swords[3], provider)));
                }
                else if (str.StartsWith("vt "))
                {
                    string[] words = str.Split(new char[] { '/', ' ' });
                    string[] swords = new string[10];
                    int i = 0; int j = 0;
                    while (i < 2)
                    {
                        if (!String.IsNullOrEmpty(words[j]))
                        {
                            swords[i] = words[j];
                            i++;
                        }
                        j++;
                    }
                    model.vt.Add(new Vector2(
                        Convert.ToSingle(swords[1], provider),
                        Convert.ToSingle(swords[2], provider)));
                }
                else if (str.StartsWith("f "))
                {
                    List<Vector3> f = new List<Vector3>();


                    //string[] words = str.TrimStart('f').Trim(' ').Split(new char[]{'/', ' '});
                    
                    string[] words = str.Split(new char[] { '/', ' ' });
                    if(words.Length > 10) {
                        string[] swords = new string[10];
                        int i = 0; int j = 0;
                        while (i < 9)
                        {
                            if (!String.IsNullOrEmpty(words[j]))
                            {
                                swords[i] = words[j];
                                i++;
                            }
                            j++;
                        }
                        f.Add(new Vector3(
                            Convert.ToSingle(swords[1], provider),
                            Convert.ToSingle(swords[2], provider),
                            Convert.ToSingle(swords[3], provider)));

                        f.Add(new Vector3(
                            Convert.ToSingle(swords[4], provider),
                            Convert.ToSingle(swords[5], provider),
                            Convert.ToSingle(swords[6], provider)));

                        f.Add(new Vector3(
                            Convert.ToSingle(swords[7], provider),
                            Convert.ToSingle(swords[8], provider),
                            Convert.ToSingle(swords[9], provider)));
                    }
                    if (words.Length < 10 && words.Length > 6)
                    {
                        string[] swords = new string[10];
                        int i = 0; int j = 0;
                        while (i < 6)
                        {
                            if (!String.IsNullOrEmpty(words[j]))
                            {
                                swords[i] = words[j];
                                i++;
                            }
                            j++;
                        }
                        f.Add(new Vector3(
                            Convert.ToSingle(swords[1], provider),
                            Convert.ToSingle(swords[2], provider),
                            Convert.ToSingle(swords[3], provider)));

                        f.Add(new Vector3(
                            Convert.ToSingle(swords[4], provider),
                            Convert.ToSingle(swords[5], provider),
                            Convert.ToSingle(swords[6], provider)));

                        f.Add(new Vector3(1, 1, 1));
                    }
                    if (words.Length < 6 )
                    {
                        string[] swords = new string[10];
                        int i = 0; int j = 0;
                        while (i < 3)
                        {
                            if (!String.IsNullOrEmpty(words[j]))
                            {
                                swords[i] = words[j];
                                i++;
                            }
                            j++;
                        }
                        f.Add(new Vector3(
                            Convert.ToSingle(swords[1], provider),
                            Convert.ToSingle(swords[2], provider),
                            Convert.ToSingle(swords[3], provider)));

                        f.Add(new Vector3(1, 1, 1));

                        f.Add(new Vector3(1, 1, 1));
                    }



                    model.f.Add(f);
                }
            }


            return model;
        }
    }
}
