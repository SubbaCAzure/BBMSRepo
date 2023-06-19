using AutoMapper;
using BBMS.Domain.Models;
using BBMS.Repository.Interfaces;
using BBMS.Services.DTO;
using BBMS.Services.Interfaces;

namespace BBMS.Services
{
    public class BarService : IBarService
    {
        private readonly IBarRepository _barRepository;
        private readonly IMapper _mapper;
        public BarService(IBarRepository barRepository, IMapper mapper)
        {
            _barRepository = barRepository;
            _mapper = mapper;
        }                   
        public async Task<Bar> AddBarAsync(Bar bar)
        {
            try
            {
                var addedBar = await _barRepository.AddBarAsync(bar);
                return addedBar;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the bar.", ex);
            }
        }
        public async Task UpdateBar(Bar bar)
        {
            try
            {
                await _barRepository.UpdateBar(bar);
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the bar.", ex);
            }
        }
        public async Task<IEnumerable<Bar>> GetAllBarsAsync()
        {
            var bars = await _barRepository.GetAllBarsAsync();
            return bars;
        }
        public async Task<BarDTO> GetBarAsync(int id)
        {
            try
            {
                Bar bar = await _barRepository.GetBarAsync(id);
                BarDTO barDTO = _mapper.Map<BarDTO>(bar);
                return barDTO;

            }
            catch (Exception ex)
            { 
                throw new Exception("Error occurred while adding the beer.", ex);
            }

            
        }

        //public async Task<BarDTO> GetBarFromDatabase(int barId)
        //{
        //    try
        //    {
        //        Bar bar = await _barRepository.GetBarAsync(barId);
        //        BarDTO barDTO = _mapper.Map<BarDTO>(bar);
        //        return barDTO;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error occurred while adding the beer.", ex);
        //    }
        //}

        public async Task<BarDTO> GetBarFromDatabase(int barId)
        {
            try
            {
                Bar bar = await _barRepository.GetBarAsync(barId);
                BarDTO barDTO = _mapper.Map<BarDTO>(bar);
                return barDTO;

            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while adding the beer.", ex);
            }
        }
    }
}
