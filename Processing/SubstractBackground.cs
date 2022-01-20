using ObjectDetection.Abstract;
using ObjectDetection.Interfaces;
using ObjectDetection.Models;
using OpenCvSharp;

namespace ObjectDetection.Processing
{
    internal class SubstractBackground : ProcessingBase, IAction
    {
        private readonly BackgroundSubtractorKNN _backSubKNN;

        public SubstractBackground()
        : base(3)
        {
            _backSubKNN = BackgroundSubtractorKNN.Create();
        }

        public void Execute(Mat frame, FrameContext context)
        {
            if (PreprareFrames(frame) && IsQueueFilled())
            {
                BackgroundSubstract(frame, context);
            }
        }

        private void BackgroundSubstract(Mat frame, FrameContext context)
        {
            using var fgMask = new Mat();
            _backSubKNN.Apply(frame, fgMask);
            Cv2.Threshold(fgMask, fgMask, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            Cv2.CvtColor(fgMask, fgMask, ColorConversionCodes.GRAY2RGB);
            context.BackgroundMask = fgMask.Clone();
        }
    }
}
