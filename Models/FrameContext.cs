using OpenCvSharp;

namespace ObjectDetection.Models
{
    internal class FrameContext
    {
        public Rect[]? Rectangles { get; set; }

        public Mat? MoveContours { get; set; }

        public Mat? BackgroundMask { get; set; }

        public double? Sharpness { get; set; }

        public bool AllReady() => MoveContours != null &&
        Rectangles != null &&
        BackgroundMask != null &&
        Sharpness != null;
    }
}
