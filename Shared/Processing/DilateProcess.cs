namespace BlazorApp.Shared.Processing
{
    public class DilateProcess : ImageProcess
    {
        public ImageKernel Kernel { get; set; }

        public DilateProcess(ImageKernel kernel)
        {
            Kernel = kernel;
        }
    }
}