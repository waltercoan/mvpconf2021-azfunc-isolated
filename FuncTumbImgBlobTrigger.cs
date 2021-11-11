using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FFMediaToolkit.Decoding;
using FFMediaToolkit.Graphics;
using Microsoft.Azure.Functions.Worker;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace br.com.waltercoan
{
    public static class FuncTumbImgBlobTrigger
    {
        [Function("FuncTumbImgBlobTrigger")]
        [BlobOutput("thumbs-workitems/thumb-{name}")]
        public static async Task<byte[]> Run(
            [BlobTrigger("samples-workitems/{name}", Connection = "storageazfuncmvpconf2021_STORAGE")] byte[] myBlob, string name,
            FunctionContext context)
        {
            IImageFormat format;
            MemoryStream imageSmall = new MemoryStream(myBlob);
            using (Image input = Image.Load(myBlob, out format))
            {
                ResizeImage(input, imageSmall, ImageSize.ExtraSmall, format);
            }
            imageSmall.Position = 0;
            return imageSmall.ToArray();
        }
        public static void ResizeImage(Image input, Stream output, ImageSize size, IImageFormat format)
        {
            var dimensions = imageDimensionsTable[size];
            input.Mutate(x => x.Resize(dimensions.Item1, dimensions.Item2));
            input.Save(output, format);
        }

        public enum ImageSize { ExtraSmall, Small, Medium }

        private static Dictionary<ImageSize, (int, int)> imageDimensionsTable = new Dictionary<ImageSize, (int, int)>() {
            { ImageSize.ExtraSmall, (320, 200) },
            { ImageSize.Small,      (640, 400) },
            { ImageSize.Medium,     (800, 600) }
        };
    }
}
