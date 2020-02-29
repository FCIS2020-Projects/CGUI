using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.Lab1
{
    class SegmentSegmentIntersection : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (HelperMethods.CheckTurn(lines[0], lines[1].Start) != HelperMethods.CheckTurn(lines[0], lines[1].End) &&
                HelperMethods.CheckTurn(lines[1], lines[0].Start) != HelperMethods.CheckTurn(lines[1], lines[0].End))
                outPoints.Add(HelperMethods.LineLineIntersectionPoint(lines[0], lines[1]));
        }

        public override string ToString()
        {
            return "Segment Segment Intersection";
        }
    }
}
