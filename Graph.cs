﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class Graph
    {
        private SortedDictionary<string, Vertex> AdjList;
        private bool directed;

        public Graph(bool d = false)
        {
            AdjList = new SortedDictionary<string, Vertex>();
            directed = d;
        }


        // Adding new vertex
        public void AddVertex(string name)
        {
            if(AdjList.ContainsKey(name))
                throw new Exception($"The vertex with name {name} already exist!");
            Vertex vert = new Vertex(name);
            AdjList.Add(name, vert);
        }

        public void AddEdge(string from, string to, uint weight)
        {
            if(!AdjList.ContainsKey(from))
                throw new Exception($"The vertex with name {from} does not exist!");
            if(!AdjList.ContainsKey(to))
                throw new Exception($"The vertex with name {to} does not exist!");
            Vertex fr = AdjList[from], t = AdjList[to];
            fr.AddEdge(t, weight);
            if(!directed){
                t.AddEdge(fr, weight);
            }
        }

        public void DeleteEdge(string from, string to)
        {
            if(!AdjList.ContainsKey(from))
                throw new Exception($"The vertex with name {from} does not exist!");
            if(!AdjList.ContainsKey(to))
                throw new Exception($"The vertex with name {to} does not exist!");
            Vertex f = AdjList[from], t = AdjList[to];
            f.DeleteEdge(t);
            if(!directed)
                t.DeleteEdge(f);
        }

        public void DeleteVertex(string name)
        {
            if(!AdjList.ContainsKey(name))
                throw new Exception($"The vertex with name {name} does not exist!");
            Vertex v = AdjList[name];
            foreach (Vertex vertex in AdjList.Values)
            {
                vertex.DeleteEdge(v);
            }
            AdjList.Remove(name);
        }

        // Dijkstra for from "from" to "to"

        private void Dijkstra(Vertex from, out Dictionary<Vertex, uint> end_distances, out Dictionary<Vertex, Vertex> prev)
        {
            // initialization
            
            end_distances = new Dictionary<Vertex, uint>(AdjList.Count);
            prev = new Dictionary<Vertex, Vertex>(AdjList.Count);
            VertexPriorityQueue queue = new VertexPriorityQueue(AdjList.Values.ToArray(), from);

            foreach (Vertex v in AdjList.Values)
            {
                prev.Add(v, null);
            }
            
            
            // Dijkstra as it is

            while (queue.heapsize > 0)
            {
                KeyValuePair<Vertex, uint> u = queue.ExtractMin();
                end_distances.Add(u.Key, u.Value);

                //relaxation
                foreach (KeyValuePair<Vertex,uint> v in u.Key.Adj)
                {
                    if (end_distances.ContainsKey(v.Key))
                    {
                        if (end_distances[v.Key] > u.Value + u.Key.Adj[v.Key])
                        {
                            end_distances[v.Key] = u.Value + u.Key.Adj[v.Key];
                        }
                    }
                    else
                    {
                        if (queue[v.Key] > u.Value + u.Key.Adj[v.Key])
                        {
                            queue.DecreaseKey(v.Key, u.Value + u.Key.Adj[v.Key]);
                        }
                    }
                    prev[v.Key] = u.Key;
                }
                
            }



        }

        public uint GetShortestPath(string f, string t, out LinkedList<string> path)
        {
            Dictionary<Vertex, uint> end_distances;
            Dictionary<Vertex, Vertex> prev;
            Vertex from = AdjList[f], to = AdjList[t];
            Dijkstra(from, out end_distances, out prev);
            uint dist = end_distances[to];

            // if the vertex "to" is not reachable
            if (dist == uint.MaxValue)
            {
                path = null;
                return dist;
            }

            path = new LinkedList<string>();
            for (Vertex cur = to; cur != null; cur = prev[cur])
            {
                path.AddFirst(cur.Name);
            }

            return dist;

        }

        // finding stronly connected components
        public Graph Transpose()
        {
            Graph Gt = new Graph();
            // copying vertices
            foreach (string key in AdjList.Keys)
            {
                Gt.AddVertex(key);
            }


            foreach (Vertex from in AdjList.Values)
            {
                foreach (KeyValuePair<Vertex,uint> to in from.Adj)
                {
                    Gt.AddEdge(to.Key.Name, from.Name, to.Value);
                }
            }
            return Gt;
        }


        public List<HashSet<string>> StronglyConnectedComponents()
        {
            List<Vertex> closedInOrder = new List<Vertex>(), closed2 = new List<Vertex>();
            HashSet<Vertex> opened = new HashSet<Vertex>();

            foreach (Vertex vertex in AdjList.Values)
            {
                if(!closedInOrder.Contains(vertex))
                    DFS_Visit(vertex, opened, closedInOrder);
            }

            opened = new HashSet<Vertex>();
            closedInOrder.Reverse();
            Graph Gt;
            if(this.directed) Gt = this.Transpose();
            else Gt = this;
            List<HashSet<string>> components = new List<HashSet<string>>();

            for (int i = 0, j = 0; i < closedInOrder.Count; i++)
            {
                // if the current vertex is already closed
                // it's in some component, so we continue
                if (closed2.Contains(Gt.AdjList[closedInOrder[i].Name]))
                {
                    continue;
                }

                // new component
                HashSet<string> component = new HashSet<string>();
                DFS_Visit(Gt.AdjList[closedInOrder[i].Name], opened, closed2);
                for (; j < closed2.Count; j++)
                {
                    component.Add(closed2[j].Name);
                }
                components.Add(component);
            }

            return components;
        }

        public void DFS_Visit(Vertex start, HashSet<Vertex> opened, List<Vertex> closedInOrder)
        {
            if (opened.Contains(start) || closedInOrder.Contains(start))
                return;
            opened.Add(start);
            foreach (Vertex next in start.Adj.Keys)
            {
                if (!opened.Contains(next) || !closedInOrder.Contains(next))
                    DFS_Visit(next, opened, closedInOrder);
            }
            opened.Remove(start);
            closedInOrder.Add(start);
        }






        // Minimum spanning tree algorithm
        public Graph Kruskal()
        {
            // Initialization
            Graph MST = new Graph(false);

            foreach (string vertName in AdjList.Keys)
            {
                MST.AddVertex(vertName);
            }


            // disjoint sets of vertices for Kruskal's algorithm
            VertexDisjointSets Sets = new VertexDisjointSets();
            foreach (Vertex vertex in AdjList.Values)
            {
                VertexDisjointSets.Node v = new VertexDisjointSets.Node(vertex);
                Sets.MakeSet(v);
            }

            // making set of the edges and sort them
            List<Edge> edges = new List<Edge>();
            foreach (Vertex fr in AdjList.Values)
            {
                VertexDisjointSets.Node from = Sets[fr];
                foreach (KeyValuePair<Vertex, uint> t in fr.Adj)
                {
                    VertexDisjointSets.Node to = Sets[t.Key];
                    Edge e = new Edge(from, to, t.Value);
                    edges.Add(e);
                }
            }
            edges.Sort();

            // main part of alg
            foreach (Edge edge in edges)
            {
                if (Sets.FindSet(edge.from) != Sets.FindSet(edge.to))
                {
                    MST.AddEdge(edge.from.vertex.Name, edge.to.vertex.Name, edge.weight);
                    Sets.Union(edge.from, edge.to);
                }
            }

            return MST;
        }


        public override string ToString()
        {            
            string vertices = "Set of vertices G.V = {";
            string edges = "Edges (from, to, weight):\n";
            Vertex[] vrt = AdjList.Values.ToArray();
            for (int i = 0; i < vrt.Length - 1; i++)
            {
                vertices += $"\"{vrt[i].Name}\", ";
                foreach (KeyValuePair<Vertex,uint> edge in vrt[i].Adj)
                {
                    edges += $"(\"{vrt[i].Name}\", \"{edge.Key.Name}\", {edge.Value})\n";
                }
            }

            if (vrt.Length != 0)
            {
                vertices += $"\"{vrt[vrt.Length - 1].Name}\"}}\n";
                foreach (KeyValuePair<Vertex, uint> edge in vrt[vrt.Length - 1].Adj)
                {
                    edges += $"(\"{vrt[vrt.Length - 1].Name}\", \"{edge.Key.Name}\", {edge.Value})\n";
                }
            }
            else return "Graph is empty";

            return vertices + edges;
        }
    }
}
