using OpenTK;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;

namespace _3D_SHOOTER
{
    public class Sphere : Object
    {
        public float radius;

        public Sphere(float radius, Shader shader, Lamp lamp, Color4 color) : base (CreateSphereVertices(radius), shader, lamp, color)
        {
            this.radius = radius;
        }

        private static float[] CreateSphereVertices(float radius)
        {
            var res = Math.Min(Convert.ToInt32(Math.Ceiling(radius * radius)), 50);
            var unParsedVertices = new List<List<Vector3>>();
            var vertices = new List<float>();
            var i = 0;
            var j = 0;


            for (double psi = 0; psi - Math.PI <= 0.1; psi += Math.PI / res)
            {
                j = 0;
                var v = new List<Vector3>();

                for (double theta = 0; theta - 2 * Math.PI < 0.1; theta += Math.PI / res)
                {
                    var vertex = new Vector3(
                        (float)(radius * Math.Cos(theta) * Math.Sin(psi)),
                        (float)(radius * Math.Sin(theta) * Math.Sin(psi)),
                        (float)(radius * Math.Cos(psi)));
                    v.Add(vertex);
                    j++;
                }

                unParsedVertices.Add(v);
                i++;
            }

            for (var index = 0; index < i - 1; index++)
                for (var jIndex = 0; jIndex < j - 1; jIndex++)
                {
                    var v01 = unParsedVertices[index][jIndex];
                    var v02 = unParsedVertices[index + 1][jIndex];
                    var v03 = unParsedVertices[index + 1][jIndex + 1];

                    var l1 = v02 - v01;
                    var l2 = v03 - v01;
                    //Normals are the same for each triangle
                    var n = Vector3.Cross(l2, l1);
                    //First Vertex
                    vertices.Add(unParsedVertices[index][jIndex].X);
                    vertices.Add(unParsedVertices[index][jIndex].Y);
                    vertices.Add(unParsedVertices[index][jIndex].Z);
                    //First Normal
                    vertices.Add(n.X);
                    vertices.Add(n.Y);
                    vertices.Add(n.Z);
                    //Second Vertex
                    vertices.Add(unParsedVertices[index + 1][jIndex].X);
                    vertices.Add(unParsedVertices[index + 1][jIndex].Y);
                    vertices.Add(unParsedVertices[index + 1][jIndex].Z);
                    //Second Normal
                    vertices.Add(n.X);
                    vertices.Add(n.Y);
                    vertices.Add(n.Z);
                    //Third Vertex
                    vertices.Add(unParsedVertices[index + 1][jIndex + 1].X);
                    vertices.Add(unParsedVertices[index + 1][jIndex + 1].Y);
                    vertices.Add(unParsedVertices[index + 1][jIndex + 1].Z);
                    //Third Normal
                    vertices.Add(n.X);
                    vertices.Add(n.Y);
                    vertices.Add(n.Z);


                    //New Triangle
                    v01 = unParsedVertices[index][jIndex];
                    v03 = unParsedVertices[index + 1][jIndex + 1];
                    v02 = unParsedVertices[index][jIndex + 1];

                    l1 = v02 - v01;
                    l2 = v03 - v01;
                    //Normals are the same for each triangle
                    n = Vector3.Cross(l1, l2);

                    //First Vertex
                    vertices.Add(unParsedVertices[index][jIndex].X);
                    vertices.Add(unParsedVertices[index][jIndex].Y);
                    vertices.Add(unParsedVertices[index][jIndex].Z);
                    //First Normal
                    vertices.Add(n.X);
                    vertices.Add(n.Y);
                    vertices.Add(n.Z);
                    //Second Vertex
                    vertices.Add(unParsedVertices[index + 1][jIndex + 1].X);
                    vertices.Add(unParsedVertices[index + 1][jIndex + 1].Y);
                    vertices.Add(unParsedVertices[index + 1][jIndex + 1].Z);
                    //Second Normal
                    vertices.Add(n.X);
                    vertices.Add(n.Y);
                    vertices.Add(n.Z);
                    //Third Vertex
                    vertices.Add(unParsedVertices[index][jIndex + 1].X);
                    vertices.Add(unParsedVertices[index][jIndex + 1].Y);
                    vertices.Add(unParsedVertices[index][jIndex + 1].Z);
                    //Third Normal
                    vertices.Add(n.X);
                    vertices.Add(n.Y);
                    vertices.Add(n.Z);
                }

            return vertices.ToArray();
        }

    }
}
