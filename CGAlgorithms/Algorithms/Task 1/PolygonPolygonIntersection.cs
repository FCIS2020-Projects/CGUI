using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.Lab1
{
    class PolygonPolygonIntersection : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            foreach (Line line1 in polygons[0].lines)
            {
                foreach (Line line2 in polygons[1].lines)
                {
                    if (HelperMethods.CheckTurn(line1, line2.Start) != HelperMethods.CheckTurn(line1, line2.End) &&
                        HelperMethods.CheckTurn(line2, line1.Start) != HelperMethods.CheckTurn(line2, line1.End))
                        outPoints.Add(HelperMethods.LineLineIntersectionPoint(line1, line2));
                }
            }
        }

        public override string ToString()
        {
            return "Polygon Polygon Intersection";
        }
    }
}
