# HexSlice - 3D Printer Slicer with Hexagonal Architecture

HexSlice is a .NET Core C# application for slicing 3D models (STEP, STL, etc.) into G-code for 3D printers. It implements the hexagonal architecture pattern (also known as ports and adapters) to create a modular, extensible application with a plugin system.

## Project Structure

The solution is organized according to the hexagonal architecture pattern:

- **HexSlice.Domain**: Contains the core domain models and business logic
- **HexSlice.Ports**: Defines the interfaces (ports) that connect the domain to the outside world
- **HexSlice.Application**: Contains the application services that coordinate between the domain and ports
- **HexSlice.Adapters.CLI**: Command-line interface adapter implemented as a plugin

## Hexagonal Architecture

The hexagonal architecture (also known as ports and adapters) is a software design pattern that allows an application to be equally driven by users, programs, automated tests, or batch scripts, and to be developed and tested in isolation from its eventual run-time devices and databases.

### Key Components

1. **Domain**: The core business logic and entities (Model, GCode, SlicerSettings)
2. **Ports**: Interfaces that define how the domain interacts with the outside world
3. **Adapters**: Implementations of the ports that connect to specific technologies
4. **Application**: Services that coordinate the flow of data between adapters and the domain

### Benefits for HexSlice

- **Modularity**: Each component can be developed and tested independently
- **Testability**: The domain logic can be tested without external dependencies
- **Flexibility**: New adapters (UI, CLI, API) can be added without changing the core logic
- **Plugin System**: The architecture naturally supports a plugin system through adapters

## CLI Plugin

The CLI plugin is the first adapter implemented for HexSlice. It provides a command-line interface for slicing 3D models and streaming G-code to printers.

### Usage

```
HexSlice - 3D Printer Slicer CLI
Usage:
  hexslice slice <input-file> [options]
  hexslice stream <input-file> <printer-connection> [options]

Commands:
  slice    Slice a 3D model file to G-code
  stream   Slice a 3D model and stream directly to a printer

Options:
  -o, --output <file>       Output G-code file path (for slice command)
  -l, --layer-height <mm>   Layer height in mm (default: 0.2)
  -i, --infill <percent>    Infill density percentage (default: 20)
  -s, --support             Generate support structures
  -r, --real-time           Use real-time streaming mode
  -h, --help                Show this help message
```

## Future Extensions

The hexagonal architecture makes it easy to extend HexSlice with new features:

1. **GUI Adapter**: A graphical user interface can be added as another adapter
2. **Web API Adapter**: A web API for remote slicing operations
3. **Additional Plugins**: Support for different file formats, slicing algorithms, etc.
4. **Real-time Feedback**: Implement the feedback loop for adaptive slicing

## Building and Running

### Prerequisites

- .NET Core SDK 9.0 or later

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run --project HexSlice.Adapters.CLI/HexSlice.Adapters.CLI.csproj -- slice input.stl -o output.gcode
```

## License

MIT
