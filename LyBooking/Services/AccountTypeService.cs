using AutoMapper;
using IRS.Data;
using IRS.DTO;
using IRS.Models;
using IRS.Services.Base;

namespace IRS.Services
{
    public interface IAccountTypeService: IServiceBase<AccountType, AccountTypeDto>
    {
    }
    public class AccountTypeService : ServiceBase<AccountType, AccountTypeDto>, IAccountTypeService
    {
        private readonly IRepositoryBase<AccountType> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;

        public AccountTypeService(
            IRepositoryBase<AccountType> repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
    }
}
