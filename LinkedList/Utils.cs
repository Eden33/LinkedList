using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Graph.Model;

namespace Graph.Util
{
    public class TreeCoordinateBuilder
    {
        private int x = 0;

        /// <summary>
        /// Build binary tree coordinates with Knuth algorithm.
        /// Works only for binary trees.
        /// Source: http://billmill.org/pymag-trees/
        /// </summary>
        /// <param name="junction"></param>
        /// <param name="depth"></param>
        public void BuildCoordinatesWithKnuth(JunctionPoint junction, int depth)
        {
            Node left = null;
            Node right = null;

            if (junction.NextPathCount == 2)
            {
                left = junction.NextPath[0];
                right = junction.NextPath[1];
            }
            if (junction.NextPathCount == 1)
            {
                left = junction.NextPath[0];
            }

            if(left != null)
            {
                BuildCoordinatesWithKnuth(left.Tail, depth + depth);
            }
            
            junction.X = x;
            junction.Y = depth;
            x += 50;
            
            if(right != null)
            {
                BuildCoordinatesWithKnuth(right.Tail, depth + depth);
            }
        }
    }
}
