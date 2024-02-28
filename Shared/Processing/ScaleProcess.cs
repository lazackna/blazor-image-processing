using System;
using Newtonsoft.Json;

namespace BlazorApp.Shared.Processing
{
    public class ScaleProcess : ImageProcess
    {
        public int width { get; set; }
        public int height { get; set; }
        
        public ScaleProcess(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}