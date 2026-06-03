using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;
using WIMS.Application.Interfaces.Common;
using WIMS.Application.Interfaces.Repositories;
using WIMS.Application.Interfaces.Services.Audit;
using WIMS.Application.Interfaces.Services.WarehouseManagement;
using WIMS.Domain.Constant;
using WIMS.Domain.Entity;
using WIMS.Domain.Enums;

namespace WIMS.Application.Service.WarehouseManagement;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;
    private readonly IInputNormalizer _inputNormalizer;
    private readonly ICodeGeneratorService _codeGeneratorService;
    private readonly IAuditService _auditService;

    public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper, IInputNormalizer inputNormalizer, ICodeGeneratorService codeGeneratorService, IAuditService auditService)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
        _inputNormalizer = inputNormalizer;
        _codeGeneratorService = codeGeneratorService;
        _auditService = auditService;
    }

    public async Task<ApiResponse<WarehouseResponse>> CreateWarehouse(WarehouseCreateRequest request, int createdByUserId)
    {
        request = _inputNormalizer.NormalizeObject(request);

        if (await _warehouseRepository.ExistsAsync(x => x.Name.ToLower() == request.Name.ToLower()))
        {
            return ApiResponse<WarehouseResponse>.Failure("Warehouse name already exists.", statusCode: 400);
        }

        await _warehouseRepository.BeginTransactionAsync();
        try
        {
            var warehouseEntity = _mapper.Map<Warehouse>(request);
            warehouseEntity.CreatedBy = createdByUserId;
            var createdWarehouse = await _warehouseRepository.CreateAsync(warehouseEntity);

            string generateCode = _codeGeneratorService.GenerateCode("warehouse", createdWarehouse.Id);
            createdWarehouse.Code = generateCode;

            await _warehouseRepository.SaveChangesAsync();
            var response = _mapper.Map<WarehouseResponse>(createdWarehouse);

            await _auditService.LogAsync(
                action: AuditActions.Created,
                entityName: "Warehouse",
                entityId: createdWarehouse.Id.ToString(),
                performedBy: createdByUserId,
                newValue: response
            );

            await _warehouseRepository.CommitTransactionAsync();
            return ApiResponse<WarehouseResponse>.Success(response, "Warehouse created successfully.", statusCode: 201);

        }
        catch (Exception)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            return ApiResponse<WarehouseResponse>.Failure("An error occurred while creating the warehouse.", statusCode: 500);
        }
    }

    public async Task<ApiResponse<WarehouseResponse>> GetWarehouseById(int id)
    {
        var warehouse = await _warehouseRepository.GetAsync(w => w.Id == id);

        if (warehouse is null)
            return ApiResponse<WarehouseResponse>.Failure("Warehouse not found.", statusCode: 404);

        var response = _mapper.Map<WarehouseResponse>(warehouse);
        return ApiResponse<WarehouseResponse>.Success(response, statusCode: 200);
    }

    public async Task<ApiResponse<PagedResult<WarehouseResponse>>> GetWarehouses(QueryParameters qp)
    {
        qp = _inputNormalizer.NormalizeObject(qp);

        var pagedWarehouses = await _warehouseRepository.GetPaginatedAsync(
            qp,
            searchableColumns: ["Name", "City", "Code"]
            );

        var result = new PagedResult<WarehouseResponse>
        {
            Items = _mapper.Map<List<WarehouseResponse>>(pagedWarehouses.Items),
            TotalCount = pagedWarehouses.TotalCount,
            PageSize = pagedWarehouses.PageSize,
            PageNumber = pagedWarehouses.PageNumber
        };
        return ApiResponse<PagedResult<WarehouseResponse>>.Success(result, statusCode: 200);
    }

    public async Task<ApiResponse<WarehouseResponse>> UpdateWarehouse(int id, WarehouseUpdateRequest request, int modifiedByUserId)
    {
        request = _inputNormalizer.NormalizeObject(request);

        var warehouse = await _warehouseRepository.GetAsync(w => w.Id == id, useNoTracking: false);

        if (warehouse is null)
            return ApiResponse<WarehouseResponse>.Failure("Warehouse not found.", statusCode: 404);

        if (!string.IsNullOrWhiteSpace(request.Name) && await _warehouseRepository.ExistsAsync(w => w.Name.ToLower() == request.Name.ToLower() && w.Id != warehouse.Id))
        {
            return ApiResponse<WarehouseResponse>.Failure("Warehouse name already exists.", statusCode: 400);
        }

        var previousValue = _mapper.Map<WarehouseResponse>(warehouse);

        warehouse.Name = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : warehouse.Name;
        warehouse.Address = !string.IsNullOrWhiteSpace(request.Address) ? request.Address : warehouse.Address;
        warehouse.City = !string.IsNullOrWhiteSpace(request.City) ? request.City : warehouse.City;
        warehouse.ContactPerson = !string.IsNullOrWhiteSpace(request.ContactPerson) ? request.ContactPerson : warehouse.ContactPerson;
        warehouse.ContactPhone = !string.IsNullOrWhiteSpace(request.ContactPhone) ? request.ContactPhone : warehouse.ContactPhone;
        warehouse.ModifiedBy = modifiedByUserId;
        warehouse.ModifiedAt = DateTime.UtcNow;

        await _warehouseRepository.SaveChangesAsync();
        var response = _mapper.Map<WarehouseResponse>(warehouse);

        await _auditService.LogAsync(
          action: AuditActions.Updated,
          entityName: "Warehouse",
          entityId: warehouse.Id.ToString(),
          performedBy: modifiedByUserId,
          previousValue: previousValue,
          newValue: response
      );

        return ApiResponse<WarehouseResponse>.Success(response, "Warehouse updated successfully.", statusCode: 200);
    }

    public async Task<ApiResponse<string>> UpdateWarehouseStatus(int id, WarehouseStatusUpdateRequest request, int modifiedByUserId)
    {

        var warehouse = await _warehouseRepository.GetAsync(w => w.Id == id, useNoTracking: false, includes: q => q.Include(w => w.Zones));

        if (warehouse is null)
            return ApiResponse<string>.Failure("Warehouse not found.", statusCode: 404);

        if (warehouse.Status == request.Status)
        {
            return ApiResponse<string>.Failure($"Warehouse is already {warehouse.Status}.", statusCode: 400);
        }

        //any zone is active then we can not inactive warehouse
        if (request.Status == EntityStatus.Inactive && warehouse.Zones.Any(z => z.Status == EntityStatus.Active))
        {
            return ApiResponse<string>.Failure("Cannot inactivate warehouse with active zones. Please inactivate all zones first.", statusCode: 400);
        }

        //if all zones are inactive then we can not active warehouse
        if (request.Status == EntityStatus.Active && warehouse.Zones.Count > 0 && warehouse.Zones.All(z => z.Status == EntityStatus.Inactive))
        {
            return ApiResponse<string>.Failure(
                "Cannot activate warehouse with all zones inactive. Please activate at least one zone first.",
                statusCode: 400);
        }

        var previousValue = warehouse.Status;

        warehouse.Status = request.Status;
        warehouse.ModifiedBy = modifiedByUserId;
        warehouse.ModifiedAt = DateTime.UtcNow;

        await _warehouseRepository.SaveChangesAsync();

        await _auditService.LogAsync(
          action: AuditActions.Updated,
          entityName: "Warehouse",
          entityId: warehouse.Id.ToString(),
          performedBy: modifiedByUserId,
          previousValue: previousValue,
          newValue: warehouse.Status
        );

        return ApiResponse<string>.Success($"Warehouse {warehouse.Status} successfully.", statusCode: 200);
    }
}
