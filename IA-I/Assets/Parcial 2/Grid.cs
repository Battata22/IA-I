using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    [SerializeField] Node _nodePrefab;
    [SerializeField] int _width = 10, _height = 10;
    [SerializeField, Range(1f,1.5f)] float _offset;
    Node[,] _grid;

    void Start()
    {
        _grid = new Node[_width, _height];

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var newNode = Instantiate(_nodePrefab);
                newNode.transform.position = new Vector3(x, y, 0) * _offset;
                newNode.Initialize(x,y);
                _grid[x, y] = newNode;
            }
        }
    }

    public Node GetNode(int xPos, int yPos)
    {
        if (xPos < 0 || xPos >= _width || yPos < 0 || yPos >= _height) return null;
        return _grid[xPos, yPos];
    }
}
