using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface ICharacteristicService
    {
        Task<List<Characteristic>> GetAllCharacteristicsAsync();
        Task<Characteristic?> GetCharacteristicByIdAsync(string id);
        Task AddCharacteristicAsync(string title);
        Task UpdateCharacteristicAsync(CharacteristicRequest request);
        Task DeleteCharacteristicAsync(string id);
    }
}