using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Astar 
{
    public class PathFinder : MonoBehaviour
    {
        public Vector2Int bottomLeft, topRight, startPos, targetPos;
        public List<Node> FinalNodeList;


        int sizeX, sizeY;
        Node[,] NodeArray;
        Node StartNode, TargetNode, CurNode;
        List<Node> HoldList, ClosedList;


        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

