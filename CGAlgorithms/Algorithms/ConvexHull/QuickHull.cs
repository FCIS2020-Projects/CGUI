using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            var uniquePoints = points.Select(myObj => new { myObj.X, myObj.Y })
                             .Distinct().ToList();
            points = new List<Point>();
            foreach (var p in uniquePoints)
                points.Add(new Point(p.X, p.Y));

            if (points.Count <= 3)
                outPoints = points;
            else
            {
                Point PmaxX = get_maxpoint_x(points);
                Point PminX = get_minpoint_x(points);

                var LP = get_left_points(points, PminX, PmaxX);
                var RP = get_left_points(points, PmaxX, PminX);

                List<Point> R1 = new List<Point>();
                List<Point> R2 = new List<Point>();

                if (LP.Count > 0)
                    R1 = Quick_Hull(LP, PminX, PmaxX);
                if (RP.Count > 0)
                    R2 = Quick_Hull(RP, PmaxX, PminX);

                List<Point> R = new List<Point>();
                if (R1.Count > 0)
                    R = R.Concat(R1).ToList();
                if (R2.Count > 0)
                    R = R.Concat(R2).ToList();
                R.Add(PmaxX);
                R.Add(PminX);

                outPoints = R;
            }
        }

        public static Point get_minpoint_x(List<Point> points)
        {
            Point minPoint = new Point(int.MaxValue, int.MaxValue);
            foreach (Point point in points)
            {
                if (point.X < minPoint.X)
                {
                    minPoint.X = point.X;
                    minPoint.Y = point.Y;
                }
            }
            return minPoint;
        }
        public static Point get_maxpoint_x(List<Point> points)
        {
            Point maxPoint = new Point(int.MinValue, int.MinValue);

            foreach (Point point in points)
            {
                if (point.X > maxPoint.X)
                {
                    maxPoint.X = point.X;
                    maxPoint.Y = point.Y;
                }
            }
            return maxPoint;
        }
        public static List<Point> get_left_points(List<Point> points, Point s, Point e)
        {
            Line l = new Line(s, e);
            List<Point> LP = new List<Point>();
            foreach (Point p in points)
            {
                Enums.TurnType type = HelperMethods.CheckTurn(l, p);

                if (type == Enums.TurnType.Left)
                    LP.Add(p);
            }
            return LP;
        }
        public static Point get_max_pointD(List<Point> points, Point s, Point e)
        {
            double m = ((e.Y - s.Y) / (e.X - s.X));
            double c = (e.Y - m * e.X);

            double m2 = -1 / m;

            if (m == 0)
            {
                Point po = new Point(int.MinValue, int.MinValue);

                foreach (Point p in points)
                {
                    if (Math.Abs(p.Y) > po.Y)
                        po = p;
                }
                return po;
            }

            double dist = int.MinValue;
            Point pointD = new Point(int.MaxValue, int.MaxValue);
            foreach (Point p in points)
            {
                double x = ((-m2 * p.X) + p.Y - c) / (m - m2);
                double Y = (m * x) + c;

                double tempdist = Math.Sqrt(Math.Pow(p.X - x, 2) + Math.Pow(p.Y - Y, 2));

                if (tempdist > dist)
                {
                    dist = tempdist;
                    pointD = p;
                }
            }

            //if (pointD.X == int.MaxValue || pointD.Y == int.MaxValue)
            //    return null;
            return pointD;

        }
        public static List<Point> Quick_Hull(List<Point> points, Point s, Point e)
        {
            if (points.Count == 0)
                return new List<Point>();

            Point PmaxD = get_max_pointD(points, s, e);

            var ps1 = get_left_points(points, s, PmaxD);
            var ps2 = get_left_points(points, PmaxD, e);


            List<Point> R1 = new List<Point>();
            List<Point> R2 = new List<Point>();

            if (ps1.Count > 0)
                R1 = Quick_Hull(ps1, s, PmaxD);
            if (ps2.Count > 0)
                R2 = Quick_Hull(ps2, PmaxD, e);

            List<Point> R = new List<Point>();
            if (R1.Count > 0)
                R = R.Concat(R1).ToList();
            if (R2.Count > 0)
                R = R.Concat(R2).ToList();
            R.Add(PmaxD);

            return R;
        }


        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
