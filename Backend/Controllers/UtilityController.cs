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
        public async Task<IActionResult> GetAllUnits()
        {
            try
            {
                var units = await _utilityService.GetAllUnitsAsync();
                return Ok(new { Message = "Units retrieved successfully.", Data = units });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving units.", Details = ex.Message });
            }
        }

        [HttpGet("GetAllLocations")]
        public async Task<IActionResult> GetAllLocations()
        {
            try
            {
                var locations = await _utilityService.GetAllLocationsAsync();
                return Ok(new { Message = "Locations retrieved successfully.", Data = locations });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { Message = "Error retrieving locations.", Details = ex.Message });
            }
        }
    }
}
