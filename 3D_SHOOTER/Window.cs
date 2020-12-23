using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace _3D_SHOOTER
{
    public class Window : Engine
    {
        Plane plane1, plane2, plane3, plane4, floor;
        Weapon rifle;
        List<Plane> planes = new List<Plane>();
        List<Sphere> enemies = new List<Sphere>();
        Sphere enemy;
        Player player;
        Crosshair crosshair;
        protected float playerSpeed = 30f;
        protected float _sensitivity = 0.2f;

        public Window(int width, int height, string title) : base(width, height, title) { }

        protected override void OnLoad(EventArgs e)
        {
            SetClearColor(Color4.BlueViolet); //Sets Background Color
            UseDepthTest = true;
            KeyboardAndMouseInput = true;
            base.OnLoad(e);
            InitRoom();
            InitPlayer();
            InitEnemies();
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Clear();
            Render3DObjects();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            if (!Focused) // check to see if the window is focused
                return;
            
            var input = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (input.IsKeyDown(Key.Escape)) Close();

            if (KeyboardAndMouseInput)
            {
                if (mouse.IsButtonDown(MouseButton.Left))
                {
                    player.Shoot(_camera, enemies);
                }

                player.MoveUpdate(_camera, input, e, playerSpeed);

                player.MouseUpdate(ref _firstMove, mouse, _camera, _sensitivity);
            }

            base.OnUpdateFrame(e);
            CheckCollision();
        }

        private void InitRoom()
        {
            CreateMainLight(new Vector3(0, 50, 0), Vector3.One);
            plane1 = CreatePlane(100.0f, 50.0f, new Vector3(0, 0, -50), Color4.Brown);
            plane2 = CreatePlane(100.0f, 50.0f, new Vector3(0, 0, -50), Color4.Brown);
            plane3 = CreatePlane(100.0f, 50.0f, new Vector3(0, 0, -50), Color4.Brown);
            plane4 = CreatePlane(100.0f, 50.0f, new Vector3(0, 0, -50), Color4.Brown);
            floor = CreatePlane(100.0f, 100.0f, new Vector3(0, -50, 0), Color4.Blue);
            planes.Add(plane1);
            planes.Add(plane2);
            planes.Add(plane3);
            planes.Add(plane4);
            planes.Add(floor);
            plane1.SetRotationY(MathHelper.DegreesToRadians(90));
            plane2.SetRotationY(MathHelper.DegreesToRadians(180));
            plane3.SetRotationY(MathHelper.DegreesToRadians(-90));
            plane4.SetRotationY(MathHelper.DegreesToRadians(0));
            floor.SetRotationX(MathHelper.DegreesToRadians(-90));
        }

        private void InitPlayer()
        {
            rifle = OpenWeapon("Obj/SniperRifle.obj", "Textures/sniper_1001_Base_Color.png");
            player = new Player(rifle);
            crosshair = CreateCrosshair("Textures/crossHair.png"); 
        }

        private void InitEnemies()
        {
            enemy = CreateEnemy(5, Color4.Yellow);
            enemy.SetScale(0.5f);
            enemy.SetPositionInSpace(0, 50, -95);

            enemies.Add(enemy);
        }

        private void CheckCollision()
        {
            if (_camera.Position.X >= 45)
            {
                _camera.Position = (_camera.Position * new Vector3(0, 1, 1)) + new Vector3(45, 0, 0);
            }
            if (_camera.Position.X <= -45)
            {
                _camera.Position = (_camera.Position * new Vector3(0, 1, 1)) + new Vector3(-45, 0, 0);
            }
            if (_camera.Position.Z >= 45)
            {
                _camera.Position = (_camera.Position * new Vector3(1, 1, 0)) + new Vector3(0, 0, 45);
            }
            if (_camera.Position.Z <= -45)
            {
                _camera.Position = (_camera.Position * new Vector3(1, 1, 0)) + new Vector3(0, 0, -45);
            }
        }

    }
}