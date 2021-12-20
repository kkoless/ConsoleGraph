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
            int userInput = 20, choose = 20, tmp = 0;
            string fileName;
            float weight;
            int[] verticesIds = Array.Empty<int>(), verticesIdsForEdge = Array.Empty<int>();
            string[] verticesIdsStr = Array.Empty<string>();

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
                "11 - Узнать пустой граф или нет\n" +
                "12 - Создать пустой граф";

            do
            {
                Console.WriteLine("---------------");
                initGraph.printMatrix();
                Console.WriteLine("---------------");
                Console.WriteLine(text);
                Console.WriteLine("---------------");
                Console.Write("Введите число: ");
                try { userInput = Convert.ToInt32(Console.ReadLine()); }
                catch {
                    Console.Clear();
                    Console.WriteLine("Некорректный ввод, повторите попытку");
                }
                    
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
                        try { choose = Convert.ToInt32(Console.ReadLine()); }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Некорректный ввод, повторите попытку");
                        }
                        Console.WriteLine("---------------");
                        switch (choose)
                        {
                            case 0:
                                Console.Clear();
                                Console.Write("Укажите название файла: ");
                                fileName = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph = Graph.loadGraphFromFileOfAdjacencyMatrix(fileName);
                                break;
                            case 1:
                                Console.Clear();
                                Console.Write("Укажите название файла: ");
                                fileName = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph = Graph.loadGraphFromFileOfEdgeList(fileName);
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
                        try { choose = Convert.ToInt32(Console.ReadLine()); }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Некорректный ввод, повторите попытку");
                        }
                        Console.WriteLine("---------------");
                        switch (choose)
                        {
                            case 0:
                                Console.Clear();
                                Console.WriteLine("Укажите название файла: ");
                                fileName = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph.saveMatrixGraphToFile(fileName);
                                break;
                            case 1:
                                Console.Clear();
                                Console.WriteLine("Укажите название файла: ");
                                fileName = Console.ReadLine();
                                Console.WriteLine("---------------");
                                initGraph.saveListEdgesToFile(fileName);
                                break;
                        }
                        break;

                    case 3:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номера вершин, которые хотите добавить в граф");
                        try {
                            verticesIdsStr = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            verticesIds = verticesIdsStr.Where(item => int.TryParse(item, out tmp)).Select(item => Convert.ToInt32(item)).ToArray();
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Некорректный ввод, повторите попытку");
                        }
                        
                        foreach (var vert in verticesIds)
                            initGraph.addVertex(new Vertex(vert));

                        break;

                    case 4:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номер начальной и конечной вершины ребра");
                        try
                        {
                            verticesIdsStr = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            verticesIdsForEdge = verticesIdsStr.Where(item => int.TryParse(item, out tmp)).Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                            Console.Clear();
                            Console.WriteLine("---------------");
                            Console.WriteLine("Введите вес ребра");
                            weight = float.Parse(Console.ReadLine());
                            if (weight == 0)
                                Console.WriteLine("Вес ребра не может равняться 0");
                            else
                                initGraph.addEdge(new Vertex(verticesIdsForEdge.First()), new Vertex(verticesIdsForEdge.Last()), weight);
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Некорректный ввод, повторите попытку");
                        }
                        break;

                    case 5:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номер начальной и конечной вершины ребра");
                        try
                        {
                            verticesIdsStr = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            verticesIdsForEdge = verticesIdsStr.Where(item => int.TryParse(item, out tmp)).Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                            initGraph.removeEdge(new Edge(new Vertex(verticesIdsForEdge.First()), new Vertex(verticesIdsForEdge.Last())));
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Некорректный ввод, повторите попытку");
                        }
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
                        try
                        {
                            Console.Clear();
                            Console.WriteLine("---------------");
                            verticesIdsStr = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            verticesIds = verticesIdsStr.Where(item => int.TryParse(item, out tmp)).Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                            Console.WriteLine(initGraph.checkAdjectVertices(new Vertex(verticesIds.First()), new Vertex(verticesIds.Last())) ? "Вершины смежны" : "Вершины не смежны");
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Некорректный ввод, повторите попытку");
                        }
                        break;

                    case 9:
                        Console.Clear();
                        initGraph.printMatrix();
                        Console.WriteLine("---------------");
                        Console.WriteLine("Введите номер начальной и конечной вершины ребра");
                        try
                        {
                            Console.Clear();
                            Console.WriteLine("---------------");
                            verticesIdsStr = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                            verticesIdsForEdge = verticesIdsStr.Where(item => int.TryParse(item, out tmp)).Select(item => Convert.ToInt32(item)).Take(2).ToArray();
                            var edge = initGraph.checkEdgesContaints(new Vertex(verticesIdsForEdge.First()), new Vertex(verticesIdsForEdge.Last())).First();
                            Console.Clear();
                            Console.WriteLine("---------------");
                            Console.WriteLine($"Вес ребра: {edge.getWeight()}");
                        }
                        catch
                        {
                            Console.Clear();
                            Console.WriteLine("Некорректный ввод, повторите попытку");
                        }
                        break;

                    case 10:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine($"Диаметр: {initGraph.getDiametr()}");
                        break;

                    case 11:
                        Console.Clear();
                        Console.WriteLine("---------------");
                        Console.WriteLine(initGraph.IsEmpty());
                        break;

                    case 12:
                        Console.Clear();
                        initGraph = new Graph();
                        break;
                }

            } while (userInput != 0);
        }
    }
}

