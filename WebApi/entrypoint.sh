#!/bin/bash
set -e

# Apply migrations
dotnet ef database update

# Start the application
exec dotnet thundersApp.dll