using AquaCaps.Application.DTOs;
using AquaCaps.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCaps.Application.Interface;

public interface IUserRepository
{
    Task<long> InsertAsync(User user);
    Task DeleteAsync(long userId);
    Task UpdateUserRoleAsync(long userId, Role role);
    Task<User?> SelectByIdAsync(long userId);
    Task<User?> SelectByPhoneNumber(string number);
    Task<User?> SelectByUserNameAsync(string userName);
    Task<ICollection<User>> SelectAllAsync(int skip, int take);
    Task<ICollection<User>> GetAllCouriersAsync();
    public Task<List<User>> GetActiveCouriersAsync();
    public Task<int> GetCountUsersAsync();
    public Task<int> CountActiveCouriersAsync();
    Task<User?> GetByPhoneAsync(string phoneNumber);
    Task<List<User>> GetClientsInactiveSinceAsync(DateTime sinceDate);
    Task <ICollection<User?>>SelectUsersByRoleAsync(string roleName);
    //IQueryable<User> SelectAll();
}
