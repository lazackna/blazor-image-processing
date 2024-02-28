using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using Azure.Core;
using BlazorApp.Shared;
using BlazorApp.Shared.Processing;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OpenCvSharp;
using Size = OpenCvSharp.Size;

namespace Api
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        public byte[] GetRedSquareImageBytes()
        {
            return Convert.FromBase64String(
                "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");
        }

        private void processImage(ImageProcessingRequest request, Mat mat)
        {
            foreach (var imageProcess in request.Processes)
            {
                if (imageProcess is ScaleProcess scaleProcess)
                {
                    Cv2.Resize(mat, mat, new Size(scaleProcess.width, scaleProcess.height));
                }
            }
        }

        [Function("Image")]
        public async Task<HttpResponseData> RunImage(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequestData req)
        {
            // foreach (var (key, value) in req.Headers)
            // {
            //     Console.Write(key + ": ");
            //     foreach (var s in value)
            //     {
            //         Console.Write(" " + s);
            //     }
            //
            //     Console.WriteLine();
            // }

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var imageProcessingRequest = JsonConvert.DeserializeObject<ImageProcessingRequest>(requestBody, new JsonSerializerSettings 
            { 
                TypeNameHandling = TypeNameHandling.All 
            });
            byte[] imageData = Convert.FromBase64String(imageProcessingRequest.base64ImageData);
            Console.WriteLine(requestBody);
            // Decode the image data using OpenCV
            using (Mat mat = Mat.FromImageData(imageData, ImreadModes.Color))
            {
                if (mat.Empty())
                {
                    return req.CreateResponse(HttpStatusCode.InternalServerError);
                }

                // Perform image processing operations here
                // For example, you can resize the image:
                //Cv2.Resize(mat, mat, new Size(mat.Width / 2, mat.Height / 2));
                // Cv2.CvtColor(mat, mat, ColorConversionCodes.BGR2GRAY);
                // // Convert the processed image back to a byte array
                // Cv2.GaussianBlur(mat, mat, new Size(5, 5), 10, 10);

                processImage(imageProcessingRequest, mat);
                
                var response = req.CreateResponse(HttpStatusCode.OK);
                byte[] buf;
                Cv2.ImEncode(".png", mat, out buf);

                //await response.WriteAsJsonAsync(buf);

                response.Headers.Add("Content-Type", "application/octet-stream");
                await response.Body.WriteAsync(buf);


                return response;
            }
        }

        [Function("WeatherForecast")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var randomNumber = new Random();
            var temp = 0;

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temp = randomNumber.Next(-20, 55),
                Summary = GetSummary(temp)
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);

            return response;
        }

        private string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing";
            }

            return summary;
        }
    }
}