using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            bool[] vis = new bool[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                vis[i] = false;
            }
            points.Sort(com);
            List<Point> list = new List<Point>();
            list.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Equals(points[i - 1]))
                {
                    continue;
                }
                list.Add(points[i]);
            }
            points = list; 
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    if (points[i].Equals(points[j])) continue;

                    Line l = new Line(points[i], points[j]);
                    int left = 0, right = 0 , col=0;
                    for (int k = 0; k < points.Count; k++)
                    {
                        if (points[i].Equals(points[k]) || points[k].Equals(points[j]))
                            continue;
                        if (HelperMethods.CheckTurn(l, points[k]) == Enums.TurnType.Left) left++;
                        else if (HelperMethods.CheckTurn(l, points[k]) == Enums.TurnType.Right) right++;
                        else if (HelperMethods.CheckTurn(l, points[k]) == Enums.TurnType.Colinear &&
                                points[k].X >= l.Start.X && points[k].X <= l.End.X &&
                                points[k].Y >= l.Start.Y && points[k].Y <= l.End.Y)
                            col++;
                    }
                    if (left == points.Count - col - 2 || right == points.Count - col - 2)
                    {
                        outLines.Add(l);
                        for (int ii = 0; ii < outPoints.Count; ii++)
                        {
                            if (points[i].X == outPoints[ii].X && points[i].Y == outPoints[ii].Y)
                            {
                                vis[i] = true;
                                break;
                            }
                        }
                        if (vis[i] == false)
                        {
                            outPoints.Add(points[i]);
                            vis[i] = true;
                        }
                        if (vis[j] == false)
                        {
                            outPoints.Add(points[j]);
                            vis[j] = true;
                        }
                    }
                }
            }
            if (outPoints.Count == 0 && points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    for (int j = 0; j < points.Count; j++)
                    {
                        if (points[i].Equals(points[j])) continue;
                        Line l = new Line(points[i], points[j]);
                        int col = 0;
                        for (int k = 0; k < points.Count; k++)
                        {
                            if (j == k || k == i)
                                continue;
                            if (HelperMethods.CheckTurn(l, points[k]) == Enums.TurnType.Colinear &&
                                points[k].X >= l.Start.X && points[k].X <= l.End.X &&
                                points[k].Y >= l.Start.Y && points[k].Y <= l.End.Y)
                            {
                                col++;
                            }
                        }
                        if (col == points.Count - 2)
                        {
                            outLines.Add(l);
                            for (int ii = 0; ii < outPoints.Count; ii++)
                            {
                                if (points[i].X == outPoints[ii].X && points[i].Y == outPoints[ii].Y)
                                {
                                    vis[i] = true;
                                    break;
                                }
                            }
                            if (vis[i] == true)
                                continue;
                            outPoints.Add(points[i]);
                            outPoints.Add(points[j]);
                            vis[i] = true;
                            vis[j] = true;
                        }


                    }

                }
            }

            
            if (outPoints.Count == 0 && points.Count > 0)
                outPoints.Add(points[0]);
        }
        static int com(Point a, Point b)
        {
            if (a.X == b.X) return a.Y.CompareTo(b.Y);
            return a.X.CompareTo(b.X);
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
