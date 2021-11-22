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

        List<string> returnValues = new List<string>();

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

        public List<string> printAllPaths(int s, int d)
        {
            bool[] isVisited = new bool[v];
            List<int> pathList = new List<int>();

            pathList.Add(s);

            List<string> returnPossiblePaths = printAllPathsUtil(s, d, isVisited, pathList);

            return returnPossiblePaths;
        }

        private List<string> printAllPathsUtil(int u, int d,
                                       bool[] isVisited,
                                       List<int> localPathList)
        {
            if (u.Equals(d))
            {
                returnValues.Add(string.Join(" ", localPathList));
            }
            else
            {
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
            }
            return returnValues;
        }
    }
}
