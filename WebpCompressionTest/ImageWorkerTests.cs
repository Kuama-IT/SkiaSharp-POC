using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using WebpCompression;

namespace WebpCompressionTest;

public class ImageWorkerTests
{
  [SetUp]
  public void Setup()
  {
  }

  [Test]
  public async Task TestWork()
  {
    var worker = new ImageWorker("./Data");

    var results = await worker.Work();

    foreach (var result in results)
    {
      Console.WriteLine($"Stats for {result.Path}:");
      Console.WriteLine($"Read: {result.ReadFile} ms");
      Console.WriteLine($"Convert: {result.EncodeInWeb} ms");
      Console.WriteLine($"Write: {result.WriteFile} ms");
      Console.WriteLine($"Resize: {result.ResizeFile} ms");
      Console.WriteLine($"Write resized: {result.WriteResizedFile} ms");
      Console.WriteLine($"Overall: {result.OverAll} ms");
    }

    Assert.IsTrue(results.Count == 2);
  }
}