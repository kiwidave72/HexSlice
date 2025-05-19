namespace HexSlice.Adapters.CLI
{
    // Parser for command line arguments
    public class CliParser
    {
        // Parse command line arguments into a CliOptions object
        public static CliOptions Parse(string[] args)
        {
            var options = new CliOptions();

            // Show help if no arguments provided
            if (args.Length == 0)
            {
                options.ShowHelp = true;
                return options;
            }

            // Parse command
            options.Command = args[0].ToLower();

            // Parse remaining arguments
            for (int i = 1; i < args.Length; i++)
            {
                string arg = args[i];

                // Handle options
                if (arg.StartsWith("-") || arg.StartsWith("--"))
                {
                    switch (arg.TrimStart('-').ToLower())
                    {
                        case "o":
                        case "output":
                            if (i + 1 < args.Length)
                                options.OutputFile = args[++i];
                            break;

                        case "l":
                        case "layer-height":
                            if (i + 1 < args.Length && double.TryParse(args[++i], out double layerHeight))
                                options.LayerHeight = layerHeight;
                            break;

                        case "i":
                        case "infill":
                            if (i + 1 < args.Length && double.TryParse(args[++i], out double infill))
                                options.InfillPercentage = infill;
                            break;

                        case "s":
                        case "support":
                            options.GenerateSupport = true;
                            break;

                        case "r":
                        case "real-time":
                            options.RealTimeMode = true;
                            break;

                        case "h":
                        case "help":
                            options.ShowHelp = true;
                            break;
                    }
                }
                // Handle positional arguments
                else
                {
                    // First positional argument after command is input file
                    if (string.IsNullOrEmpty(options.InputFile))
                    {
                        options.InputFile = arg;
                    }
                    // Second positional argument for stream command is printer connection
                    else if (options.Command == "stream" && string.IsNullOrEmpty(options.PrinterConnection))
                    {
                        options.PrinterConnection = arg;
                    }
                    // Default output file name if not specified
                    else if (options.Command == "slice" && string.IsNullOrEmpty(options.OutputFile))
                    {
                        options.OutputFile = arg;
                    }
                }
            }

            return options;
        }
    }
}
