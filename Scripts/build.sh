#!/bin/bash
# Sport-Net Build Script
# Builds the entire solution in Release configuration.
# Run from the repository root or from any directory.

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
SOLUTION_ROOT="$(dirname "$SCRIPT_DIR")"

echo "🔨 Building Sport-Net solution..."
dotnet restore "$SOLUTION_ROOT/Sport.sln"
dotnet build "$SOLUTION_ROOT/Sport.sln" -c Release --no-restore
echo "✅ Build succeeded."
