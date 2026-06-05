using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;
using WIMS.Application.Interfaces.Services.WarehouseManagement;

namespace WIMS.Api.Controllers.WarehouseManagement;

[ApiController]
[Route("api/admin/bins")]
[Authorize(Policy = "AdminOnly")]
public class BinController : ControllerBase
{
    private readonly IBinService _binService;

    public BinController(IBinService binService)
    {
        _binService = binService;
    }

    private int GetCurrentUserId()
        => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> CreateBin(BinCreateRequest request)
    {
        var response = await _binService.CreateBin(request, GetCurrentUserId());

        if (!response.IsSuccess)
        {
            if(response.StatusCode == 404)
                return NotFound(response);
            return BadRequest(response);
        }

        return StatusCode(201, response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBinById(int id)
    {
        var response = await _binService.GetBinById(id);

        if (!response.IsSuccess)
            return NotFound(response);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBin(int id)
    {
        var response = await _binService.DeleteBin(id,GetCurrentUserId());

        if (!response.IsSuccess)
            return NotFound(response);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetBins([FromQuery] QueryParameters qp)
    {
        var response = await _binService.GetBins(qp);
        return Ok(response);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetBinsDropdown(
        [FromQuery] int? warehouseId = null,
        [FromQuery] int? zoneId = null)
    {
        var response = await _binService.GetBinsDropdown(warehouseId, zoneId);
        return Ok(response);
    }

    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UpdateBin(int id, [FromBody] BinUpdateRequest request)
    {
        var response = await _binService.UpdateBin(id, request, GetCurrentUserId());

        if (!response.IsSuccess)
            return StatusCode(response.StatusCode ?? 400, response);

        return Ok(response);
    }

    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateBinStatus(int id, [FromBody] BinStatusUpdateRequest request)
    {
        var response = await _binService.UpdateBinStatus(id, request, GetCurrentUserId());

        if (!response.IsSuccess)
            return StatusCode(response.StatusCode ?? 400, response);

        return Ok(response);
    }
}