using BenchmarkDotNet.Attributes;
using DataLabeling.Infrastructure.ImageHelpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace PerformanceTests
{
    [MemoryDiagnoser]
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class ImageComparingBenchmarks
    {
        private static Func<Stream> Img1Func = () => File.OpenRead("057211e1-85d6-4744-816e-74dcf0aeb7a6_animal2.jpg");
        private static Func<Stream> Img2Func = () => File.OpenRead("67281694-8ba6-4c34-872d-f8a0e9d048a8_animal3.png");
        public static List<Func<Stream>> Images = new List<Func<Stream>>();

        [GlobalSetup]
        public void Setup()
        {
            // Emulating comparison of 100 (50 * 2) images.
            for (int i = 0; i < 50; i++)
            {
                Images.Add(Img1Func);
                Images.Add(Img2Func);
            }
        }

        [Benchmark]
        public void LibraryComparing()
        {
            ImageHelper.Compare(Images);
        }

        [Benchmark]
        public void ManualComparing()
        {
            ImageHelper.CompareManual(Images);
        }
    }
}
