namespace ObjectDetection.Interfaces
{
    internal interface IReader
    {
        Task ReadFrames(List<IAction> actions, IOutput output);

        void SetInputPath(string inputPath);
    }
}
