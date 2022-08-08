using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using IRS.Constants;
using IRS.Data;
using IRS.DTO;
using IRS.Helpers;
using IRS.Models;
using IRS.Services.Base;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IRS.Services
{
    public interface IChemicalService : IServiceBase<Chemical2, Chemical2Dto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);

    }
    public class ChemicalService : ServiceBase<Chemical2, Chemical2Dto>, IChemicalService
    {
        private readonly IRepositoryBase<Chemical2> _repo;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ChemicalService(
            IRepositoryBase<Chemical2> repo,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public override async Task<OperationResult> AddAsync(Chemical2Dto model)
        {
            try
            {
                var item = _mapper.Map<Chemical2>(model);
                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                item.isShow = true;
                _repo.Add(item);

                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = model
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public override async Task<List<Chemical2Dto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.isShow == true).ProjectTo<Chemical2Dto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            //item.CancelFlag = "Y";
            item.isShow = false;
            _repo.Update(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.DeleteSuccess,
                    Success = true,
                    Data = item
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<object> LoadData(DataManager data, string farmGuid)
        {
            var datasource = _repo.FindAll(x => x.isShow == true).Include(x => x.Supplier).Include(x => x.Processes)
            .OrderByDescending(x=> x.ID)
            .Select(x => new Chemical2Dto {
                ID = x.ID,
                Code = x.Code,
                Name = x.Name,
                NameEn = x.NameEn,
                Color = x.Processes.Color,
                CreatedDate = x.CreatedDate,
                MaterialNO = x.MaterialNO,
                VOC = x.VOC,
                Unit = x.Unit,
                Supplier = x.Supplier.Name,
                Process = x.Processes.Name,
                DaysToExpiration = x.DaysToExpiration,
                Modify = x.Modify,
                Guid = x.Guid,
                SupplierID = x.SupplierID,
                ProcessID = x.ProcessID
            });
            var count = await datasource.CountAsync();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = await datasource.CountAsync();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }
        public async Task<object> GetAudit(object id)
        {
            var data = await _repo.FindAll(x => x.ID.Equals(id)).AsNoTracking().Select(x => new { 
                x.ModifiedBy,
                x.CreatedBy, 
                x.ModifiedDate, 
                x.CreatedDate 
            }).FirstOrDefaultAsync();
            string createBy = "N/A";
            string createDate = "N/A";
            string updateBy = "N/A";
            string updateDate = "N/A";
            if (data == null)
                return new
                {
                    createBy,
                    createDate,
                    updateBy,
                    updateDate
                };
            if (data.ModifiedBy > 0)
            {
                var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.ModifiedBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                updateBy = updateBy != null ? updateAudit.Uid : "N/A";
                updateDate = data.ModifiedDate != null ? data.ModifiedDate.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            }
            if (data.CreatedBy > 0)
            {
                var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.CreatedBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                createBy = createAudit != null ? createAudit.Uid : "N/A";
                createDate = data.CreatedDate != null ? data.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            }
            return new
            {
                createBy,
                createDate,
                updateBy,
                updateDate
            };
        }

        public async Task<object> LoadDataBySite(string siteID)
        {
            var query = _repo.FindAll(x => x.isShow).Select(x => new { 
                x.Name,
                x.MaterialNO
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }
    }
}
