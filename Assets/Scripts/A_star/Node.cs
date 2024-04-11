using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Astar
{
    public class Node
    {
        public Node(bool _isBlocked, int _x, int _y) { isBlocked = _isBlocked; x = _x; y = _y; }

        // D for distance(from Origin), H for Heuristic value, W for total Weigh(D + H)
        public bool isBlocked;
        public Node ParantNode;
        public int x, y, D, H;
        public int W { get { return D + H; } }
    }

}
