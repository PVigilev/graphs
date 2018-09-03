using System;

namespace Graphs
{
    // min-heap
    public class VertexPriorityQueue
    {
        private Vertex[] vertices;
        private uint[] dist;
        private int _heapsize;

        //real size of the heap
        public int heapsize => _heapsize;


        public VertexPriorityQueue(Vertex[] vertices, uint[] dist)
        {
            if(vertices.Length != dist.Length)
                throw new Exception("Something went wrong!");
            _heapsize = vertices.Length;
            this.vertices = vertices;
            this.dist = dist;

            // build min-heap;
            for (int i = heapsize / 2; i >= 0; i--)
            {
                Heapify(i);
            }
        }

        private int parent(int i)
            => i / 2;
        private int left(int i)
            => 2 * i + 1;
        private int right(int i)
            => 2 * i + 2;

        private void Heapify(int i)
        {
            int l = left(i), r = right(i);
            int min;
            if (l < heapsize && dist[l] < dist[i])
                min = l;
            else min = i;
            if (r < heapsize && dist[r] < dist[min])
                min = r;
            if (min != i)
            {
                // swap in dist
                uint tmp = dist[i];
                dist[i] = dist[min];
                dist[min] = tmp;
                // swap in vertices
                Vertex t = vertices[i];
                vertices[i] = vertices[min];
                vertices[min] = t;
                Heapify(min);
            }


        }

        public struct Pair
        {
            public Vertex v { get; }
            public uint d { get; set; }
            public Pair(Vertex v, uint d)
            {
                this.v = v;
                this.d = d;
            }
        }

        public Pair ExtractMin()
        {
            if(heapsize == 0)
                throw new Exception("Queue is empty");
            Pair max = new Pair(vertices[0], dist[0]);
            dist[0] = dist[heapsize - 1];
            vertices[0] = vertices[heapsize - 1];
            _heapsize--;
            Heapify(0);
            return max;
        }

        private void DecreaseKey(int i, uint d)
        {
            if (d > dist[i])
                throw new Exception($"New distance is greater than current ({d} > {dist[i]})");
            dist[i] = d;
            for (; i > 0 && dist[parent(i)] > dist[i]; i = parent(i))
            {
                // swap in dist
                uint tmp = dist[i];
                dist[i] = dist[parent(i)];
                dist[parent(i)] = tmp;
                // swap in vertices
                Vertex t = vertices[i];
                vertices[i] = vertices[parent(i)];
                vertices[parent(i)] = t;
            }
        }

        public void DecreaseKey(Pair v)
        {
            for (int i = 0; i < heapsize; i++)
            {
                if (vertices[i] == v.v)
                {
                    DecreaseKey(i, v.d);
                    break;
                }
            }

        }

        public Pair? this[Vertex v]
        {
            get
            {
                for (int i = 0; i < heapsize; i++)
                {
                    if (vertices[i] == v)
                    {
                        return new Pair(v, dist[i]);
                    }
                }

                return null;
            }
        }
    }
}
