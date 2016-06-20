using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;

using CSharpRayTracer.Source.Cameras;
using CSharpRayTracer.Source.Rays;
using CSharpRayTracer.Source.Vectors;
using CSharpRayTracer.Source.Worlds;

namespace CSharpRayTracer.Source
{
    public class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("C# Ray Tracer, by Aaron Fritz!");
            Console.WriteLine();

            Console.Write("Output width (i.e., 1-3840): ");
            int screenWidth = Int32.Parse(Console.ReadLine());

            Console.Write("Output height (i.e., 1-2160): ");
            int screenHeight = Int32.Parse(Console.ReadLine());

            Console.Write("Super samples (i.e., 1-8): ");
            int superSamples = Int32.Parse(Console.ReadLine());

            Console.Write("Shape count (i.e., 1-200): ");
            int shapeCount = Int32.Parse(Console.ReadLine());

            Console.Write("Light count (i.e., 1-5): ");
            int lightCount = Int32.Parse(Console.ReadLine());

            Console.Write("Light samples (i.e., 1-128): ");
            int lightSamples = Int32.Parse(Console.ReadLine());

            Console.Write("Indirect light samples (i.e., 1-128): ");
            int indirectLightSamples = Int32.Parse(Console.ReadLine());

            Console.Write("Max recursion depth (i.e., 0-10): ");
            int maxDepth = Int32.Parse(Console.ReadLine());

            Bitmap image = new Bitmap(screenWidth, screenHeight);

            double aspect = (double)screenWidth / screenHeight;
            Camera camera = Camera.LookAt(new Vector3(6.0, 3.0, 12.0), new Vector3(), aspect, 60.0);

            World world = new World(shapeCount, lightCount, lightSamples, 
                indirectLightSamples, maxDepth, 10.0);
            
            double widthRecip = 1.0 / screenWidth;
            double heightRecip = 1.0 / screenHeight;
            double superSamplesRecip = 1.0 / superSamples;
            double superSamplesHalfRecip = superSamplesRecip * 0.5;
            double superSamplesSquaredRecip = 1.0 / (superSamples * superSamples);

            object obj = new object();
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.For(0, screenHeight, y =>
            {
                for (int x = 0; x < screenWidth; ++x)
                {
                    Vector3 color = new Vector3();
                    for (int j = 0; j < superSamples; ++j)
                    {
                        double jj = j * superSamplesRecip;
                        double yy = (y + superSamplesHalfRecip + jj) * heightRecip;
                        for (int i = 0; i < superSamples; ++i)
                        {
                            double ii = i * superSamplesRecip;
                            double xx = (x + superSamplesHalfRecip + ii) * widthRecip;
                            Ray ray = camera.ImageRay(xx, yy);
                            color = color + world.RayTrace(ray).Clamped();
                        }
                    }

                    color = color * superSamplesSquaredRecip;                     
                    Color colorARGB = Color.FromArgb(color.toARGB());
                    lock (obj)
                    {
                        image.SetPixel(x, y, colorARGB);
                    }
                }
            });

            stopwatch.Stop();

            image.Save("Output.png", ImageFormat.Png);
            image.Dispose();

            Console.WriteLine("Done! Took {0}s.", stopwatch.Elapsed.TotalMilliseconds / 1000);
            Console.Read();
        }
    }
}
