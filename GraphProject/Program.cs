using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GraphProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph initGraph = new Graph();
            int userInput, choose;
            string path, fileName;
            float weight;
            int[] verticesIds, verticesIdsForEdge;

            string text =
                "0 - Выйти\n" +
                "1 - Загрузить данные о графе из текстового файла\n" +
                "2 - Сохранить граф в текстовый файл\n" +
                "3 - Добавить вершины в граф\n" +
                "4 - Добавить ребро в граф\n" +
                "5 - Удалить ребро из графа\n" +
                "6 - Узнать количество вершин в графе\n" +
                "7 - Узнать количество ребер в графе\n" +
                "8 - Проверить смежность двух вершин\n" +
                "9 - Узнать вес ребра\n" +
                "10 - Определить диаметр графа\n" +
                "11 - Узнать пустой граф или нет";

            do
            {
                Console.WriteLine("---------------");
                initGraph.printMatrix();
                Console.WriteLine("---------------");
                Console.WriteLine(text);
                Console.WriteLine("---------------");
                Console.Write("Введите число: ");
                userInput = Convert.ToInt32(Console.ReadLine());

                switch (userInput)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine(
                            "0 - для того, чтобы загрузить из матрицы смежности\n" +
                            "1 - для того, чтобы загрузить из списка ребер"
                            );
                        Console.WriteLine("---------------");
                        Console.Write("Введите число: ");
                        choose = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("---------------");
                        switch (choose)
                        {
                            case 0:
                                Console.Clear();
                                Console.Write("Укажите путь до файла: ");
                                path = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph = Graph.loadGraphFromFileOfAdjacencyMatrix(path);
                                break;
                            case 1:
                                Console.Clear();
                                Console.Write("Укажите путь до файла: ");
                                path = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph = Graph.loadGraphFromFileOfEdgeList(path);
                                break;
                        }
                        break;
                        
                    case 2:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine(
                            "0 - для того, чтобы сохранить граф в виде матрицы смежности\n" +
                            "1 - для того, чтобы сохранть граф в виде списка ребер"
                            );
                        Console.WriteLine("---------------");
                        Console.Write("Введите число: ");
                        choose = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("---------------");
                        switch (choose)
                        {
                            case 0:
                                Console.Clear();
                                Console.Write("Укажите путь до файла: ");
                                path = Console.ReadLine();
                                Console.WriteLine("Укажите название файла: ");
                                fileName = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph.saveMatrixGraphToFile(path, fileName);
                                break;
                            case 1:
                                Console.Clear();
                                Console.Write("Укажите путь до файла: ");
                                path = Console.ReadLine();
                                Console.WriteLine("Укажите название файла: ");
                                fileName = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph.saveListEdgesToFile(path, fileName);
                                break;
                        }
                        break;

                    case 3:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номера вершин, которые хотите добавить в граф");
                        verticesIds = Console.ReadLine().Split(' ').Select(item => Convert.ToInt32(item)).ToArray();
                        foreach(var vert in verticesIds)
                            initGraph.addVertex(new Vertex(vert));
                        break;

                    case 4:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номер начальной и конечной вершины ребра");
                        verticesIdsForEdge = Console.ReadLine().Split(' ').Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите вес ребра");
                        weight = float.Parse(Console.ReadLine());
                        initGraph.addEdge(new Vertex(verticesIdsForEdge.First()), new Vertex(verticesIdsForEdge.Last()), weight);
                        break;

                    case 5:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номер начальной и конечной вершины ребра");
                        verticesIdsForEdge = Console.ReadLine().Split(' ').Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                        initGraph.removeEdge(new Edge(new Vertex(verticesIdsForEdge.First()), new Vertex(verticesIdsForEdge.Last())));
                        break;

                    case 6:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine($"Количество вершин в графе: {initGraph.getVerticesCount()}");
                        break;

                    case 7:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine($"Количество ребер в графе: {initGraph.getEdgesCount()}");
                        break;

                    case 8:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номер первой и второй вершины");
                        verticesIds = Console.ReadLine().Split(' ').Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine(initGraph.checkAdjectVertices(new Vertex(verticesIds.First()), new Vertex(verticesIds.Last())) ? "Вершины смежны" : "Вершины не смежны");
                        break;

                    case 9:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номер начальной и конечной вершины ребра");
                        verticesIdsForEdge = Console.ReadLine().Split(' ').Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                        var edge = initGraph.checkEdgesContaints(new Vertex(verticesIdsForEdge.First()), new Vertex(verticesIdsForEdge.Last())).First();
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine($"Вес ребра: {edge.getWeight()}");
                        break;

                    case 10:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine($"Диаметр: {initGraph.getDiametr(initGraph.getMatrix(), initGraph.getVerticesCount())}");
                        break;

                    case 11:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine(initGraph.IsEmpty());
                        break;
                }

            } while (userInput != 0);
        }
    }
}

