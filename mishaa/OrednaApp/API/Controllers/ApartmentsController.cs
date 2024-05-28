using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ApartmentsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ApartmentsController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<ApartmentDto>>> GetApartments([FromQuery] ApartmentParams apartmentParams)
    {
        // var currentUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        // apartmentParams.CurrentUserId = currentUser.Id;
        var apartments = await _unitOfWork.ApartmentRepository.GetApartmentsAsync(apartmentParams);

        var apartmentsToReturn = _mapper.Map<IEnumerable<ApartmentDto>>(apartments);
        Response.AddPaginationHeader(new PaginationHeader(apartments.CurrentPage, apartments.PageSize, apartments.TotalCount, apartments.TotalPages));
        
        return Ok(apartmentsToReturn);
    }

    [HttpGet("{apartmentId:int}")]
    public async Task<ApartmentDto> GetApartment(int apartmentId)
    {
        return await _unitOfWork.ApartmentRepository.GetApartmentDtoByIdAsync(apartmentId);
    }

    [HttpGet("search/{name}")]
    public async Task<ActionResult<PagedList<ApartmentDto>>> Search([FromQuery] ApartmentParams apartmentParams, string name)
    {
        var apartments = await _unitOfWork.ApartmentRepository.GetApartmentsAsync(apartmentParams, name);
    
        var apartmentsToReturn = _mapper.Map<IEnumerable<ApartmentDto>>(apartments);
        Response.AddPaginationHeader(new PaginationHeader(apartments.CurrentPage, apartments.PageSize, apartments.TotalCount, apartments.TotalPages));
        
        return Ok(apartmentsToReturn);
    }
}