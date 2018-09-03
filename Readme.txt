The program can start with 1, 2 or no arguments
The first argument may be the file with commands.
The second argument may be the file where the program write
the result of its work.
When the program starts with no arguments it works with console
for reading commands and write the result.

The first command in the file with commands must be the string 'directed' or
'undirected' without quotes. It marks if the graph is directed or undirected.

Commands:
new
Create a new graph

vertices name1 [name2 name3 ...]
Add vertices with name1, [name2, name3, ...] into the graph

edge from to [weight]
Connect vertices 'from' and 'to' by a new edge with weight 'weight'
by default weight=1


delete-vertices name1 [name2 name3 ...]
Remove vertices from the graph

delete-edge from to
Delete the edge that connects vertices 'from' and 'to'
If the graph is undirected, delete also the edge 'to' 'from'

delete-edge vertex
Isolate the vertex in undirected graph

scc
Find and print strongly connected components

path from to
Find the shortest way in the graph from the vertex 'from' to the 'to'

minspantree
Find the minimum spanning tree of the graph and print it

print
Print the set of vertices and the set of edges


Example:
start program like this
mono ./Program.exe ./test.txt

content of the file.txt:
directed
vertices a b c d e
edge a b
edge a d
edge b e
edge b c
edge c e
edge e d
scc
path a b
delete-edge b
scc
path a b

output:
Created new directed graph.
The graph has 1 strongly connected components
1. c d e b a

Distance of the path from a to b = 1
Path = (a, b)

The graph has 2 strongly connected components
1. a d e c
2. b

b is not reachable from a
