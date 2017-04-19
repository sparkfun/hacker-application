/* maze_solve.cpp
Steven Mills */


/* this program processes a command-line maze and outputs the same maze data, and uses dijkstra's algorithm to find a path through said maze.
The command line maze is composed of three possibile data types:

ROWS X
COLS X
WALLS X Y

The rows and cols values contain the number of rows and columns in the maze.  The maze therefore consists of
rows*cols-1 numbers of squares.  With WALLS, the X and Y represent two tiles between which there is a wall,
X and Y can be any number between 0 and rows*cols-1, but they have to be adjacent for it to be a valid wall */

#include <iostream>
#include <string>
#include <stdlib.h>
#include <vector>
#include <algorithm>
using namespace std;

class Node {
	public:
		int id;
		vector <int> vdir; // valid directions to go
		vector <int> edges; // using this one for the walls
		int component;
};

class Graph {
	public:
		int rows,cols;
		vector <Node *> nodes;
		void Print();
		void Find_End(int index, int cn);
};

void Graph::Find_End(int index, int cn) {
	/* This function looks at what walls are adjacent
	   to the node and assigns the node a set of 
	   'valid directions' i.e. possible maze paths 
	   that don't hit a wall or the edge of the maze.

	   After it does that, it recursively traverses
	   all possible paths */

	Node *n;
	int i;
	vector <int>::iterator vit;

	n = nodes[index];
	if (n->component != -1) return;
	n->component = cn;

	if (index+cols < cols*rows) { // down
		vit = find(n->edges.begin(),n->edges.end(),index+cols);
		// is it in the list of walls?
		if (vit == n->edges.end()) n->vdir.push_back(index+cols);}
		// if not, add it to the list of possible directions

	if ((index+1)%cols != 0) { // right
		vit = find(n->edges.begin(),n->edges.end(),index+1);
		if (vit == n->edges.end()) n->vdir.push_back(index+1);} 

	if (index-1 >= 0 && (index-1)%cols != cols-1) {// up
		vit = find(n->edges.begin(),n->edges.end(),index-1);
		if (vit == n->edges.end()) n->vdir.push_back(index-1);}

	if (index-cols >= 0) { // left
		vit = find(n->edges.begin(),n->edges.end(),index-cols);
	if (vit == n->edges.end()) n->vdir.push_back(index-cols); }
	
	for (i = 0; i < n->vdir.size();i++) Find_End(n->vdir[i],cn+1);
}

void Graph::Print() {
int a,b;
Node *n;
vector <int> path;

n = nodes[(rows*cols)-1]; // end of the maze
a = n->component;
if (a == -1) return; // no valid solution

while (a > 0) {
for (b = 0;b < n->vdir.size();b++) {
	if (nodes[n->vdir[b]]->component == n->component-1) {
		path.push_back(n->id);
		n = nodes[n->vdir[b]];
		a = n->component;
		break;
	}}
}
path.push_back(0);

for (a = path.size()-1;a > -1;a--) cout << "PATH " << path[a] << endl;

}

main (int argc, char **argv) {
	string s;
	int i,n1,n2;
	Graph g;
	Node *n;

	cin >> s;
	if ( s != "ROWS") { cerr << "Bad maze file\n"; exit(1); }
	cin >> g.rows >> s;
	if ( s != "COLS") { cerr << "Bad maze file\n"; exit(1); }
	cin >> g.cols;
	cout << "ROWS " << g.rows << " COLS " << g.cols << endl;

	for (i = 0; i < g.rows*g.cols;i++) {
	n = new Node;
	n->component = -1;
	n->id = i;
	g.nodes.push_back(n);
	}

	while (!cin.fail()) {
	cin >> s >> n1 >> n2;
	if (!cin.fail()) {
		if (s != "WALL") { cerr << "Bad maze file\n"; exit(1); }
		g.nodes[n1]->edges.push_back(n2);
		g.nodes[n2]->edges.push_back(n1);
		cout << "WALL " << n1 << " " << n2 << endl;
		}
	}

	g.Find_End(0,0);
	g.Print();

}
