using OpenTK;
using OpenTK.Graphics;
using System.Collections.Generic;

namespace _3D_SHOOTER
{
    public class Weapon : Object
    {
        Raycaster raycaster;
        public Weapon(string path, Shader lightingShader, Lamp lamp, Color4 col) : base(path, lightingShader, lamp, col)
        {
            DrawRelativeToCamera = true;
            _pos = new Vector3(1.4f, -1.0f, -2.0f);
            _rotY = 3.31613f;
            _scale = 0.4f;
        }
        public Weapon(string path, Shader lightingShader, Lamp lamp, string texturePath) : base(path, lightingShader, lamp, texturePath)
        {
            DrawRelativeToCamera = true;
            _pos = new Vector3(1.4f, -1.0f, -2.0f);
            _rotY = 3.31613f;
            _scale = 0.4f;

        }
        public void Fire(Camera camera, List<Sphere> enemies)
        {
            raycaster = new Raycaster();
            raycaster.CastRay();
            raycaster.Update(camera, enemies);
        }
    }
}
