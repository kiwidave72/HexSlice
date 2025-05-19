namespace HexSlice.Domain
{
    // Basic representation of a 3D model
    public class Model
    {
        public string Name { get; set; } = string.Empty;
        public ModelType Type { get; set; }
        public byte[] Data { get; set; } = Array.Empty<byte>();

        // In a real implementation, this would contain the actual 3D model data
        // such as vertices, faces, etc.
    }

    public enum ModelType
    {
        STEP,
        STL,
        OBJ,
        ThreeMF
    }
}
