using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FurnitureController : ControllerBase
    {
        private readonly IFurnitureService _furnitureService;
        public FurnitureController(IFurnitureService furnitureService)
        {
            _furnitureService = furnitureService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<FurnitureResponse>>> GetAll()
        {
            var furnitures = await _furnitureService.GetFurnituresAsync();
            var result = furnitures.Select(c => new FurnitureResponse(c.FurId, c.Name));
            return Ok(ApiResponse<IEnumerable<FurnitureResponse>>.SuccessResponse(result, "Furnitures retrieved successfully."));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromQuery] string name)
        {
            await _furnitureService.CreateFurnitureAsync(name);
            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Furniture created successfully."));
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update([FromBody] FurnitureRequest request)
        {
            try
            {
                await _furnitureService.UpdateFurnitureAsync(request);
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Furniture updated successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _furnitureService.DeleteFurnitureAsync(id);
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Furniture deleted successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}