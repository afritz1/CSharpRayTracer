using CSharpRayTracer.Source.Shapes;

namespace CSharpRayTracer.Source.Lights
{
    public class Light
    {
        private Shape shape;

        public Light(Shape shape)
        {
            this.shape = shape;
        }

        public Shape Shape { get { return this.shape; } }
    }
}
