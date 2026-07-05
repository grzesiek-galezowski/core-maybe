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
using var cancellationTokenSource = new CancellationTokenSource();
var cancellationToken = cancellationTokenSource.Token;


//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Target("Clean", async () =>
{
  await RunAsync("dotnet", Clean().Configuration(configuration),
    workingDirectory: root.ToString(), ct: cancellationToken);
  if (nugetPath.Exists())
  {
    nugetPath.Delete(recursive: true);
  }
});

Target("Build", async () =>
{
  await RunAsync("dotnet",
    Build()
      .Configuration(configuration)
      .WithArg($"-p:VersionPrefix={version}")
      .WithArg($"-p:SymbolPackageFormat=snupkg"),
    workingDirectory: root.ToString(), ct: cancellationToken);

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

Target("Test", dependsOn: ["NScan"], async () =>
{
  await RunAsync(
    "dotnet",
    Test("Core.Maybe.NTests/Core.Maybe.NTests.csproj")
      .NoBuild()
      .Configuration(configuration)
      .WithArg($"-p:VersionPrefix={version}"),
    workingDirectory: root.ToString(), ct: cancellationToken);
});

Target("Pack", dependsOn: ["Clean", "Test"], async () =>
{
  await RunAsync("dotnet", Pack()
      .NoBuild()
      .Configuration(configuration)
      .WithArg($"-p:VersionPrefix={version}")
      .Output(nugetPath),
      workingDirectory: root.ToString(), ct: cancellationToken);
});

Target("Push", dependsOn: ["Pack"], async () =>
{
  foreach (var nupkgPath in nugetPath.GetFiles("*.nupkg"))
  {
    await RunAsync("dotnet", NugetPush(nupkgPath).Source("https://api.nuget.org/v3/index.json"), ct: cancellationToken);
  }
});

Target("default", dependsOn: ["Test"]);

await RunTargetsAndExitAsync(args, ex => ex is SimpleExec.ExitCodeException);