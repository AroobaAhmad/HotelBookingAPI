using HotelBookingAPI.Models;
using System;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingAPI.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties

        private readonly AppDbContext _context;
        public IGenericRepository<Customer> Customers { get; private set; }

        #endregion

        #region Methods

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Customers = new GenericRepository<Customer>(_context);
        }
        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        #endregion

    }
}
