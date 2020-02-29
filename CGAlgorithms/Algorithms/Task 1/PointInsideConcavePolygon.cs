using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.Lab1
{
    class PointInsideConcavePolygon : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            foreach (Point point in points)
            {
                if (HelperMethods.PointInConcavePolygon(point, polygons[0]) == Enums.PointInPolygon.Inside)
                    outPoints.Add(point);
            }
        }

        public override string ToString()
        {
            return "Point Inside Concave Polygon";
        }
    }
}
