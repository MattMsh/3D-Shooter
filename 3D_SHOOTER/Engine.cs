using System;
using System.Collections.Generic;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using static _3D_SHOOTER.Shaders;

namespace _3D_SHOOTER
{
    public class Engine : GameWindow
    {
        private readonly List<Object> _mainObjects = new List<Object>();
        protected Camera _camera;
        private Shader _lampShader, _lightingShader, _textureShader, _textureLightingShader, _2dTextured;
        private Lamp _mainLamp;
        protected bool _firstMove = true;
        private Crosshair crosshair;

        protected bool UseDepthTest = false, UseAlpha = true, KeyboardAndMouseInput = true, LastTime = true;

        public Engine(int width, int height, string title) :
            base(width, height, GraphicsMode.Default, title, GameWindowFlags.Fullscreen)
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            if (UseAlpha) GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Enable(EnableCap.Blend);
            _lightingShader = new Shader(ShaderVert, LightingFrag);
            _textureLightingShader = new Shader(TextureVert, TextureLightingFrag);
            _lampShader = new Shader(ShaderVert, ShaderFrag);
            _textureShader = new Shader(TextureVert, TextureFrag);
            _2dTextured = new Shader(Texture2DVert, Texture2DFrag);
            _lightingShader.Use();
            _lampShader.Use();
            _textureShader.Use();
            _2dTextured.Use();

            _camera = new Camera(Vector3.UnitZ * 3, Size.Width / (float)Size.Height);

            Width = Size.Width;
            Height = Size.Height;
            CursorGrabbed = KeyboardAndMouseInput;
            CursorVisible = !KeyboardAndMouseInput;
            base.OnLoad(e);
        }

        protected void Render3DObjects()
        {
            if (UseDepthTest) GL.Enable(EnableCap.DepthTest);

            foreach (var obj in _mainObjects)
                obj.Show(_camera);
            crosshair.Show(_camera);

            GL.Disable(EnableCap.DepthTest);
        }

        protected void RenderLight()
        {
            _mainLamp.Show(_camera, _lampShader);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            base.OnResize(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);


            GL.DeleteProgram(_lampShader.Handle);
            GL.DeleteProgram(_lightingShader.Handle);
            GL.DeleteProgram(_textureShader.Handle);

            foreach (var obj in _mainObjects) obj.Dispose();

            _mainLamp?.Dispose();

            base.OnUnload(e);
        }

        protected void CreateMainLight(Vector3 pos, Vector3 color)
        {
            _mainLamp = new Lamp(pos, color, _lampShader, 10);
        }

        protected Object OpenObj(string obj, Color4 color)
        {
            _mainObjects.Add(new Object(obj, _lightingShader, _mainLamp, color));
            return _mainObjects[_mainObjects.Count - 1];
        }
        
        protected Object OpenObj(string obj, string texture)
        {
            _mainObjects.Add(new Object(obj, _textureLightingShader, _mainLamp, texture));
            return _mainObjects[_mainObjects.Count - 1];
        }

        protected Weapon OpenWeapon(string obj, Color4 color)
        {
            var w = new Weapon(obj, _lightingShader, _mainLamp, color);
            _mainObjects.Add(w);
            return w;
        }

        protected Weapon OpenWeapon(string obj, string texture)
        {
            var w = new Weapon(obj, _textureLightingShader, _mainLamp, texture);
            _mainObjects.Add(w);
            return w;
        }

        protected Sphere CreateEnemy(float radius, Color4 color)
        {
            var e = new Sphere(radius, _lightingShader, _mainLamp, color);
            _mainObjects.Add(e);
            return e;
        }
        
        protected Plane CreatePlane(float width, float height, Vector3 position, Color4 color)
        {
            var p = new Plane(width, height, _lightingShader, _mainLamp, color);
            p.SetPositionInSpace(position);
            _mainObjects.Add(p);
            return p;
        }

        protected Crosshair CreateCrosshair(string texturePath)
        {
            crosshair = new Crosshair(_2dTextured, texturePath);
            return crosshair;
        }

        protected void SetClearColor(Color4 color)
        {
            GL.ClearColor(color);
        }

        protected void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        
    }
}
