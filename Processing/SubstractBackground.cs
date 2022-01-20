using ObjectDetection.Abstract;
using ObjectDetection.Interfaces;
using ObjectDetection.Models;
using OpenCvSharp;

namespace ObjectDetection.Processing
{
    internal class SubstractBackground : ProcessingBase, IAction
    {
        private readonly BackgroundSubtractorKNN _backSubKNN;

        private readonly double[,] _morphologyKernel =
        {
            { 1, 1, 1 },
            { 1, 2, 1 },
            { 1, 1, 1 }
        };

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
            var contoursMask = fgMask.Clone();
            Cv2.CvtColor(fgMask, fgMask, ColorConversionCodes.GRAY2RGB);
            context.BackgroundMask = fgMask.Clone();

            Cv2.Threshold(fgMask, fgMask, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            Cv2.Threshold(contoursMask, contoursMask, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
            Cv2.GaussianBlur(contoursMask, contoursMask, new Size(53, 53), 1);
            for (int i = 0; i < 4; i++)
            {
                Cv2.MorphologyEx(contoursMask, contoursMask, MorphTypes.Erode, null);
            }
            for (int i = 0; i < 2; i++)
            {
                Cv2.MorphologyEx(contoursMask, contoursMask, MorphTypes.Open, null);
            }
            Cv2.FindContours(contoursMask, out Point[][] contours, out _, RetrievalModes.CComp, ContourApproximationModes.ApproxNone);

            context.Rectangles = contours.Select(c => Cv2.BoundingRect(c)).ToArray();

            contoursMask.Dispose();
        }
    }
}
