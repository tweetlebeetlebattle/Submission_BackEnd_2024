using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UtilityController : ControllerBase
    {
        private readonly UtilityService _utilityService;

        public UtilityController(UtilityService utilityService)
        {
            _utilityService = utilityService;
        }
        [HttpGet("GetAllUnits")]
        public async Task<ActionResult<List<string>>> GetAllUnits()
        {
            try
            {
                var units = await _utilityService.GetAllUnitsAsync();
                return Ok(units);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving units.", details = ex.Message });
            }
        }
        [HttpGet("GetAllLocations")]
        public async Task<ActionResult<List<string>>> GetAllLocations()
        {
            try
            {
                var locations = await _utilityService.GetAllLocationsAsync();
                return Ok(locations);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "Error retrieving locations.", details = ex.Message });
            }
        }
    }
}
