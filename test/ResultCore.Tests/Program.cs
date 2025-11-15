using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ResultCore.Tests;

public static class Program
{

    #region Constants & Statics

    public static void Main()
    {
        var a = Benchmarks.ReturnRef_2();

        var b = Benchmarks.Return_2();
        var b2 = Benchmarks.Return_3();

        var c = Benchmarks.Return_FileError();

        //var a = DefaultConfig.Instance.ArtifactsPath;
        //D:\Project\Result\artifacts\bin\ResultCore.Tests\release\BenchmarkDotNet.Artifacts

        var config = DefaultConfig.Instance
            .WithArtifactsPath(
                $".\\BenchmarkDotNet.Aritfacts.{DateTime.Now.ToString("u").Replace(' ', '_').Replace(':', '-')}")
            .AddExporter(MarkdownExporter.GitHub)
            .AddDiagnoser(MemoryDiagnoser.Default)
            .AddJob(Job.ShortRun.WithLaunchCount(1));

        _ = BenchmarkRunner.Run<Benchmarks>(config);
    }

    #endregion

}
