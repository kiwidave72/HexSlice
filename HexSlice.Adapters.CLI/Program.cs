using HexSlice.Adapters.CLI;
using HexSlice.Application;
using HexSlice.Domain;
using HexSlice.Ports;

namespace HexSlice.Adapters.CLI
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Parse command line arguments
                var cliOptions = CliParser.Parse(args);

                // Create mock implementations of our ports
                var modelRepository = new MockModelRepository();
                var slicerService = new MockSlicerService();
                var gcodeOutput = new MockGCodeOutput();

                // Create the core application service
                var slicerApplication = new SlicerApplication(
                    modelRepository,
                    slicerService,
                    gcodeOutput);

                // Create and initialize the CLI plugin
                var cliPlugin = new CliPlugin(cliOptions, slicerApplication);
                cliPlugin.Initialize();

                // Execute the CLI command
                return await cliPlugin.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
                return 1;
            }
        }
    }

    // Mock implementation of IModelRepository
    public class MockModelRepository : IModelRepository
    {
        public Task<Model> LoadModelAsync(string filePath)
        {
            Console.WriteLine($"Loading model from {filePath}");
            
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var modelType = GetModelTypeFromExtension(extension);
            
            var model = new Model
            {
                Name = Path.GetFileNameWithoutExtension(filePath),
                Type = modelType,
                Data = new byte[1024] // Dummy data
            };
            
            return Task.FromResult(model);
        }

        public Task SaveModelAsync(Model model, string filePath)
        {
            Console.WriteLine($"Saving model to {filePath}");
            return Task.CompletedTask;
        }

        public ModelType GetModelTypeFromExtension(string extension)
        {
            return extension switch
            {
                ".step" or ".stp" => ModelType.STEP,
                ".stl" => ModelType.STL,
                ".obj" => ModelType.OBJ,
                ".3mf" => ModelType.ThreeMF,
                _ => throw new NotSupportedException($"Unsupported file extension: {extension}")
            };
        }
    }

    // Mock implementation of ISlicerService
    public class MockSlicerService : ISlicerService
    {
        public Task<GCode> SliceModelAsync(Model model, SlicerSettings settings)
        {
            Console.WriteLine($"Slicing model {model.Name} with layer height {settings.LayerHeight}mm");
            
            // Generate more realistic G-code for the first layer of a cube
            var commands = new List<GCodeCommand>();
            
            // Start with standard initialization commands
            commands.Add(new GCodeCommand { Command = "M140", Parameters = new Dictionary<string, string> { { "S", "60" } }, Comment = "Set bed temperature" });
            commands.Add(new GCodeCommand { Command = "M190", Parameters = new Dictionary<string, string> { { "S", "60" } }, Comment = "Wait for bed temperature" });
            commands.Add(new GCodeCommand { Command = "M104", Parameters = new Dictionary<string, string> { { "S", "200" } }, Comment = "Set hotend temperature" });
            commands.Add(new GCodeCommand { Command = "M109", Parameters = new Dictionary<string, string> { { "S", "200" } }, Comment = "Wait for hotend temperature" });
            commands.Add(new GCodeCommand { Command = "G28", Comment = "Home all axes" });
            commands.Add(new GCodeCommand { Command = "G92", Parameters = new Dictionary<string, string> { { "E", "0" } }, Comment = "Reset extruder" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "Z", "0.3" }, { "F", "5000" } }, Comment = "Move to first layer height" });
            
            // Add first layer commands - simulating a 10x10 cube at origin
            // First, prime the extruder
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "0" }, { "Y", "0" }, { "F", "3000" } }, Comment = "Move to start position" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "E", "5" }, { "F", "200" } }, Comment = "Prime extruder" });
            commands.Add(new GCodeCommand { Command = "G92", Parameters = new Dictionary<string, string> { { "E", "0" } }, Comment = "Reset extruder position" });
            
            // Draw perimeter of the first layer (assuming 10x10 cube)
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "0" }, { "Y", "0" }, { "F", "3000" } }, Comment = "Move to corner 1" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "10" }, { "Y", "0" }, { "E", "0.4" }, { "F", "1500" } }, Comment = "Draw line to corner 2" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "10" }, { "Y", "10" }, { "E", "0.8" }, { "F", "1500" } }, Comment = "Draw line to corner 3" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "0" }, { "Y", "10" }, { "E", "1.2" }, { "F", "1500" } }, Comment = "Draw line to corner 4" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "0" }, { "Y", "0" }, { "E", "1.6" }, { "F", "1500" } }, Comment = "Draw line to corner 1" });
            
            // Add simple infill (just a few crossing lines)
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "Z", "0.3" }, { "F", "5000" } }, Comment = "Ensure we're at correct height" });
            
            // Diagonal infill lines
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "2" }, { "Y", "2" }, { "F", "3000" } }, Comment = "Move to infill start" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "8" }, { "Y", "8" }, { "E", "2.0" }, { "F", "1500" } }, Comment = "Infill line 1" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "2" }, { "Y", "8" }, { "F", "3000" } }, Comment = "Move to next infill start" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "8" }, { "Y", "2" }, { "E", "2.4" }, { "F", "1500" } }, Comment = "Infill line 2" });
            
            // Finish up
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "Z", "5" }, { "F", "5000" } }, Comment = "Lift nozzle" });
            commands.Add(new GCodeCommand { Command = "G1", Parameters = new Dictionary<string, string> { { "X", "0" }, { "Y", "0" }, { "F", "3000" } }, Comment = "Move to home position" });
            
            // Create the G-code content from commands
            var gcodeContent = string.Join("\n", commands.Select(cmd => cmd.ToString()));
            
            var gcode = new GCode
            {
                Content = gcodeContent,
                Flavor = GCodeFlavor.Marlin,
                Commands = commands
            };
            
            return Task.FromResult(gcode);
        }

        public SlicerSettings GetDefaultSettings(Model model)
        {
            return new SlicerSettings
            {
                // Default settings based on model type
                LayerHeight = model.Type == ModelType.STEP ? 0.1 : 0.2,
                InfillDensity = 0.2,
                WallCount = 3,
                GenerateSupport = false,
                PrintSpeed = 60.0
            };
        }

        public Task<bool> ValidateModelAsync(Model model)
        {
            // In a real implementation, this would check for issues like non-manifold edges,
            // proper dimensions, etc.
            Console.WriteLine($"Validating model {model.Name}");
            return Task.FromResult(true);
        }
    }

    // Mock implementation of IGCodeOutput
    public class MockGCodeOutput : IGCodeOutput
    {
        public Task SaveGCodeAsync(GCode gcode, string filePath)
        {
            Console.WriteLine($"Saving G-code to {filePath}");
            
            // In a real implementation, this would write the G-code to a file
            File.WriteAllText(filePath, GetGCodeContent(gcode));
            
            return Task.CompletedTask;
        }

        public Task StreamGCodeToPrinterAsync(GCode gcode, string printerConnection, bool realTimeMode = false)
        {
            Console.WriteLine($"Streaming G-code to printer at {printerConnection} (Real-time mode: {realTimeMode})");
            
            // In a real implementation, this would establish a connection to the printer
            // and stream the G-code commands
            
            return Task.CompletedTask;
        }

        public string GetGCodeContent(GCode gcode)
        {
            if (!string.IsNullOrEmpty(gcode.Content))
            {
                return gcode.Content;
            }
            
            // Generate content from commands if not already set
            return string.Join("\n", gcode.Commands.Select(cmd => cmd.ToString()));
        }
    }
}
