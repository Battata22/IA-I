using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<Node> CalculateBFS(Node StartNode, Node EndNode)
    {
       var frontier = new Queue<Node>();
        frontier.Enqueue(StartNode);

        var cameFrom = new Dictionary<Node, Node>();
        
        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current == EndNode)
            {
                List<Node> path = new();
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

        return new List<Node>();

    }

}
