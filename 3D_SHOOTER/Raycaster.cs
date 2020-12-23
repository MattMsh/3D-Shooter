using OpenTK;
using System;
using System.Collections.Generic;

namespace _3D_SHOOTER
{
    struct Ray
    {
        public Vector3 pos;
        public Vector3 dir;
    }
    public class Raycaster
    {
        private bool castRay = false;
        private bool collision = false;
        private Ray ray;
        Random rnd = new Random();

        public void CastRay()
        {
            castRay = true;
        }


        public void Update(Camera camera, List<Sphere> enemies)
        {
            if (castRay)
            {
                castRay = false;
                ray = CastRayFromWeapon(camera);
                IntersectRayWithEnemy(camera, enemies);
            }
        }

        Ray CastRayFromWeapon(Camera camera)
        {
            Ray ray;
            ray.pos = camera.Position;
            ray.dir = camera.Front;
            return ray;
        }

        private void IntersectRayWithEnemy(Camera camera, List<Sphere> enemies)
        {
            foreach (var enemy in enemies)
            {
                collision = RaySphere(camera, ray.dir, enemy.radius, enemy._pos.X, enemy._pos.Y, enemy._pos.Z);

                // Check if the ray is colliding with the sphere
                if (collision)
                {
                    OnEnemyHit(enemy);
                }
            }
        }

        private bool RaySphere(Camera camera, Vector3 RayDirWorld, double SphereRadius, float x, float y, float z)
        {
            Vector3 v = new Vector3(x, y, z) - camera.Position;
            double a = Vector3.Dot(RayDirWorld, RayDirWorld);
            double b = 2.0 * Vector3.Dot(v, RayDirWorld);
            double c = Vector3.Dot(v, v) - SphereRadius * SphereRadius;
            double b_squared_minus_4ac = b * b + (-4.0 * a * c);

            if (b_squared_minus_4ac == 0)
            {
                return true;
            }
            else if (b_squared_minus_4ac > 0)
            {
                double x1 = (-b - Math.Sqrt(b_squared_minus_4ac)) / (2.0 * a);
                double x2 = (-b + Math.Sqrt(b_squared_minus_4ac)) / (2.0 * a);

                if (x1 >= 0.0 || x2 >= 0.0)
                    return true;
                if (x1 < 0.0 || x2 >= 0.0)
                    return true;
            }
            
            return false;
        }

        private void OnEnemyHit(Sphere enemy)
        {   
            enemy._pos = new Vector3(rnd.Next(-45, 45), rnd.Next(5, 45), enemy._pos.Z);
        }
    }
}
