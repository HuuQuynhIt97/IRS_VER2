﻿using AutoMapper;
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
    public interface IShoeService : IServiceBase<Shoe, ShoeDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);

    }
    public class ShoeService : ServiceBase<Shoe, ShoeDto>, IShoeService
    {
        private readonly IRepositoryBase<Shoe> _repo;
        private readonly IRepositoryBase<Process> _repoTreatment;
        private readonly IRepositoryBase<Process2> _repoProcess;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ShoeService(
            IRepositoryBase<Shoe> repo,
            IRepositoryBase<Process> repoTreatment,
            IRepositoryBase<Process2> repoProcess,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoTreatment = repoTreatment;
            _repoProcess = repoProcess;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<OperationResult> IsExistKey(string key , string version)
        {
            var item = await _repo.FindAll(x => x.ModelNo == key && x.Status == 1 && x.Version == version).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "MODEL_NO_AND_VERSION_ALREADY_EXISTED", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }
        public override async Task<OperationResult> AddAsync(ShoeDto model)
        {
            try
            {
                var check = await IsExistKey(model.ModelNo, model.Version);
                if (!check.Success) return check;
                var item = _mapper.Map<Shoe>(model);
                item.ModelNo = model.ModelNo.Trim();
                item.Status = 1;
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

        public override async Task<OperationResult> UpdateAsync(ShoeDto model)
        {
            try
            {
                
                var checkKey_pre = await _repo.FindAll(x => x.ModelNo == model.ModelNo && x.Status == 1 && x.Version == model.Version).AsNoTracking().FirstOrDefaultAsync();
                var checkKey = await _repo.FindAll(x => x.Id == model.Id && x.Status == 1).AsNoTracking().FirstOrDefaultAsync();
                if (checkKey != null && checkKey_pre != null)
                {
                    if (checkKey.ModelNo != model.ModelNo || checkKey.Version != model.Version)
                    {
                        var check = await IsExistKey(model.ModelNo, model.Version);
                        if (!check.Success) return check;
                    }
                }
                var item = _mapper.Map<Shoe>(model);
                item.ModelNo = model.ModelNo.Trim();
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

        public override async Task<List<ShoeDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<ShoeDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            //item.CancelFlag = "Y";
            item.Status = 0;
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
            var datasource = _repo.FindAll(x => x.Status == 1)
            .OrderByDescending(x => x.Id)
            .Select(x => new {
                x.Id,
                x.Guid,
                x.ModelName,
                x.ModelNo,
                x.Article1,
                x.Article2,
                x.Version,
                x.Remark,
                x.ProductionDate,
                x.TreatmentGuid,
                Treatment = _repoTreatment.FindAll().Where(o => o.Guid == x.TreatmentGuid).FirstOrDefault() != null
                ? _repoTreatment.FindAll().Where(o => o.Guid == x.TreatmentGuid).FirstOrDefault().Name : null,
                Process = _repoProcess.FindAll().Where(o => o.Guid == x.ProcessGuid).FirstOrDefault() != null
                ? _repoProcess.FindAll().Where(o => o.Guid == x.ProcessGuid).FirstOrDefault().Name : null
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
            var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x => new { x.UpdateBy, x.CreateBy, x.UpdateDate, x.CreateDate }).FirstOrDefaultAsync();
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
            if (data.UpdateBy.HasValue)
            {
                var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.UpdateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                updateBy = updateBy != null ? updateAudit.Uid : "N/A";
                updateDate = data.UpdateDate.HasValue ? data.UpdateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
            }
            if (data.CreateBy.HasValue)
            {
                var createAudit = await _repoXAccount.FindAll(x => x.AccountId == data.CreateBy).AsNoTracking().Select(x => new { x.Uid }).FirstOrDefaultAsync();
                createBy = createAudit != null ? createAudit.Uid : "N/A";
                createDate = data.CreateDate.HasValue ? data.CreateDate.Value.ToString("yyyy/MM/dd HH:mm:ss") : "N/A";
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
            var query = _repo.FindAll(x => x.Status == 1).Select(x => new { 
                x.ModelName,
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }
    }
}