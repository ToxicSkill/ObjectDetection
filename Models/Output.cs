using ObjectDetection.Interfaces;
using OpenCvSharp;

namespace ObjectDetection.Models
{
    internal class Output : IDisposable, IOutput
    {
        private VideoWriter? _videoWriter;
        private bool _disposedValue;

        public string? OutputPath { get; private set; }

        public Output(string outputPath)
        {
            SetOutputPath(outputPath);
        }

        public void SetOutputPath(string outputPath)
        {
            if (string.IsNullOrEmpty(outputPath))
            {
                return;
            }

            OutputPath = outputPath;
        }

        public bool InitializeVideoWriter(Frame frame)
        {
            if (string.IsNullOrEmpty(OutputPath))
            {
                return false;
            }

            var dsize = new Size(frame.Width, frame.Height);
            _videoWriter = new VideoWriter(OutputPath + "outMovie.mp4", -1, frame.Fps, dsize);
            return true;
        }

        public bool WriteFrameToFile(Mat? frame, string fileName)
        {
            return frame is not null && frame.SaveImage(OutputPath + fileName);
        }

        public void WriteToMovieFile(Mat? frame)
        {
            if (_videoWriter != null)
            {
                if (!_videoWriter.IsDisposed && frame != null)
                {
                    _videoWriter.Write(frame.Clone());
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (_videoWriter != null)
                {
                    if (disposing)
                    {
                        _videoWriter.Dispose();
                    }
                }

                _disposedValue = true;
            }
        }
    }
}

