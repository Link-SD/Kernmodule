using System;
using System.Collections.Generic;
using UnityEngine;
namespace SD.AI.Pathfinding.BFS {
    public class Graph<Location> {
        // NameValueCollection would be a reasonable alternative here, if
        // you're always using string location types
        public Dictionary<Location, Location[]> edges
            = new Dictionary<Location, Location[]>();

        public Location[] Neighbors(Location id) {
            return edges[id];
        }
    };


    public class BFS : MonoBehaviour {

        static void Search(Graph<string> graph, string start) {
            var frontier = new Queue<string>();
            frontier.Enqueue(start);

            var visited = new HashSet<string>();
            visited.Add(start);

            while (frontier.Count > 0) {
                var current = frontier.Dequeue();

                //      Console.WriteLine("Visiting {0}", current);
                Debug.Log(string.Format("Visiting {0}", current));
                foreach (var next in graph.Neighbors(current)) {
                    if (!visited.Contains(next)) {
                        frontier.Enqueue(next);
                        visited.Add(next);
                    }
                }
            }
        }

        private void Start() {
            Graph<string> g = new Graph<string>();
            g.edges = new Dictionary<string, string[]>
                {
            { "A", new [] { "B" } },
            { "B", new [] { "A", "C", "D" } },
            { "C", new [] { "A" } },
            { "D", new [] { "E", "A" } },
            { "E", new [] { "B" } }
        };

            Search(g, "A");
        }
    }
}