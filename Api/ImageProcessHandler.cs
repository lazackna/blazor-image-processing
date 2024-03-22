using BlazorApp.Shared.Processing;
using OpenCvSharp;

namespace Api;

public class ImageProcessHandler
{
    public static void Resize(ResizeProcess process, Mat mat)
    {
        Cv2.Resize(mat, mat, new Size(process.width, process.height));
    }

    public static void Blur(BlurProcess process, Mat mat)
    {
        Cv2.Blur(mat, mat, new Size(process.width, process.height));
    }

    public static void MedianBlur(MedianBlurProcess process, Mat mat)
    {
        Cv2.MedianBlur(mat, mat, process.size);
    }

    public static void GaussianBlurProcess(GaussianBlurProcess process, Mat mat)
    {
        Cv2.GaussianBlur(mat, mat, new Size(process.width, process.height),
            process.sigmaX);
    }
}