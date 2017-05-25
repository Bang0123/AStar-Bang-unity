using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinderTest
{
    public enum NodeState
    {
        Wall, Path, Start, End,
        Walked, Open, Closed, Untested
    }
}
