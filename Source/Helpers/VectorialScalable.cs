using System.Numerics;
using Application.Source.Utils;
namespace Application.Source.Helpers
{
    public class VectorialScalable
    {
        private int width;
        private int height;
        private readonly Widget widget;
        private readonly Interoperability interoperability;

        public VectorialScalable(Interoperability runtime, Widget component)
        {
            width = 1024;
            height = 768;
            widget = component;
            interoperability = runtime;
        }

        public int Width { get => width; }

        public int Height { get => height; }

        public class Path()
        {
            private List<Vector2> points = new List<Vector2>();

            public void Add(Vector2 point)
            {
                points.Add(point);
            }
        }
    }
}
