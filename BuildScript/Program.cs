using AtmaFileSystem;
using AtmaFileSystem.IO;
using AwesomeAssertions;
using BuildScript;
using NScan.Adapters.Secondary.NotifyingSupport;
using TddXt.NScan;
using static Bullseye.Targets;
using static DotnetExeCommandLineBuilder.DotnetExeCommands;
using static SimpleExec.Command;

const string configuration = "Release";
const string version = "8.0.0";

// Define directories.
var root = AbsoluteFilePath.OfThisFile().ParentDirectory(1).Value();
var nugetPath = root.AddDirectoryName("NuGets").AddDirectoryName(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Target("Clean", () =>
{
  Run("dotnet", Clean().Configuration(configuration),
    workingDirectory: root.ToString());
  if (nugetPath.Exists())
  {
    nugetPath.Delete(recursive: true);
  }
});

Target("Build", () =>
{
  Run("dotnet",
    Build()
      .Configuration(configuration)
      .WithArg($"-p:VersionPrefix={version}")
      .WithArg($"-p:SymbolPackageFormat=snupkg"),
    workingDirectory: root.ToString());

});

Target("NScan", dependsOn: ["Build"], () =>
{
  NScanMain.Run(
    new InputArgumentsDto
    {
      RulesFilePath = AbsoluteDirectoryPath.OfThisFile().AddFileName("rules.txt").AsAnyFilePath(),
      SolutionPath = root.AddFileName("Core.Maybe.sln").AsAnyFilePath()
    },
    new ConsoleOutput(),
    new ConsoleSupport(Console.WriteLine)
  ).Should().Be(0);
});

Target("Test", dependsOn: ["NScan"], () =>
{
  Run(
    "dotnet",
    Test("Core.Maybe.NTests/Core.Maybe.NTests.csproj")
      .NoBuild()
      .Configuration(configuration)
      .WithArg($"-p:VersionPrefix={version}"),
    workingDirectory: root.ToString());
});

Target("Pack", dependsOn: ["Clean", "Test"], () =>
{
  Run("dotnet", Pack()
      .NoBuild()
      .Configuration(configuration)
      .WithArg($"-p:VersionPrefix={version}")
      .Output(nugetPath),
      workingDirectory: root.ToString());
});

Target("Push", dependsOn: ["Pack"], () =>
{
  foreach (var nupkgPath in nugetPath.GetFiles("*.nupkg"))
  {
    Run("dotnet", NugetPush(nupkgPath).Source("https://api.nuget.org/v3/index.json"));
  }
});

Target("default", dependsOn: ["Test"]);

await RunTargetsAndExitAsync(args);