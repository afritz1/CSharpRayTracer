using CSharpRayTracer.Source.Materials;
using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Shapes
{
    public class Intersection
    {
        private double t;
        private Vector3 point, normal;
        private Material material;

        public const double MaxT = 1.0e30;

        public Intersection()
        {
            this.t = MaxT;
            this.point = null;
            this.normal = null;
            this.material = null;
        }

        public Intersection(double t, Vector3 point, Vector3 normal, Material material)
        {
            this.t = t;
            this.point = point;
            this.normal = normal;
            this.material = material;
        }

        public double T { get { return this.t; } }
        public Vector3 Point { get { return this.point; } }
        public Vector3 Normal { get { return this.normal; } }
        public Material Material { get { return this.material; } }
    }
}
