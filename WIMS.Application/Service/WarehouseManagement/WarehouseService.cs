using AutoMapper;
using WIMS.Application.DTOs;
using WIMS.Application.DTOs.Warehouse;
using WIMS.Application.Interfaces.Common;
using WIMS.Application.Interfaces.Repositories;
using WIMS.Application.Interfaces.Services.WarehouseManagement;
using WIMS.Domain.Entity;

namespace WIMS.Application.Service.WarehouseManagement;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;
    private readonly IInputNormalizer _inputNormalizer;
    private readonly ICodeGeneratorService _codeGeneratorService;

    public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper, IInputNormalizer inputNormalizer, ICodeGeneratorService codeGeneratorService)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
        _inputNormalizer = inputNormalizer;
        _codeGeneratorService = codeGeneratorService;
    }

    public async Task<ApiResponse<WarehouseResponse>> CreateWarehouse(WarehouseCreateRequest request, int createdByUserId)
    {
        request = _inputNormalizer.NormalizeObject(request);

        if (await _warehouseRepository.ExistsAsync(x => x.Name == request.Name))
        {
            return ApiResponse<WarehouseResponse>.Failure("Warehouse name already exists.", statusCode: 400);
        }

        await _warehouseRepository.BeginTransactionAsync();
        try
        {
            var warehouseEntity = _mapper.Map<Warehouse>(request);
            var createdWarehouse = await _warehouseRepository.CreateAsync(warehouseEntity);

            string generateCode = _codeGeneratorService.GenerateCode("warehouse", createdWarehouse.Id);
            createdWarehouse.Code = generateCode;
            createdWarehouse.CreatedBy = createdByUserId;

            await _warehouseRepository.SaveChangesAsync();
            var response = _mapper.Map<WarehouseResponse>(createdWarehouse);

            await _warehouseRepository.CommitTransactionAsync();
            return ApiResponse<WarehouseResponse>.Success(response, "Warehouse created successfully.", statusCode: 201);

        }
        catch (Exception)
        {
            await _warehouseRepository.RollbackTransactionAsync();
            return ApiResponse<WarehouseResponse>.Failure("An error occurred while creating the warehouse.", statusCode: 500);
        }
    }

    public async Task<ApiResponse<WarehouseResponse>> GetWarehouseByCode(string code)
    {
        code = _inputNormalizer.Normalize(code);
        var warehouse = await _warehouseRepository.GetAsync(w => w.Code == code);

        if (warehouse is null)
            return ApiResponse<WarehouseResponse>.Failure("Warehouse not found.", statusCode: 404);

        var response = _mapper.Map<WarehouseResponse>(warehouse);
        return ApiResponse<WarehouseResponse>.Success(response,statusCode : 200);
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
        return ApiResponse<PagedResult<WarehouseResponse>>.Success(result,statusCode: 200);
    }

    public async Task<ApiResponse<WarehouseResponse>> UpdateWarehouse(string code, WarehouseUpdateRequest request, int modifiedByUserId)
    {
        request = _inputNormalizer.NormalizeObject(request);
        code = _inputNormalizer.Normalize(code);

        var warehouse = await _warehouseRepository.GetAsync(w => w.Code == code,useNoTracking: false);

        if (warehouse is null)
            return ApiResponse<WarehouseResponse>.Failure("Warehouse not found.", statusCode: 404);

        if (await _warehouseRepository.ExistsAsync(w => w.Name == request.Name && w.Id != warehouse.Id))
        {
            return ApiResponse<WarehouseResponse>.Failure("Warehouse name already exists.", statusCode: 400);
        }

        warehouse.Name = !string.IsNullOrWhiteSpace(request.Name) ? request.Name : warehouse.Name;
        warehouse.Address = !string.IsNullOrWhiteSpace(request.Address) ? request.Address : warehouse.Address;
        warehouse.City =!string.IsNullOrWhiteSpace(request.City) ? request.City : warehouse.City;
        warehouse.ContactPerson = !string.IsNullOrWhiteSpace(request.ContactPerson) ? request.ContactPerson : warehouse.ContactPerson;
        warehouse.ContactPhone = !string.IsNullOrWhiteSpace(request.ContactPhone) ? request.ContactPhone : warehouse.ContactPhone;
        warehouse.ModifiedBy = modifiedByUserId;
        warehouse.ModifiedAt = DateTime.UtcNow;

        await _warehouseRepository.SaveChangesAsync();

        var response = _mapper.Map<WarehouseResponse>(warehouse);
        return ApiResponse<WarehouseResponse>.Success(response, "Warehouse updated successfully.",statusCode: 200);
    }

    public async Task<ApiResponse<string>> UpdateWarehouseStatus(string code,WarehouseStatusUpdateRequest request, int modifiedByUserId)
    {
        code = _inputNormalizer.Normalize(code);

        var warehouse = await _warehouseRepository.GetAsync(w => w.Code == code,useNoTracking: false);

        if (warehouse is null)
            return ApiResponse<string>.Failure("Warehouse not found.", statusCode: 404);

        if(warehouse.Status == request.Status)
        {
            return ApiResponse<string>.Failure($"Warehouse is already {warehouse.Status}.", statusCode: 400);
        }

        warehouse.Status = request.Status;
        warehouse.ModifiedBy = modifiedByUserId;
        warehouse.ModifiedAt = DateTime.UtcNow;

        await _warehouseRepository.SaveChangesAsync();

        return ApiResponse<string>.Success($"Warehouse {warehouse.Status} successfully.",statusCode: 200);
    }
}
