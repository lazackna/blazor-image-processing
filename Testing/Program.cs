// See https://aka.ms/new-console-template for more information

using System.Reflection;
using System.Threading.Channels;
using OpenCvSharp;

class Program
{
    Dictionary<string, MethodInfo> methods = new Dictionary<string, MethodInfo>();

    public static void Main(string[] args)
    {
        new Program();
    }

    private Program()
    {
        var cvMethods = new List<MethodInfo>(typeof(Cv2).GetMethods());
        var imageProcessors = GetImageProcesses(cvMethods);
    }

    public List<MethodInfo> GetImageProcesses(List<MethodInfo> methods)
    {
        List<MethodInfo> result = new List<MethodInfo>();

        foreach (var method in methods)
        {
            var parameters = method.GetParameters();
            var parameterDescriptions = string.Join
            (", ", method.GetParameters()
                .Select(x => x.ParameterType + " " + x.Name)
                .ToArray());

            if (method.ReturnType == typeof(void))
            {
                bool inputFound = false;
                bool outputFound = false;
                foreach (var parameterInfo in parameters)
                {
                    if (parameterInfo.ParameterType == typeof(InputArray))
                    {
                        if (inputFound)
                        {
                            inputFound = false;
                            break;
                        }

                        inputFound = true;
                    }

                    if (parameterInfo.ParameterType == typeof(OutputArray))
                    {
                        if (outputFound)
                        {
                            outputFound = false;
                            break;
                        }

                        outputFound = true;
                    }
                }

                if (inputFound && outputFound)
                {
                    Console.WriteLine("{0} ({1})\n",
                        method.Name,
                        parameterDescriptions);
                    result.Add(method);
                }
            }
        }

        return result;
    }
}