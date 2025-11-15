```

BenchmarkDotNet v0.15.6, Windows 10 (10.0.19045.5487/22H2/2022Update)
Intel Xeon CPU E5-2667 v4 3.20GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.307
  [Host]   : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3
  ShortRun : .NET 9.0.11 (9.0.11, 9.0.1125.51716), X64 RyuJIT x86-64-v3

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method      | Mean       | Error       | StdDev    | Ratio | RatioSD | Gen0      | LLCReference/Op | LLCMisses/Op | Code Size | Allocated  | Alloc Ratio |
|------------ |-----------:|------------:|----------:|------:|--------:|----------:|----------------:|-------------:|----------:|-----------:|------------:|
| Ref1        | 5,828.0 μs | 2,452.10 μs | 134.41 μs | 19.44 |    0.53 | 3054.6875 |         733,013 |        2,731 |      57 B | 40000000 B |          NA |
| Ref2        | 5,917.3 μs | 2,467.34 μs | 135.24 μs | 19.74 |    0.53 | 3054.6875 |         732,843 |        1,877 |      57 B | 40000000 B |          NA |
| Ref3        | 5,880.5 μs | 7,271.45 μs | 398.57 μs | 19.62 |    1.21 | 3054.6875 |         731,793 |        2,219 |      57 B | 40000000 B |          NA |
| Struct1     |   299.8 μs |   117.66 μs |   6.45 μs |  1.00 |    0.03 |         - |             480 |           43 |      10 B |          - |          NA |
| Struct2     |   301.0 μs |   161.65 μs |   8.86 μs |  1.00 |    0.03 |         - |             608 |           75 |      10 B |          - |          NA |
| Struct3     |   308.6 μs |   239.91 μs |  13.15 μs |  1.03 |    0.04 |         - |             587 |           53 |      10 B |          - |          NA |
| StructSmall |   295.7 μs |    24.31 μs |   1.33 μs |  0.99 |    0.02 |         - |             480 |           32 |      10 B |          - |          NA |
