using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Node : MonoBehaviour
{
    //Grid _myGrid;
    int _xPos, _yPos;
    [SerializeField] List<Node> _neighbors = new List<Node>();
    public int Cost { get; private set; }


    public void Initialize(int xPos, int yPos)
    {
        _xPos = xPos;
        _yPos = yPos;
        Cost = 1;
    }


    public List<Node> GetNeighbors
    {
        get
        {
            //if (_neighbors.Count > 0) return _neighbors;

            //var nodeUp = _myGrid.GetNode(_xPos, _yPos + 1);
            //if (nodeUp != null) _neighbors.Add(nodeUp);

            //var nodeRight = _myGrid.GetNode(_xPos + 1, _yPos);
            //if (nodeRight != null) _neighbors.Add(nodeRight);

            //var nodeDown = _myGrid.GetNode(_xPos, _yPos - 1);
            //if (nodeDown != null) _neighbors.Add(nodeDown);

            //var nodeLeft = _myGrid.GetNode(_xPos - 1, _yPos);
            //if (nodeLeft != null) _neighbors.Add(nodeLeft);

            //Por FOV o a mano

            return _neighbors;
        }
    }

}
