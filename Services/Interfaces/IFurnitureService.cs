using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.DTOs.Requests;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface IFurnitureService
    {
        Task<List<Furniture>> GetFurnituresAsync();
        Task CreateFurnitureAsync(string name);
        Task UpdateFurnitureAsync(FurnitureRequest request);
        Task DeleteFurnitureAsync(string furId);
    }
}