using HexSlice.Domain;

namespace HexSlice.Ports
{
    // Port for G-code output operations
    public interface IGCodeOutput
    {
        // Save G-code to a file
        Task SaveGCodeAsync(GCode gcode, string filePath);
        
        // Stream G-code directly to a printer
        Task StreamGCodeToPrinterAsync(GCode gcode, string printerConnection, bool realTimeMode = false);
        
        // Get G-code as a string
        string GetGCodeContent(GCode gcode);
    }
}
