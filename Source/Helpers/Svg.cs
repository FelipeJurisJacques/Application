using System.Numerics;

namespace Application.Source.Helpers
{
    public static class Svg
    {
        public static string GeneratePointsAttribute(List<Vector2> points)
        {
            return string.Join(" ", points.Select(point => $"{point.X},{point.Y}"));
        }
    }
}
