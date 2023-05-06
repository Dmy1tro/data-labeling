using SimpleImageComparisonClassLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DataLabeling.Infrastructure.ImageHelpers
{
    public static class ImageHelper
    {
        public static ImageComparasionResult Compare(IEnumerable<Func<Stream>> imageStreams, double? validDifference = null)
        {
            // If difference is less than 40%.
            validDifference ??= 0.4;
            var images = imageStreams.ToList();
            var cached = new Dictionary<int, ImageInfo>();

            for (int i = 0; i < images.Count - 1; i++)
            {
                for (int j = i + 1; j < images.Count; j++)
                {
                    var img1 = GetOrAddCache(i);
                    var img2 = GetOrAddCache(j);

                    var difference = ImageTool.GetPercentageDifference(img1, img2);

                    if (difference < validDifference)
                    {
                        return ImageComparasionResult.Similar;
                    }
                }
            }

            return ImageComparasionResult.Different;

            ImageInfo GetOrAddCache(int i)
            {
                if (!cached.ContainsKey(i))
                {
                    using var imgStream = images[i]();
                    using var img = Image.FromStream(imgStream);
                    cached[i] = new ImageInfo(img);
                }

                return cached[i];
            }
        }

        public static ImageComparasionResult CompareManual(IEnumerable<Func<Stream>> imageStreams, double? validDifference = null)
        {
            validDifference ??= 0.4;
            var bitMapSize = 16;
            var totalPixels = bitMapSize * bitMapSize;

            var images = imageStreams.ToList();
            var cached = new Dictionary<int, ImageHash>();

            for (int i = 0; i < images.Count - 1; i++)
            {
                for (int j = i + 1; j < images.Count; j++)
                {
                    var img1 = GetOrAddCache(i);
                    var img2 = GetOrAddCache(j);

                    var equalElements = img1.Hash.Zip(img2.Hash, (first, second) => first == second).Count(h => h == true);
                    var difference = 1.0 - (double)equalElements / totalPixels;

                    if (difference < validDifference)
                    {
                        return ImageComparasionResult.Similar;
                    }
                }
            }

            return ImageComparasionResult.Different;

            ImageHash GetOrAddCache(int i)
            {
                if (!cached.ContainsKey(i))
                {
                    using var stream = images[i]();
                    using var img = Image.FromStream(stream);
                    using var bitmap = new Bitmap(img, new Size(bitMapSize, bitMapSize));
                    var hash = GetHash(bitmap);
                    cached[i] = hash;
                }

                return cached[i];
            }
        }

        public static async Task<string> GetHashAsync(Func<Stream> streamFunc)
        {
            using var stream = streamFunc();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            await stream.DisposeAsync();
            var bytes = ms.ToArray();
            using var sha1 = SHA1.Create();
            var hash = string.Concat(sha1.ComputeHash(bytes));

            return hash;
        }

        private static ImageHash GetHash(Bitmap bitmap)
        {
            var averageBrightness = GetAverageBrightness(bitmap);
            var hash = new List<bool>();

            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    hash.Add(bitmap.GetPixel(i, j).GetBrightness() < averageBrightness);
                }
            }

            return new ImageHash(hash);
        }

        private static float GetAverageBrightness(Bitmap bitmap)
        {
            var brightness = new float[bitmap.Width * bitmap.Height];
            var index = 0;
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    brightness[index++] = bitmap.GetPixel(i, j).GetBrightness();
                }
            }

            return brightness.Average();
        }
    }

    public record ImageHash(IList<bool> Hash);

    public enum ImageComparasionResult
    {
        Similar = 1,
        Different = 2
    }
}
