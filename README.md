# Sh.Converter Console Application

## Overview
Sh.Converter Console is a .NET 9 console application that converts structured text input into an XML document. The application reads lines from a source file, parses them into nodes and edges based on configurable relation definitions, and constructs an XML document representing a hierarchical structure.

## Features
- **Text-to-XML Conversion:**  
  Transforms lines of text into XML elements using a defined mapping (nodes and edges).
  
- **Configurable Relations:**  
  Uses relation definitions specified in configuration to determine how input lines map to node types and hierarchical relationships.  
  
- **Hierarchical Processing:**  
  Maintains a parent-child relationship using a stack to build nested XML elements.

- **Visual Feedback:**  
  Displays ASCII art on startup and prints the constructed XML document to the console in color.

- **Robust Logging:**  
  Uses logging to capture errors and conversion status.

## Configuration
- **SettingsOptions:**  
  - `SourceFilePath`: Path to the text file containing input data.
  - `SourceSeparator`: Delimiter to separate key and field values on each line.
  - `OutputDocumentRoot`: Root element name for the generated XML document.
  
- **RelationOptions:**  
  - `Relations`: List of relation definitions used to map input keys to node types and determine parent-child relationships.
  
## How It Works
1. **Initialization:**  
   The application uses dependency injection with the options pattern to load settings and relation definitions.

2. **Reading Input:**  
   It reads through the source file using a filtering predicate that identifies valid starting lines.

3. **Parsing and Mapping:**  
   - The `RelationMapper` parses input lines based on the specified separator.
   - It builds nodes (with unique IDs) and edges according to the relation definitions and maintains hierarchy using a parent stack.

4. **XML Document Construction:**  
   - The `XmlBuilder` creates an XML document using the generated nodes and edges.
   - XML elements are recursively built to mirror the hierarchical relationships.

5. **Output:**  
   The final XML document is displayed on the console.

## Build and Run
- **Prerequisites:** .NET 9 SDK
- **Build:**  
  Use Visual Studio 2022 or run `dotnet build` from the command line.
- **Run:**  
  Execute the application via Visual Studio or run `dotnet run` in the project directory.

## Logging
Error handling and status messages are logged using the built-in logging infrastructure. In case of conversion errors, detailed log messages are generated.

## License
This application follows the licensing terms provided in the project's documentation.
