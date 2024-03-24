namespace BlazorApp.Shared.Processing
{
    public class BlurProcess : ImageProcess
    {
        public int width { get; set; }
        public int height { get; set; }
        
        public BlurProcess(int width, int height)
        {
            this.width = width;
            this.height = height;
        }
    }
}