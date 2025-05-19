using HexSlice.Domain;
using HexSlice.Ports;

namespace HexSlice.Application
{
    // Main application service that coordinates between the domain and ports
    public class SlicerApplication
    {
        private readonly IModelRepository _modelRepository;
        private readonly ISlicerService _slicerService;
        private readonly IGCodeOutput _gcodeOutput;

        public SlicerApplication(
            IModelRepository modelRepository,
            ISlicerService slicerService,
            IGCodeOutput gcodeOutput)
        {
            _modelRepository = modelRepository;
            _slicerService = slicerService;
            _gcodeOutput = gcodeOutput;
        }

        // Load a model, slice it, and save the G-code
        public async Task<string> SliceFileToGCodeAsync(string inputFilePath, string outputFilePath, SlicerSettings? settings = null)
        {
            // Load the model
            var model = await _modelRepository.LoadModelAsync(inputFilePath);

            // Validate the model
            bool isValid = await _slicerService.ValidateModelAsync(model);
            if (!isValid)
            {
                throw new InvalidOperationException("The model is not valid for slicing.");
            }

            // Use provided settings or get defaults
            settings ??= _slicerService.GetDefaultSettings(model);

            // Slice the model
            var gcode = await _slicerService.SliceModelAsync(model, settings);

            // Save the G-code
            await _gcodeOutput.SaveGCodeAsync(gcode, outputFilePath);

            // Return the G-code content for preview or further processing
            return _gcodeOutput.GetGCodeContent(gcode);
        }

        // Stream G-code directly to a printer
        public async Task StreamToprinterAsync(string inputFilePath, string printerConnection, SlicerSettings? settings = null, bool realTimeMode = false)
        {
            // Load the model
            var model = await _modelRepository.LoadModelAsync(inputFilePath);

            // Validate the model
            bool isValid = await _slicerService.ValidateModelAsync(model);
            if (!isValid)
            {
                throw new InvalidOperationException("The model is not valid for slicing.");
            }

            // Use provided settings or get defaults
            settings ??= _slicerService.GetDefaultSettings(model);

            // Slice the model
            var gcode = await _slicerService.SliceModelAsync(model, settings);

            // Stream the G-code to the printer
            await _gcodeOutput.StreamGCodeToPrinterAsync(gcode, printerConnection, realTimeMode);
        }
    }
}
