using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2 : MonoBehaviour
{
    [SerializeField] Node2 _nodeprefab;
    [SerializeField] int _width, _height;
    [SerializeField, Range(1,2)] float _offset;

    Node2[,] _grid;

    //public List<Node> nodesBorrar;

    private void Start()
    {
       _grid = new Node2[_width, _height]; 

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var newNode = Instantiate(_nodeprefab);
                newNode.transform.position = new Vector3(x, 0, y) * _offset;
                newNode.Initialize(this, x, y);
                _grid[x, y] = newNode;

            }
        }

    }

    public Node2 GetNode(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _width || y >= _height) return null;

        return _grid[x, y];
    }

    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.N))
    //    {
    //        foreach (var node in nodesBorrar)
    //        {
    //            node.GetNeightbors;
    //        }
    //    }
    //}

}
