using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Graphs
{
    // min-heap
    public class VertexPriorityQueue
    {
        private int parent(int i)
            => i / 2;
        private int left(int i)
            => 2 * i + 1;
        private int right(int i)
            => 2 * i + 2;

        private uint[] distances;
        private Vertex[] vertices;
        private Dictionary<Vertex, int> VtoI; // from Vertices to indeces in the dist
        private int _heapsize;
        //real size of the heap
        public int heapsize => _heapsize;

        public VertexPriorityQueue(Vertex[] vertices, Vertex start)
        {
            VtoI = new Dictionary<Vertex, int>();
            _heapsize = vertices.Length;
            this.vertices = vertices;
            distances = new uint[this.vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                VtoI.Add(vertices[i], i);
                if (start == vertices[i])
                {
                    VtoI[start] = i;
                    distances[i] = 0;
                }
                else
                {
                    distances[i] = uint.MaxValue;
                    VtoI[vertices[i]] = i;
                }
            }
            
            // building heap
            for (int i = _heapsize / 2; i > 0; i--)
            {
                Heapify(i);
            }
        }

        private void Swap(int i, int j)
        {
            VtoI[vertices[i]] = j;
            VtoI[vertices[j]] = i;
            
            uint dt = distances[i];
            distances[i] = distances[j];
            distances[j] = dt;

            Vertex vt = vertices[i];
            vertices[i] = vertices[j];
            vertices[j] = vt;
        }
        
        // Heap-functions

        private void Heapify(int i)
        {
            int l = left(i), r = right(i);
            
            //find min of the i, l, r
            int min = i;
            if (l < heapsize)
            {
                if (distances[l] < distances[i])
                {
                    min = l;
                }
            }

            if (r < heapsize)
            {
                if (distances[r] < distances[min])
                {
                    min = r;
                }
            }

            if (min != i)
            {
                Swap(i, min);
                Heapify(min);
            }
        }

        public void DecreaseKey(Vertex v, uint d)
        {
            int i = VtoI[v];
            if(d > distances[i])
                throw new Exception("Error in decreasing key");

            distances[i] = d;
            while (i > 0 && distances[parent(i)] > distances[i])
            {
                Swap(i, parent(i));
                i = parent(i);
            }

        }

        public KeyValuePair<Vertex, uint> ExtractMin()
        {
            if(heapsize < 1)
                throw new Exception("Heap underflow");
            
            KeyValuePair<Vertex, uint> res = new KeyValuePair<Vertex, uint>(vertices[0], distances[0]);
            vertices[0] = vertices[heapsize - 1];
            distances[0] = distances[heapsize - 1];
            VtoI.Remove(res.Key);
            VtoI[vertices[heapsize - 1]] = 0;
            _heapsize--;
            Heapify(0);
            
            return res;
        }

        public uint this[Vertex v]
        {
            get { return distances[VtoI[v]]; }
        }
    }
}
