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
    public interface IProcessService : IServiceBase<Process, ProcessDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);

    }
    public class ProcessService : ServiceBase<Process, ProcessDto>, IProcessService
    {
        private readonly IRepositoryBase<Process> _repo;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ProcessService(
            IRepositoryBase<Process> repo,
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

        public async Task<OperationResult> IsExistKey(string key)
        {
            var item = await _repo.FindAll(x => x.Name == key).AnyAsync();
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

        public override async Task<OperationResult> AddAsync(ProcessDto model)
        {
            try
            {
                var check = await IsExistKey(model.Name);
                if (!check.Success) return check;
                var item = _mapper.Map<Process>(model);

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

        public override async Task<OperationResult> UpdateAsync(ProcessDto model)
        {
            try
            {
                var checkKey = await _repo.FindAll(x => x.ID == model.ID).AsNoTracking().FirstOrDefaultAsync();
                if (checkKey != null )
                {
                    if (checkKey.Name != model.Name)
                    {
                        var check = await IsExistKey(model.Name);
                        if (!check.Success) return check;
                    }
                    
                }
                var item = _mapper.Map<Process>(model);
               
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

        public override async Task<List<ProcessDto>> GetAllAsync()
        {
            return await _repo.FindAll().ProjectTo<ProcessDto>(_configMapper).OrderBy(x => x.ID).ToListAsync();
            //return await _repo.FindAll().Select(x => new ProcessDto
            //{
            //    ID = x.Id,
            //    Name = x.Name,
            //    Color = x.Color
            //}).OrderByDescending(x => x.ID).ToListAsync();

        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            //item.CancelFlag = "Y";
            _repo.Remove(item);
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
            var datasource = _repo.FindAll()
            .OrderByDescending(x=> x.ID)
            .Select(x => new ProcessDto {
                ID = x.ID,
                Name = x.Name,
                Guid = x.Guid,
                Color = x.Color
              
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
            //var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x => new { 
            //    x.ModifiedBy, 
            //    x.CreatedBy, 
            //    x.CreatedDate, 
            //    x.ModifiedDate 
            //}).FirstOrDefaultAsync();
            //string createBy = "N/A";
            //string createDate = "N/A";
            //string updateBy = "N/A";
            //string updateDate = "N/A";
            //if (data == null)
            //    return new
            //    {
            //        createBy,
            //        createDate,
            //        updateBy,
            //        updateDate
            //    };
            //if (data.ModifiedBy > 0)
            //{
            //    var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.ModifiedBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
            //    updateBy = updateBy != null ? updateAudit.Uid : "N/A";
            //    updateDate = data.ModifiedDate != null ? data.ModifiedDate.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            //}
            //if (data.CreatedBy > 0)
            //{
            //    var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.CreatedBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
            //    createBy = createAudit != null ? createAudit.Uid : "N/A";
            //    createDate = data.CreatedDate != null ? data.CreatedDate.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            //}
            //return new
            //{
            //    createBy,
            //    createDate,
            //    updateBy,
            //    updateDate
            //};
            throw new NotImplementedException();
        }

        public async Task<object> LoadDataBySite(string siteID)
        {
            var query = _repo.FindAll().Select(x => new { 
                x.Name,
                x.Color
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }
    }
}
