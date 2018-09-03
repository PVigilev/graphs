using System;
using System.Collections.Generic;

namespace Graphs
{
    public class Vertex
    {

        private readonly Dictionary<Vertex, uint> _adj;
        public string Name { get; }

        public Dictionary<Vertex, uint> Adj => _adj;

        public int EdgeWeight(Vertex to)
        {
            if (Adj.ContainsKey(to))
                return (int) Adj[to];
            return -1;
        }

        public Vertex(string name)
        {
            Name = name;
            _adj = new Dictionary<Vertex, uint>();
        }

        public void AddEdge(Vertex to, uint weight)
        {
            if(Adj.ContainsKey(to)) throw new Exception($"The edge from {Name} to {to} already exist.");
            Adj.Add(to, weight);
        }

        public void DeleteEdge(Vertex to)
        {
            Adj.Remove(to);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
