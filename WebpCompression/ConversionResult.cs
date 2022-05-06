using System.Diagnostics;

namespace WebpCompression;

/**
 * A silly container to track operation timings
 */
public class ConversionResult
{
  public readonly string Path;

  private Stopwatch _readFileWatch = null!;
  public long ReadFile => _readFileWatch.ElapsedMilliseconds;

  private Stopwatch _encodeInWebWatch = null!;
  public long EncodeInWeb => _encodeInWebWatch.ElapsedMilliseconds;

  private Stopwatch _writeWebpFileWatch = null!;
  public long WriteFile => _writeWebpFileWatch.ElapsedMilliseconds;

  private Stopwatch? _resizeFileWatch = null;
  public long ResizeFile => _resizeFileWatch?.ElapsedMilliseconds ?? 0;

  private Stopwatch? _writeResizedFileWatch = null;
  public long WriteResizedFile => _writeResizedFileWatch?.ElapsedMilliseconds ?? 0;

  public long OverAll => ReadFile + EncodeInWeb + WriteFile + WriteResizedFile + ResizeFile;

  public ConversionResult(string path)
  {
    Path = path;
  }

  public void ReadFileStart()
  {
    _readFileWatch = Stopwatch.StartNew();
  }

  public void ReadFileStop()
  {
    _readFileWatch.Stop();
  }

  public void EncodeInWebStart()
  {
    _encodeInWebWatch = Stopwatch.StartNew();
  }

  public void EncodeInWebStop()
  {
    _encodeInWebWatch.Stop();
  }

  public void WriteWebpFileStart()
  {
    _writeWebpFileWatch = Stopwatch.StartNew();
  }

  public void WriteWebpFileStop()
  {
    _writeWebpFileWatch.Stop();
  }

  public void ResizeFileStart()
  {
    _resizeFileWatch = Stopwatch.StartNew();
  }

  public void ResizeFileStop()
  {
    _resizeFileWatch.Stop();
  }

  public void WriteResizedFileStart()
  {
    _writeResizedFileWatch = Stopwatch.StartNew();
  }

  public void WriteResizeFileStop()
  {
    _writeResizedFileWatch.Stop();
  }
}