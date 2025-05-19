using HexSlice.Domain;

namespace HexSlice.Ports
{
    // Port for loading and saving 3D models
    public interface IModelRepository
    {
        // Load a model from a file
        Task<Model> LoadModelAsync(string filePath);
        
        // Save a model to a file
        Task SaveModelAsync(Model model, string filePath);
        
        // Get the type of model from a file extension
        ModelType GetModelTypeFromExtension(string extension);
    }
}
