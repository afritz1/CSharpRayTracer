using System;

using CSharpRayTracer.Source.Lights;
using CSharpRayTracer.Source.Materials;
using CSharpRayTracer.Source.Rays;
using CSharpRayTracer.Source.Shapes;
using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Worlds
{
    public class World
    {
        private Shape[] shapes;
        private Light[] lights;
        private double radius;
        private int directLightSamples, indirectLightSamples, maxDepth;

        private const double AmbientLight = 0.60;
        private const double Reflectivity = 0.30;

        public World(int shapeCount, int lightCount, int directLightSamples,
            int indirectLightSamples, int maxDepth, double radius)
        {
            Random random = new Random();
            
            this.shapes = new Shape[shapeCount];

            for (int i = 0; i < this.shapes.Length; ++i)
            {
                if (random.Next(2) == 0)
                {
                    this.shapes[i] = new Sphere(new Material(Vector3.RandomColor),
                        Vector3.RandomPointInSphere(radius), 0.5 + random.NextDouble());
                }
                else
                {
                    this.shapes[i] = new Cuboid(new Material(Vector3.RandomColor),
                        Vector3.RandomPointInSphere(radius), 0.5 + random.NextDouble(),
                        0.5 + random.NextDouble(), 0.5 + random.NextDouble());
                }
            }

            this.lights = new Light[lightCount];

            for (int i = 0; i < this.lights.Length; ++i)
            {
                if (random.Next(2) == 0)
                {
                    Sphere sphere = new Sphere(new Material(Vector3.RandomColor),
                        new Vector3(0.0, 1.0, 0.0) + Vector3.RandomPointInSphere(radius),
                        0.5 + random.NextDouble());
                    this.lights[i] = new Light(sphere);
                }
                else
                {
                    Cuboid cuboid = new Cuboid(new Material(Vector3.RandomColor),
                        new Vector3(0.0, 1.0, 0.0) + Vector3.RandomPointInSphere(radius), 
                        0.5 + random.NextDouble(), 0.5 + random.NextDouble(), 
                        0.5 + random.NextDouble());
                    this.lights[i] = new Light(cuboid);
                }
            }

            this.radius = radius;
            this.maxDepth = maxDepth;
            this.directLightSamples = directLightSamples;
            this.indirectLightSamples = indirectLightSamples;
        }

        private Vector3 BackgroundColor(Vector3 direction)
        {
            Vector3 horizonColor = new Vector3(0.60, 0.80, 1.0);
            Vector3 zenithColor = horizonColor * 0.70;
            double elevation = direction.Y;
            double percent = (elevation < 0.0) ? 0.0 : elevation;
            return horizonColor + ((zenithColor - horizonColor) * percent);
        }

        private Intersection NearestShape(Ray ray)
        {
            Intersection nearestHit = new Intersection();

            foreach (Shape shape in this.shapes)
            {
                Intersection currentTry = shape.Intersect(ray);

                if (currentTry.T < nearestHit.T)
                {
                    nearestHit = currentTry;
                }
            }

            return nearestHit;
        }

        private Intersection NearestLight(Ray ray)
        {
            Intersection nearestHit = new Intersection();

            foreach (Light light in this.lights)
            {
                Intersection currentTry = light.Shape.Intersect(ray);

                if (currentTry.T < nearestHit.T)
                {
                    nearestHit = currentTry;
                }
            }

            return nearestHit;
        }

        private double AmbientPercent(Vector3 point, Vector3 normal)
        {
            int unoccludedRays = 0;
            for (int n = 0; n < this.indirectLightSamples; ++n)
            {
                Vector3 hemisphereDir = Vector3.RandomHemisphereDirection(normal);
                Ray hemisphereRay = new Ray(point, hemisphereDir, Ray.InitialDepth);
                Intersection currentTry = this.NearestShape(hemisphereRay);
                if (currentTry.T == Intersection.MaxT)
                {
                    ++unoccludedRays;
                }
            }

            return ((double)unoccludedRays / this.indirectLightSamples) * AmbientLight;
        }

        private Vector3 PhongAt(Intersection intersection, Ray ray)
        {
            Vector3 viewVector = -ray.Direction;
            Vector3 localNormal = (viewVector.Dot(intersection.Normal) > 0.0) ?
                intersection.Normal : -intersection.Normal;
            Vector3 localNormalEps = localNormal * Vector3.EPSILON;
            Vector3 localNormalEpsPoint = intersection.Point + localNormalEps;

            Vector3 materialColor = intersection.Material.Color;
            Vector3 ambientColor = (materialColor * this.BackgroundColor(ray.Direction)) *
                this.AmbientPercent(localNormalEpsPoint, localNormal);

            Vector3 totalColor = ambientColor;

            foreach (Light light in this.lights)
            {
                Vector3 diffuseColorSum = new Vector3();

                for (int n = 0; n < this.directLightSamples; ++n)
                {
                    Vector3 lightDir = (light.Shape.RandomPoint - intersection.Point).Normalized;
                    Ray lightRay = new Ray(localNormalEpsPoint, lightDir, Ray.InitialDepth);
                    Intersection lightTry = light.Shape.Intersect(lightRay);
                    Intersection shadowTry = this.NearestShape(lightRay);

                    if (lightTry.T < shadowTry.T)
                    {
                        double lnDot = lightDir.Dot(localNormal);
                        Vector3 lightColor = light.Shape.Material.Color;
                        Vector3 diffuseColor = (materialColor * lightColor) * lnDot;
                        diffuseColorSum = diffuseColorSum + diffuseColor;
                    }
                }

                totalColor = totalColor + (diffuseColorSum * (1.0 / this.directLightSamples));
            }

            if (ray.Depth < this.maxDepth)
            {
                Vector3 reflectDir = viewVector.Reflected(localNormal).Normalized;
                Ray reflectRay = new Ray(localNormalEpsPoint, reflectDir, ray.Depth + 1);
                Vector3 reflectColor = this.RayTrace(reflectRay) * Reflectivity;
                totalColor = totalColor + reflectColor;
            }

            return totalColor;
        }

        public Vector3 RayTrace(Ray ray)
        {
            Intersection nearestShape = this.NearestShape(ray);
            Intersection nearestLight = this.NearestLight(ray);

            if (nearestShape.T < nearestLight.T)
            {
                return this.PhongAt(nearestShape, ray);
            }
            else if (nearestLight.T < Intersection.MaxT)
            {
                return nearestLight.Material.Color;
            }
            else
            {
                return this.BackgroundColor(ray.Direction);
            }
        }
    }
}
