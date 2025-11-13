using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;

namespace CoHabit.API.Repositories.Interfaces
{
    public interface ISubcriptionRepository
    {
        public Task<Subcription> GetSubcriptionById(int subcriptionId);
    }
}