using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPhotoService _photoService;
    private readonly IMapper _mapper;

    public UsersController(IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _photoService = photoService;
        _mapper = mapper;
    }
    
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await _unitOfWork.UserRepository.GetUsersAsync();

        var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);

        return Ok(usersToReturn);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        return await _unitOfWork.UserRepository.GetMemberAsync(username);
    }

    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(User.GetUserName());

        if (user == null)
            return NotFound();

        _mapper.Map(memberUpdateDto, user);
        if (await _unitOfWork.Complete())
            return NoContent();

        return BadRequest("Failed to update user");
    }

    [HttpGet("{username}/apartments")]
    public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetApartments(string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        if (user == null)
            return NotFound();
        
        var apartments = user.Apartments;
        
        var apartmentsToReturn = _mapper.Map<IEnumerable<ApartmentDto>>(apartments);
        
        return Ok(apartmentsToReturn);
    }

    [HttpPost("{username}/create")]
    public async Task<ActionResult> Create([FromBody]CreateApartmentDto apartmentDto, string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        
        if (user == null)
            return NotFound();
        
        var apartment = new Apartment
        {
            Title = apartmentDto.Title,
            City = apartmentDto.City,
            Country = apartmentDto.Country,
            Price = apartmentDto.Price,
            MaxPeople = apartmentDto.MaxPeople,
            HasTV = apartmentDto.HasTV,
            HasWifi = apartmentDto.HasWifi,
            IsAvaliable = true
        };

        if (apartment == null)
            return NotFound();
        
        user.Apartments.Add(apartment);
        if (await _unitOfWork.Complete())
            return NoContent();

        return BadRequest("Problem creating apartment photo");

        // return new ApartmentDto
        // {
        //     PhotoUrl = apartment.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        //     Title = apartment.Title,
        //     Details = apartment.Details,
        //     City = apartment.City,
        //     Country = apartment.Country,
        //     Price = apartment.Price,
        //     MaxPeople = apartment.MaxPeople,
        //     HasTV = apartment.HasTV,
        //     HasWifi = apartment.HasWifi,
        //     IsAvaliable = true
        // };
    }
    
    [HttpDelete("{username}/{apartmentId}/delete")]
    public async Task<ActionResult> Delete(string username, int apartmentId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var apartment = user.Apartments.Find(x => x.Id == apartmentId);

        if (user == null)
            return NotFound();
        if (apartment == null)
            return NotFound();
        
        user.Apartments.Remove(apartment);

        if (await _unitOfWork.Complete())
            return Ok();
        
        return BadRequest("Problem deleting apartment");
    }
    
    [HttpPut("{username}/{apartmentId}/update")]
    public async Task<ActionResult> UpdateApartment(string username, int apartmentId, ApartmentUpdateDto apartmentUpdateDto)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var apartment = user.Apartments.Find(x => x.Id == apartmentId);

        if (user == null)
            return NotFound();
        if (apartment == null)
            return NotFound();
        
        _mapper.Map(apartmentUpdateDto, apartment);
        if (await _unitOfWork.Complete())
            return NoContent();

        return BadRequest("Failed to update apartment");
    }

    [HttpPost("{username}/{apartmentId}/add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(string username, int apartmentId, IFormFile file)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var apartment = user.Apartments.Find(x => x.Id == apartmentId);

        if (user == null)
            return NotFound();
        if (apartment == null)
            return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null)
            return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (apartment.Photos.Count == 0)
            if (apartment.Photos.Where(photo => photo.IsMain) == null)
                photo.IsMain = true;
            apartment.Photos.Add(photo);

        if (await _unitOfWork.Complete())
            return _mapper.Map<PhotoDto>(photo);

        return BadRequest("Problem adding photo");
    }

    [HttpPut("{username}/{apartmentId}/set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(string username, int apartmentId, int photoId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var apartment = user.Apartments.Find(x => x.Id == apartmentId);

        if (user == null)
            return NotFound();
        if (apartment == null)
            return NotFound();
        
        var photo = apartment.Photos.FirstOrDefault(photo => photo.Id == photoId);

        if (photo == null)
            return NotFound();
        if (photo.IsMain)
            return BadRequest("This is already your main photo");

        var currentMainPhoto = apartment.Photos.FirstOrDefault(photo => photo.IsMain);
        if (currentMainPhoto != null)
            currentMainPhoto.IsMain = false;
        
        photo.IsMain = true;

        if (await _unitOfWork.Complete())
            return Ok();

        return BadRequest("Problem setting the main photo");
    }

    [HttpDelete("{username}/{apartmentId:int}/delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(string username, int apartmentId, int photoId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        var apartment = user.Apartments.Find(x => x.Id == apartmentId);

        if (user == null)
            return NotFound();
        if (apartment == null)
            return NotFound();
        
        var photo = apartment.Photos.FirstOrDefault(photo => photo.Id == photoId);
        
        if (photo == null)
            return NotFound();

        if (photo.IsMain)
            return BadRequest("You cannot delete your main photo");
        
         if (photo.PublicId != null)
         {
             var result = await _photoService.DeletePhotoAsync(photo.PublicId);
        
             if (result.Error != null)
                 return BadRequest(result.Error.Message);
         }
        
        apartment.Photos.Remove(photo);
        
         if (await _unitOfWork.Complete())
             return Ok();

        return BadRequest("Problem deleting photo");
    }

    [HttpGet("{username}/rented-apartments")]
    public async Task<ActionResult<IEnumerable<ApartmentDto>>> GetRentedApartments(string username)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);

        if (user == null)
            return NotFound();

        var apartments = new List<ApartmentDto>();

        foreach (var apartment in user.RentedApartments)
        {
            var test = await _unitOfWork.ApartmentRepository.GetApartmentDtoByIdAsync(apartment.Id);
            apartments.Add(test);
        }

        // var query = _mapper.Map<PagedList<ApartmentDto>>(user.Apartments);
        // Response.AddPaginationHeader(new PaginationHeader(query.CurrentPage, query.PageSize, query.TotalCount, query.TotalPages));
        
        return Ok(apartments);
    }
    
    [HttpPost("{username}/rent/{apartmentId:int}")]
    public async Task<ActionResult> RentApartment(string username, int apartmentId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        
        if (user == null)
            return NotFound();
        
        if (user.Apartments.FirstOrDefault(x => x.Id == apartmentId) != null)
            return BadRequest("You cannot rent your own apartment");
        
        var apartment = await _unitOfWork.ApartmentRepository.GetApartmentByIdAsync(apartmentId);
    
        if (apartment == null)
            return NotFound();
    
        if (!apartment.IsAvaliable)
            return BadRequest("The apartment is not available to rent");

        user.RentedApartments.Add(apartment);
        apartment.IsAvaliable = false;

        if (await _unitOfWork.Complete())
            return Ok();
    
        return BadRequest("Problem renting apartment");
    }
    
    [HttpPost("{username}/cancel-rent/{apartmentId:int}")]
    public async Task<ActionResult> CancelRentApartment(string username, int apartmentId)
    {
        var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
        
        if (user == null)
            return NotFound();

        if (user.Apartments.FirstOrDefault(x => x.Id == apartmentId) != null)
            return BadRequest("You cannot cancel rent your own apartment");

        var apartment = user.RentedApartments.FirstOrDefault(x => x.Id == apartmentId);
        
        if (apartment == null)
            return NotFound();
    
        if (apartment.IsAvaliable)
            return BadRequest("The apartment is not available to cancel rent");

        if (user.Id != apartment.RentedByUserId)
            return BadRequest("Not you rent apartment");
        
        user.RentedApartments.Remove(apartment);
        apartment.RentedByUserId = null;
        apartment.RentedByUser = null;
        apartment.IsAvaliable = true;

        if (await _unitOfWork.Complete())
            return Ok();
        
        return BadRequest("Problem canceling rent apartment");
    }
}