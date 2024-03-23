using System.Drawing;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
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
            GenerateFunctionBindings();
        }

        private List<string> _processes = new List<string>();

        private Dictionary<Type, MethodInfo> typeToMethodBindings = new Dictionary<Type, MethodInfo>();

        private void GenerateFunctionBindings()
        {
            var methods = typeof(ImageProcessHandler).GetMethods();

            foreach (var methodInfo in methods)
            {
                if (methodInfo.Name == "ToString" || methodInfo.Name == "Equals" || methodInfo.Name == "GetHashCode" ||
                    methodInfo.Name == "GetType")
                {
                    continue;
                }

                var parameterInfo = methodInfo.GetParameters()[0];
                typeToMethodBindings.Add(parameterInfo.ParameterType, methodInfo);
                
                _processes.Add(parameterInfo.ParameterType.FullName);
                Console.WriteLine(_processes[^1]);
            }
        }

        private void ProcessImage(ImageProcessingRequest request, Mat mat)
        {
            
            
            foreach (var imageProcess in request.Processes)
            {
                if(typeToMethodBindings.TryGetValue(imageProcess.GetType(), out MethodInfo methodInfo))
                {
                    methodInfo.Invoke(null, new object[]{imageProcess, mat});
                }
                
                continue;
                if (imageProcess is ResizeProcess scaleProcess)
                {
                    ImageProcessHandler.Resize(scaleProcess, mat);
                }
                else if (imageProcess is BlurProcess blurProcess)
                {
                    ImageProcessHandler.Blur(blurProcess, mat);
                }
                else if (imageProcess is MedianBlurProcess medianBlurProcess)
                {
                    ImageProcessHandler.MedianBlur(medianBlurProcess, mat);
                }
                else if (imageProcess is GaussianBlurProcess gaussianBlurProcess)
                {
                    ImageProcessHandler.GaussianBlurProcess(gaussianBlurProcess, mat);
                }
            }
        }

        [Function("ImageInfo")]
        public async Task<HttpResponseData> ProcessInfoRequest([HttpTrigger(AuthorizationLevel.Anonymous, "get")]
            HttpRequestData req)
        {
            Console.WriteLine("Got Request");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_processes);
            return response;
        }

        [Function("Image")]
        public async Task<HttpResponseData> ProcessImageMethod(
            [HttpTrigger(AuthorizationLevel.Function, "post")]
            HttpRequestData req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var imageProcessingRequest = JsonConvert.DeserializeObject<ImageProcessingRequest>(requestBody,
                new JsonSerializerSettings
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
                    return req.CreateResponse(HttpStatusCode.BadRequest);
                }


                ProcessImage(imageProcessingRequest, mat);

                var response = req.CreateResponse(HttpStatusCode.OK);
                byte[] buf;
                Cv2.ImEncode(".png", mat, out buf);

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