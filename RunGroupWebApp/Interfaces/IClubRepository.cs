﻿using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IClubRepository
    {

        Task<IEnumerable<Club>> GetAllClubsAsync();

        Task<Club> GetClubByIdAsync(int id);
        Task<Club> GetClubByIdAsyncNoTracking(int id);

        Task<IEnumerable<Club>> GetClubByCityAsync(string city);

        bool Add(Club club);
        bool Update(Club club);
        bool Delete(Club club);

        bool Save();
    }
}
