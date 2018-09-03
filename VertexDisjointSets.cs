using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{



    public class VertexDisjointSets
    {
        // Forest of disjoint sets

        public class Node
        {
            public Vertex vertex { get; }
            public Node parent { get; set; }
            public uint rank = 0;

            public Node(Vertex v)
            {
                vertex = v;
            }
        }
        HashSet<Node> Sets = new HashSet<Node>();

        // Make-Set
        public void MakeSet(Node n)
        {
            if (Sets.Contains(n)) return;
            n.parent = n;
            n.rank = 0;
            Sets.Add(n);
        }

        // Find-Set
        public Node FindSet(Node n)
        {
            if (n != n.parent)
                n.parent = FindSet(n.parent);
            return n.parent;
        }

        private void Link(Node x, Node y)
        {
            if (x.rank > y.rank)
            {
                y.parent = x;
            }
            else
            {
                x.parent = y;
                if (x.rank == y.rank)
                {
                    y.rank += 1;
                }
            }
        }

        public void Union(Node x, Node y)
        {
            Link(FindSet(x), FindSet(y));
        }

        public Node this[Vertex v]
        {
            get
            {
                foreach (Node node in Sets)
                {
                    if (node.vertex == v) return node;
                }

                return null;
            }
        }

    }

    public class Edge: IComparable<Edge>
    {
        public VertexDisjointSets.Node from { get; }
        public VertexDisjointSets.Node to { get; }
        public uint weight { get; }

        public Edge(VertexDisjointSets.Node f, VertexDisjointSets.Node t, uint w)
        {
            from = f;
            to = t;
            weight = w;
        }

        public int CompareTo(Edge edge)
        {
            return weight.CompareTo(edge.weight);
        }
    }
}
