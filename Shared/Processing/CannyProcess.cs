namespace BlazorApp.Shared.Processing
{
    public class CannyProcess : ImageProcess
    {
        public CannyProcess(int threshold1, int threshold2)
        {
            this.threshold1 = threshold1;
            this.threshold2 = threshold2;
        }

        public int threshold1 { get; set; }
        public int threshold2 { get; set; }
        
        
    }
}