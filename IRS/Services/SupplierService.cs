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
    public interface ISupplierService : IServiceBase<Supplier, SupplierDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task<object> GetAllByTreatment(int ID);

    }
    public class SupplierService : ServiceBase<Supplier, SupplierDto>, ISupplierService
    {
        private readonly IRepositoryBase<Supplier> _repo;
        private readonly IRepositoryBase<Process> _repoProcess;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public SupplierService(
            IRepositoryBase<Supplier> repo,
            IRepositoryBase<Process> repoProcess,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoProcess = repoProcess;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public async Task<OperationResult> IsExistKey(string key)
        {
            var item = await _repo.FindAll(x => x.Name == key && x.IsShow == true).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "GLUE_NAME_ALREADY_EXISTED", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }

        public override async Task<OperationResult> AddAsync(SupplierDto model)
        {
            try
            {
                var check = await IsExistKey(model.Name);
                if (!check.Success) return check;
                var item = _mapper.Map<Supplier>(model);
                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
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

        public override async Task<OperationResult> UpdateAsync(SupplierDto model)
        {
            try
            {
                var checkKey = await _repo.FindAll(x => x.Id == model.ID && x.IsShow == true).AsNoTracking().FirstOrDefaultAsync();
                if (checkKey != null )
                {
                    if (checkKey.Name != model.Name)
                    {
                        var check = await IsExistKey(model.Name);
                        if (!check.Success) return check;
                    }
                    
                }
                var item = _mapper.Map<Supplier>(model);
               
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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

        public override async Task<List<SupplierDto>> GetAllAsync()
        {
            return await _repo.FindAll(x => x.IsShow == true).Select(x => new SupplierDto
            {
                ID = x.Id,
                Name = x.Name,
                ProcessID = x.ProcessId,
                Process = _repoProcess.FindByID(x.ProcessId) != null ? _repoProcess.FindByID(x.ProcessId).Name : null
            }).OrderByDescending(x => x.ID).ToListAsync();

        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            item.IsShow = false;
            //item.CancelFlag = "Y";
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
            var datasource = _repo.FindAll(x => x.IsShow == true)
            .Select(x => new SupplierDto {
                ID = x.Id,
                Name = x.Name,
            });
            var count = await datasource.CountAsync();
            if (data.Where != null)// for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)// for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)// for sorting
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);

            count = await datasource.CountAsync();

            if (data.Skip >= 0)// for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)// for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = await datasource.ToListAsync(),
                Count = count
            };
        }

        public async Task<object> GetAudit(object id)
        {
            var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x => new { 
                x.ModifiedBy, 
                x.ModifiedDate, 
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
            if (data.ModifiedBy > 0)
            {
                var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.ModifiedBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                createBy = createAudit != null ? createAudit.Uid : "N/A";
                createDate = data.ModifiedDate != null ? data.ModifiedDate.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
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
            var query = _repo.FindAll().Select(x => new { 
                x.Name,
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }

        public async Task<object> GetAllByTreatment(int id)
        {
            var data = from a in _repo.FindAll(x => x.IsShow == true && x.ProcessId == id)
                       join b in _repoProcess.FindAll() on a.ProcessId equals b.ID
                       select new SupplierDto
                       {
                           ID = a.Id,
                           Name = a.Name,
                           Process = b.Name
                       };

            return data.OrderByDescending(x => x.ID).ToList();
        }
    }
}
