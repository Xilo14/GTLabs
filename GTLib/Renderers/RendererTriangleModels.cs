using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using GTLib.Cameras;
using GTLib.Elements;
using GTLib.Primitives;
using GTLib.Scenes;
using OpenTK;

namespace GTLib.Renderers
{
    public class RendererTriangleModels : Renderer
    {
        public new Scene3DSimple Scene3D { get; set; }
        public Scene2D Scene2D { get; set; }

        public RendererTriangleModels(Scene3DSimple scene3d)
        {
            Scene3D = scene3d;
            Scene2D = new Scene2D();
        }
        public RendererTriangleModels(Scene3DSimple scene3d, Scene2D scene2d)
        {
            Scene3D = scene3d;
            Scene2D = scene2d;
        }
        public override void Render()
        {
            _dictRenderTypeChooseCamera[Scene3D.Camera.GetType()](this);
        }

        private readonly Dictionary<Type, RenderTypeChooseCamera> _dictRenderTypeChooseCamera
            = new Dictionary<Type, RenderTypeChooseCamera>()
        {
            {typeof(PerspectiveCamera), (self) =>
            {
                var camera = (PerspectiveCamera)self.Scene3D.Camera;
                Random rnd = new Random();

                Vector3 Position = new Vector3(
                    (float)camera.Eye.X,
                    (float)camera.Eye.Y,
                    (float)camera.Eye.Z);
                Vector3 Top = new Vector3(
                    (float)camera.Up.X,
                    (float)camera.Up.Y,
                    (float)camera.Up.Z);
                Vector3 CursNormal = new Vector3(
                    (float)camera.Focus.X,
                    (float)camera.Focus.Y,
                    (float)camera.Focus.Z);

                Vector3 z = Vector3.Normalize(CursNormal - Position);
                Vector3 x = Vector3.Normalize(Vector3.Cross(z,Top-Position));
                Vector3 y = Vector3.Normalize(Vector3.Cross(x,z));

                //2)Переход в систему координат камеры
                //C = Tc * Rc
                Matrix4 Tc = new Matrix4(
                    1.0f, 0.0f, 0.0f, -Position.X,
                    0.0f, 1.0f, 0.0f, -Position.Y,
                    0.0f, 0.0f, 1.0f, -Position.Z,
                    0, 0, 0, 1.0f);

                Matrix4 Rc = new Matrix4(
                    x.X, x.Y, x.Z, 0.0f,
                    y.X, y.Y, y.Z, 0.0f,
                    z.X, z.Y, z.Z, 0.0f,
                    0.0f, 0.0f, 0.0f, 1.0f);


                Matrix4 C = Rc * Tc;

                //3)Переход в однородное пространство отсечения и нормализация координат
                float right = 0;
                float left = 0;
                float near = 0.01f;
                float far = 20;
                float top = 0;
                float bottom = 0;

                float projection_plane_z = 1;

                float _aspectRatio = 800 / 800;
                float hfov = 70;
                float vfov = hfov * _aspectRatio;

                right =(float) Math.Tan(hfov / 2.0f) * projection_plane_z;
                left = -right;
                top =(float) Math.Tan(vfov / 2.0f) * projection_plane_z;
                bottom = -top;

                Matrix4 ClipMatrix = new Matrix4(
                    2/(right-left) ,0              ,(left+right)/(left-right) ,0,
                    0              ,2/(top-bottom) ,(bottom+top)/(bottom-top) ,0,
                    0              ,0              ,(far+near)/(far-near)     ,-2*(near*far)/(far - near),
                    0              ,0              ,1                         ,0);


                //4)Переход в систему координат экрана
                
                float width = 800;
                float height = 800;
                Matrix4 Viewport = new Matrix4(
                    (width-1)/2 ,0              ,0 ,(width-1)/2,
                    0           ,-(height -1)/2 ,0 ,(height -1)/2,
                    0           ,0              ,1 ,0,
                    0           ,0              ,0 ,1);

                foreach (Element3D el in self.Scene3D.Get3DElements())
                {
                    //1)Переход в мировую систему координат
                    //Mw = So * Ro * To = S * Rox * Roy * Roz * To
                    Matrix4 So = new Matrix4()
                    {
                        M11 = el.Scale,
                        M22 = el.Scale,
                        M33 = el.Scale,
                        M44 = 1
                    };
                    float radx = el.RadX;
                    float rady = el.RadY;
                    float radz = el.RadZ;
                    Matrix4 Rox = new Matrix4(
                        1.0f, 0.0f, 0.0f, 0.0f,
                        0.0f, (float) Math.Cos(radx), -(float) Math.Sin(radx), 0.0f,
                        0.0f, (float) Math.Sin(radx), (float) Math.Cos(radx), 0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f);
                    Matrix4 Roy = new Matrix4(
                        (float) Math.Cos(rady), 0.0f, (float) Math.Sin(rady), 0.0f,
                        0.0f, 1.0f, 0.0f, 0.0f,
                        -(float) Math.Sin(rady), 0.0f, (float) Math.Cos(rady), 0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f);
                    Matrix4 Roz = new Matrix4(
                        (float) Math.Cos(radz), -(float) Math.Sin(radz), 0.0f, 0.0f,
                        (float) Math.Sin(radz), (float) Math.Cos(radz), 0.0f, 0.0f,
                        0.0f, 0.0f, 1.0f, 0.0f,
                        0.0f, 0.0f, 0.0f, 1.0f);

                    Matrix4 To = new Matrix4(
                        1.0f, 0.0f, 0.0f, el.Position.X,
                        0.0f, 1.0f, 0.0f, el.Position.Y,
                        0.0f, 0.0f, 1.0f, el.Position.Z,
                        0.0f, 0.0f, 0.0f, 1.0f);

                    Matrix4 Mw = To * Roz * Roy * Rox * So;

                    Vector4 a = new Vector4();
                    Vector4 b = new Vector4();
                    Vector4 c = new Vector4();

                    if (el.GetType() == typeof(FilledTriangleModel))
                    {
                        foreach (Triangle3D triangle in ((FilledTriangleModel)el).Primitives)
                    {
                        a.X = (float)triangle.a.X;
                        a.Y = (float)triangle.a.Y;
                        a.Z = (float)triangle.a.Z;
                        a.W = 1;

                        b.X = (float)triangle.b.X;
                        b.Y = (float)triangle.b.Y;
                        b.Z = (float)triangle.b.Z;
                        b.W = 1;

                        c.X = (float)triangle.c.X;
                        c.Y = (float)triangle.c.Y;
                        c.Z = (float)triangle.c.Z;
                        c.W = 1;
                        //1)Переход в мировую систему координат
                        //Mw = So * Ro * To = S * Rox * Roy * Roz * To
                        //Посчитанно вне цикла
                        a = Mw * a;
                        b = Mw * b;
                        c = Mw * c;
                        //2)Переход в систему координат камеры
                        //C = T * R
                        //Посчитанно вне цикла
                        a = C * a;
                        b = C * b;
                        c = C * c;
                        

                        //3)Переход в однородное пространство отсечения и нормализация координат
                        //Посчитанно вне цикла
                        a = ClipMatrix * a;
                        b = ClipMatrix * b;
                        c = ClipMatrix * c;

                        //4)Переход в систему координат экрана
                        //Посчитанно вне цикла


                        if (a.W != 0 && b.W != 0 && c.W != 0 &&
                            a.Z < a.W && a.Z > -a.W &&
                            b.Z < b.W && b.Z > -b.W &&
                            c.Z < c.W && c.Z > -c.W)
                        {
                            a.X /= a.W;
                            a.Y /= a.W;
                            a.Z /= a.W;
                            a.W = 1;

                            b.X /= b.W;
                            b.Y /= b.W;
                            b.Z /= b.W;
                            b.W = 1;

                            c.X /= c.W;
                            c.Y /= c.W;
                            c.Z /= c.W;
                            c.W = 1;

                            //4)Переход в систему координат экрана
                            //Посчитанно вне цикла

                            a = Viewport * a;
                            b = Viewport * b;
                            c = Viewport * c;



                            self.Scene2D.AddElement(new FilledTriangle2D(
                                new Dot2D(a.X,a.Y),
                                new Dot2D(b.X,b.Y),
                                new Dot2D(c.X,c.Y),
                                Color.FromArgb(1,
                                    rnd.Next(256),
                                    rnd.Next(256),
                                    rnd.Next(256))));
                        }

                        
                    }

                    }
                    if (el.GetType() == typeof(WireModel))
                    {
                        foreach (Line3D line3D in ((WireModel)el).Primitives)
                        {
                            a.X = (float)line3D.start.X;
                            a.Y = (float)line3D.start.Y;
                            a.Z = (float)line3D.start.Z;
                            a.W = 1;

                            b.X = (float)line3D.finish.X;
                            b.Y = (float)line3D.finish.Y;
                            b.Z = (float)line3D.finish.Z;
                            b.W = 1;
                            
                            //1)Переход в мировую систему координат
                            //Mw = So * Ro * To = S * Rox * Roy * Roz * To
                            //Посчитанно вне цикла

                            a = Mw * a;
                            b = Mw * b;

                            //a = So * a;
                            //a = Rox * a;
                            //a = Roy * a;
                            //a = Roz * a;
                            //a = To * a;
                            //b = So * b;
                            //b = Rox * b;
                            //b = Roy * b;
                            //b = Roz * b;
                            //b = To * b;

                            //2)Переход в систему координат камеры
                            //C = Tc * Rc
                            //Посчитанно вне цикла

                            //a = Tc * a;
                            //a = Rc * a;
                            //b = Tc * b;
                            //b = Rc * b;

                            a = C * a;
                            b = C * b;

                            //3)Переход в однородное пространство отсечения и нормализация координат
                            //Посчитанно вне цикла
                            a = ClipMatrix * a;
                            b = ClipMatrix * b;
                            if (a.W != 0 && b.W != 0 &&
                                a.Z < a.W && a.Z > -a.W &&
                                b.Z < b.W && b.Z > -b.W)
                            {
                                a.X /= a.W;
                                a.Y /= a.W;
                                a.Z /= a.W;
                                a.W = 1;

                                b.X /= b.W;
                                b.Y /= b.W;
                                b.Z /= b.W;
                                b.W = 1;

                                //4)Переход в систему координат экрана
                                //Посчитанно вне цикла

                                a = Viewport * a;
                                b = Viewport * b;




                                self.Scene2D.AddElement(new Line2D(
                                    new Dot2D(a.X, a.Y),
                                    new Dot2D(b.X, b.Y)));
                            }
                        }
                    }

                }


            }}
        };

        private delegate void RenderTypeChooseCamera(RendererTriangleModels self);
    }
}