namespace API.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IApartmentRepository ApartmentRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}