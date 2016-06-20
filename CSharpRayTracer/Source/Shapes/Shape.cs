using CSharpRayTracer.Source.Materials;
using CSharpRayTracer.Source.Rays;
using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Shapes
{
    public abstract class Shape
    {
        protected Material material;
        protected Vector3 point;

        public Shape(Material material, Vector3 point)
        {
            this.material = material;
            this.point = point;
        }

        public Material Material { get { return this.material; } }
        public Vector3 Point { get { return this.point; } }

        public abstract Vector3 RandomPoint { get; }

        public abstract Intersection Intersect(Ray ray);
    }
}
