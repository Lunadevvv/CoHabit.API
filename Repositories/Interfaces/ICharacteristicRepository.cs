using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface ICharacteristicRepository
    {
        //CRUD methods for Characteristic entity
        Task<List<Characteristic>> GetAllCharacteristicsAsync();
        Task<Characteristic?> GetCharacteristicByIdAsync(string id);
        Task AddCharacteristicAsync(Characteristic characteristic);
        Task UpdateCharacteristicAsync(Characteristic characteristic);
        Task DeleteCharacteristicAsync(Characteristic characteristic);
        Task<string> GetLastCharIdAsync();
    }
}