using System.ComponentModel.DataAnnotations;

namespace br.com.waltercoan
{
    public class PhotoItem
    {
        public string RowKey { get; set; }
        public string Path { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        
    }
}