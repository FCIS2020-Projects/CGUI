using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class Event
    {
        public Point P { get; set; }
        public Segment Seg1 { get; set; }
        public Segment Seg2 { get; set; }
        public Enums.EventType Type { get; set; }
        ///
        public int VertexIndx { get; set; }
        public Enums.VertexType VertexType { get; set; }
        

        public Event() { }
        public Event(Point p, Segment seg1, Enums.EventType type)
        {
            P = p;
            Seg1 = seg1;
            Type = type;
        }
        public Event(Point p, Segment seg1, Segment seg2, Enums.EventType type)
        {
            P = p;
            Seg1 = seg1;
            Seg2 = seg2;
            Type = type;
        }
        public Event(int vertexindx  , Point p, Enums.VertexType vertextype , Segment seg)
        {
            VertexIndx = vertexindx;
            P = p;
            VertexType = vertextype;
            Seg1 = seg;
        }
    }
}
