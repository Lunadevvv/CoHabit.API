using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoHabit.API.Enitites;
using CoHabit.API.Repositories.Interfaces;
using CoHabit.API.Services.Interfaces;

namespace CoHabit.API.Services.Implements
{
    public class SubcriptionService : ISubcriptionService
    {
        private readonly ISubcriptionRepository _subcriptionRepository;
        public SubcriptionService(ISubcriptionRepository subcriptionRepository)
        {
            _subcriptionRepository = subcriptionRepository;
        }

        public async Task<Subcription> GetSubcriptionById(int subcriptionId)
        {
            return await _subcriptionRepository.GetSubcriptionById(subcriptionId);
        }
    }
}