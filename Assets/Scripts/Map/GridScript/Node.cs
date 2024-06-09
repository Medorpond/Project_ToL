using UnityEditor;
using UnityEngine;


public class Node
{
    public Node(bool _isDeployable, bool _isBlocked, int _x, int _y) { isDeployable = _isDeployable; isBlocked = _isBlocked; x = _x; y = _y; }

    // D for distance(from Origin), H for Heuristic value, W for total Weigh(D + H)
    public bool isBlocked;
    public bool isDeployable;
    public Unit unitOn = null;

    public Node ParentNode;
    public int x, y, D, H;
    public int W { get { return D + H; } }

}