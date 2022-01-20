namespace ObjectDetection.Models
{
    internal class Frame
    {
        public Frame(int width, int height, double fps)
        {
            Width = width;
            Height = height;
            Fps = fps;
        }

        public int Width { get; init; }

        public int Height { get; init; }

        public double Fps { get; init; }
    }
}
