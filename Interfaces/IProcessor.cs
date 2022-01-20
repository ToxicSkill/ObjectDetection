using ObjectDetection.Models;
using OpenCvSharp;

namespace ObjectDetection.Interfaces
{
    internal interface IProcessor
    {
        Mat? DrawMoveRectangles(Mat? frame, FrameContext context);

        Mat? DrawMoveFocus(Mat? frame, FrameContext context);
    }
}
