using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGUtilities
{
    public class Enums
    {
        public enum TurnType
        {
            Left,
            Right,
            Colinear
        }
        public enum PointInPolygon
        {
            Inside,
            Outside,
            OnEdge
        }

        public enum InputType
        {
            Points,
            Lines,
            Polygons
        }

        public enum EventType
        {
            Left,
            Right,
            Intersection
        }

        public enum VertexType
        {
            Start,
            End,
            Merge,
            Split,
            Regular
        }
    }
}
