namespace BlazorApp.Shared.Processing
{
    public class GaussianBlurProcess : ImageProcess
    {
        public int width { get; set; }
        public int height { get; set; }
        
        public int sigmaX { get; set; }
        
        public GaussianBlurProcess(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}