using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node2 : MonoBehaviour
{
    List<Node2> _neightborsNodes = new();

    int _xPos, _yPos;

    Grid2 _myGrid;

    public void Initialize(Grid2 myGrid, int x, int y)
    {
        _xPos = x;
        _yPos = y;
        _myGrid = myGrid;
    }

    public List<Node2> GetNeightbors
    {
        get
        {
            if (_neightborsNodes.Count > 0) return _neightborsNodes;
            
            var nodeRight = _myGrid.GetNode(_xPos + 1, _yPos);
            if (nodeRight != null)
            {
                _neightborsNodes.Add(nodeRight);
            }

            var nodeDown = _myGrid.GetNode(_xPos, _yPos - 1);
            if (nodeDown != null)
            {
                _neightborsNodes.Add(nodeDown);
            }

            var nodeLeft = _myGrid.GetNode(_xPos - 1, _yPos);
            if (nodeLeft != null)
            {
                _neightborsNodes.Add(nodeLeft);
            }

            var nodeUp = _myGrid.GetNode(_xPos, _yPos + 1);
            if (nodeUp != null)
            {
                _neightborsNodes.Add(nodeUp);
            }
         

            return _neightborsNodes;
            
        }
    }


}
