using System.Collections.Generic;

namespace BlazorApp.Shared
{
    public class ImageKernel
    {
        public List<List<int>> Data = new List<List<int>>();

        public ImageKernel(int width, int height)
        {
            for (int i = 0; i < height; i++)
            {
                Data.Add(new List<int>());
                for (int j = 0; j < width; j++)
                {
                    Data[i].Add(0);
                }
            }
        }

        public void setData(int x, int y, int value)
        {
            Data[x][y] = value;
        }

        public int getData(int x, int y)
        {
            return Data[x][y];
        }
    }
}