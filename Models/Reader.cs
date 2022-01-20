using ObjectDetection.Interfaces;
using OpenCvSharp;
using System.Threading.Tasks.Dataflow;

namespace ObjectDetection.Models
{
    internal class Reader : IReader
    {
        private const int Miliseconds = 1000;
        private const int ParallelismDegree = 3;

        private readonly IProcessor _processor;
        private readonly FrameContext _context;
        private TransformBlock<Mat, Mat> _pipeline;

        public string InputPath { get; private set; }

        public Reader(string inputPath)
        {
            SetInputPath(inputPath);
            _processor = new Processor();
            _context = new FrameContext();
        }

        public void SetInputPath(string inputPath)
        {
            if (string.IsNullOrEmpty(inputPath))
            {
                return;
            }

            if (!File.Exists(inputPath))
            {
                return;
            }

            InputPath = inputPath;
        }

        public async Task ReadFrames(List<IAction> actions, IOutput output)
        {
            using var capture = new VideoCapture(InputPath);
            if (!output.InitializeVideoWriter(new Frame(capture.FrameWidth, capture.FrameHeight, capture.Fps)))
                return;
            _pipeline = CreatePipeline(actions, output);
            var interval = (int)(Miliseconds / capture.Fps);
            using var image = new Mat();
            while (true)
            {
                capture.Read(image);
                if (image.Empty())
                {
                    break;
                }

                Thread.Sleep(interval);
                await _pipeline.SendAsync(image);
            }
            _pipeline.Complete();
            ((IDisposable)output).Dispose();
        }

        private TransformBlock<Mat, Mat> CreatePipeline(List<IAction> actions, IOutput output)
        {
            var options = new ExecutionDataflowBlockOptions() { MaxDegreeOfParallelism = ParallelismDegree };

            var actionBlock = new TransformBlock<Mat, Mat>((image) => ExecuteActions(actions, image), options);
            var moveBlock = new TransformBlock<Mat, Mat?>((image) => _processor.DrawMoveFocus(image, _context), options);
            var combineBlock = new TransformBlock<Mat?, Mat?>((frame) => _processor.DrawMoveRectangles(frame, _context), options);
            var saveBlock = new ActionBlock<Mat?>((image) =>
            {
                output.WriteFrameToFile(image, "focus.jpg");
                output.WriteToMovieFile(image);
            }, options);

            DataflowLinkOptions linkOptions = new() { PropagateCompletion = true };

            actionBlock.LinkTo(moveBlock, new DataflowLinkOptions());
            moveBlock.LinkTo(combineBlock, new DataflowLinkOptions());
            combineBlock.LinkTo(saveBlock, new DataflowLinkOptions());

            return actionBlock;
        }

        private Mat ExecuteActions(List<IAction> actions, Mat image)
        {
            foreach (var action in actions)
            {
                action.Execute(image.Clone(), _context);
            }
            return image;
        }
    }
}

