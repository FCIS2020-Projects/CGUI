using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class Segment
    {
        public double Y_Value { get; set; }
        public int Index { get; set; }
        //
        public Point Vertex;
        public Segment() { }
        public Segment(double y, int i)
        {
            Y_Value = y;
            Index = i;
        }
        public Segment(int i, Point vertex)
        {
            Vertex = vertex;
            Index = i;
        }

    }
}
