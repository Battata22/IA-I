using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding2 : MonoBehaviour
{
    public List<Node2> CalculateBFS(Node2 StartNode, Node2 EndNode)
    {
       var frontier = new Queue<Node2>();
        frontier.Enqueue(StartNode);

        var cameFrom = new Dictionary<Node2, Node2>();
        
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == EndNode)
            {
                List<Node2> path = new();
                while (current != StartNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Add(current); //para el startNode

                path.Reverse();

                return path;
            }

            foreach (var node in current.GetNeightbors)
            {
                if (!cameFrom.ContainsKey(node))
                {
                    frontier.Enqueue(node);
                    cameFrom.Add(node, current);
                }
            }
        }

        return new List<Node2>();

    }

}
