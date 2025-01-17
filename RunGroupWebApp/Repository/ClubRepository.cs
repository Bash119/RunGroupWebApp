﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly ApplicationDbContext _context;

        public ClubRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
      

        public bool Add(Club club)
        {
           _context.Add(club);
            return  Save();
        }

      
        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

      

        public async Task<IEnumerable<Club>> GetAllClubsAsync()
        {
           return await _context.Clubs.ToListAsync();  
        }

       

        public async Task<IEnumerable<Club>> GetClubByCityAsync(string city)
        {
            return await _context.Clubs.Where(c=>c.Address.City.Contains(city)).ToListAsync(); 
        }

      

        public async Task<Club> GetClubByIdAsync(int id)
        {
            return await _context.Clubs.Include(a => a.Address).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Club> GetClubByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved > 0 ? true:false;
        }

       

        public bool Update(Club club)
        {
           _context.Update(club);
            return Save();
        }
    }
}
