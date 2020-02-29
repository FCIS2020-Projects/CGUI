using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.Lab1
{
    class PolygonSegmentIntersection : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            foreach (Line line in polygons[0].lines)
            {
                if (HelperMethods.CheckTurn(lines[0], line.Start) != HelperMethods.CheckTurn(lines[0], line.End) &&
                    HelperMethods.CheckTurn(line, lines[0].Start) != HelperMethods.CheckTurn(line, lines[0].End))
                    outPoints.Add(HelperMethods.LineLineIntersectionPoint(lines[0], line));
            }
        }

        public override string ToString()
        {
            return "Polygon Segment Intersection";
        }
    }
}
