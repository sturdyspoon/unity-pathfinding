using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Pathfinding
{
    /// <summary>
    /// A graph interface for use with A* Search.
    /// </summary>
    public interface IGraph
    {
        IEnumerable<Vector3Int> Neighbors(Vector3Int v);
        float Cost(Vector3Int a, Vector3Int b);
    }

    /// <summary>
    /// A graph based off a tilemap where you can move in up, down, left, and right.
    /// </summary>
    public class FourDirectionGraph : IGraph
    {
        static readonly Vector3Int[] NEIGHBORS = {
            new Vector3Int(1, 0, 0),
            new Vector3Int(0, -1, 0),
            new Vector3Int(-1, 0, 0),
            new Vector3Int(0, 1, 0)
        };

        Tilemap map;
        BoundsInt bounds;

        /// <summary>
        /// Creates a new FourDirectionalGraph using the tilemap's cell bounds
        /// as the graph bounds. If the tilemap does not have tiles on its
        /// intended boundary then the graph bounds could be smaller than it
        /// should be and you should use the other constructor to specify the
        /// graph bounds.
        /// </summary>
        public FourDirectionGraph(Tilemap map)
        {
            this.map = map;
            this.bounds = map.cellBounds;
        }

        public FourDirectionGraph(Tilemap map, BoundsInt bounds)
        {
            this.map = map;
            this.bounds = bounds;
        }

        public IEnumerable<Vector3Int> Neighbors(Vector3Int v)
        {
            foreach (Vector3Int dir in NEIGHBORS)
            {
                Vector3Int next = v + dir;

                if (bounds.Contains(next) && map.GetTile(next) == null)
                {
                    yield return next;
                }
            }
        }

        public float Cost(Vector3Int a, Vector3Int b)
        {
            return Vector3Int.Distance(a, b);
        }
    }
}
