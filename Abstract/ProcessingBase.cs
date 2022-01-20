using OpenCvSharp;
using System.Collections.Concurrent;

namespace ObjectDetection.Abstract
{
    internal abstract class ProcessingBase : MatExtension
    {
        public ProcessingBase(int numberOfFrames)
        {
            NumberOfFrames = numberOfFrames;
            Frames = new ConcurrentQueue<Mat>();
        }

        public int NumberOfFrames { get; init; }

        protected ConcurrentQueue<Mat> Frames { get; init; }

        protected bool PreprareFrames(Mat? frame)
        {
            if (!IsNullOrEmpty(frame))
            {
                Frames.Enqueue(frame);
                if (Frames.Count > NumberOfFrames)
                {
                    return Frames.TryDequeue(out _);
                }
            }
            return false;
        }

        protected bool IsQueueFilled() => Frames.Count == NumberOfFrames;
    }
}
