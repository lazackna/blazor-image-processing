using System.Collections.Generic;
using BlazorApp.Shared.Processing;

namespace BlazorApp.Shared
{
    public class ImageProcessingRequest
    {
        public string base64ImageData { get; set; }

        public List<ImageProcess> Processes { get; set; } = new List<ImageProcess>();
    }
}