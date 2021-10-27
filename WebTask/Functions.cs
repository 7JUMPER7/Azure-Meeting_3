using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace WebTask
{
    public class Functions
    {
        public static void DownscaleImageAndSave(
            [BlobTrigger("images/{name}")]Stream blob,
            string name,
            [Blob("images-min/{name}", FileAccess.Write)]Stream outBlob,
            ILogger logger
        )
        {
            Image image = Image.Load(blob);
            

            var options = new ResizeOptions {
                Size = new Size(200, 100),
                Compand = true,
                Mode = ResizeMode.Stretch
            };
            image.Mutate(i => i.Resize(options));

            var encoder = new JpegEncoder {
                Quality = 50
            };
            image.Save(outBlob, encoder);

            logger.LogInformation($"File: {name} resized!");
        }
    }
}