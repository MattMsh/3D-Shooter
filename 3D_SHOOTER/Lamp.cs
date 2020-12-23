using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace _3D_SHOOTER
{
    public class Lamp
    {
        private readonly int _mainObject;
        private readonly int _vertexBufferObject;
        private readonly float[] _vertices;
        public readonly Vector3 LightColor;
        public Vector3 Pos;

        public Lamp(Vector3 pos, Vector3 lightColor, Shader lampShader, float radius)
        {
            Pos = pos;
            LightColor = lightColor;

            _vertices = CreateSphereVertices(radius);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            _mainObject = GL.GenVertexArray();
            GL.BindVertexArray(_mainObject);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            var positionLocation = lampShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            lampShader.SetVector3("lightColor", lightColor);
        }

        public void Show(Camera camera, Shader lampShader)
        {
            GL.BindVertexArray(_mainObject);

            lampShader.Use();

            var lampMatrix = Matrix4.Identity;
            lampMatrix *= Matrix4.CreateScale(1);
            lampMatrix *= Matrix4.CreateTranslation(Pos);

            lampShader.SetMatrix4("model", lampMatrix);
            lampShader.SetMatrix4("view", camera.GetViewMatrix());
            lampShader.SetMatrix4("projection", camera.GetProjectionMatrix());

            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 6);
        }

        public void Dispose()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_mainObject);
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
