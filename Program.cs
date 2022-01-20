using ObjectDetection.Interfaces;
using ObjectDetection.Models;
using ObjectDetection.Processing;

const string _inputPath = @"C:\Users\Adam\Desktop\WŁASNE_PROGRAMY\C#\ObjectDetection\Input\vid1.mp4";
const string _outputPath = @"C:\Users\Adam\Desktop\WŁASNE_PROGRAMY\C#\ObjectDetection\Output\";

IReader reader = new Reader(_inputPath);
IOutput output = new Output(_outputPath);
var actions = new List<IAction>()
            {
               new MoveDetection(),
               new SharpnessDetection(),
               new ContoursDetecion(),
               new SubstractBackground()
            };

await reader.ReadFrames(actions, output);