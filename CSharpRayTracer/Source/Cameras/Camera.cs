using System;

using CSharpRayTracer.Source.Rays;
using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Cameras
{
    public class Camera
    {
        private Vector3 eye, forward, right, up;

        public static readonly Vector3 GlobalUp = new Vector3(0.0, 1.0, 0.0);

        private Camera(Vector3 eye, Vector3 forward, Vector3 right, Vector3 up)
        {
            this.eye = eye;
            this.forward = forward;
            this.right = right;
            this.up = up;
        }

        public static Camera LookAt(Vector3 eye, Vector3 focus, double aspect, double fovY)
        {
            double zoom = 1.0 / Math.Tan((fovY * 0.5) * (Math.PI / 180.0));

            Vector3 forward = (focus - eye).Normalized * zoom;
            Vector3 right = forward.Cross(GlobalUp).Normalized * aspect;
            Vector3 up = right.Cross(forward).Normalized;

            return new Camera(eye, forward, right, up);
        }

        public Vector3 Eye { get { return this.eye; } }
        public Vector3 Forward { get { return this.forward; } }
        public Vector3 Right { get { return this.right; } }
        public Vector3 Up { get { return this.up; } }

        public Ray ImageRay(double xx, double yy)
        {
            Vector3 rightComp = this.right * ((2.0 * xx) - 1.0);
            Vector3 upComp = this.up * ((2.0 * yy) - 1.0);
            Vector3 direction = (this.forward + rightComp - upComp).Normalized;
            return new Ray(this.eye, direction, Ray.InitialDepth);
        }
    }
}
