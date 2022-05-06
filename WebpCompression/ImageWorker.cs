using SkiaSharp;

namespace WebpCompression;

/**
 * Just a proof of concept on manipulating images (resizing) e converting them (to webp) using [SkiaSharp](https://github.com/mono/SkiaSharp)
 *
 */
public class ImageWorker
{
  private readonly List<ConversionFileHolder> _toBeConverted;
  private static readonly int FILE_THRESHOLD = 5242880; // 5MB
  private static readonly double RESIZE_FACTOR = 0.9;

  public ImageWorker(string imagesFolder)
  {
    _toBeConverted = Directory.EnumerateFiles(imagesFolder)
      .Where(it => it.Contains(".webp") == false)
      .Select(it => new ConversionFileHolder(it))
      .ToList();
  }

  /**
   * TODOs:
   * - read a file
   * - create the target image: a webp image of max <paramref name="FILE_THRESHOLD"/> size
   * - write the new file
   */
  public async Task<List<ConversionResult>> Work()
  {
    // We're aiming to an image with size < of FILE_THRESHOLD

    // timing results
    var results = new List<ConversionResult>();

    // work
    foreach (var conversionFile in _toBeConverted)
    {
      if (conversionFile.Size <= FILE_THRESHOLD) continue;

      var result = new ConversionResult(conversionFile.Source);

      result.ReadFileStart();
      var bytes = await File.ReadAllBytesAsync(conversionFile.Source);
      result.ReadFileStop();

      result.EncodeInWebStart();
      var webpData = SKBitmap.Decode(bytes).Encode(SKEncodedImageFormat.Webp, 80);
      result.EncodeInWebStop();

      if (webpData == null)
      {
        throw new Exception($"Could not convert {conversionFile.Source}");
      }


      result.WriteWebpFileStart();
      await File.WriteAllBytesAsync(conversionFile.Target, webpData.ToArray());
      result.WriteWebpFileStop();
      
      var currentSize = webpData.Size;
      webpData.Dispose();

      // ensure the generated file is still under the threshold
      var info = new FileInfo(conversionFile.Target);

      if (currentSize <= FILE_THRESHOLD)
      {
        results.Add(result);
        continue;
      }

      // we need to downscale
      result.ResizeFileStart();
      var bitmap = SKBitmap.Decode(conversionFile.Target);

      var src = SKImage.FromBitmap(bitmap);
      while (currentSize > FILE_THRESHOLD)
      {
        
        var output = SKImage.Create(ImageInfoForBitmap(bitmap));
        src.ScalePixels(output.PeekPixels(), SKFilterQuality.High);

        bitmap = SKBitmap.FromImage(output);
        currentSize = bitmap.Bytes.Length;
      }

      result.ResizeFileStop();

      result.WriteResizedFileStart();
      await File.WriteAllBytesAsync(conversionFile.Target, bitmap.Encode(SKEncodedImageFormat.Webp, 80).ToArray());
      result.WriteResizeFileStop();


      results.Add(result);
      
    }
    return results;
  }

  private static SKImageInfo ImageInfoForBitmap(SKBitmap bitmap)
  {
    return new SKImageInfo(
      (int)Math.Round(bitmap.Width * RESIZE_FACTOR),
      (int)Math.Round(bitmap.Height * RESIZE_FACTOR)
    );
  }
  
}