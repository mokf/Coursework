using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class ApartmentRepository : IApartmentRepository
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly IMapper _mapper;

    public ApartmentRepository(DataBaseContext dataBaseContext, IMapper mapper)
    {
        _dataBaseContext = dataBaseContext;
        _mapper = mapper;
    }

    public void Update(Apartment apartment)
    {
        _dataBaseContext.Entry(apartment).State = EntityState.Modified;
    }

    public async Task<PagedList<ApartmentDto>> GetApartmentsAsync(ApartmentParams apartmentParams, string name = "")
    {
        var query = _dataBaseContext.Apartments.AsQueryable();

        // if (!string.IsNullOrEmpty(username))
        // {
        //     var user = _dataBaseContext.Users.FirstOrDefault(x => x.UserName == username);
        //
        //     if (user != null)
        //         query = user.Apartments.AsQueryable();
        // }

        //query = query.Where(x => x.AppUserId != apartmentParams.CurrentUserId);

        if (!string.IsNullOrEmpty(name))
            query = query.Where(x => x.Title.Contains(name.ToLower()));

        if (!string.IsNullOrEmpty(apartmentParams.Price))
        {
            switch (apartmentParams.Price)
            {
                case "priceAsc":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(apartmentParams.People))
        {
            switch (apartmentParams.People)
            {
                case "peopleAsc":
                    query = query.OrderBy(x => x.MaxPeople);
                    break;
                case "peopleDesc":
                    query = query.OrderByDescending(x => x.MaxPeople);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(apartmentParams.HasTV))
        {
            switch (apartmentParams.HasTV)
            {
                case "yes":
                    query = query.Where(x => x.HasTV);
                    break;
                case "no":
                    query = query.Where(x => !x.HasTV);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(apartmentParams.HasWifi))
        {
            switch (apartmentParams.HasWifi)
            {
                case "yes":
                    query = query.Where(x => x.HasWifi);
                    break;
                case "no":
                    query = query.Where(x => !x.HasWifi);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(apartmentParams.IsAvaliable))
        {
            switch (apartmentParams.IsAvaliable)
            {
                case "yes":
                    query = query.Where(x => x.IsAvaliable);
                    break;
                case "no":
                    query = query.Where(x => !x.IsAvaliable);
                    break;
                default:
                    query = query.OrderBy(x => x.Id);
                    break;
            }
        }

        return await PagedList<ApartmentDto>
            .CreateAsync(query.AsNoTracking()
                    .ProjectTo<ApartmentDto>(_mapper.ConfigurationProvider),
                apartmentParams.PageNumber, apartmentParams.PageSize);
    }

    public async Task<ApartmentDto> GetApartmentDtoByIdAsync(int id)
    {
        return await _dataBaseContext.Apartments
            .Include("Photos")
            .Where(x => x.Id == id)
            .ProjectTo<ApartmentDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<Apartment> GetApartmentByIdAsync(int id)
    {
        return await _dataBaseContext.Apartments
            .Include("Photos")
            .Where(x => x.Id == id)
            .ProjectTo<Apartment>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}