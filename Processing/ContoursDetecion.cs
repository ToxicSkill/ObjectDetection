using ObjectDetection.Abstract;
using ObjectDetection.Interfaces;
using ObjectDetection.Models;
using OpenCvSharp;
namespace ObjectDetection.Processing
{
    internal class ContoursDetecion : ProcessingBase, IAction
    {
        private const int FirstBlurringPower = 11;
        private const int SecondBlurringPower = 43;

        private readonly double[,] _morphologyKernel =
        {
            { 1, 1, 1 },
            { 1, 2, 1 },
            { 1, 1, 1 }
        };

        public ContoursDetecion()
        : base(3)
        {
        }

        public void Execute(Mat frame, FrameContext context)
        {
            if (PreprareFrames(frame) && IsQueueFilled())
            {
                DetectContours(context);
            }
        }

        private void DetectContours(FrameContext context)
        {
            using var diff = Preprocess.GetDiffrence(Frames);
            if (!IsNullOrEmpty(diff))
            {
                Cv2.MedianBlur(diff, diff, FirstBlurringPower);
                Cv2.Laplacian(diff, diff, MatType.CV_8UC1, delta: 1);
                var sharpness = diff.Mean()[0];
                Cv2.MorphologyEx(diff, diff, MorphTypes.Close, null);
                Cv2.MedianBlur(diff, diff, SecondBlurringPower);


                Cv2.Threshold(diff, diff, 0, 255, ThresholdTypes.Otsu | ThresholdTypes.Binary);
                Cv2.MorphologyEx(diff, diff, MorphTypes.Dilate, InputArray.Create(_morphologyKernel));
                Cv2.FindContours(diff, out Point[][] contours, out _, RetrievalModes.CComp, ContourApproximationModes.ApproxTC89L1);

                context.Rectangles = contours.Select(c => Cv2.BoundingRect(c)).ToArray();
            }
        }
    }
}
