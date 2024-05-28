using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IApartmentRepository
{
    void Update(Apartment apartment);
    Task<PagedList<ApartmentDto>> GetApartmentsAsync(ApartmentParams apartmentParams, string name = "");
    Task<ApartmentDto> GetApartmentDtoByIdAsync(int id);
    Task<Apartment> GetApartmentByIdAsync(int id);
}