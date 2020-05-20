using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace Komiwojazer
{
    /*  [Sztuczna Inteligencja LAB]
         Damian Kaniewski - 291565 - IS IIIr. 
         Problem Komiwojażera:
            - Algorytm Nearest Neighbour    [X]
            - Algorytm Annealing            [X]
            - Algorytm A*                   [ ]
     */
    
    class Komiwojazer
    {
       
        static int NodeAmount = Defined.nodeAmount;
        Random rnd = new Random();

        public void Random(Random random)
        {
            rnd = random;
        }
        void nearest_neighbour(int[,]graph, int startNode)
        {
            Console.WriteLine("=========================================================================== ");
            Console.WriteLine("     \t\t\t--Algorytm Nearest Neighbour--");
            Console.WriteLine("===========================================================================");
            string path = "Ścieżka: ";

            bool[] visited = new bool[NodeAmount];
            for (int i = 0; i < NodeAmount; i++)
            {
                visited[i] = false;
            }

            int currentNode = startNode;
            int minCost = int.MaxValue;
            visited[currentNode] = true;
            int tmp = currentNode;
            int currentCost = 0;
            int endCost = 0;

            for(int j=0;j< NodeAmount-1;j++)
            {

                for (int i = 0; i < NodeAmount; i++)
                {
                    if (visited[i] == false && graph[currentNode, i] < minCost && graph[currentNode, i] != 0)
                    {
                        minCost = graph[currentNode, i];
                        tmp = i;
                    }
                }
                Console.WriteLine("Koszt drogi " + currentNode + " --> " + tmp + " = " + graph[currentNode, tmp]);
                path += currentNode + " --> ";
                currentCost += graph[currentNode, tmp];
                Console.WriteLine("Aktualny koszt: " + currentCost);
                currentNode = tmp;
                visited[currentNode] = true;
                minCost = int.MaxValue;
            }
            path += currentNode;
            endCost = currentCost + graph[currentNode, startNode];
            Console.WriteLine("Koszt drogi " + currentNode + " --> " + startNode + " = " + graph[currentNode, startNode]);
            Console.WriteLine("===========================================================================");
            Console.WriteLine("\t\t\tKoszt Koncowy: " + endCost);
            Console.WriteLine("\t\t\t" + path + " --> " + startNode);
            Console.WriteLine("===========================================================================");

        }          
                            
        void permutationFunc(List<int> CurrentNodeList, out List<int> newNodeList)
        {
            newNodeList = new List<int>(CurrentNodeList);

            int i1 = (int)(rnd.Next(CurrentNodeList.Count));
            int i2 = (int)(rnd.Next(CurrentNodeList.Count));

            int aux = newNodeList[i1];
            newNodeList[i1] = newNodeList[i2];
            newNodeList[i2] = aux;
        }
        int sumDistance(int[,] graph, List<int> NodeList)
        {
            int distance = 0;
            for (int i = 0; i < NodeList.Count - 1; i++)
            {
                distance += graph[NodeList[i], NodeList[i + 1]];
            }
            distance += graph[NodeList[0], NodeList[NodeList.Count - 1]];

            return distance;
        }           
        void annealing(int [,] graph)
        {
            Console.WriteLine("===========================================================================");
            Console.WriteLine("            \t\t--Algorytm Annealing--");
            Console.WriteLine("===========================================================================");


            List<int> CurrentNodeList = new List<int>();
            List<int> newNodeList = new List<int>();
            
            for (int i = 0; i < NodeAmount; i++)
            {
                CurrentNodeList.Add(i);
                newNodeList.Add(i);
            }

            int iteration = -1;            
            double proba;
            double alpha = 0.999;
            double temperature = 400.0;
            double epsilon = 0.01;
            int delta;
            int distance = sumDistance(graph,newNodeList);


            while (temperature > epsilon)
            {
                iteration++;

                permutationFunc(CurrentNodeList, out newNodeList);

                delta = sumDistance(graph,newNodeList) - distance;

                if (delta < 0)
                {
                    CurrentNodeList = newNodeList;
                    distance = delta + distance;
                }
                else
                {
                    proba = rnd.NextDouble();
                    if (proba < Math.Exp(-delta / temperature))
                    {
                        CurrentNodeList = newNodeList;
                        distance = delta + distance;
                    }
                }
                temperature *= alpha;
                #region Drukowanie
                if (iteration % 400 == 0) // drukujemy co 400 iteracji
                {
                    
                    if (iteration == 0)
                    {
                        
                           Console.Write("\nIteracja = " + iteration + "  \t\tKoszt = " + distance + ",\tTemperatura = " + Math.Round(temperature, 5));
                        for (int i =0;i<newNodeList.Count;i++)
                        {
                            if (i == newNodeList.Count - 1)
                            { Console.Write(newNodeList[i] + " -- > " + newNodeList.First());}
                            else
                            { if (i == 0) { Console.Write("\tPath: "); };
                                Console.Write(newNodeList[i] + " -- > ");                                
                            }                                       
                        }     
                    }
                    else
                    {
                        Console.Write("\nIteracja = " + iteration + "  \tKoszt = "  + distance + ",\tTemperatura = " + Math.Round(temperature, 5) );
                        for (int i = 0; i < newNodeList.Count; i++)
                        {
                            if (i == newNodeList.Count - 1)
                            {Console.Write(newNodeList[i] + " -- > " + newNodeList.First()); }
                            else
                            { if (i == 0) { Console.Write("\tPath: "); };
                                Console.Write(newNodeList[i] + " -- > ");
                            }
                        }
                    }
                }
                #endregion

            }
            Console.WriteLine("\n===========================================================================");
            Console.WriteLine("     \t\t\tNajmniejszy Koszt = " + distance);
            Console.WriteLine("===========================================================================");
        }
        static class Defined
        {
            public const int startNode = 2;
            public const int nodeAmount = 4;
        }

        public static void Main()
        {
            
            //Przykładowy graf z 6 nodami. Poniżej Macierz Sąsiedztwa 
            int[,] graph2 = new int[,] {     /*0  1  2  3  4  5*/
            /* 0  */                        { 0, 2, 3, 2, 1, 5 },
            /* 1 */                         { 2, 0, 6, 2, 5, 1 },
            /* 2  */                        { 3, 6, 0, 3, 2, 7 },
            /* 3  */                        { 2, 2, 3, 0, 5, 1 },
            /* 4  */                        { 1, 5, 2, 5, 0, 9 },
            /* 5  */                        { 5, 1, 7, 1, 9, 0 },};
            //Przykładowy graf z 4 nodami. Poniżej Macierz Sąsiedztwa 
            int[,] graph = new int[,] {     /*0  1  2  3  
                                  /* 0 */   { 0, 2, 3, 5},
                                  /* 1 */   { 2, 0, 6, 1},
                                  /* 2 */   { 3, 6, 0, 7},
                                  /* 3 */   { 5, 1, 7, 0},};
            
            Komiwojazer Algorytm = new Komiwojazer();

            Algorytm.nearest_neighbour(graph,Defined.startNode);
            Algorytm.annealing(graph);
           
            
        }
    }
}


 
