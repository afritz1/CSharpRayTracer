using System;

using CSharpRayTracer.Source.Materials;
using CSharpRayTracer.Source.Rays;
using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Shapes
{
    public class Sphere : Shape
    {
        private double radius, radiusRecip, radiusSquared;

        public Sphere(Material material, Vector3 point, double radius)
            : base(material, point)
        {
            this.radius = radius;
            this.radiusRecip = 1.0 / radius;
            this.radiusSquared = radius * radius;
        }

        public override Vector3 RandomPoint
        {
            get
            {
                return this.point + Vector3.RandomPointInSphere(this.radius);
            }
        }

        public override Intersection Intersect(Ray ray)
        {
            Vector3 diff = this.point - ray.Point;
            double b = diff.Dot(ray.Direction);
            double determinant = (b * b) - diff.Dot(diff) + this.radiusSquared;
            if (determinant < 0.0)
            {
                return new Intersection();
            }
            else
            {
                double detSqrt = Math.Sqrt(determinant);
                double b1 = b - detSqrt;
                double b2 = b + detSqrt;
                double t = (b1 > Vector3.EPSILON) ? b1 : 
                    ((b2 > Vector3.EPSILON) ? b2 : Intersection.MaxT);
                Vector3 point = ray.PointAt(t);
                Vector3 normal = (point - this.point) * this.radiusRecip;
                return new Intersection(t, point, normal, material);
            }
        }
    }
}
