#!/usr/bin/env bash

# Exit on any error
set -e

# Build Solution
cd src
dotnet restore
dotnet build

# Execute Unit tests
# cd tests
# dotnet restore
# dotnet xunit
