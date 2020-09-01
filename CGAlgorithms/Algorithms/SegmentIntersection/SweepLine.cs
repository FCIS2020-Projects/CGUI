using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGUtilities.DataStructures;
using CGUtilities;

namespace CGAlgorithms.Algorithms.SegmentIntersection
{
    class SweepLine : Algorithm
    {
        public override void Run(List<CGUtilities.Point> points, List<CGUtilities.Line> lines, List<CGUtilities.Polygon> polygons, ref List<CGUtilities.Point> outPoints, ref List<CGUtilities.Line> outLines, ref List<CGUtilities.Polygon> outPolygons)
        {
            OrderedSet<Event> events = new OrderedSet<Event>(
                (a, b) => {
                    if (a.P.X == b.P.X) return a.P.Y.CompareTo(b.P.Y);
                    return a.P.X.CompareTo(b.P.X);
                });
            OrderedSet<Segment> segments = new OrderedSet<Segment>(
                (a, b) => {                    
                    return -a.Y_Value.CompareTo(b.Y_Value);
                });           
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Start.X < lines[i].End.X)
                {
                    Segment s = new Segment(lines[i].Start.Y, i);
                    events.Add(new Event(lines[i].Start, s, Enums.EventType.Left));
                    events.Add(new Event(lines[i].End, s, Enums.EventType.Right));
                }
                else
                {
                    Segment s = new Segment(lines[i].End.Y, i);
                    events.Add(new Event(lines[i].End, s, Enums.EventType.Left));
                    events.Add(new Event(lines[i].Start, s, Enums.EventType.Right));
                }
            }
            while (events.Count != 0)
            {
                Event e = events.First();
                if (e.Type == Enums.EventType.Left)
                {
                    foreach(Segment s in segments)
                    {
                        double m = HelperMethods.Slope(lines[s.Index]);
                        double c = lines[s.Index].Start.Y - m * lines[s.Index].Start.X;
                        s.Y_Value = m * e.P.X + c;
                    }
                    Segment segC = e.Seg1;
                    segments.Add(segC);
                    var UpperAndLower = segments.DirectUpperAndLower(segC);
                    Segment segU = UpperAndLower.Key;
                    Segment segL = UpperAndLower.Value;
                    if (segU != null && HelperMethods.Intersection(lines[segU.Index], lines[segC.Index]))
                    {
                        Point intersectionPoint = HelperMethods.LineLineIntersectionPoint(lines[segU.Index], lines[segC.Index]);                        
                        if (!outPoints.Contains(intersectionPoint))
                            events.Add(new Event(intersectionPoint, segU, segC, Enums.EventType.Intersection));
                    }
                    if (segL != null && HelperMethods.Intersection(lines[segC.Index], lines[segL.Index]))
                    {
                        Point intersectionPoint = HelperMethods.LineLineIntersectionPoint(lines[segC.Index], lines[segL.Index]);                        
                        if (!outPoints.Contains(intersectionPoint))
                            events.Add(new Event(intersectionPoint, segC, segL, Enums.EventType.Intersection));
                    }
                }
                else if (e.Type == Enums.EventType.Right)
                {
                    Segment segC = e.Seg1;
                    var UpperAndLower = segments.DirectUpperAndLower(segC);
                    Segment segU = UpperAndLower.Key;
                    Segment segL = UpperAndLower.Value;
                    segments.Remove(segC);

                    if (segU != null && segL != null && HelperMethods.Intersection(lines[segU.Index], lines[segL.Index]))
                    {
                        Point intersectionPoint = HelperMethods.LineLineIntersectionPoint(lines[segU.Index], lines[segL.Index]);                        
                        if (!outPoints.Contains(intersectionPoint))
                            events.Add(new Event(intersectionPoint, segU, segL, Enums.EventType.Intersection));
                    }
                }
                else
                {
                    outPoints.Add(e.P);
                    segments.Remove(e.Seg1);
                    segments.Remove(e.Seg2);
                    double temp = e.Seg1.Y_Value;
                    e.Seg1.Y_Value = e.Seg2.Y_Value;
                    e.Seg2.Y_Value = temp;
                    segments.Add(e.Seg1);
                    segments.Add(e.Seg2);

                    var seg1UpperAndLower = segments.DirectUpperAndLower(e.Seg2);
                    Segment segU = seg1UpperAndLower.Key;
                    var seg2UpperAndLower = segments.DirectUpperAndLower(e.Seg1);
                    Segment segL = seg2UpperAndLower.Value;
                    if (segU != null && HelperMethods.Intersection(lines[segU.Index], lines[e.Seg2.Index]))
                    {
                        Point intersectionPoint = HelperMethods.LineLineIntersectionPoint(lines[segU.Index], lines[e.Seg2.Index]);                        
                        if (!outPoints.Contains(intersectionPoint))
                            events.Add(new Event(intersectionPoint, segU, e.Seg2, Enums.EventType.Intersection));
                    }
                    if (segL != null && HelperMethods.Intersection(lines[e.Seg1.Index], lines[segL.Index]))
                    {
                        Point intersectionPoint = HelperMethods.LineLineIntersectionPoint(lines[e.Seg1.Index], lines[segL.Index]);                        
                        if (!outPoints.Contains(intersectionPoint))
                            events.Add(new Event(intersectionPoint, e.Seg1, segL, Enums.EventType.Intersection));
                    }
                }
                events.RemoveFirst();
            }
        }

        public override string ToString()
        {
            return "Sweep Line";
        }
    }
}
