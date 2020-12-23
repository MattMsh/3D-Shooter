using System.Collections.Generic;
using OpenTK;
using OpenTK.Input;

namespace _3D_SHOOTER
{
    public class Player
    {
        Vector2 mouseLastPos = Vector2.Zero;
        Weapon rifle;

        public Player(Weapon weapon)
        {
            rifle = weapon;
        }

        public void Shoot(Camera camera, List<Sphere> enemies)
        {
            rifle.Fire(camera, enemies);
        }

        public void MoveUpdate(Camera camera, KeyboardState input, FrameEventArgs e, float playerSpeed)
        {
            if (input.IsKeyDown(Key.W)) camera.Position += camera.Front * playerSpeed * (float)e.Time; // Forward

            if (input.IsKeyDown(Key.S)) camera.Position -= camera.Front * playerSpeed * (float)e.Time; // Backwards

            if (input.IsKeyDown(Key.A)) camera.Position -= camera.Right * playerSpeed * (float)e.Time; // Left

            if (input.IsKeyDown(Key.D)) camera.Position += camera.Right * playerSpeed * (float)e.Time; // Right

            camera.Position *= new Vector3(1.0f, 0.0f, 1.0f);

            camera.Position += new Vector3(0.0f, 5.0f, 0.0f);
        }

        public void MouseUpdate(ref bool firstMove, MouseState mouse, Camera camera, float sensitivity)
        {
            if (firstMove)
            {
                mouseLastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - mouseLastPos.X;
                var deltaY = mouse.Y - mouseLastPos.Y;
                mouseLastPos = new Vector2(mouse.X, mouse.Y);

                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity;
            }
        }

    }
}