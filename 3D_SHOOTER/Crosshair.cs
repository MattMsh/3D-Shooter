using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace _3D_SHOOTER
{
    public class Crosshair 
    {
        private float[] _vertices;
        private int _vertexBufferObject;
        private int _mainObject;
        private Texture _texture;
        private Shader _shader;

        public Crosshair(Shader shader2D, string texturePath)
        {
            _vertices = GetVertices();

            _vertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
                BufferUsageHint.StaticDraw);

            _mainObject = GL.GenVertexArray();
            GL.BindVertexArray(_mainObject);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

            var positionLocation = shader2D.GetAttribLocation("aPosition");
            GL.EnableVertexAttribArray(positionLocation);
            GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

            var texCoordLocation = shader2D.GetAttribLocation("aTexCoord");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));


            _texture = new Texture(texturePath, TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            _texture.Use();

            
            _shader = shader2D;
        }

        private static float[] GetVertices()
        {
            float[] vertices =
            {
                0.08f,  0.08f, 0.0f, 1.0f, 1.0f, // top right
                0.08f, -0.08f, 0.0f, 1.0f, 0.0f, // bottom right
               -0.08f, -0.08f, 0.0f, 0.0f, 0.0f, // bottom left

               -0.08f,  0.08f, 0.0f, 0.0f, 1.0f,  // top left
                0.08f,  0.08f, 0.0f, 1.0f, 1.0f, // top right
               -0.08f, -0.08f, 0.0f, 0.0f, 0.0f, // bottom left

            };
            return vertices; 
        }

        public void Show(Camera camera)
        {
            Matrix4 model = Matrix4.CreateTranslation(new Vector3(0,0,0));
            GL.BindVertexArray(_mainObject);
            _texture.Use();
            _shader.Use();
            _shader.SetMatrix4("transform", model);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);
        }
    }
}
