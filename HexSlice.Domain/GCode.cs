namespace HexSlice.Domain
{
    // Basic representation of G-code
    public class GCode
    {
        public string Content { get; set; } = string.Empty;
        public GCodeFlavor Flavor { get; set; } = GCodeFlavor.Marlin;
        public List<GCodeCommand> Commands { get; set; } = new List<GCodeCommand>();

        // In a real implementation, this would contain structured G-code data
        // and methods for manipulating and analyzing the G-code
    }

    public class GCodeCommand
    {
        public string Command { get; set; } = string.Empty;
        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
        public string Comment { get; set; } = string.Empty;

        public override string ToString()
        {
            string paramString = string.Join(" ", Parameters.Select(p => $"{p.Key}{p.Value}"));
            string commentStr = string.IsNullOrEmpty(Comment) ? "" : $" ; {Comment}";
            return $"{Command} {paramString}{commentStr}".Trim();
        }
    }

    public enum GCodeFlavor
    {
        Marlin,
        Repetier,
        Klipper,
        Prusa,
        Custom
    }
}
