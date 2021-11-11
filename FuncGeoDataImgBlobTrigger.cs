using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace br.com.waltercoan
{
    public class FuncGeoDataImgBlobTrigger
    {
        private FuncDbContext dbContext;
        public FuncGeoDataImgBlobTrigger(FuncDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        [Function("FuncGeoDataImgBlobTrigger")]
        public async Task RunAsync([BlobTrigger("samples-workitems/{name}", Connection = "storageazfuncmvpconf2021_STORAGE")] byte[] myBlob, string name,
            FunctionContext context)
        {
            var logger = context.GetLogger("FuncGeoDataImgBlobTrigger");
            MemoryStream imageStream = new MemoryStream(myBlob);
            try
            {    
                var gps = ImageMetadataReader.ReadMetadata(imageStream)
                             .OfType<GpsDirectory>()
                             .FirstOrDefault();

                var location = gps.GetGeoLocation();

                var photoItem = new PhotoItem(){
                    RowKey = Guid.NewGuid().ToString(),
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Path = context.BindingContext.BindingData["Uri"].ToString()
                };
                dbContext.PhotoItens.Add(photoItem);
                await dbContext.SaveChangesAsync();

                logger.LogTrace($"{photoItem.Path} at {photoItem.Latitude},{photoItem.Longitude}");
            }
            catch (System.Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }
    }
}
