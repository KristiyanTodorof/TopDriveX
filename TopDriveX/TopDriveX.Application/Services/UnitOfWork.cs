using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Application.Contracts;
using TopDriveX.Domain.Models;
using TopDriveX.Infrastructure.Data;

namespace TopDriveX.Application.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public IRepository<Make> Makes { get; }
        public IRepository<Model> Models { get; }
        public IRepository<VehicleType> VehicleTypes { get; }
        public IRepository<Vehicle> Vehicles { get; }
        public IRepository<User> Users { get; }
        public IRepository<Advertisement> Advertisements { get; }
        public IRepository<VehicleImage> VehicleImages { get; }
        public IRepository<Favorite> Favorites { get; }
        public IRepository<SavedSearch> SavedSearches { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Makes = new Repository<Make>(context);
            Models = new Repository<Model>(context);
            VehicleTypes = new Repository<VehicleType>(context);
            Vehicles = new Repository<Vehicle>(context);
            Users = new Repository<User>(context);
            Advertisements = new Repository<Advertisement>(context);
            VehicleImages = new Repository<VehicleImage>(context);
            Favorites = new Repository<Favorite>(context);
            SavedSearches = new Repository<SavedSearch>(context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
