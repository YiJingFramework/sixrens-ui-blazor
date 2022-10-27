namespace SixRens.UI.Blazor.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadAsBytes(this Stream stream)
        {
            using var memoryStream = new MemoryStream();
            stream.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
