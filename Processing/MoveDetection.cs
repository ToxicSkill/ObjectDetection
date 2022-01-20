using ObjectDetection.Abstract;
using ObjectDetection.Interfaces;
using ObjectDetection.Models;
using OpenCvSharp;

namespace ObjectDetection.Processing
{
    internal class MoveDetection : ProcessingBase, IAction
    {
        public MoveDetection()
        : base(3)
        {
        }

        public void Execute(Mat frame, FrameContext context)
        {
            if (PreprareFrames(frame) && IsQueueFilled())
            {
                DetectMotion(context);
            }
        }

        private void DetectMotion(FrameContext context)
        {
            using var diff = Preprocess.GetDiffrence(Frames);
            if (!IsNullOrEmpty(diff))
            {
                context.MoveContours = diff.Clone();
            }
        }
    }
}
