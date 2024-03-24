namespace BlazorApp.Shared.Processing
{
    public class ErodeProcess : ImageProcess
    {
        public ImageKernel Kernel { get; set; }

        public ErodeProcess(ImageKernel kernel)
        {
            Kernel = kernel;
        }
    }
}