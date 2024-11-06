using RunGroupWebApp.Models;

namespace RunGroupWebApp.Interfaces
{
    public interface IRaceRepository
    {
        Task<IEnumerable<Race>> GetAllRacesAsync();

        Task<Race> GetRaceByIdAsync(int id);

        Task<IEnumerable<Race>> GetAllRacesByCityAsync(string city);

        bool Add(Race race);
        bool Update(Race race);
        bool Delete(Race race);

        bool Save();
    }
}
