using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace _3D_SHOOTER
{
    public class Object
    {
        protected readonly bool textured = false;
        protected bool DrawRelativeToCamera = false;
        protected readonly Color4 _color;
        protected readonly Lamp _lamp;
        protected readonly Texture _texture;
        protected readonly Shader _shader;
        protected readonly int _mainObject;
        protected readonly int _vertexBufferObject;
        protected readonly float[] _vertices;
        public Vector3 _pos;
        public float _rotX, _rotY, _rotZ;
        protected float _scale = 1.0f;
        protected Matrix4 translation;
        protected Matrix4 rotation;
        protected Matrix4 scale;

        #region Constructors

        public Object(string path, Shader lightingShader, Lamp lamp, Color4 col)
        {
            _vertices = LoadObj(path);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            _mainObject = GL.GenVertexArray();
            GL.BindVertexArray(_mainObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            var positionLocation = lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var normalLocation = lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float),
                3 * sizeof(float));
            _rotX = 0.0f;
            _rotY = 0.0f;
            _rotZ = 0.0f;
            _pos = new Vector3(0.0f, 0.0f, 0.0f);
            _shader = lightingShader;
            _lamp = lamp;
            _color = col;
        }

        public Object(float[] vertices, Shader lightingShader, Lamp lamp, Color4 col)
        {
            _vertices = vertices;

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            _mainObject = GL.GenVertexArray();
            GL.BindVertexArray(_mainObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            var positionLocation = lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);

            var normalLocation = lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float),
                3 * sizeof(float));
            _rotX = 0.0f;
            _rotY = 0.0f;
            _rotZ = 0.0f;
            _pos = new Vector3(0.0f, 0.0f, 0.0f);
            _shader = lightingShader;
            _lamp = lamp;
            _color = col;
        }

        public Object(string path, Shader lightingShader, Lamp lamp, string texturePath)
        {
            _vertices = LoadObjTextured(path);

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            _mainObject = GL.GenVertexArray();
            GL.BindVertexArray(_mainObject);

            var positionLocation = lightingShader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);

            var normalLocation = lightingShader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float),
                3 * sizeof(float));

            var textureLocation = lightingShader.GetAttribLocation("aTexture");
            GL.EnableVertexAttribArray(textureLocation);
            GL.VertexAttribPointer(textureLocation, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float),
                6 * sizeof(float));

            _texture = new Texture(texturePath, TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            _texture.Use();
            textured = true;

            _rotX = 0.0f;
            _rotY = 0.0f;
            _rotZ = 0.0f;
            _pos = new Vector3(0.0f, 0.0f, 0.0f);
            _shader = lightingShader;
            _lamp = lamp;
        }

        #endregion

        public void Show(Camera camera)
        {
            Update();
            GL.BindVertexArray(_mainObject);
            if (textured)
            {
                _texture.Use();
            }
            _shader.Use();

            Matrix4 model = Matrix4.Identity;
            
            if (DrawRelativeToCamera)
            {
                Matrix4 invViewMat = Matrix4.Invert(camera.GetViewMatrix());
                model = scale * rotation * translation * invViewMat;
            }
            else
            {
                model = translation * rotation * scale;
            }

            _shader.SetMatrix4("model", model);
            _shader.SetMatrix4("view", camera.GetViewMatrix());
            _shader.SetMatrix4("projection", camera.GetProjectionMatrix());
            
            if (!textured)
            {
                _shader.SetVector4("objectColor", new Vector4(_color.R, _color.G, _color.B, _color.A));
            } 
            _shader.SetVector3("lightColor", _lamp.LightColor);
            _shader.SetVector3("lightPos", _lamp.Pos);

            if (textured)
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 8);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length / 6);
            }
        }

        protected void Update()
        {
            translation = Matrix4.CreateTranslation(_pos);
            rotation = Matrix4.CreateRotationX(_rotX) * Matrix4.CreateRotationY(_rotY) * Matrix4.CreateRotationZ(_rotZ);
            scale = Matrix4.CreateScale(_scale);
        }

        #region For Debug
        private void DrawAxis()
        {
            GL.PushMatrix();
            // Draw lines
            GL.Begin(PrimitiveType.Lines);
            //Draw X axis
            GL.Color3(1, 0, 0);
            GL.Vertex3(_pos);
            GL.Vertex3(_pos + new Vector3(30, 0, 0));
            //Draw Y axis
            GL.Color3(0, 1, 0);
            GL.Vertex3(_pos);
            GL.Vertex3(_pos + new Vector3(0, 30, 0));
            //Draw Z axis
            GL.Color3(0, 0, 1);
            GL.Vertex3(_pos);
            GL.Vertex3(_pos + new Vector3(0, 0, 30));
            GL.End();
            GL.PopMatrix();
        }
        #endregion

        #region Translations

        public void SetRotationX(float angle)
        {
            _rotX = angle;
        }

        public void SetRotationY(float angle)
        {
            _rotY = angle;
        }

        public void SetRotationZ(float angle)
        {
            _rotZ = angle;
        }

        public void SetPositionInSpace(float x, float y, float z)
        {
            _pos = new Vector3(x, y, z);
        }

        public void SetPositionInSpace(Vector3 vector)
        {
            _pos = vector;
        }

        public Vector3 GetPositionInSpace()
        {
            return _pos;
        }

        public void SetScale(float scale)
        {
            _scale = scale;
        }

        #endregion

        #region OBJLoaders

        private float[] LoadObj(string path)
        {
            var lines = File.ReadAllLines(path);
            var vertices = new List<float[]>();
            var final = new List<float>();
            foreach (var line in lines)
            {
                var lineSplitted = line.Split(' ');
                if (lineSplitted[0] == "v")
                {
                    var toAdd = new float[3];
                    toAdd[0] = float.Parse(lineSplitted[1], CultureInfo.InvariantCulture);
                    toAdd[1] = float.Parse(lineSplitted[2], CultureInfo.InvariantCulture);
                    toAdd[2] = float.Parse(lineSplitted[3], CultureInfo.InvariantCulture);
                    vertices.Add(toAdd);
                }

                if (lineSplitted[0] == "f")
                {
                    string[] s = { "/" };
                    var t1 = lineSplitted[1].Split(s, StringSplitOptions.RemoveEmptyEntries);
                    var t2 = lineSplitted[2].Split(s, StringSplitOptions.RemoveEmptyEntries);
                    var t3 = lineSplitted[3].Split(s, StringSplitOptions.RemoveEmptyEntries);

                    var v1 = vertices[int.Parse(t1[0], CultureInfo.InvariantCulture) - 1];
                    var v2 = vertices[int.Parse(t2[0], CultureInfo.InvariantCulture) - 1];
                    var v3 = vertices[int.Parse(t3[0], CultureInfo.InvariantCulture) - 1];

                    var v01 = new Vector3(v1[0], v1[1], v1[2]);
                    var v02 = new Vector3(v2[0], v2[1], v2[2]);
                    var v03 = new Vector3(v3[0], v3[1], v3[2]);

                    var l1 = v02 - v01;
                    var l2 = v03 - v01;

                    var n1 = Vector3.Cross(l2, l1);

                    final.Add(v1[0]);
                    final.Add(v1[1]);
                    final.Add(v1[2]);
                    final.Add(n1.X);
                    final.Add(n1.Y);
                    final.Add(n1.Z);
                    final.Add(v2[0]);
                    final.Add(v2[1]);
                    final.Add(v2[2]);
                    final.Add(n1.X);
                    final.Add(n1.Y);
                    final.Add(n1.Z);
                    final.Add(v3[0]);
                    final.Add(v3[1]);
                    final.Add(v3[2]);
                    final.Add(n1.X);
                    final.Add(n1.Y);
                    final.Add(n1.Z);

                    if (lineSplitted.Length == 5)
                    {
                        var t4 = lineSplitted[4].Split(s, StringSplitOptions.RemoveEmptyEntries);
                        var v4 = vertices[int.Parse(t4[0], CultureInfo.InvariantCulture) - 1];
                        var v04 = new Vector3(v4[0], v4[1], v4[2]);
                        var l3 = v04 - v01;
                        var n2 = Vector3.Cross(l3, l1);
                        final.Add(v1[0]);
                        final.Add(v1[1]);
                        final.Add(v1[2]);
                        final.Add(n2.X);
                        final.Add(n2.Y);
                        final.Add(n2.Z);
                        final.Add(v3[0]);
                        final.Add(v3[1]);
                        final.Add(v3[2]);
                        final.Add(n2.X);
                        final.Add(n2.Y);
                        final.Add(n2.Z);
                        final.Add(v4[0]);
                        final.Add(v4[1]);
                        final.Add(v4[2]);
                        final.Add(n2.X);
                        final.Add(n2.Y);
                        final.Add(n2.Z);
                    }
                }
            }


            return final.ToArray();
        }

        private float[] LoadObjTextured(string path)
        {
            var lines = File.ReadAllLines(path);
            var vertices = new List<float[]>();
            var textureCords = new List<float[]>();
            var final = new List<float>();
            foreach (var line in lines)
            {
                var lineSlitted = line.Split(' ');
                if (lineSlitted[0] == "v")
                {
                    var toAdd = new float[3];
                    toAdd[0] = float.Parse(lineSlitted[1], CultureInfo.InvariantCulture);
                    toAdd[1] = float.Parse(lineSlitted[2], CultureInfo.InvariantCulture);
                    toAdd[2] = float.Parse(lineSlitted[3], CultureInfo.InvariantCulture);
                    vertices.Add(toAdd);
                }

                if (lineSlitted[0] == "vt")
                {
                    var toAdd = new float[2];
                    toAdd[0] = float.Parse(lineSlitted[1], CultureInfo.InvariantCulture);
                    toAdd[1] = -(float.Parse(lineSlitted[2], CultureInfo.InvariantCulture) - 1);
                    textureCords.Add(toAdd);
                }

                if (lineSlitted[0] == "f")
                {
                    var t1 = lineSlitted[1].Split('/');
                    var t2 = lineSlitted[2].Split('/');
                    var t3 = lineSlitted[3].Split('/');


                    var v1 = vertices[int.Parse(t1[0]) - 1];
                    if (int.Parse(t2[0]) - 1 >= 0 && vertices.Count > int.Parse(t2[0]) - 1)
                    {
                        var v2 = vertices[int.Parse(t2[0]) - 1];
                        var v3 = vertices[int.Parse(t3[0]) - 1];
                        var tex1 = textureCords[int.Parse(t1[1]) - 1];
                        var tex2 = textureCords[int.Parse(t2[1]) - 1];
                        var tex3 = textureCords[int.Parse(t3[1]) - 1];

                        var v01 = new Vector3(v1[0], v1[1], v1[2]);
                        var v02 = new Vector3(v2[0], v2[1], v2[2]);
                        var v03 = new Vector3(v3[0], v3[1], v3[2]);

                        var l1 = v02 - v01;
                        var l2 = v03 - v01;

                        var n1 = Vector3.Cross(l2, l1);

                        final.Add(v1[0]);
                        final.Add(v1[1]);
                        final.Add(v1[2]);
                        final.Add(n1.X);
                        final.Add(n1.Y);
                        final.Add(n1.Z);
                        final.Add(tex1[0]);
                        final.Add(tex1[1]);
                        final.Add(v2[0]);
                        final.Add(v2[1]);
                        final.Add(v2[2]);
                        final.Add(n1.X);
                        final.Add(n1.Y);
                        final.Add(n1.Z);
                        final.Add(tex2[0]);
                        final.Add(tex2[1]);
                        final.Add(v3[0]);
                        final.Add(v3[1]);
                        final.Add(v3[2]);
                        final.Add(n1.X);
                        final.Add(n1.Y);
                        final.Add(n1.Z);
                        final.Add(tex3[0]);
                        final.Add(tex3[1]);

                        if (lineSlitted.Length == 5)
                        {
                            var t4 = lineSlitted[4].Split('/');
                            var v4 = vertices[int.Parse(t4[0], CultureInfo.InvariantCulture) - 1];
                            var tex4 = textureCords[int.Parse(t4[1]) - 1];
                            var v04 = new Vector3(v4[0], v4[1], v4[2]);
                            var l3 = v04 - v01;
                            var n2 = Vector3.Cross(l3, l1);


                            final.Add(v1[0]);
                            final.Add(v1[1]);
                            final.Add(v1[2]);
                            final.Add(n2.X);
                            final.Add(n2.Y);
                            final.Add(n2.Z);

                            final.Add(tex1[0]);
                            final.Add(tex1[1]);

                            final.Add(v3[0]);
                            final.Add(v3[1]);
                            final.Add(v3[2]);
                            final.Add(n2.X);
                            final.Add(n2.Y);
                            final.Add(n2.Z);

                            final.Add(tex3[0]);
                            final.Add(tex3[1]);

                            final.Add(v4[0]);
                            final.Add(v4[1]);
                            final.Add(v4[2]);
                            final.Add(n2.X);
                            final.Add(n2.Y);
                            final.Add(n2.Z);

                            final.Add(tex4[0]);
                            final.Add(tex4[1]);





                        }
                    }
                }
            }


            return final.ToArray();
        }

        #endregion

        public void Dispose()
        {
            GL.DeleteBuffer(_vertexBufferObject);
            GL.DeleteVertexArray(_mainObject);
        }
    }
}
