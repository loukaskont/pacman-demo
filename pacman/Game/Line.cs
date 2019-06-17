using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Line
    {
        public Point startPoint;
        public Point entPoint;
        public Line(Point _startPoint, Point _entPoint) 
        {
            startPoint = _startPoint;
            entPoint = _entPoint;
        }
    }
}
