using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSlugsEngine.Physics
{
    public class PolygonCollider : Collider
    {
        private List<Vector2> vertexList;
        private bool clockwise;

        public Triangle[] Triangles { get; private set; }

        //TODO: Check that vertices array contains a simple polygon
        public PolygonCollider(Vector2[] vertices)
        {
            if (vertices == null || vertices.Length < 3)
            {
                throw new ArgumentException("Must have at least 3 vertices to create a polygon");
            }
            
            //Create vertex list
            vertexList = vertices.ToList();

            //Trim redundant vertices
            for (int i = 0; i < vertexList.Count; i++)
            {
                if (Vector2.CrossProduct(vertexList[GetPrevIndex(i)], vertexList[i], vertexList[GetNextIndex(i)]) == 0)
                {
                    vertexList.RemoveAt(i);
                    i--;
                }
            }

            if (vertexList.Count < 3)
            {
                throw new ArgumentException("Less than 3 vertices after trimming redundancies");
            }

            //Determine which direction this vertex list goes around the polygon
            float area = 0;
            for (int i = 0; i < vertexList.Count; i++)
            {
                area += (vertexList[GetNextIndex(i)].X - vertexList[i].X) * (vertexList[GetNextIndex(i)].Y + vertexList[i].Y);
            }

            clockwise = area > 0;
            
            //Trim ears until all that's left is a triangle
            List<Triangle> triangles = new List<Triangle>();
            while (vertexList.Count > 3)
            {
                for (int i = 0; i < vertexList.Count; i++)
                {
                    if (VertexIsEar(i))
                    {
                        triangles.Add(new Triangle(vertexList[GetPrevIndex(i)], vertexList[i], vertexList[GetNextIndex(i)]));
                        vertexList.RemoveAt(i);
                        break;
                    }
                }
            }

            //Add remaining triangle
            triangles.Add(new Triangle(vertexList[0], vertexList[1], vertexList[2]));

            //Remove redundant triangles
            triangles.RemoveAll(tri => tri.Area == 0f);

            if (triangles.Count == 0)
            {
                throw new ArgumentException("Given polygon has no area");
            }

            //Make triangles publicly viewable
            Triangles = triangles.ToArray();
        }

        private bool VertexIsEar(int vert)
        {
            if (vert < 0 || vert >= vertexList.Count)
            {
                throw new ArgumentOutOfRangeException("Argument vert out of range");
            }

            //Get vertices for the relevant triangle
            int vert1 = GetPrevIndex(vert);
            int vert2 = vert;
            int vert3 = GetNextIndex(vert);

            //Determine if the angle at vert is reflex
            float crossProd = Vector2.CrossProduct(vertexList[vert1], vertexList[vert2], vertexList[vert3]);
            if ((crossProd > 0 && clockwise) || (crossProd < 0 && !clockwise))
            {
                return false;
            }

            //Check if any point is inside the triangle formed from this vertex
            Triangle tri = new Triangle(vertexList[vert1], vertexList[vert2], vertexList[vert3]);
            for (int i = 0; i < vertexList.Count; i++)
            {
                //No point checking the vertices of the current triangle
                if (i != vert1 && i != vert2 && i != vert3)
                {
                    if (tri.ContainsPoint(vertexList[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private int GetPrevIndex(int i)
        {
            if (i < 0 || i >= vertexList.Count)
            {
                throw new ArgumentOutOfRangeException("Argument i out of range");
            }

            return i == 0 ? vertexList.Count - 1 : i - 1;
        }

        private int GetNextIndex(int i)
        {
            if (i < 0 || i >= vertexList.Count)
            {
                throw new ArgumentOutOfRangeException("Argument i out of range");
            }

            return i == vertexList.Count - 1 ? 0 : i + 1;
        }

        public override bool IsTouching(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}
