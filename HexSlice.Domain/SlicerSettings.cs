namespace HexSlice.Domain
{
    // Basic representation of slicer settings
    public class SlicerSettings
    {
        public double LayerHeight { get; set; } = 0.2; // mm
        public int WallCount { get; set; } = 3;
        public double InfillDensity { get; set; } = 0.2; // 20%
        public InfillPattern InfillPattern { get; set; } = InfillPattern.Grid;
        public bool GenerateSupport { get; set; } = false;
        public double SupportAngle { get; set; } = 45.0; // degrees
        public double PrintTemperature { get; set; } = 200.0; // Celsius
        public double PrintSpeed { get; set; } = 60.0; // mm/s

        // In a real implementation, this would contain many more settings
    }

    public enum InfillPattern
    {
        Grid,
        Triangles,
        Honeycomb,
        Cubic,
        Gyroid,
        Lines,
        Concentric
    }
}
