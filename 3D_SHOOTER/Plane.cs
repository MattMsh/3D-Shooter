using OpenTK;
using OpenTK.Graphics;

namespace _3D_SHOOTER
{
    public class Plane : Object
    {
        public Plane(float width, float height, Shader shader, Lamp lamp, Color4 color) : base(CreatePlane(width, height), shader, lamp, color)
        {

        }

        private static float[] CreatePlane(float width, float height)
        {
            var vector1 = new Vector3(-width / 2, 0, 0);
            var vector2 = new Vector3(width / 2, 0, 0);
            var vector3 = new Vector3(width / 2, height, 0);
            var vector4 = new Vector3(-width / 2, height, 0);
            var l1 = vector2 - vector1;
            var l2 = vector3 - vector1;
            var normal = Vector3.Cross(l2, l1);

            float[] vertices =
            {
                vector1.X, vector1.Y, vector1.Z, normal.X, normal.Y, normal.Z,
                vector3.X, vector3.Y, vector3.Z, normal.X, normal.Y, normal.Z,
                vector2.X, vector2.Y, vector2.Z, normal.X, normal.Y, normal.Z,

                vector1.X, vector1.Y, vector1.Z, normal.X, normal.Y, normal.Z,
                vector3.X, vector3.Y, vector3.Z, normal.X, normal.Y, normal.Z,
                vector4.X, vector4.Y, vector4.Z, normal.X, normal.Y, normal.Z
            };
            
            return vertices;
        }
    }
}
