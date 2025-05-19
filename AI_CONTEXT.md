# HexSlice - AI Context Document

This document serves as a comprehensive reference for the HexSlice 3D printer slicer application, collating requirements, features, architecture decisions, and implementation details in a single place.

## Project Overview

HexSlice is a .NET Core C# application designed to convert 3D model files (primarily STEP files) into G-code for 3D printers. The application implements a hexagonal architecture pattern (ports and adapters) to create a modular, extensible system with a plugin-based approach.

## Requirements and Features

### Core Requirements

1. **File Handling**
   - STEP file import (.step, .stp)
   - STL file import (for compatibility)
   - 3MF file import (for compatibility)
   - OBJ file import (for compatibility)
   - G-code generation compatible with major 3D printer firmware
   - Support for different flavors of G-code (Marlin, Repetier, Klipper, etc.)

2. **Model Processing**
   - STEP to mesh conversion with adjustable tessellation resolution
   - Feature recognition for supports and potential print issues
   - Model repair and optimization

3. **Slicing Engine**
   - Layer height configuration (variable and fixed)
   - Infill patterns and density settings
   - Wall thickness and count
   - Support structure generation and customization
   - Multi-material and multi-extruder support

4. **Real-time Capabilities**
   - Stream G-code to printer in real-time during printing
   - Dynamic slicing with ability to modify upcoming layers while printing
   - Feedback loop system for adaptive slicing based on print quality

5. **User Interface**
   - Command-line interface (CLI) as a plugin
   - STEP file visualization with feature recognition and highlighting
   - G-code visualization with syntax highlighting and 3D toolpath visualization
   - Future GUI implementation planned

### Technical Requirements

1. **Implementation Language**
   - .NET Core C#
   - Cross-platform compatibility

2. **Architecture**
   - Hexagonal (Ports and Adapters) architecture pattern
   - Clear separation of domain logic from external concerns
   - Domain-driven design principles
   - Well-defined interfaces (ports) for all external interactions
   - Pluggable adapters for different implementations

## Current Implementation

### Project Structure

The solution is organized according to the hexagonal architecture pattern:

- **HexSlice.Domain**: Contains the core domain models and business logic
  - Model.cs: Represents 3D models with different file formats
  - GCode.cs: Represents G-code with commands and parameters
  - SlicerSettings.cs: Contains settings for the slicing process

- **HexSlice.Ports**: Defines the interfaces (ports) that connect the domain to the outside world
  - IModelRepository.cs: Interface for loading and saving 3D models
  - ISlicerService.cs: Interface for slicing operations
  - IGCodeOutput.cs: Interface for G-code output operations
  - IPlugin.cs: Interface for plugin functionality

- **HexSlice.Application**: Contains the application services that coordinate between the domain and ports
  - SlicerApplication.cs: Main application service for slicing operations
  - PluginManager.cs: Manages plugins for the application

- **HexSlice.Adapters.CLI**: Command-line interface adapter implemented as a plugin
  - CliPlugin.cs: CLI plugin implementation
  - CliOptions.cs: Class to hold CLI options
  - CliParser.cs: Parser for command line arguments
  - Program.cs: Entry point with mock implementations

### Implemented Features

1. **Hexagonal Architecture**
   - Clear separation of concerns with domain, ports, and adapters
   - Plugin system through the IPlugin interface
   - Mock implementations for demonstration purposes

2. **CLI Adapter**
   - Command-line interface for slicing operations
   - Support for various options (layer height, infill percentage, etc.)
   - Commands for slicing and streaming to printer

3. **Basic Slicing**
   - Mock implementation that generates G-code for the first layer of a cube
   - Simulated processing of STEP files
   - G-code generation with proper syntax and comments

### Mock Implementations

The current implementation includes mock adapters for demonstration purposes:

1. **MockModelRepository**: Simulates loading models from files
2. **MockSlicerService**: Generates G-code for the first layer of a cube
3. **MockGCodeOutput**: Writes G-code to files or simulates streaming to a printer

## Future Development

### Planned Features

1. **Real STEP File Processing**
   - Integration with a STEP file parsing library
   - Actual tessellation of STEP geometry
   - Feature recognition for supports and print issues

2. **Advanced Slicing**
   - Implementation of actual slicing algorithms
   - Support for variable layer heights
   - Adaptive infill based on structural requirements

3. **Additional Adapters**
   - GUI adapter for interactive use
   - Web API adapter for remote slicing operations
   - Integration with OctoPrint and other print servers

4. **Real-time Features**
   - Actual implementation of G-code streaming
   - Feedback loop for adaptive slicing
   - Integration with printer sensors

### Technical Roadmap

1. **Short-term**
   - Replace mock implementations with real ones
   - Add unit and integration tests
   - Improve error handling and logging

2. **Medium-term**
   - Implement GUI adapter
   - Add support for more file formats
   - Enhance G-code generation with optimization

3. **Long-term**
   - Implement real-time streaming and feedback
   - Add machine learning for print quality prediction
   - Develop cloud-based services for remote slicing

## GitHub Repository

The project is hosted on GitHub at: https://github.com/kiwidave72/HexSlice

## References

1. Hexagonal Architecture (Ports and Adapters) Pattern
   - https://alistair.cockburn.us/hexagonal-architecture/
   - https://netflixtechblog.com/ready-for-changes-with-hexagonal-architecture-b315ec967749

2. STEP File Format
   - ISO 10303-21 standard
   - https://www.steptools.com/stds/step/step_standard.html

3. G-code Reference
   - https://marlinfw.org/meta/gcode/
   - https://reprap.org/wiki/G-code

---

*This document serves as a living reference for the HexSlice project and should be updated as the project evolves.*
