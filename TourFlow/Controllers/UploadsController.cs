using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using TourFlow.Data;

namespace TourFlow.Controllers
{
    [Route("uploads")]
    [ApiController]
    public class UploadsController(ApplicationDbContext context) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        [OutputCache(VaryByRouteValueNames = ["id"], Duration = 60 * 60 * 24)]
        public async Task<IActionResult> GetFileAsync(Guid id)
        {
            var file = await context.Uploads.FirstOrDefaultAsync(i => i.Id == id);

            if (file == null)
            {
                return NotFound();
            }

            return File(file.Data!, file.Type!);
        }
    }
}