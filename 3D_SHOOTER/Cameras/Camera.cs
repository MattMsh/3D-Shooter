using System;
using OpenTK;

namespace _3D_SHOOTER
{
    public class Camera
    {
        // The field of view of the camera (radians)
        private float _fov = MathHelper.PiOver2;

        private Vector3 _front = -Vector3.UnitZ;

        private bool perspective;

        // Rotation around the X axis (radians)
        private float _pitch;

        // Rotation around the Y axis (radians)
        private float _yaw = -MathHelper.PiOver2;

        public Camera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
            perspective = true;
        }


        private float left;
        private float right;
        private float bottom;
        private float up;
        private float near;
        private float far;

        public Camera(Vector3 position, float left, float right, float bottom, float up, float near, float far)
        {
            Position = position;
            this.left = left;
            this.right = right;
            this.bottom = bottom;
            this.up = up;
            this.near = near;
            this.far = far;
            perspective = false; 
        }

        public Vector3 Position { get; set; }

        private float AspectRatio { get; }

        public Vector3 Front => _front;

        public Vector3 Up { get; private set; } = Vector3.UnitY;

        public Vector3 Right { get; private set; } = Vector3.UnitX;

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(_pitch);
            set
            {
                var angle = MathHelper.Clamp(value, -89f, 89f);
                _pitch = MathHelper.DegreesToRadians(angle);
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(_yaw);
            set
            {
                _yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public float Fov
        {
            get => MathHelper.RadiansToDegrees(_fov);
            set
            {
                var angle = MathHelper.Clamp(value, 1f, 45f);
                _fov = MathHelper.DegreesToRadians(angle);
            }
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, Up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            if (perspective)
            {
                return Matrix4.CreatePerspectiveFieldOfView(_fov, AspectRatio, 0.01f, 1000f);
            }
            else
            {
                return Matrix4.CreateOrthographicOffCenter(left, right, bottom, up, near, far);
            }
        }

        private void UpdateVectors()
        {
            _front.X = (float)Math.Cos(_pitch) * (float)Math.Cos(_yaw);
            _front.Y = (float)Math.Sin(_pitch);
            _front.Z = (float)Math.Cos(_pitch) * (float)Math.Sin(_yaw);

            _front = Vector3.Normalize(_front);

            Right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
            Up = Vector3.Normalize(Vector3.Cross(Right, _front));
        }
        
    }
}