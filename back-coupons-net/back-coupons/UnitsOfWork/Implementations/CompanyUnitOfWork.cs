using back_coupons.DTOs;
using back_coupons.Entities;
using back_coupons.Repositories.Interfaces;
using back_coupons.Responses;
using back_coupons.UnitsOfWork.Interfaces;

namespace back_coupons.UnitsOfWork.Implementations
{
    public class CompanyUnitOfWork : GenericUnitOfWork<Company>, ICompanyUnitOfWork
    {
        private readonly ICompanyRepository _companyRepository;
        public CompanyUnitOfWork(IGenericRepository<Company> repository, ICompanyRepository companyRepository) : base(repository)
        {
            _companyRepository = companyRepository;
        }

        public async Task<ActionResponse<Company>> GetCompanyByUserAsync(string userId) => await _companyRepository.GetCompanyByUserAsync(userId);
        public override async Task<ActionResponse<IEnumerable<Company>>> GetAsyncFull() => await _companyRepository.GetAsyncFull();
        public override async Task<ActionResponse<Company>> GetAsync(int id) => await _companyRepository.GetAsync(id);
        public override async Task<ActionResponse<IEnumerable<Company>>> GetAsync(PaginationDTO pagination) => await _companyRepository.GetAsync(pagination);
    }
}
