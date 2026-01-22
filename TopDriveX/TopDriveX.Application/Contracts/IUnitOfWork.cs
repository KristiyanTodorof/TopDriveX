using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.Models;

namespace TopDriveX.Application.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Make> Makes { get; }
        IRepository<Model> Models { get; }
        IRepository<VehicleType> VehicleTypes { get; }
        IRepository<Vehicle> Vehicles { get; }
        IRepository<User> Users { get; }
        IRepository<Advertisement> Advertisements { get; }
        IRepository<VehicleImage> VehicleImages { get; }
        IRepository<Favorite> Favorites { get; }
        IRepository<SavedSearch> SavedSearches { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
