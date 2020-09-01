using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
           
            bool[] vis = new bool[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                vis[i] = false;
            }
            for (int i = 0; i < points.Count; i++)
            {
                bool f = false;
                for (int j = 0; j < points.Count && f == false; j++)
                {
                    for (int k = 0; k < points.Count && f == false; k++)
                    {
                        for (int l = 0; l < points.Count && f == false; l++)
                        {
                            if (points[i].Equals(points[j]) || points[i].Equals(points[k]) || points[i].Equals(points[l])) continue;
                            if (HelperMethods.PointInTriangle(points[i], points[j], points[k], points[l])
                                != Enums.PointInPolygon.Outside)
                            {

                                f = true;
                                break;
                            }
                        }
                    }
                }

                for (int ii = 0; ii < outPoints.Count; ii++)
                {
                    if (points[i].X == outPoints[ii].X && points[i].Y == outPoints[ii].Y)
                    {
                        f = true;
                        vis[i] = true;
                        break;
                    }
                }
                if (f == false && vis[i] == false)
                {
                    outPoints.Add(points[i]);
                    vis[i] = true;
                }
            }

        }
        bool isequal(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
