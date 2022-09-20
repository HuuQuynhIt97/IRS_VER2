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
using Microsoft.AspNetCore.Http;

namespace IRS.Services
{
    public interface IStockInInkService : IServiceBase<StockInInk, StockInInkDto>
    {
        Task<object> LoadData(string colorGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task<bool> Approve(string guid);
        Task<bool> UnApprove(string guid);
        Task<bool> UpdateExecution(List<StockInInkDto> model);

        Task<object> DataFiterExecuteAndCreate(StockInInkFilterRequestDto filter);

    }
    public class StockInInkService : ServiceBase<StockInInk, StockInInkDto>, IStockInInkService
    {
        private readonly IRepositoryBase<StockInInk> _repo;
        private readonly IRepositoryBase<Ink> _repoInk;
        private readonly IRepositoryBase<Process> _repoProcess;
        private readonly IRepositoryBase<Supplier> _repoSupplier;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly MapperConfiguration _configMapper;
        public StockInInkService(
            IRepositoryBase<StockInInk> repo,
            IRepositoryBase<Process> repoProcess,
            IRepositoryBase<Supplier> repoSupplier,
            IRepositoryBase<Ink> repoInk,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IHttpContextAccessor contextAccessor,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _contextAccessor = contextAccessor;
            _repoProcess = repoProcess;
            _repoSupplier = repoSupplier;
            _repoInk = repoInk;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        //public async Task<OperationResult> IsExistKey(string key , string version)
        //{
        //    var item = await _repo.FindAll(x => x.Status == 1 && x.Name == key ).AnyAsync();
        //    if (item)
        //    {
        //        return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "COLOR_NAME_ALREADY_EXISTED", Success = false };
        //    }
        //    operationResult = new OperationResult
        //    {
        //        StatusCode = HttpStatusCode.OK,
        //        Success = true,
        //        Data = item
        //    };
        //    return operationResult;
        //}
        public override async Task<OperationResult> AddAsync(StockInInkDto model)
        {
            try
            {
                //var check = await IsExistKey(model.Name, model.Name);
                //if (!check.Success) return check;
                var item = _mapper.Map<StockInInk>(model);
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

        public override async Task<OperationResult> UpdateAsync(StockInInkDto model)
        {
            try
            {

                //var checkKey_pre = await _repo.FindAll(x => x.Name == model.Name && x.Status == 1).AsNoTracking().FirstOrDefaultAsync();
                //var checkKey = await _repo.FindAll(x => x.Id == model.Id && x.Status == 1).AsNoTracking().FirstOrDefaultAsync();
                //if (checkKey != null && checkKey_pre != null)
                //{
                //    if (checkKey.Name != model.Name)
                //    {
                //        var check = await IsExistKey(model.Name, model.Name);
                //        if (!check.Success) return check;
                //    }
                //}
                var item = _mapper.Map<StockInInk>(model);
            
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

        public override async Task<List<StockInInkDto>> GetAllAsync()
        {
            var time = DateTime.Now;
            var query = await _repo.FindAll(x => x.Status == 1 && x.CreateDate.Value.Date == time.Date && x.CreateDate.Value.Month == time.Month).ToListAsync();
            var ink = await _repoInk.FindAll(x => x.IsShow).ToListAsync();
            var account = await _repoXAccount.FindAll(x => x.Status == "1").ToListAsync();
            var data = (from x in query
                       join y in ink on x.InkGuid equals y.Guid
                       let approveBy = account.Where(o => o.AccountId == x.ApproveBy).FirstOrDefault() != null
                       ? account.Where(o => o.AccountId == x.ApproveBy).FirstOrDefault().AccountName : null
                       select new StockInInkDto
                       {
                           Id = x.Id,
                           InkName = y.Name,
                           ExecuteDate = x.ExecuteDate,
                           RealAmount = x.RealAmount,
                           Guid = x.Guid,
                           ApproveStatus = x.ApproveDate == null ? false : true,
                           RemainingAmount = x.RemainingAmount,
                           CreateDate = x.CreateDate,
                           ApproveByName = approveBy
                       }).ToList();

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

        public async Task<object> LoadData(string colorGuid)
        {
            //var inkColor = await _repo.FindAll(x => x.Status == 1 && x.ColorGuid == colorGuid)
            //.OrderByDescending(x => x.Id).ToListAsync();
            //var ink = await _repoInk.FindAll(x => x.IsShow).ToListAsync();
            //var process = await _repoProcess.FindAll().ToListAsync();
            //var datasource = (from x in inkColor
            //                  join y in ink on x.InkGuid equals y.Guid
            //                  join z in process on y.ProcessId equals z.ID
            //                  select new
            //                  {
            //                      x.Id,
            //                      x.Guid,
            //                      x.InkGuid,
            //                      x.ColorGuid,
            //                      x.Percentage,
            //                      x.Unit,
            //                      y.Code,
            //                      Name = y.Name + "(" + z.Name + ")",
            //                      Process = z.Name
            //                  }).OrderBy(o => o.Id).ToList();

            //return datasource;
            throw new NotImplementedException();
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
                x.InkGuid,
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }

        public async Task<bool> UpdateExecution(List<StockInInkDto> model)
        {
            try
            {

                if (model.Count > 0)
                {
                    foreach (var item in model)
                    {
                        var item_update = _repo.FindAll(x => x.Guid == item.Guid).FirstOrDefault();
                        item_update.ExecuteDate = DateTime.Now;
                        _repo.Update(item_update);
                        await _unitOfWork.SaveChangeAsync();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<bool> Approve(string guid)
        {
            try
            {
                var httpContext = _contextAccessor.HttpContext;
                var accessToken = httpContext.Request.Headers["Authorization"];
                var accountID = JWTExtensions.GetDecodeTokenByID(accessToken).ToDecimal();
                var item_update = _repo.FindAll(x => x.Guid == guid).FirstOrDefault();
                item_update.ApproveDate = DateTime.Now;
                item_update.ApproveBy = accountID;
                _repo.Update(item_update);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            //throw new NotImplementedException();
        }

        public async Task<bool> UnApprove(string guid)
        {
            try
            {
                var httpContext = _contextAccessor.HttpContext;
                var accessToken = httpContext.Request.Headers["Authorization"];
                var accountID = JWTExtensions.GetDecodeTokenByID(accessToken).ToDecimal();
                var item_update = _repo.FindAll(x => x.Guid == guid).FirstOrDefault();
                item_update.ApproveDate = null;
                item_update.ApproveBy = accountID;
                _repo.Update(item_update);
                await _unitOfWork.SaveChangeAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
        }

        public async Task<object> DataFiterExecuteAndCreate(StockInInkFilterRequestDto filter)
        {
            var query = await _repo.FindAll(x => x.Status == 1 &&
            (filter.executionDate.Value.Date == x.ExecuteDate.Value.Date 
                && filter.executionDate.Value.Month == x.ExecuteDate.Value.Month && filter.executionDate.Value.Year == x.ExecuteDate.Value.Year
                || filter.createDate.Value.Date == x.CreateDate.Value.Date
                && filter.createDate.Value.Month == x.CreateDate.Value.Month && filter.createDate.Value.Year == x.CreateDate.Value.Year
            )
            ).ToListAsync();
            var ink = await _repoInk.FindAll(x => x.IsShow).ToListAsync();
            var account = await _repoXAccount.FindAll(x => x.Status == "1").ToListAsync();
            var data = (from x in query
                        join y in ink on x.InkGuid equals y.Guid
                        let approveBy = account.Where(o => o.AccountId == x.ApproveBy).FirstOrDefault() != null
                        ? account.Where(o => o.AccountId == x.ApproveBy).FirstOrDefault().AccountName : null
                        select new StockInInkDto
                        {
                            Id = x.Id,
                            InkName = y.Name,
                            ExecuteDate = x.ExecuteDate,
                            RealAmount = x.RealAmount,
                            Guid = x.Guid,
                            ApproveStatus = x.ApproveDate == null ? false : true,
                            RemainingAmount = x.RemainingAmount,
                            CreateDate = x.CreateDate,
                            ApproveByName = approveBy
                        }).ToList();

            return data;
        }
    }
}
