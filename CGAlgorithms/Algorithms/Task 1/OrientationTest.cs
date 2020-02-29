using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.Lab1
{
    class OrientationTest : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Enums.TurnType turnType = HelperMethods.CheckTurn(lines[0], points[0]);            
            if (turnType == Enums.TurnType.Right)
            {             
                outPoints.Add(points[0]);
            }
            else if (turnType == Enums.TurnType.Left)
            {                
                outLines.Add(lines[0]);
            }
            else
            {                
                outPoints.Add(points[0]);
                outLines.Add(lines[0]);
            }
        }

        public override string ToString()
        {
            return "Orientation Test";
        }
    }
}
