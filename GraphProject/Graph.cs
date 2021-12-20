using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace GraphProject
{
    public class Graph
    {
        private List<Vertex> vertices = new List<Vertex>();
        private List<Edge> edges = new List<Edge>();

        public bool IsEmpty() { return this.edges.Count == 0 ? true : false; }
        public int getVerticesCount() => vertices.Count;
        public int getEdgesCount() => edges.Count;

        private float[,] getMatrix()
        {
            var matrix = new float[vertices.Count, vertices.Count];

            foreach (var edge in edges)
            {
                var row = edge.getStartVertex().getId() - 1;
                var column = edge.getEndVertex().getId() - 1;

                matrix[row, column] = edge.getWeight();
            }

            foreach (var edge in edges)
            {
                var row = edge.getEndVertex().getId() - 1;
                var column = edge.getStartVertex().getId() - 1;

                matrix[row, column] = edge.getWeight();
            }

            return matrix;
        }

        public void saveMatrixGraphToFile(string fileName)
        {
            var matrix = getMatrix();
            var newPath = $"/Users/kirillkolesnikov/Projects/GraphProject/GraphProject/{fileName}.txt" ;

            using (StreamWriter w = new StreamWriter(newPath))
            {
                w.WriteLine(getVerticesCount());
                for (int row = 0; row < getVerticesCount(); row++)
                {
                    for (int column = 0; column < getVerticesCount(); column++)
                        w.Write(matrix[row, column] + "\t");

                    w.WriteLine();
                }
            }
        }

        public void saveListEdgesToFile(string fileName)
        {
            var table = new float[getEdgesCount(), 3];
            var newPath = $"/Users/kirillkolesnikov/Projects/GraphProject/GraphProject/{fileName}.txt";

            using (StreamWriter w = new StreamWriter(newPath))
            {
                w.WriteLine(getVerticesCount());
                for (int row = 0; row < edges.Count; row++)
                {
                    var verticesOfEdge = new Vertex[2];
                    verticesOfEdge[0] = edges[row].getStartVertex();
                    verticesOfEdge[1] = edges[row].getEndVertex();

                    for (int column = 0; column < verticesOfEdge.Length + 1; column++)
                    {
                        table[row, column] = (float)(column != 2 ? verticesOfEdge[column].getId() : edges[row].getWeight());
                        w.Write(table[row, column] + "\t");
                    }
                    w.WriteLine();
                }
            }
        }

        public void addVertex(Vertex newVertex) { checkVerticesContains(newVertex); }

        public void addEdge(Vertex startVertex, Vertex endVertex, double weight = 1)
        {
            Vertex firstVert = startVertex, secondVert = endVertex;
            var vertIds = this.vertices.Select(x => x.getId());

            if (vertIds.Contains(firstVert.getId()) && vertIds.Contains(secondVert.getId()))
            {
                firstVert = this.vertices.Find(x => x.getId() == firstVert.getId());
                secondVert = this.vertices.Find(x => x.getId() == secondVert.getId());
            }

            Edge newEdge = new Edge(firstVert, secondVert, (float)weight);
            Edge editEdge = checkEdgesContaints(firstVert, secondVert).First();
            Edge revertEdge = checkEdgesContaints(firstVert, secondVert).Last();

            if (!this.edges.Contains(revertEdge) && !this.edges.Contains(editEdge) || (revertEdge != null && editEdge != null))
                this.edges.Add(newEdge);
        }

        public void removeEdge(Edge removeEdge)
        {
            var startVertex = removeEdge.getStartVertex();
            var endVertex = removeEdge.getEndVertex();

            var verts = checkVerticesContains(startVertex, endVertex);

            Edge editEdge = checkEdgesContaints(verts.First(), verts.Last()).First();
            Edge reverseEdge = checkEdgesContaints(verts.First(), verts.Last()).Last();

            if (edges.Contains(editEdge))
            {
                edges.Remove(editEdge);
                verts.First().removeAdjacentVertex(editEdge.getEndVertex());
                verts.Last().removeAdjacentVertex(editEdge.getStartVertex());
            }
            if (reverseEdge != null)
            {
                edges.Remove(reverseEdge);
                verts.First().removeAdjacentVertex(reverseEdge.getEndVertex());
                verts.Last().removeAdjacentVertex(reverseEdge.getStartVertex());
            }
            else
                Console.WriteLine("Ошибка! В графе нет данного ребра");
        }
        
        // Проверка на смежность вершин
        public bool checkAdjectVertices(Vertex firstVertex, Vertex secondVertex)
        {
            var vertIds = this.vertices.Select(x => x.getId());
            bool result = false;

            if (vertIds.Contains(firstVertex.getId()) && vertIds.Contains(secondVertex.getId()))
            {
                Vertex firstVert = this.vertices.Find(x => x.getId() == firstVertex.getId());
                Vertex secondVert = this.vertices.Find(x => x.getId() == secondVertex.getId());
                result = firstVert.checkAdjectVertex(secondVert) &&
                secondVert.checkAdjectVertex(firstVert);
            }

            return result;
        }

        private List<Vertex> checkVerticesContains(params Vertex[] vertices)
        {
            int[] vertIds = Array.Empty<int>();
            try { vertIds = this.vertices.Select(x => x.getId()).ToArray(); }
            catch {}

            List<Vertex> result = new List<Vertex>();
            foreach (Vertex vert in vertices) {         
                if (!vertIds.Contains(vert.getId()) || vertIds.Length == 0)
                {
                    this.vertices.Add(vert);
                    result.Add(vert);
                }
                else
                    result.Add(this.vertices.Find(x => x.getId() == vert.getId()));
            }
            return result;
        }

        public Edge[] checkEdgesContaints(Vertex firstVertex, Vertex secondVertex)
        {
            Edge[] result = new Edge[2];
            var verts = checkVerticesContains(firstVertex, secondVertex);
            Edge reverseEdge = null, editEdge = null;
            try
            {
                editEdge = this.edges.Find(x => x.getStartVertex().getId() == verts.First().getId() && x.getEndVertex().getId() == verts.Last().getId());
                reverseEdge = this.edges.Find(x => x.getStartVertex().getId() == verts.Last().getId() && x.getEndVertex().getId() == verts.First().getId());
                result[0] = editEdge;
                result[1] = reverseEdge;
            }
            catch
            {
                result[0] = editEdge;
                result[1] = reverseEdge;
            }
            return result;
        }

        //------------------------------------------------------------------

        // В виде списка ребер
        static public Graph loadGraphFromFileOfEdgeList(string fileName)
        {
            Graph newGraph = new Graph();
            parseGraphFromFileOfEdgeList(fileName, newGraph);
            return newGraph;
        }

        static private Graph parseGraphFromFileOfEdgeList(string fileName, Graph editGraph)
        {
            var path = $"/Users/kirillkolesnikov/Projects/GraphProject/GraphProject/{fileName}.txt";
            string[] dataFromFile = File.ReadAllLines(path);
            configureGraphOfEdgeList(dataFromFile, editGraph);
            return editGraph;
        }

        static private void configureGraphOfEdgeList(string[] dataFromFile, Graph editGraph)
        {
            string[] numbers;
            List<string[]> resVertex = new List<string[]>();
            List<string> edgeWeight = new List<string>();
            List<Vertex> newVertices = new List<Vertex>();
            foreach (string line in dataFromFile)
            {
                if (line != dataFromFile.First())
                {
                    numbers = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    var vert = numbers.Take(2);
                    var weight = numbers.Last();
                    resVertex.Add(vert.ToArray());
                    edgeWeight.Add(weight);
                }
            }

            for (int vertexIndex = 0; vertexIndex <= resVertex.Count - 1; vertexIndex++)
            {
                Vertex vert1, vert2;
                if (newVertices.Count == 0)
                {
                    vert1 = new Vertex(Convert.ToInt32(resVertex[vertexIndex].First()));
                    vert2 = new Vertex(Convert.ToInt32(resVertex[vertexIndex].Last()));
                    Console.WriteLine(vert1.ToString());
                    Console.WriteLine(vert2.ToString());
                    newVertices.Add(vert1);
                    newVertices.Add(vert2);
                }
                else {
                    vert1 = checkVerticesOfEdgeList(newVertices, Convert.ToInt32(resVertex[vertexIndex].First()));
                    vert2 = checkVerticesOfEdgeList(newVertices, Convert.ToInt32(resVertex[vertexIndex].Last()));
                    vert1.addAdjacentVertex(vert2);
                    vert2.addAdjacentVertex(vert1);
                }

                editGraph.addEdge(vert1, vert2, float.Parse(edgeWeight[vertexIndex]));
            }
        }

        static private Vertex checkVerticesOfEdgeList(List<Vertex> vertices, int id)
        {
            Vertex flagVert;
            try { flagVert = vertices.Where(item => item.getId() == id).First(); }
            catch
            {
                flagVert = new Vertex(id);
                vertices.Add(flagVert);
            }
            return flagVert;
        }

        //------------------------------------------------------------------

        // В виде матрицы смежности
        static public Graph loadGraphFromFileOfAdjacencyMatrix(string fileName)
        {
            Graph newGraph = new Graph();
            parseGraphFromFileOfAdjacencyMatrix(fileName, newGraph);
            return newGraph;
        }

        static private Graph parseGraphFromFileOfAdjacencyMatrix(string fileName, Graph editGraph)
        {
            var path = $"/Users/kirillkolesnikov/Projects/GraphProject/GraphProject/{fileName}.txt";
            var dataFromFile = File.ReadAllLines(path);
            configureGraphOfOfAdjacencyMatrix(dataFromFile, editGraph);
            return editGraph;
        }

        static private void configureGraphOfOfAdjacencyMatrix(string[] dataFromFile, Graph editGraph)
        {
            var matrix = new string[dataFromFile.Length - 1, dataFromFile.Length - 1];

            for (int row = 0; row < dataFromFile.Length - 1; row++)
            {
                Vertex firstVert = new Vertex(row + 1);
                for (int column = 0; column < dataFromFile.Length - 1; column++)
                {
                    Vertex secondVert = new Vertex(column + 1);
                    float weight = float.Parse(dataFromFile[row + 1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[column]);
                    matrix[row, column] = weight.ToString();
                    if (weight != 0)
                        editGraph.addEdge(firstVert, secondVert, weight);
                }
            }
        }

        //-----------------------

        // Алгоритм Флойда-Уолшера
        private void floyd(float[,] matrix, int matrixSize)
        {
            for (int i = 0; i < matrixSize; i++)
                matrix[i, i] = 0; 

            for (int k = 0; k < matrixSize; ++k)
                for (int i = 0; i < matrixSize; ++i)
                    for (int j = 0; j < matrixSize; ++j)
                        if (matrix[i, k] != 0 && matrix[k, j] != 0 && i != j)
                            if (matrix[i, k] + matrix[k, j] < matrix[i, j] || matrix[i, j] == 0)
                                matrix[i, j] = matrix[i, k] + matrix[k, j];
        }

        public float getDiametr()
        {
            float diametr = 0;
            var matrixSize = getVerticesCount();
            var matrix = getMatrix();
            float[] e = new float[matrixSize];

            floyd(matrix, matrixSize);

            // Нахождение эксцентриситета
            for (int i = 0; i < matrixSize; i++)
                for (int j = 0; j < matrixSize; j++)
                    e[i] = Math.Max(e[i], matrix[i, j]);

            for (int i = 0; i < matrixSize; i++)
                diametr = Math.Max(diametr, e[i]);

            return diametr;
        }

        public void printMatrix()
        {
            var matrix = getMatrix();

            for (int row = 0; row < getVerticesCount(); row++)
            {
                for (int column = 0; column < getVerticesCount(); column++)
                    Console.Write(matrix[row, column] + "\t");

                Console.WriteLine();
            }
        }
    }
}