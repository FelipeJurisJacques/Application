namespace Application.Source.Helpers
{
    public class VectorialScalable(int width, int height)
    {
        private int index = 0;
        private readonly int width = width;
        private readonly int height = height;
        private readonly List<string> elements = [];

        public void AddPolygon(List<Path> paths, string color)
        {
            elements.Add(CreatePolygon(paths, color));
        }

        public string GetStyle()
        {
            var svg = $"<svg width='{width}px' height='{height}px' viewBox='0 0 {width} {height}' xmlns='http://www.w3.org/2000/svg'>";
            svg += string.Join("", elements);
            svg += "</svg>";
            var data = $"url('data:image/svg+xml,{Uri.EscapeDataString(svg)}')";
            return $"background-image: {data}; background-size: cover; background-position: center; background-repeat: no-repeat;";
        }

        public class Path()
        {
            private readonly List<string> points = [];

            public void Add(int x, int y)
            {
                points.Add($"{x} {y}");
            }

            public string Build()
            {
                return string.Join(" ", points);
            }
        }

        private string CreateClipPath(string id, Path path)
        {
            return $"<clipPath id='{id}'><polygon points='{path.Build()}' /></clipPath>";
        }

        private string CreatePolygon(List<Path> paths, string color)
        {
            if (paths.Count == 0)
            {
                throw new Exception("No paths provided");
            }
            else if (paths.Count == 1)
            {
                return $"<polygon points='{paths[0].Build()}' fill='{color}' />";
            }
            else
            {
                var id = $"clip{index++}";
                elements.Add(CreateClipPath(id, paths[0]));
                if (paths.Count == 2)
                {
                    return $"<polygon points='{paths[1].Build()}' fill='{color}' clip-path='url(#{id})' />";
                }
                else
                {
                    return $"<g clip-path='url(#{id})'>{CreatePolygon([.. paths.Skip(1)], color)}</g>";
                }
            }
        }
    }
}


