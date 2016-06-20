using CSharpRayTracer.Source.Materials;
using CSharpRayTracer.Source.Rays;
using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Shapes
{
    public class Cuboid : Shape
    {
        private double width, height, depth;

        public Cuboid(Material material, Vector3 point, double width, double height, double depth)
            : base(material, point)
        {
            this.width = width;
            this.height = height;
            this.depth = depth;
        }

        public double Width { get { return this.width; } }
        public double Height { get { return this.height; } }
        public double Depth { get { return this.depth; } }

        public override Vector3 RandomPoint
        {
            get
            {
                return this.point + 
                    Vector3.RandomPointInCuboid(this.width, this.height, this.depth);
            }
        }

        public override Intersection Intersect(Ray ray)
        {
            double nMinX, nMinY, nMinZ, nMaxX, nMaxY, nMaxZ;
            double tMin, tMax;
            double tX1 = (-this.width + this.point.X - ray.Point.X) / ray.Direction.X;
            double tX2 = (this.width + this.point.X - ray.Point.X) / ray.Direction.X;

            if (tX1 < tX2)
            {
                tMin = tX1;
                tMax = tX2;
                nMinX = -this.width;
                nMinY = 0.0;
                nMinZ = 0.0;
                nMaxX = this.width;
                nMaxY = 0.0;
                nMaxZ = 0.0;
            }
            else
            {
                tMin = tX2;
                tMax = tX1;
                nMinX = this.width;
                nMinY = 0.0;
                nMinZ = 0.0;
                nMaxX = -this.width;
                nMaxY = 0.0;
                nMaxZ = 0.0;
            }

            if (tMin > tMax)
            {
                return new Intersection();
            }

            double tY1 = (-this.height + this.point.Y - ray.Point.Y) / ray.Direction.Y;
            double tY2 = (this.height + this.point.Y - ray.Point.Y) / ray.Direction.Y;

            if (tY1 < tY2)
            {
                if (tY1 > tMin)
                {
                    tMin = tY1;
                    nMinX = 0.0;
                    nMinY = -this.height;
                    nMinZ = 0.0;
                }
                if (tY2 < tMax)
                {
                    tMax = tY2;
                    nMaxX = 0.0;
                    nMaxY = this.height;
                    nMaxZ = 0.0;
                }
            }
            else
            {
                if (tY2 > tMin)
                {
                    tMin = tY2;
                    nMinX = 0.0;
                    nMinY = this.height;
                    nMinZ = 0.0;
                }
                if (tY1 < tMax)
                {
                    tMax = tY1;
                    nMaxX = 0.0;
                    nMaxY = -this.height;
                    nMaxZ = 0.0;
                }
            }

            if (tMin > tMax)
            {
                return new Intersection();
            }

            double tZ1 = (-this.depth + this.point.Z - ray.Point.Z) / ray.Direction.Z;
            double tZ2 = (this.depth + this.point.Z - ray.Point.Z) / ray.Direction.Z;

            if (tZ1 < tZ2)
            {
                if (tZ1 > tMin)
                {
                    tMin = tZ1;
                    nMinX = 0.0;
                    nMinY = 0.0;
                    nMinZ = -this.depth;
                }
                if (tZ2 < tMax)
                {
                    tMax = tZ2;
                    nMaxX = 0.0;
                    nMaxY = 0.0;
                    nMaxZ = this.depth;
                }
            }
            else
            {
                if (tZ2 > tMin)
                {
                    tMin = tZ2;
                    nMinX = 0.0;
                    nMinY = 0.0;
                    nMinZ = this.depth;
                }
                if (tZ1 < tMax)
                {
                    tMax = tZ1;
                    nMaxX = 0.0;
                    nMaxY = 0.0;
                    nMaxZ = -this.depth;
                }
            }

            if (tMin > tMax)
            {
                return new Intersection();
            }

            if (tMin < 0.0)
            {
                tMin = tMax;
                nMinX = nMaxX;
                nMinY = nMaxY;
                nMinZ = nMaxZ;
            }

            if (tMin >= 0.0)
            {
                double t = tMin;
                Vector3 point = ray.PointAt(t);
                Vector3 normal = new Vector3(nMinX, nMinY, nMinZ).Normalized;
                return new Intersection(t, point, normal, this.material);
            }
            else
            {
                return new Intersection();
            }
        }
    }
}
