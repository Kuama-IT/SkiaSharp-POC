namespace WebpCompression;

/**
 * A silly container to load files
 */
class ConversionFileHolder
{
  public readonly String Source;
  public readonly String Target;
  private readonly FileInfo _sourceInfo;

  public long Size => _sourceInfo.Length;


  public ConversionFileHolder(string source)
  {
    this.Source = source;
    this._sourceInfo = new FileInfo(source);
    this.Target = $@"{this._sourceInfo.DirectoryName}/{Path.GetFileNameWithoutExtension(source)}.webp";
  }
}