using ObjectDetection.Interfaces;
using OpenCvSharp;

namespace ObjectDetection.Models
{
    internal class Processor : IProcessor
    {
        public Mat? DrawMoveRectangles(Mat? frame, FrameContext context)
        {
            if (context.AllReady() && frame != null && context.BackgroundMask != null)
            {
                if (!frame.Empty() && !context.BackgroundMask.Empty())
                {
                    using (var rectalnglesFrame = frame.Clone())
                    {
                        var percent = context.Sharpness / 255;
                        Cv2.PutText(rectalnglesFrame, string.Format("{0:0.00} %", (1 - percent) * 100), new Point(40, 50), HersheyFonts.Italic, 0.8, new Scalar(80, 255, 30), 3);

                        foreach (var cont in context.Rectangles)
                        {
                            Cv2.Rectangle(rectalnglesFrame, cont, new Scalar(255, 255, 120));
                        }

                        return rectalnglesFrame.Clone();
                    }
                }
            }

            return null;
        }

        public Mat? DrawMoveFocus(Mat? frame, FrameContext context)
        {
            if (context.AllReady() && frame != null && context.BackgroundMask != null)
            {
                if (!frame.Empty() && !context.BackgroundMask.Empty())
                    return frame & context.BackgroundMask;
            }
            return null;
        }
    }
}
