using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.DTOs.Responses;
using CoHabit.API.Helpers;
using CoHabit.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CoHabit.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CharacteristicController : ControllerBase
    {
        private readonly ICharacteristicService _characteristicService;

        public CharacteristicController(ICharacteristicService characteristicService)
        {
            _characteristicService = characteristicService;
        }

        // GET: api/v1/Characteristic
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CharacteristicResponse>>> GetAll()
        {
            var characteristics = await _characteristicService.GetAllCharacteristicsAsync();
            var result = characteristics.Select(c => new CharacteristicResponse(c.CharId, c.Title));
            return Ok(ApiResponse<IEnumerable<CharacteristicResponse>>.SuccessResponse(result, "Characteristics retrieved successfully."));
        }

        // GET: api/v1/Characteristic/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CharacteristicResponse>> GetById(string id)
        {
            var characteristic = await _characteristicService.GetCharacteristicByIdAsync(id);
            if (characteristic == null)
                return NotFound();
            return Ok(ApiResponse<CharacteristicResponse>.SuccessResponse(new CharacteristicResponse(characteristic.CharId, characteristic.Title), "Characteristic retrieved successfully."));
        }

        // POST: api/v1/Characteristic
        [HttpPost]
        public async Task<ActionResult> Create([FromQuery] string title)
        {
            await _characteristicService.AddCharacteristicAsync(title);
            // The created resource's id is not returned by AddCharacteristicAsync, so we can't set Location header properly
            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Characteristic created successfully."));
        }

        // PUT: api/v1/Characteristic
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] CharacteristicRequest request)
        {
            try
            {
                await _characteristicService.UpdateCharacteristicAsync(request);
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Characteristic updated successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/v1/Characteristic/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                await _characteristicService.DeleteCharacteristicAsync(id);
                return Ok(ApiResponse<object>.SuccessResponse(new { }, "Characteristic deleted successfully."));
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}