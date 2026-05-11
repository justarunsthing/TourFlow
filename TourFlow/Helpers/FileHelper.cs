using Microsoft.AspNetCore.Components.Forms;

namespace TourFlow.Helpers
{
    public static class FileHelper
    {
        public static async Task<byte[]> GetAllBytesAsync(this IBrowserFile file)
        {
            await using var ms = new MemoryStream();
            await file.OpenReadStream().CopyToAsync(ms);

            return ms.ToArray();
        }
    }
}