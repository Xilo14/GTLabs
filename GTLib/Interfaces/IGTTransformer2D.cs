using GTLib.Primitives;
using GTLib.Scenes;

namespace GTLib.Interfaces
{
    interface IGTTransformer2D
    {
        public void Transform(Primitive2D primitive);
        public void Transform(Scene2D scene);
    }
}