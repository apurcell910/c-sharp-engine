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
                if (CrossProduct(vertexList[GetPrevIndex(i)], vertexList[i], vertexList[GetNextIndex(i)]) == 0)
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
            float crossProd = CrossProduct(vertexList[vert1], vertexList[vert2], vertexList[vert3]);
            if ((crossProd > 0 && clockwise) || (crossProd < 0 && !clockwise))
            {
                return false;
            }
            
            //Check if any point is inside the triangle formed from this vertex
            for (int i = 0; i < vertexList.Count; i++)
            {
                //No point checking the vertices of the current triangle
                if (i != vert1 && i != vert2 && i != vert3)
                {
                    if (PointInTriangle(vertexList[i], vertexList[vert1], vertexList[vert2], vertexList[vert3]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool PointInTriangle(Vector2 point, Vector2 tri1, Vector2 tri2, Vector2 tri3)
        {
            float crossProd1 = CrossProduct(tri1, point, tri2);
            float crossProd2 = CrossProduct(tri2, point, tri3);
            float crossProd3 = CrossProduct(tri3, point, tri1);

            if (clockwise)
            {
                return crossProd1 > 0 && crossProd2 > 0 && crossProd3 > 0;
            }

            return crossProd1 < 0 && crossProd2 < 0 && crossProd3 < 0;
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

        private float CrossProduct(Vector2 p1, Vector2 p2, Vector2 p3)
        {
            return (p2.X - p1.X) * (p3.Y - p1.Y) - (p3.X - p1.X) * (p2.Y - p1.Y);
        }

        public override bool IsTouching(Collider other)
        {
            throw new NotImplementedException();
        }
    }
}
