using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastDeliveryServices
{
    public class Graph
    {
        private int v;

        private List<int>[] adjList;

        public Graph(int vertices)
        {

            this.v = vertices;

            initAdjList();
        }

        private void initAdjList()
        {
            adjList = new List<int>[v];

            for (int i = 0; i < v; i++)
            {
                adjList[i] = new List<int>();
            }
        }

        public void addEdge(int u, int v)
        {
            adjList[u].Add(v);
        }

        public List<int> printAllPaths(int s, int d)
        {
            bool[] isVisited = new bool[v];
            List<int> pathList = new List<int>();

            pathList.Add(s);

            return printAllPathsUtil(s, d, isVisited, pathList);
        }

        private List<int> printAllPathsUtil(int u, int d,
                                       bool[] isVisited,
                                       List<int> localPathList)
        {

            if (u.Equals(d))
            {
                Console.WriteLine(string.Join(" ", localPathList));
                return localPathList;
            }

            isVisited[u] = true;

            foreach (int i in adjList[u])
            {
                if (!isVisited[i])
                {
                    localPathList.Add(i);
                    printAllPathsUtil(i, d, isVisited,
                                      localPathList);

                    localPathList.Remove(i);
                }
            }

            isVisited[u] = false;
            return new List<int>();
        }
    }
}
