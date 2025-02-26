# Azure Table Cleaner

A .NET 9 console application for bulk deletion of Azure Table Storage records.

## Features

- Delete Azure Table Storage records by partition key
- Delete records by partition key and row key combination
- Delete records using custom filter expressions
- Interactive command-line interface with validation
- Batched deletion for improved performance

## Requirements

- .NET 9 SDK
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

- `Program.cs` - Main entry point and application flow
- `Models/DeleteOptions.cs` - Data models for deletion options and results
- `Services/AzureTableService.cs` - Azure Table Storage operations
- `Utils/ConsoleHelper.cs` - Console UI helpers
- `Utils/InputValidator.cs` - Input validation logic

## Caution

Always double-check before confirming deletion operations, as they cannot be undone.
