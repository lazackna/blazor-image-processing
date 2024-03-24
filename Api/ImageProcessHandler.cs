using BlazorApp.Shared;
using BlazorApp.Shared.Processing;
using OpenCvSharp;

namespace Api;

public class ImageProcessHandler
{
    private static Mat ImageKernelToMat(ImageKernel kernel)
    {
        Mat mat = new Mat(kernel.Data[0].Count, kernel.Data.Count, MatType.CV_8U);
        for (int i = 0; i < kernel.Data.Count; i++)
        {
            for (int j = 0; j < kernel.Data[i].Count; j++)
            {
                // Console.WriteLine(kernel.getData(j, i));
                mat.Set<int>(i, j, kernel.getData(j, i));
            }
        }

        return mat;
    }
    public static void Resize(ResizeProcess process, Mat mat)
    {
        Console.WriteLine(process.width + " | " + process.height);
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

    public static void InvertProcess(InvertProcess process, Mat mat)
    {
        Cv2.Invert(mat, mat);
    }

    public static void ErodeProcess(ErodeProcess process, Mat mat)
    {
        Cv2.Erode(mat, mat, ImageKernelToMat(process.Kernel));
    }

    public static void GreyScale(GreyScaleProcess process, Mat mat)
    {
        Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);
    }

    public static void Canny(CannyProcess process, Mat mat)
    {
        Cv2.Canny(mat, mat, process.threshold1, process.threshold2);
    }

    public static void Dilate(DilateProcess process, Mat mat)
    {
        Cv2.Dilate(mat, mat, ImageKernelToMat(process.Kernel));
    }
}