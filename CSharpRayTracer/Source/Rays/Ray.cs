using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Rays
{
    public class Ray
    {
        private Vector3 point, direction;
        private int depth;

        public const int InitialDepth = 0;

        public Ray(Vector3 point, Vector3 direction, int depth)
        {
            this.point = point;
            this.direction = direction;
            this.depth = depth;
        }

        public Vector3 Point { get { return this.point; } }
        public Vector3 Direction { get { return this.direction; } }
        public int Depth { get { return this.depth; } }

        public Vector3 PointAt(double distance)
        {
            return this.point + (this.direction * distance);
        }
    }
}
