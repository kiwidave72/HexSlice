using HexSlice.Application;
using HexSlice.Domain;
using HexSlice.Ports;

namespace HexSlice.Adapters.CLI
{
    // CLI plugin implementation
    public class CliPlugin : IPlugin
    {
        public string Id => "HexSlice.CLI";
        public string Name => "Command Line Interface";
        public string Version => "1.0.0";

        private readonly CliOptions _options;
        private readonly SlicerApplication _slicerApplication;

        public CliPlugin(CliOptions options, SlicerApplication slicerApplication)
        {
            _options = options;
            _slicerApplication = slicerApplication;
        }

        public void Initialize()
        {
            // Plugin initialization logic
            Console.WriteLine($"Initializing {Name} plugin v{Version}");
        }

        public void Shutdown()
        {
            // Plugin cleanup logic
            Console.WriteLine($"Shutting down {Name} plugin");
        }

        // Execute the CLI command based on the provided options
        public async Task<int> ExecuteAsync()
        {
            try
            {
                // Display help if requested
                if (_options.ShowHelp)
                {
                    DisplayHelp();
                    return 0;
                }

                // Validate required options
                if (string.IsNullOrEmpty(_options.InputFile))
                {
                    Console.Error.WriteLine("Error: Input file is required.");
                    DisplayHelp();
                    return 1;
                }

                // Execute the appropriate command
                if (_options.Command == "slice")
                {
                    await ExecuteSliceCommandAsync();
                }
                else if (_options.Command == "stream")
                {
                    await ExecuteStreamCommandAsync();
                }
                else
                {
                    Console.Error.WriteLine($"Error: Unknown command '{_options.Command}'.");
                    DisplayHelp();
                    return 1;
                }

                return 0;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return 1;
            }
        }

        private async Task ExecuteSliceCommandAsync()
        {
            Console.WriteLine($"Slicing {_options.InputFile} to {_options.OutputFile}...");
            
            // Convert CLI options to SlicerSettings
            var settings = new SlicerSettings
            {
                LayerHeight = _options.LayerHeight,
                InfillDensity = _options.InfillPercentage / 100.0, // Convert percentage to decimal
                GenerateSupport = _options.GenerateSupport
            };
            
            // Use the SlicerApplication to perform the slicing
            try
            {
                string gcodeContent = await _slicerApplication.SliceFileToGCodeAsync(
                    _options.InputFile,
                    _options.OutputFile,
                    settings);
                
                Console.WriteLine("Slicing completed successfully.");
                Console.WriteLine($"G-code file created: {_options.OutputFile}");
                Console.WriteLine($"G-code preview (first few lines):");
                Console.WriteLine(gcodeContent.Substring(0, Math.Min(gcodeContent.Length, 200)) + "...");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to slice file: {ex.Message}", ex);
            }
        }

        private async Task ExecuteStreamCommandAsync()
        {
            if (string.IsNullOrEmpty(_options.PrinterConnection))
            {
                throw new InvalidOperationException("Printer connection is required for streaming.");
            }

            Console.WriteLine($"Streaming {_options.InputFile} to printer at {_options.PrinterConnection}...");
            
            // Convert CLI options to SlicerSettings
            var settings = new SlicerSettings
            {
                LayerHeight = _options.LayerHeight,
                InfillDensity = _options.InfillPercentage / 100.0, // Convert percentage to decimal
                GenerateSupport = _options.GenerateSupport
            };
            
            // Use the SlicerApplication to perform the streaming
            try
            {
                await _slicerApplication.StreamToprinterAsync(
                    _options.InputFile,
                    _options.PrinterConnection,
                    settings,
                    _options.RealTimeMode);
                
                Console.WriteLine("Streaming started successfully.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to stream to printer: {ex.Message}", ex);
            }
        }

        private void DisplayHelp()
        {
            Console.WriteLine("HexSlice - 3D Printer Slicer CLI");
            Console.WriteLine("Usage:");
            Console.WriteLine("  hexslice slice <input-file> [options]");
            Console.WriteLine("  hexslice stream <input-file> <printer-connection> [options]");
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("  slice    Slice a 3D model file to G-code");
            Console.WriteLine("  stream   Slice a 3D model and stream directly to a printer");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  -o, --output <file>       Output G-code file path (for slice command)");
            Console.WriteLine("  -l, --layer-height <mm>   Layer height in mm (default: 0.2)");
            Console.WriteLine("  -i, --infill <percent>    Infill density percentage (default: 20)");
            Console.WriteLine("  -s, --support             Generate support structures");
            Console.WriteLine("  -r, --real-time           Use real-time streaming mode");
            Console.WriteLine("  -h, --help                Show this help message");
        }
    }
}
