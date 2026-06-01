using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;
using WIMS.Application.Interfaces.Services.WarehouseManagement;

namespace WIMS.Api.Controllers.WarehouseManagement;

[ApiController]
[Route("api/admin/warehouses")]
[Authorize(Policy = "AdminOnly")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    private int GetCurrentUserId()
    => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse(WarehouseCreateRequest request)
    {
        var response = await _warehouseService.CreateWarehouse(request, GetCurrentUserId());

        if (!response.IsSuccess)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }


    [HttpGet("{code}")]
    public async Task<IActionResult> GetWarehouseByCode(string code)
    {
        var response = await _warehouseService.GetWarehouseByCode(code);

        if (!response.IsSuccess)
        {
            return NotFound(response);
        }

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetWarehouses([FromQuery] QueryParameters qp)
    {
        var response = await _warehouseService.GetWarehouses(qp);
        return Ok(response);
    }

    [HttpPatch("{code}")]
    public async Task<IActionResult> UpdateWarehouse(string code, WarehouseUpdateRequest request)
    {
        var response = await _warehouseService.UpdateWarehouse(code, request, GetCurrentUserId());

        if (!response.IsSuccess)
        {
            if (response.StatusCode == 404)
                return NotFound(response);
            return BadRequest(response);
        }

        return Ok(response);
    }

    [HttpPatch("{code}/status")]
    public async Task<IActionResult> UpdateWarehouseStatus(string code, WarehouseStatusUpdateRequest request)
    {
        var response = await _warehouseService.UpdateWarehouseStatus(code, request, GetCurrentUserId());

        if (!response.IsSuccess)
        {
            if (response.StatusCode == 404)
                return NotFound(response);
            return BadRequest(response);
        }

        return Ok(response);
    }

}
