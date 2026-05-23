# Repository Guidelines

## Project Structure & Module Organization
`src/ResultCore` contains the core result types, extensions, and task helpers. `src/ResultCore.Serialization` adds JSON, MessagePack, and Orleans integration while linking shared core types where needed. Tests live in `test/ResultCore.Tests` for core behavior and `test/ResultCore.Serialization.Tests` for serialization behavior. Build output is routed to `artifacts/`, NuGet packages are written to `nupkgs/`, and compiler-generated files appear under each project's `_Generated/` folder and should not be edited by hand.

## Build, Test, and Development Commands
Use the .NET CLI from the repository root:

- `dotnet build Result.sln -c Debug` builds all projects for local development.
- `dotnet test Result.sln -c Debug` runs both xUnit test projects.
- `dotnet test test/ResultCore.Serialization.Tests/ResultCore.Serialization.Tests.csproj --collect:"XPlat Code Coverage"` runs one suite with coverlet collection.
- `dotnet run --project test/ResultCore.Tests/ResultCore.Tests.csproj -c Release` runs the BenchmarkDotNet harness defined in `test/ResultCore.Tests/Program.cs`.
- `publish-nuget.bat` cleans, builds, packs, and pushes both packages; use it only for release publishing.

## Coding Style & Naming Conventions
Follow `.editorconfig`: UTF-8, final newline, 4-space indentation for C#, and 2 spaces for project, XML, and JSON files. Keep lines under 120 characters where practical. Use file-scoped namespaces, explicit accessibility, and braces on new lines. Prefer `var` when the type is obvious. Public types and members use PascalCase; interfaces must start with `I`. Do not manually edit generated sources under `_Generated/`.

## Testing Guidelines
Tests use xUnit with Shouldly assertions. Add tests in the matching test project and name files after the behavior under test, for example `JsonSerializationTest.cs` or `ResultTaskTest.cs`. Add regression coverage for every change to result semantics, converters, or serialization metadata. No repository-enforced coverage threshold is configured, so use coverlet locally when a change adds branching or serializer edge cases.

## Commit & Pull Request Guidelines
Recent commits use short imperative subjects such as `Add ResultConverterFactory`; release/version commits use `vX.Y.Z`. Keep each commit focused on one package or behavior change. Pull requests should summarize the user-visible change, list validation steps such as `dotnet test`, and include benchmark notes or payload examples when performance or serialization output changes. Link the related issue before merge when applicable.
