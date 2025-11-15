# Result

Result is a lightweight library designed to simplify error handling in C# applications. It provides a structured way to represent success and failure states without relying on exceptions for control flow.

1. The Error Model see `BaseError`.
2. More information can be found in the tests located in the `ResultCore.Tests` project.

## About Benchmarks

* BenchmarkDotNet.Aritfacts.2025-11-13_23-36-33Z
    * `Result<MyClass, BaseError>` 大小为 40 byte。`Result<MyClass, FileError>` 大小为 16 byte。
    * 调用 `list.Add()`

* BenchmarkDotNet.Aritfacts.2025-11-14_01-57-33Z
    * `Result<MyClass, BaseError>` 大小为 40 byte。`Result<MyClass, FileError>` 大小为 32 byte。
    * 调用 `list.Add()`

* BenchmarkDotNet.Aritfacts.2025-11-15_13-23-49Z
    * `Result<MyClass, BaseError>` 大小为 40 byte。`Result<MyClass, FileError>` 大小为 32 byte。
    * 不调用 `list.Add()`

* BenchmarkDotNet.Aritfacts.2025-11-15_15-18-51Z
    * `Result<MyClass, BaseError>` 大小为 40 byte。`Result<MyClass, FileError>` 大小为 32 byte。
    * 不调用 `list.Add()`，不创建 list
