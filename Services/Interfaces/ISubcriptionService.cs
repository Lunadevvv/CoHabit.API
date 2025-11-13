using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Services.Interfaces
{
    public interface ISubcriptionService
    {
        public Task<Subcription> GetSubcriptionById(int subcriptionId);
    }
}