using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface IFurnitureRepository
    {
        Task<List<Furniture>> GetFurnituresAsync();
        Task<Furniture> GetFurnitureByIdAsync(string furId);
        void CreateFurnitureAsync(Furniture furniture);
        void UpdateOrDeleteFurnitureAsync(Furniture furniture);
        Task<string> GetLatestFurnitureIdAsync();
        Task SaveChangesAsync();
    }
}