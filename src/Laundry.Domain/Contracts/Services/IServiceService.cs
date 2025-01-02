using Laundry.Domain.Entities;
using Laundry.Domain.Utils;

namespace Laundry.Domain.Contracts.Services;

public interface IServiceService
{
    Task<IQueryable<Service>> GetAllAvailableServicesAsync();
    Task<IQueryable<Service>> GetAllServicesAsync();
    Task<Service> GetServiceByIdAsync(int id);
    Task<int> AddServiceAsync(Service service);
    Task UpdateServiceAsync(Service service);
    Task DeleteServiceAsync(int id);
}