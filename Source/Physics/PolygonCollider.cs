using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpSlugsEngine.Physics
{
    /// <summary>
    /// A <see cref="Collider"/> class containing an arbitrary simple polygon
    /// </summary>
    public class PolygonCollider : Collider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PolygonCollider"/> class with the given vertices
        /// </summary>
        /// <param name="vertices">Vertices defining the edge of the <see cref="PolygonCollider"/></param>
        public PolygonCollider(params Vector2[] vertices) // TODO: Check that vertices array contains a simple polygon
        {
            if (vertices == null || vertices.Length < 3)
            {
                throw new ArgumentException("Must have at least 3 vertices to create a polygon");
            }
            
            // Create vertex list
            List<Vector2> vertexList = vertices.ToList();

            // Trim redundant vertices
            for (int i = 0; i < vertexList.Count; i++)
            {
                if (Vector2.CrossProduct(vertexList.GetPrevItem<Vector2>(i), vertexList[i], vertexList.GetNextItem<Vector2>(i)) == 0)
                {
                    vertexList.RemoveAt(i);
                    i--;
                }
            }

            if (vertexList.Count < 3)
            {
                throw new ArgumentException("Less than 3 vertices after trimming redundancies");
            }

            VerticesInternal = vertexList.ToArray();

            // Determine which direction this vertex list goes around the polygon
            float area = 0;
            for (int i = 0; i < vertexList.Count; i++)
            {
                area += (vertexList.GetNextItem<Vector2>(i).X - vertexList[i].X) * (vertexList.GetNextItem<Vector2>(i).Y + vertexList[i].Y);
            }

            bool clockwise = area > 0;
            
            // Trim ears until all that's left is a triangle
            List<PTriangle> triangles = new List<PTriangle>();
            while (vertexList.Count > 3)
            {
                for (int i = 0; i < vertexList.Count; i++)
                {
                    if (VertexIsEar(vertexList, i, clockwise))
                    {
                        triangles.Add(new PTriangle(vertexList.GetPrevItem<Vector2>(i), vertexList[i], vertexList.GetNextItem<Vector2>(i)));
                        vertexList.RemoveAt(i);

                        // Recalculate clockwise/counterclockwise
                        area = 0;
                        for (int j = 0; j < vertexList.Count; j++)
                        {
                            area += (vertexList.GetNextItem<Vector2>(j).X - vertexList[j].X) * (vertexList.GetNextItem<Vector2>(j).Y + vertexList[j].Y);
                        }

                        clockwise = area > 0;

                        break;
                    }
                }
            }

            // Add remaining triangle
            triangles.Add(new PTriangle(vertexList[0], vertexList[1], vertexList[2]));

            // Remove redundant triangles
            triangles.RemoveAll(tri => tri.Area == 0f);

            if (triangles.Count == 0)
            {
                throw new ArgumentException("Given polygon has no area");
            }

            // Make triangles publicly viewable
            TrianglesInternal = triangles.ToArray();
        }

        /// <summary>
        /// Generates a random polygon collider with a roughly circular shape
        /// </summary>
        /// <param name="radius">The radius of the base ellipse to randomize</param>
        /// <returns>A new instance of the <see cref="PolygonCollider"/> class containing the randomized polygon</returns>
        public static PolygonCollider GenerateRandom(float radius)
        {
            try
            {
                Random rnd = new Random();

                PEllipse circle = new PEllipse(Vector2.Zero, radius, radius);

                List<Vector2> verts = new List<Vector2>();

                float multiplier = -1;
                for (int i = 0; i < circle.Triangles.Count; i++)
                {
                    if (multiplier == -1 || rnd.NextDouble() <= 0.33f)
                    {
                        multiplier = rnd.Next(8, 12) / 10f;
                    }
                    else
                    {
                        continue;
                    }

                    verts.Add(circle.Triangles[i].VertexTwo * multiplier);
                }

                return new PolygonCollider(verts.ToArray());
            }
            catch (Exception)
            {
                // It's possible for this to generate invalid polygons, but it should be very rare
                return GenerateRandom(radius);
            }
        }

        /// <summary>
        /// Checks whether or not a given vertex is an ear of the given polygon
        /// </summary>
        /// <param name="vertices">Vertices composing the edges of the polygon</param>
        /// <param name="vert">The vertex to check</param>
        /// <param name="clockwise">Indicates which direction the vertices list rotates around the polygon</param>
        /// <returns>A bool indicating if the vertex is an ear</returns>
        private bool VertexIsEar(List<Vector2> vertices, int vert, bool clockwise)
        {
            if (vert < 0 || vert >= vertices.Count)
            {
                throw new ArgumentOutOfRangeException("Argument vert out of range");
            }

            // Get vertices for the relevant triangle
            int vert1 = vertices.GetPrevIndex(vert);
            int vert2 = vert;
            int vert3 = vertices.GetNextIndex(vert);

            // Determine if the angle at vert is reflex
            float crossProd = Vector2.CrossProduct(vertices[vert1], vertices[vert2], vertices[vert3]);
            if ((crossProd > 0 && clockwise) || (crossProd < 0 && !clockwise))
            {
                return false;
            }

            // Check if any point is inside the triangle formed from this vertex
            PTriangle tri = new PTriangle(vertices[vert1], vertices[vert2], vertices[vert3]);
            for (int i = 0; i < vertices.Count; i++)
            {
                // No point checking the vertices of the current triangle
                if (i != vert1 && i != vert2 && i != vert3)
                {
                    if (tri.ContainsPoint(vertices[i]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
