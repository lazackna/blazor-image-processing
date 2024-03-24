using System;
using Newtonsoft.Json;

namespace BlazorApp.Shared.Processing
{
    public class ResizeProcess : ImageProcess
    {
        public int width { get; set; }
        public int height { get; set; }
        
        public ResizeProcess(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}