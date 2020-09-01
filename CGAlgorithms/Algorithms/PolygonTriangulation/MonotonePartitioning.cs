using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities.DataStructures;
using CGUtilities;


namespace CGAlgorithms.Algorithms.PolygonTriangulation
{
    class MonotonePartitioning : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            // Check Polygon Orientation
            Polygon P = polygonCounterClockwise(polygons[0]);
            //Construct list contain all types of vertices
            List<Enums.VertexType> Vtype = Vertex_Type(P);

            // Construct Q
            OrderedSet<Event> Q = new OrderedSet<Event>(
                (a, b) =>
                {
                    if (a.P.Y == b.P.Y) return a.P.X.CompareTo(b.P.X);
                    return -a.P.Y.CompareTo(b.P.Y);
                });
            Dictionary<int, Event> DC = new Dictionary<int, Event>();
            for (int i = 0; i < P.lines.Count; i++)
            {
                Event ev = new Event(i, P.lines[i].Start, Vtype[i], new Segment(i, P.lines[i].Start));
                Q.Add(ev);
            }

            Event prevr = new Event(P.lines.Count-1, P.lines[P.lines.Count - 1].Start, Vtype[P.lines.Count - 1], new Segment(P.lines.Count - 1, P.lines[P.lines.Count - 1].Start));

            DC.Add(0, prevr);

            for (int i = 1; i < P.lines.Count; i++)
            {
                Event prev = new Event(i-1, P.lines[i-1].Start, Vtype[i-1], new Segment(i-1, P.lines[i-1].Start));

                DC.Add(i, prev);

            }
            OrderedSet<Event> T = new OrderedSet<Event>(
                (a, b) =>
                {
                    return a.Seg1.Vertex.X.CompareTo(b.Seg1.Vertex.X);
                });
            Dictionary<int, Event> helper = new Dictionary<int, Event>();

            while (Q.Count != 0)
            {
                Event e = Q.First();
                if (e.VertexType == Enums.VertexType.Start)
                {
                    T.Add(e);
                    helper.Add(e.Seg1.Index, e);
                }
                else if (e.VertexType == Enums.VertexType.Split)
                {
                    Event e_j = null;
                    foreach (Event j in T)
                    {
                        if (intersectsWithSweepLine(P, e, j) &&
                                liesOnTheLeftSideof(P, e, j))
                        {
                            if (e_j == null)
                            {
                                e_j = j;
                            }
                            else if (liesOnTheRightSideof(P, e, j, e_j))
                            {
                                e_j = j;
                            }
                        }
                    }

                    outLines.Add(new Line(e.P, helper[e_j.Seg1.Index].P));
                    helper[e_j.Seg1.Index] = e;
                    T.Add(e);
                    helper[e.Seg1.Index] = e;

                }
                else if (e.VertexType == Enums.VertexType.End)
                {
                    //var UpperAndLower = T.DirectUpperAndLower(e);
                    //Event temp = UpperAndLower.Key;
                   //Event temp = UpperAndLower.Value;

                    Event ei_1 = helper[e.Seg1.Index - 1];
                    if (ei_1.VertexType == Enums.VertexType.Merge)
                        outLines.Add(new Line(e.P, ei_1.P));

                    T.Remove(ei_1);
                }
                else if (e.VertexType == Enums.VertexType.Merge)
                {
                    //var UpperAndLower = T.DirectUpperAndLower(e);
                    ////Event temp = UpperAndLower.Key;
                    //Event temp = UpperAndLower.Value;
                   
                    Event ei_1 = helper[e.Seg1.Index-1];
                    if (ei_1.VertexType == Enums.VertexType.Merge)
                        outLines.Add(new Line(e.P, ei_1.P));
                    Event R_e = DC[e.VertexIndx];
                    T.Remove(R_e);

                    Event e_j = null;
                    foreach (Event j in T)
                    {
                        if (intersectsWithSweepLine(P, e, j) &&
                                liesOnTheLeftSideof(P, e, j))
                        {
                            if (e_j == null)
                            {
                                e_j = j;
                            }
                            else if (liesOnTheRightSideof(P, e, j, e_j))
                            {
                                e_j = j;
                            }
                        }
                    }

                    Event e_jj = helper[e_j.Seg1.Index];
                    if (e_jj.VertexType == Enums.VertexType.Merge)
                    {
                        outLines.Add(new Line(e.P, e_jj.P));
                    }
                    helper[e_j.Seg1.Index] = e;


                }
                else
                {
                    Point PMinus = P.lines[((e.VertexIndx - 1) + P.lines.Count) % P.lines.Count].Start;
                    if (PMinus.Y > e.P.Y)
                    {
                        //var UpperAndLower = T.DirectUpperAndLower(e);
                        //Event temp = UpperAndLower.Key;
                        //Event temp = UpperAndLower.Value;

                        Event ei_1 = helper[e.Seg1.Index-1];
                        if (ei_1.VertexType == Enums.VertexType.Merge)
                            outLines.Add(new Line(e.P, ei_1.P));

                        //T.ElementAt
                        T.Remove(DC[e.VertexIndx]);
                        T.Add(e);
                        helper.Add(e.Seg1.Index, e);

                    }
                    else
                    {
                        Event e_j = null;
                        foreach (Event j in T)
                        {
                            if (intersectsWithSweepLine(P, e, j) &&
                                    liesOnTheLeftSideof(P, e, j))
                            {
                                if (e_j == null)
                                {
                                    e_j = j;
                                }
                                else if (liesOnTheRightSideof(P, e, j, e_j))
                                {
                                    e_j = j;
                                }
                            }
                        }

                        Event e_jj = helper[e_j.Seg1.Index];
                        if (e_jj.VertexType == Enums.VertexType.Merge)
                        {
                            outLines.Add(new Line(e.P, e_jj.P));
                        }
                        helper[e_j.Seg1.Index] = e;
                    }
                }
                Q.Remove(e);
            }
        }

        public override string ToString()
        {
            return "Monotone Partitioning";
        }
        public Polygon polygonCounterClockwise(Polygon polygons)
        {
            double sum = 0;
            for (int i = 0; i < polygons.lines.Count; i++)
            {
                sum += ((polygons.lines[i].End.X - polygons.lines[i].Start.X) * (polygons.lines[i].End.Y + polygons.lines[i].Start.Y));
            }
            if (sum > 0)
            { 
                polygons.lines.Reverse();
                for(int i = 0; i < polygons.lines.Count; i++)
                {
                    Point temp = polygons.lines[i].Start;
                    polygons.lines[i].Start = polygons.lines[i].End;
                    polygons.lines[i].End = temp;
                }
            }
            return polygons;
        }
        public List<Enums.VertexType> Vertex_Type(Polygon p)
        {
            List<Enums.VertexType> type = new List<Enums.VertexType>();
            int pi = 180;
            for (int i = 0; i < p.lines.Count; i++)
            {
                int PPlus = (i + 1) % p.lines.Count;
                int PMinus = ((i - 1) + p.lines.Count) % p.lines.Count;

                Point next = p.lines[PPlus].Start;
                Point prev = p.lines[PMinus].Start;
                Point C = p.lines[i].Start;

                if (next.Y < C.Y && prev.Y < C.Y && IsConvex(p , i))
                    type.Add(Enums.VertexType.Start);
                else if (next.Y < C.Y && prev.Y < C.Y && !IsConvex(p, i))
                    type.Add(Enums.VertexType.Split);
                else if (next.Y > C.Y && prev.Y > C.Y && IsConvex(p, i))
                    type.Add(Enums.VertexType.End);
                else if (next.Y > C.Y && prev.Y > C.Y && !IsConvex(p, i))
                    type.Add(Enums.VertexType.Merge);
                else
                    type.Add(Enums.VertexType.Regular);
            }
            return type;
        }
        public bool intersectsWithSweepLine(Polygon polygon, Event e , Event j)
        {
            var sweepY = e.P.Y;
            Point jj = polygon.lines[(j.VertexIndx + 1) % polygon.lines.Count()].Start;
                //(index + 1) % polygon.size()
            return (sweepY >= j.P.Y && sweepY <= jj.Y) ||
                    (sweepY >= jj.Y && sweepY <= j.P.Y);
        }
        public bool liesOnTheLeftSideof(Polygon polygon,  Event e, Event j)
        {
            Point p1 = j.P;
            Point p2 = polygon.lines[(j.VertexIndx + 1) % polygon.lines.Count()].Start;
            double a = p1.Y - p2.Y;
            double b = p2.X - p1.X;
            double c = p1.X * p2.Y - p2.X * p1.Y;

            return (-c - b * e.P.Y) / a < e.P.X;
        }
        public bool liesOnTheRightSideof(Polygon polygon, Event e, Event j,Event ej)
        {

            Point p1 = j.P;
            Point p2 = polygon.lines[(j.VertexIndx + 1) % polygon.lines.Count()].Start;
            double a = p1.Y - p2.Y;
            double b = p2.X - p1.X;
            double c = p1.X * p2.Y - p2.X * p1.Y;

            double A = (-c - b * e.P.Y) / a ;

            p1 = ej.P;
            p2 = polygon.lines[(j.VertexIndx + 1) % polygon.lines.Count()].Start;
            a = p1.Y - p2.Y;
            b = p2.X - p1.X;
            c = p1.X * p2.Y - p2.X * p1.Y;

            double B = (-c - b * e.P.Y) / a;
            return A > B;
        }
        public bool IsConvex(Polygon p , int Cur)
        {
            int prev = Cur - 1;
            if (prev < 0) prev += p.lines.Count;
            int next = (Cur + 1) % p.lines.Count;

            Point p0 = p.lines[prev].Start;
            Point p1 = p.lines[Cur].Start;
            Point p2 = p.lines[next].Start;

            double res =  (p1.X - p0.X) * (p2.Y - p0.Y) - (p2.X - p0.X) * (p1.Y - p0.Y);

            if (res > 0)
                return true;
            return false;
        }
    }
}
