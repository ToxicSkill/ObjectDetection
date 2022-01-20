using ObjectDetection.Abstract;
using OpenCvSharp;
using System.Collections.Concurrent;

namespace ObjectDetection.Processing
{
    internal class Preprocess : MatExtension
    {
        public static Mat? GetDiffrence(ConcurrentQueue<Mat> framesQue)
        {
            Mat? currentDiff = new ();
            framesQue.TryDequeue(out Mat? first);
            if (IsNullOrEmpty(first))
            {
                return default;
            }

            Cv2.CvtColor(first, first, ColorConversionCodes.RGB2GRAY);
            var diff = new Mat();

            int length = framesQue.Count;

            for (int i = 0; i < length; i++)
            {
                if (i != 0)
                {
                    first = diff.Clone();
                }

                framesQue.TryDequeue(out currentDiff);
                if (IsNullOrEmpty(currentDiff))
                {
                    break;
                }
                Cv2.CvtColor(currentDiff, currentDiff, ColorConversionCodes.RGB2GRAY);
                Cv2.Absdiff(first, currentDiff, diff);
            }

            if (IsNullOrEmpty(currentDiff))
            {
                return default;
            }

            Cv2.BitwiseAnd(first, currentDiff, diff);
            currentDiff.Dispose();
            first.Dispose();

            return diff;
        }
    }
}
