using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Graphs
{
    public class Commands
    {
        private Graph graph;
        private TextReader streamIn;
        private TextWriter streamOut;

        public Commands(string r = null, string w = null){
            if(r == null)
                streamIn = Console.In;
            else streamIn = new StreamReader(r);
            if(w == null)
                streamOut = Console.Out;
            else streamOut = new StreamWriter(w);

        }
        private void init(){
            bool d = true;
            if(streamIn == Console.In){
                if(streamOut == Console.Out){
                    streamOut.WriteLine("Create an undirected graph? (y/n)");
                    string a = streamIn.ReadLine();
                    if(a == "y") d = false;
                    else if(a == "n") d = true;
                    else throw new Exception("Wrong command");
                }
            }
            else{
                string a = streamIn.ReadLine();
                if(a == "undirected") d = false;
                else if(a == "directed") d = true;
                else throw new Exception("Wrong command");
            }
            graph = new Graph(d);
            streamOut.WriteLine($"Created new {(d ? "directed" : "undirected" )} graph.");
        }

        public void e(){
            init();
            string command;
            while((command = streamIn.ReadLine()) != null){
                command = command.ToLower();
                try{

                    if(command == "print"){
                        streamOut.WriteLine(graph.ToString());
                        continue;
                    }
                    if(command == "scc"){
                        streamOut.WriteLine(scc());
                        continue;
                    }
                    if(command == "minspantree"){
                        streamOut.WriteLine(minspantree());
                        continue;
                    }
                    if(command == "new"){
                        init();
                    }

                    string[] com = command.Split(' ');
                    switch(com[0]){
                        case "vertices":{
                            for(int i = 1; i <com.Length; i++){
                                vertex(com[i]);
                            }
                        } break;
                        case "delete-vertices":{
                            for(int i = 1; i <com.Length; i++){
                                deleteVertex(com[i]);
                            }
                        } break;
                        case "edge":{
                            if(com.Length == 4)
                                edge(com[1], com[2], com[3]);
                            else if(com.Length == 3)
                                    edge(com[1],com[2]);
                            else throw new Exception("Wrong number of arguments");
                        } break;
                        case "delete-edge":{
                            if(com.Length == 3)
                               deleteEdge(com[1],com[2]);
                            // isolate vertex
                            else if(com.Length == 2){
                                deleteVertex(com[1]);
                                vertex(com[1]);
                            }
                        } break;
                        case "path":{
                            streamOut.WriteLine(path(com[1], com[2]));
                        } break;
                        default: {
                            streamOut.WriteLine("Wrong command \"" + command + "\"");
                        } break;
                    }
                }
                catch(Exception ex){
                    streamOut.WriteLine(ex.Message);
                    streamOut.WriteLine(ex.StackTrace);
                }
            }
            if(streamOut != Console.Out)
                streamOut.Close();
        }

        private void vertex(string v){
            graph.AddVertex(v);
        }
        private void edge(string from, string to, string weight){
            graph.AddEdge(from, to, uint.Parse(weight));
        }
        private void edge(string from, string to){
            graph.AddEdge(from, to, 1);
        }
        private void deleteVertex(string v){
            graph.DeleteVertex(v);

        }
        private void deleteEdge(string from, string to){
            graph.DeleteEdge(from, to);
        }
        private string scc(){
            List<HashSet<string>> components = graph.StronglyConnectedComponents();
            string result = $"The graph has {components.Count} strongly connected components\n";
            for(int i = 0; i < components.Count; i++){
                result += $"{(i+1)}. ";
                foreach(string vname in components[i]){
                    result += vname + " ";
                }
                result += "\n";
            }
            return result;
        }
        private string path(string from, string to){
            LinkedList<string> path;
            uint d = graph.GetShortestPath(from, to, out path);
            if(d == uint.MaxValue)
                return $"{to} is not reachable from {from}";

            string result = $"Distance of the path from {from} to {to} = {d}\nPath = (";
            while (path.First != null){
                if(path.First.Next != null)
                    result += $"{path.First.Value}, ";
                else
                    result += $"{path.First.Value}";
                path.RemoveFirst();
            }
            result += ")\n";
            return result;
        }
        private string minspantree(){
            Graph mst = graph.Kruskal();
            return mst.ToString();
        }





    }
}
