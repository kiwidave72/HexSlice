using HexSlice.Domain;

namespace HexSlice.Ports
{
    // Port for slicing operations
    public interface ISlicerService
    {
        // Slice a model into G-code using the provided settings
        Task<GCode> SliceModelAsync(Model model, SlicerSettings settings);
        
        // Get default settings for a specific model
        SlicerSettings GetDefaultSettings(Model model);
        
        // Validate if a model can be sliced
        Task<bool> ValidateModelAsync(Model model);
    }
}
