using System.Text;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository : IUserRepository
{
    private readonly DataBaseContext _dataBaseContext;
    private readonly IMapper _mapper;

    public UserRepository(DataBaseContext dataBaseContext, IMapper mapper)
    {
        _dataBaseContext = dataBaseContext;
        _mapper = mapper;
    }
    
    public void Update(AppUser user)
    {
        _dataBaseContext.Entry(user).State = EntityState.Modified;
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await _dataBaseContext.Users
            .Include("Apartments.Photos")
            .ToListAsync();
    }

    public async Task<AppUser> GetUserByIdAsync(int id)
    {
        return await _dataBaseContext.Users.FindAsync(id);
    }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
        return await _dataBaseContext.Users
            .Include("Apartments.Photos")
            .Include("RentedApartments")
            .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<MemberDto>> GetMembersAsync()
    {
        return await _dataBaseContext.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<MemberDto> GetMemberAsync(string username)
    {
        return await _dataBaseContext.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}