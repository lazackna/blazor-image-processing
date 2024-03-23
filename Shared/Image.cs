namespace BlazorApp.Shared
{
    public struct Image
    {
        public string Base64Data { get; private set; }

        public Image(string base64Data)
        {
            Base64Data = base64Data;
        }
    }
}