namespace HexSlice.Adapters.CLI
{
    // Class to hold CLI options
    public class CliOptions
    {
        // Command to execute (slice, stream, etc.)
        public string Command { get; set; } = string.Empty;
        
        // Input file path
        public string InputFile { get; set; } = string.Empty;
        
        // Output file path (for slice command)
        public string OutputFile { get; set; } = string.Empty;
        
        // Printer connection string (for stream command)
        public string PrinterConnection { get; set; } = string.Empty;
        
        // Layer height in mm
        public double LayerHeight { get; set; } = 0.2;
        
        // Infill density percentage
        public double InfillPercentage { get; set; } = 20.0;
        
        // Whether to generate support structures
        public bool GenerateSupport { get; set; } = false;
        
        // Whether to use real-time streaming mode
        public bool RealTimeMode { get; set; } = false;
        
        // Whether to show help
        public bool ShowHelp { get; set; } = false;
    }
}
