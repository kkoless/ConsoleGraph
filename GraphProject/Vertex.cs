using System;
using System.Collections.Generic;
using System.Linq;

namespace GraphProject
{
    public class Vertex
    {
        private int id;
        private string name;
        private List<Vertex> adjacentVertices = new List<Vertex>();

        public Vertex(int id, string name = "")
        {
            this.id = id;
            this.name = name;
        }

        public string getName() { return name; }
        public void setName(string newName) { this.name = newName; }

        public int getId() { return id; }

        public List<Vertex> getAdjacentVerticesList() { return this.adjacentVertices; }

        public void addAdjacentVertex(Vertex newVertex)
        {
            if (!checkAdjectVertex(newVertex))
                this.adjacentVertices.Add(newVertex);
        }

        public void removeAdjacentVertex(Vertex removeAdjVertex)
        {
            if (adjacentVertices.Contains(removeAdjVertex))
                adjacentVertices.Remove(removeAdjVertex);
        }

        public bool checkAdjectVertex(Vertex otherVertex)
        {
            var vertIds = this.adjacentVertices.Select(x => x.getId());
            return vertIds.Contains(otherVertex.getId());
        }

        public override string ToString() { return id.ToString(); }
    }
}

