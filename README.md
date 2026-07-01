# Azure Table Cleaner

A .NET 10 console application for bulk deletion of Azure Table Storage records.

## Features

- Delete Azure Table Storage records by partition key
- Delete records by partition key and row key combination
- Delete records using custom filter expressions
- Interactive command-line interface with validation
- Batched deletion for improved performance

## Requirements

- .NET 10 SDK
- Azure Storage account

## Usage

1. Build the application:
   ```
   dotnet build
   ```

2. Run the application:
   ```
   dotnet run
   ```

3. Follow the prompts to enter:
   - Azure Storage Connection String
   - Table Name
   - Filtering criteria

## Project Structure

- `Program.cs` - Composition root: dependency injection wiring and startup
- `Services/CleanerApplication.cs` - Orchestrates the interactive workflow
- `Services/AzureTableService.cs` - Azure Table Storage operations
- `Models/` - Data models (`DeleteOptions`, `DeleteResult`, `FilterType`)
- `Utils/ConsoleHelper.cs` - Console UI helpers
- `Utils/InputValidator.cs` - Input validation logic

## Caution

Always double-check before confirming deletion operations, as they cannot be undone.
