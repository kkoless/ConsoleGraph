using System;
namespace GraphProject
{
    public class Edge
    {
        private float weight;
        private Vertex startVertex;
        private Vertex endVertex;

        public Edge(Vertex startVertex, Vertex endVertex, double weight = 1)
        {
            this.weight = (float)weight;
            this.startVertex = startVertex;
            this.endVertex = endVertex;
        }

        public void setStartVertex(Vertex startVertex)
        {
            this.startVertex = startVertex;
            this.startVertex.addAdjacentVertex(this.endVertex);
        }
        public void setEndVertex(Vertex endVertex)
        {
            this.endVertex.addAdjacentVertex(this.startVertex);
            this.endVertex = endVertex;
        }

        public void setWeight(float newWeight) { this.weight = newWeight; }
        public float getWeight() { return weight; }

        public Vertex getStartVertex() { return startVertex; }
        public Vertex getEndVertex() { return endVertex; }

        //private void addAdjacentVertex()
        //{
        //    this.startVertex.addAdjacentVertex(this.endVertex);
        //    this.endVertex.addAdjacentVertex(this.startVertex);
        //}

        public override string ToString() { return $"({startVertex}); ({endVertex})"; }
    }
}

