using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace ResultCore.Tests;

public static class Program
{

    #region Constants & Statics

    public static void Main()
    {
        //var a = DefaultConfig.Instance.ArtifactsPath;
        //D:\Project\Result\artifacts\bin\ResultCore.Tests\release\BenchmarkDotNet.Artifacts

        _ = BenchmarkRunner.Run<Benchmarks>(
            DefaultConfig.Instance
                .WithArtifactsPath(
                    Path.Combine(
                            AppContext.BaseDirectory,
                            "../../../../test/ResultCore.Tests",
                            "BenchmarkDotNet.Artifacts",
                            DateTime.Now.ToString("yyyy-MM-dd")))
                .AddExporter(MarkdownExporter.Default)
                .AddJob(Job.ShortRun.WithLaunchCount(1)));
    }

    #endregion

}
