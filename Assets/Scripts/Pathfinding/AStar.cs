using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using System;

namespace Pathfinding
{
    /// <summary>
    /// A* search based off Amit Patel's implementation.
    /// </summary>
    /// <remarks>
    /// Patel's implementation is different than what you see in most algorithms and AI textbooks.
    /// https://www.redblobgames.com/pathfinding/a-star/implementation.html
    /// </remarks>
    public static class AStar
    {
        /// <summary>
        /// Finds a path in the tilemap using world coordinates.
        /// </summary>
        public static List<Vector3> FindPath(Tilemap map, Vector3 start, Vector3 goal)
        {
            List<Vector3> result = null;

            List<Vector3Int> path = FindPath(map, map.WorldToCell(start), map.WorldToCell(goal));

            if(path != null)
            {
                result = new List<Vector3>(path.Capacity);

                foreach (Vector3Int v in path)
                {
                    result.Add(map.GetCellCenterWorld(v));
                }
            }

            return result;
        }

        /// <summary>
        /// Finds a path in the tilemap using cell coordinates.
        /// </summary>
        public static List<Vector3Int> FindPath(Tilemap map, Vector3Int start, Vector3Int goal)
        {
            return FindPath(new FourDirectionGraph(map), start, goal, Vector3Int.Distance);
        }

        /// <summary>
        /// Finds a path in the graph using cell coordinates.
        /// </summary>
        public static List<Vector3Int> FindPath(IGraph graph, Vector3Int start, Vector3Int goal, Func<Vector3Int, Vector3Int, float> heuristic)
        {
            PriorityQueue<Vector3Int> open = new PriorityQueue<Vector3Int>();
            open.Enqueue(start, 0);

            Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            cameFrom[start] = start;

            Dictionary<Vector3Int, float> costSoFar = new Dictionary<Vector3Int, float>();
            costSoFar[start] = 0;

            while (open.Count > 0)
            {
                Vector3Int current = open.Dequeue();

                if (current == goal)
                {
                    break;
                }

                foreach (Vector3Int next in graph.Neighbors(current))
                {
                    float newCost = costSoFar[current] + graph.Cost(current, next);

                    if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                    {
                        costSoFar[next] = newCost;
                        float priority = newCost + heuristic(next, goal);
                        open.Enqueue(next, priority);
                        cameFrom[next] = current;
                    }
                }
            }

            List<Vector3Int> path = null;

            if (cameFrom.ContainsKey(goal))
            {
                path = new List<Vector3Int>();

                Vector3Int v = goal;

                while(v != start)
                {
                    path.Add(v);
                    v = cameFrom[v];
                }

                path.Add(start);

                path.Reverse();
            }

            return path;
        }
    }
}