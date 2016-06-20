using System;
using System.Threading;

namespace CSharpRayTracer.Source.Vectors
{
    public class Vector3
    {
        private double x, y, z;

        private static ThreadLocal<Random> random =
            new ThreadLocal<Random>(() => new Random());

        public const double EPSILON = 1.0e-6;

        public Vector3()
        {
            this.x = 0.0;
            this.y = 0.0;
            this.z = 0.0;
        }

        public Vector3(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(Vector3 v)
        {
            this.x = v.x;
            this.y = v.y;
            this.z = v.z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }

        public static Vector3 operator -(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
        }

        public static Vector3 operator -(Vector3 v)
        {
            return new Vector3(-v.x, -v.y, -v.z);
        }

        public static Vector3 operator *(Vector3 v1, double m)
        {
            return new Vector3(v1.x * m, v1.y * m, v1.z * m);
        }

        public static Vector3 operator *(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 RandomColor
        {
            get
            {
                return new Vector3(
                    random.Value.NextDouble(),
                    random.Value.NextDouble(),
                    random.Value.NextDouble());
            }
        }

        public static Vector3 RandomPointInSphere(double radius)
        {
            return new Vector3(
                (2.0 * random.Value.NextDouble()) - 1.0,
                (2.0 * random.Value.NextDouble()) - 1.0,
                (2.0 * random.Value.NextDouble()) - 1.0).Normalized * 
                (random.Value.NextDouble() * radius);
        }

        public static Vector3 RandomPointInCuboid(double width, double height, double depth)
        {
            return new Vector3(
                width * ((2.0 * random.Value.NextDouble()) - 1.0),
                height * ((2.0 * random.Value.NextDouble()) - 1.0),
                depth * ((2.0 * random.Value.NextDouble()) - 1.0));
        }

        public static Vector3 RandomHemisphereDirection(Vector3 normal)
        {
            Vector3 direction = new Vector3(
                (2.0 * random.Value.NextDouble()) - 1.0,
                (2.0 * random.Value.NextDouble()) - 1.0,
                (2.0 * random.Value.NextDouble()) - 1.0).Normalized;
            return (direction.Dot(normal) > 0.0) ? direction : -direction;
        }

        public double X { get { return this.x; } }
        public double Y { get { return this.y; } }
        public double Z { get { return this.z; } }

        public override string ToString()
        {
            return this.x.ToString() + ", " + this.y.ToString() + ", " + this.z.ToString();
        }
        
        public int toARGB()
        {
            byte a = 255;
            byte r = (byte)(this.x * 255.0);
            byte g = (byte)(this.y * 255.0);
            byte b = (byte)(this.z * 255.0);
            return (a << 24) | (r << 16) | (g << 8) | b;
        }

        public double LengthSquared
        {
            get
            {
                return (this.x * this.x) + (this.y * this.y) + (this.z * this.z);
            }
        }

        public double Length
        {
            get
            {
                return Math.Sqrt(this.LengthSquared);
            }
        }

        public double Dot(Vector3 v)
        {
            return (this.x * v.x) + (this.y * v.y) + (this.z * v.z);
        }

        public Vector3 Cross(Vector3 v)
        {
            return new Vector3(
                (this.y * v.z) - (v.y * this.z),
                (v.x * this.z) - (this.x * v.z),
                (this.x * v.y) - (v.x * this.y));
        }

        public Vector3 Normalized
        {
            get
            {
                double lenRecip = 1.0 / this.Length;
                return new Vector3(this.x * lenRecip, this.y * lenRecip, this.z * lenRecip);
            }
        }

        public Vector3 Reflected(Vector3 normal)
        {
            return (normal * (2.0 * this.Dot(normal))) - this;
        }

        public Vector3 Clamped(double low, double high)
        {
            return new Vector3(
                (this.x > high) ? high : ((this.x < low) ? low : this.x),
                (this.y > high) ? high : ((this.y < low) ? low : this.y),
                (this.z > high) ? high : ((this.z < low) ? low : this.z));
        }

        public Vector3 Clamped()
        {
            return this.Clamped(0.0, 1.0);
        }

        public Vector3 Lerp(Vector3 v, double percent)
        {
            return this + ((v - this) * percent);
        }
    }
}
