using ObjectDetection.Abstract;
using ObjectDetection.Interfaces;
using ObjectDetection.Models;
using OpenCvSharp;

namespace ObjectDetection.Processing
{
    internal class SharpnessDetection : ProcessingBase, IAction
    {
        public SharpnessDetection()
        : base(2)
        {
        }

        public void Execute(Mat frame, FrameContext context)
        {
            if (PreprareFrames(frame) && IsQueueFilled())
            {
                DetectSharpness(context);
            }
        }

        private void DetectSharpness(FrameContext context)
        {
            using var diff = Preprocess.GetDiffrence(Frames);
            if (!IsNullOrEmpty(diff))
            {
                Cv2.Laplacian(diff, diff, MatType.CV_8UC1, delta: 1);
                context.Sharpness = diff.Mean()[0];
            }
        }
    }
}
