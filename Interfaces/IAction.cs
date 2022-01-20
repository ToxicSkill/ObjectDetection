using ObjectDetection.Models;
using OpenCvSharp;

namespace ObjectDetection.Interfaces
{
    internal interface IAction
    {
        void Execute(Mat frame, FrameContext context);
    }
}
