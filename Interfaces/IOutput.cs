using ObjectDetection.Models;
using OpenCvSharp;

namespace ObjectDetection.Interfaces
{
    internal interface IOutput
    {
        bool WriteFrameToFile(Mat? frame, string fileName);

        void WriteToMovieFile(Mat? frame);

        bool InitializeVideoWriter(Frame frame);

        void SetOutputPath(string outputPath);
    }
}
