```

BenchmarkDotNet v0.15.6, Windows 10 (10.0.19045.6332/22H2/2022Update)
12th Gen Intel Core i7-12800HX 2.00GHz, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.307
  [Host]   : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3
  ShortRun : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method      | Mean       | Error     | StdDev   | Ratio | RatioSD | Gen0      | Allocated  | Alloc Ratio |
|------------ |-----------:|----------:|---------:|------:|--------:|----------:|-----------:|------------:|
| Ref1        | 2,180.4 μs | 332.29 μs | 18.21 μs | 10.05 |    0.08 | 3058.5938 | 40000000 B |          NA |
| Ref2        | 2,232.2 μs | 863.02 μs | 47.31 μs | 10.29 |    0.19 | 3058.5938 | 40000000 B |          NA |
| Ref3        | 2,163.4 μs | 737.99 μs | 40.45 μs |  9.97 |    0.17 | 3058.5938 | 40000000 B |          NA |
| Struct1     |   216.9 μs |  15.87 μs |  0.87 μs |  1.00 |    0.00 |         - |          - |          NA |
| Struct2     |   220.3 μs |  24.88 μs |  1.36 μs |  1.02 |    0.01 |         - |          - |          NA |
| Struct3     |   212.0 μs |   5.35 μs |  0.29 μs |  0.98 |    0.00 |         - |          - |          NA |
| StructSmall |   215.8 μs |  18.53 μs |  1.02 μs |  0.99 |    0.01 |         - |          - |          NA |
