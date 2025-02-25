using System.Numerics;
using Application.Source.Utils;
namespace Application.Source.Helpers
{
    public class VectorialScalable
    {
        private int index;
        private string payload;
        private readonly int width;
        private readonly int height;
        public VectorialScalable(int width, int height)
        {
            index = 0;
            payload = "";
            this.width = width;
            this.height = height;
        }

        public void AddPolygon(List<Path> paths, string color)
        {
            payload += CreatePolygon(paths, color);
        }

        public string GetStyle()
        {
            var svg = $"<svg width='{width}px' height='{height}px' viewBox='0 0 {width} {height}' xmlns='http://www.w3.org/2000/svg'>";
            svg += payload;
            svg += "</svg>";
            var data = $"url('data:image/svg+xml,{Uri.EscapeDataString(svg)}')";
            return $"background-image: {data}; background-size: cover; background-position: center; background-repeat: no-repeat;";
        }

        public class Path()
        {
            private List<Vector2> points = new List<Vector2>();

            public void Add(Vector2 point)
            {
                points.Add(point);
            }

            public string ToString()
            {
                var result = "";
                foreach (var point in points)
                {
                    result += $"{point.X} {point.Y} ";
                }
                return result;
            }
        }

        private string CreateClipPath(string id, Path path)
        {
            return $"<clipPath id='{id}'><polygon points='{path}' /></clipPath>";
        }

        private string CreatePolygon(List<Path> paths, string color)
        {
            if (paths.Count == 0)
            {
                throw new Exception("No paths provided");
            }
            else if (paths.Count == 1)
            {
                return $"<polygon points='{paths[0]}' fill='{color}' />";
            }
            else
            {
                var id = $"clip{index++}";
                payload += CreateClipPath(id, paths[0]);
                if (paths.Count == 2)
                {
                    return $"<polygon points='{paths[1]}' fill='{color}' clip-path='url(#{id})' />";
                }
                else
                {
                    return $"<g clip-path='url(#{id})'>{CreatePolygon(paths.Skip(1).ToList(), color)}</g>";
                }
            }
        }
    }
}


