using System.Text;

namespace ResultCore.Tests;

/// <summary>
/// 一个支持行缓冲的 TextWriter，用于将 Console 的标准输出完美重定向到 xUnit 的 ITestOutputHelper。
/// </summary>
public class XunitTextWriter : TextWriter
{
    private readonly ITestOutputHelper _output;
    private readonly Lock _lock = new();
    private readonly StringBuilder _buffer = new();

    public XunitTextWriter(ITestOutputHelper output)
    {
        _output = output ?? throw new ArgumentNullException(nameof(output));
    }

    #region Properties

    // 默认使用 UTF-8 编码
    public override Encoding Encoding => Encoding.UTF8;

    #endregion

    #region Methods

    private void FlushLine()
    {
        // 必须在 lock 保护下调用
        _output.WriteLine(_buffer.ToString());
        _buffer.Clear();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Flush(); // 释放时刷新缓冲区
        }
        base.Dispose(disposing);
    }

    /// <summary>
    /// 当调用 Flush 或测试结束（Dispose）时，将残留未换行的文本一次性输出。
    /// </summary>
    public override void Flush()
    {
        lock (_lock)
        {
            if (_buffer.Length > 0)
            {
                FlushLine();
            }
        }
    }

    /// <summary>
    /// 最核心的写入单个字符方法。所有派生的写入操作最终都会调用此方法。
    /// </summary>
    public override void Write(char value)
    {
        lock (_lock)
        {
            if (value == '\n')
            {
                FlushLine();
            }
            else if (value != '\r') // 忽略 Windows 的回车符，统一通过换行符 '\n' 触发整行刷新
            {
                _buffer.Append(value);
            }
            else
            {
                //skip
            }
        }
    }

    /// <summary>
    /// 重写写入字符串的方法，以提高大段文本输出时的性能。
    /// </summary>
    public override void Write(string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return;
        }

        lock (_lock)
        {
            foreach (var c in value)
            {
                if (c == '\n')
                {
                    FlushLine();
                }
                else if (c != '\r')
                {
                    _buffer.Append(c);
                }
                else
                {
                    continue;
                }
            }
        }
    }

    /// <summary>
    /// 重写字符数组写入，确保底层缓冲区写入的鲁棒性。
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void Write(char[] buffer, int index, int count)
    {
        ArgumentNullException.ThrowIfNull(buffer);

        if (index < 0 || count < 0 || index + count > buffer.Length)
        {
            throw new ArgumentOutOfRangeException();
        }

        lock (_lock)
        {
            for (var i = 0; i < count; i++)
            {
                var c = buffer[index + i];
                if (c == '\n')
                {
                    FlushLine();
                }
                else if (c != '\r')
                {
                    _buffer.Append(c);
                }
                else
                {
                    continue;
                }
            }
        }
    }

    #endregion

}
