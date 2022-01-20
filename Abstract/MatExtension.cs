using OpenCvSharp;

namespace ObjectDetection.Abstract
{
    public abstract class MatExtension
    {
        protected static bool IsNullOrEmpty(Mat? mat)
        {
            if (mat != null)
            {
                if (!mat.Empty())
                {
                    return false;
                }
            }
            return true;
        }
    }
}
