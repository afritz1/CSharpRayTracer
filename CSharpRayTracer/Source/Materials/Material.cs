using CSharpRayTracer.Source.Vectors;

namespace CSharpRayTracer.Source.Materials
{
    public class Material
    {
        private Vector3 color;
        
        public Material(Vector3 color)
        {
            this.color = color;
        }

        public Vector3 Color { get { return this.color; } }
    }
}
