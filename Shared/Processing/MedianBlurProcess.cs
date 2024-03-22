namespace BlazorApp.Shared.Processing
{
    public class MedianBlurProcess : ImageProcess
    {
        public int size { get; set; }

        public MedianBlurProcess(int size)
        {
            this.size = size;
        }
    }
}