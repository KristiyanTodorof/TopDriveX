using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopDriveX.Domain.BaseEntities;

namespace TopDriveX.Infrastructure.Data.Extensions
{
    public static class DbContextExtensions
    {
        public static void SoftDelete<T>(this DbContext context, T entity) where T : BaseEntity
        {
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
            context.Entry(entity).State = EntityState.Modified;
        }

        public static void RestoreSoftDeleted<T>(this DbContext context, T entity) where T : BaseEntity
        {
            entity.IsDeleted = false;
            entity.DeletedAt = null;
            context.Entry(entity).State = EntityState.Modified;
        }

        public static IQueryable<T> IncludeDeleted<T>(this DbSet<T> dbSet) where T : class
        {
            return dbSet.IgnoreQueryFilters();
        }

        public static IQueryable<T> OnlyDeleted<T>(this DbSet<T> dbSet) where T : BaseEntity
        {
            return dbSet.IgnoreQueryFilters().Where(e => e.IsDeleted);
        }
    }
}
