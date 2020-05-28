using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using GTLib.Cameras;
using GTLib.Primitives;
using GTLib.Scenes;

namespace GTLib.Renderers
{
    /// <summary>
    /// Render Scene3DPreviewWireModel in Scene2D
    /// </summary>
    public class RendererPreviewWireModel : Renderer
    {
        public new Scene3DPreviewWireModel Scene3D { get; set; }
        public Scene2D Scene2D { get; set; }

        public RendererPreviewWireModel(Scene3DPreviewWireModel scene3d)
        {
            Scene3D = scene3d;
            Scene2D = new Scene2D();
        }
        public RendererPreviewWireModel(Scene3DPreviewWireModel scene3d, Scene2D scene2d)
        {
            Scene3D = scene3d;
            Scene2D = scene2d;
        }
        public override void Render()
        {
            _stdMethodRender[Scene3D.Camera.GetType()](this);
        }

        private Dictionary<Type,RenderType> _stdMethodRender = new Dictionary<Type,RenderType>()
        {
            {typeof(OrthogonalCamera), (self) =>
            {
                var camera = (OrthogonalCamera)self.Scene3D.Camera;
                Vector3 Position = new Vector3(
                    (float)camera.Position.X,
                    (float)camera.Position.Y,
                    (float)camera.Position.Z);
                Vector3 Top = new Vector3(
                    (float)camera.Top.X,
                    (float)camera.Top.Y,
                    (float)camera.Top.Z);
                Vector3 CursNormal = new Vector3(
                    (float)camera.CursNormal.X,
                    (float)camera.CursNormal.Y,
                    (float)camera.CursNormal.Z);



                Vector3 BaseY = -(Top - Position);
                Vector3 BaseX = Vector3.Cross(BaseY, CursNormal - Position);
                foreach (var line3D in self.Scene3D.Model.Primitives)
                {
                    Vector3 startDot3d = new Vector3(
                        (float)line3D.start.X,
                        (float)line3D.start.Y,
                        (float)line3D.start.Z);
                    Vector3 finishDot3d = new Vector3(
                        (float)line3D.finish.X,
                        (float)line3D.finish.Y,
                        (float)line3D.finish.Z);

                    Dot2D startDot2d;
                    Dot2D finishDot2d;

                    startDot2d = new Dot2D(
                        Vector3.Dot(startDot3d, BaseX)/BaseX.Length(),
                        Vector3.Dot(startDot3d, BaseY)/BaseY.Length());

                    finishDot2d = new Dot2D(
                        Vector3.Dot(finishDot3d, BaseX)/BaseX.Length(),
                        Vector3.Dot(finishDot3d, BaseY)/BaseY.Length());

                    self.Scene2D.AddElement(new Line2D(startDot2d,finishDot2d));
                }
            }}
        };

        private delegate void RenderType(RendererPreviewWireModel self);
    }
}
