using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
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
            // points filter 
            //-------------------------------------------------------

            double mny = double.MaxValue;
            double x = 0;
            bool[] vis = new bool[points.Count];
            for (int i = 0; i < points.Count; i++)
                vis[i] = false;
            int ind = 0, find = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (mny > points[i].Y)
                {
                    mny = points[i].Y;
                    x = points[i].X;
                    ind = i;
                }
            }
            find = ind;
            Line l = new Line(new Point(x, mny), new Point(x + 1000.0, mny));


            // first line ---------------------------------------------
            double mnang = double.MaxValue;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Equals(l.Start) || points[i].Equals(l.End)) continue;

                Point v1 = tovector(l.End, l.Start);
                Point v2 = tovector(points[i], l.Start);
                double cros = HelperMethods.CrossProduct(v1, v2);
                double dot = HelperMethods.DotProduct(v1, v2);
                double angle = Math.Atan2(cros, dot);
                if (angle < 0)
                    angle += 360;
                if (angle <= mnang)
                {
                    mnang = angle;
                    ind = i;
                }
            }
            l = new Line(points[ind], points[find]);
            outLines.Add(l);
            vis[ind] = true;

            for (int t = 0; true ; t++)
            {
                mnang = double.MaxValue;
                double dis = double.MinValue;
                int ind1 = ind;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].Equals(l.Start) || points[i].Equals(l.End)) continue;

                    Point v1 = tovector(l.End, l.Start);
                    Point v2 = tovector(points[i], l.Start);
                    double cros = HelperMethods.CrossProduct(v1, v2);
                    double dot = HelperMethods.DotProduct(v1, v2);
                    double angle = Math.Atan2(cros, dot) * (180.00 / Math.PI); ;
                    if (angle < 0)
                        angle += 360;
                    if (angle == mnang && dis < HelperMethods.distance(l.Start, points[i]))
                    {
                        ind = i;
                        dis = HelperMethods.distance(l.Start, points[i]);
                    }
                    else if (angle < mnang && angle > 0)
                    {
                        mnang = angle;
                        ind = i;
                        dis = dis = HelperMethods.distance(l.Start, points[i]);
                    }
                }
                if (mnang == 0 || points[ind].Equals(l.Start) || points[ind].Equals(l.End) ) 
                    break;
                l = new Line(points[ind], points[ind1]);
                vis[ind] = true;
                outLines.Add(l);
                if (ind == find)
                    break;
            }
            for (int i = 0; i < outLines.Count; i++)
            {
                bool ff = true;
                for (int ii = 0; ii < outPoints.Count; ii++)
                {
                    if (outLines[i].Start.Equals(outPoints[ii]))
                    {
                        ff = false;
                        break;
                    }
                }
                if (ff == true)
                {
                    outPoints.Add(outLines[i].Start);
                }
                ff = true;
                for (int ii = 0; ii < outPoints.Count; ii++)
                {
                    if (outLines[i].End.Equals(outPoints[ii]))
                    {
                        ff = false;
                        break;
                    }
                }
                if (ff == true)
                {
                    outPoints.Add(outLines[i].End);
                }
            }
        }
        static int com(Point a, Point b)
        {
            if (a.X == b.X) return a.Y.CompareTo(b.Y);
            return a.X.CompareTo(b.X);
        }
        Point tovector(Point a, Point b)
        {
            return new Point(b.X - a.X, b.Y - a.Y);
        }
        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
