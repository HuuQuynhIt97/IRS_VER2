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
    public interface IInkColorService : IServiceBase<InkColor, InkColorDto>
    {
        Task<object> LoadData(string colorGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);

    }
    public class InkColorService : ServiceBase<InkColor, InkColorDto>, IInkColorService
    {
        private readonly IRepositoryBase<InkColor> _repo;
        private readonly IRepositoryBase<Ink> _repoInk;
        private readonly IRepositoryBase<Supplier> _repoSupplier;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public InkColorService(
            IRepositoryBase<InkColor> repo,
            IRepositoryBase<Supplier> repoSupplier,
            IRepositoryBase<Ink> repoInk,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
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
        public override async Task<OperationResult> AddAsync(InkColorDto model)
        {
            try
            {
                //var check = await IsExistKey(model.Name, model.Name);
                //if (!check.Success) return check;
                var supplierID = _repoInk.FindAll(x => x.Guid == model.InkGuid).FirstOrDefault().SupplierId;
                var supplierGUID = _repoSupplier.FindByID(supplierID).Guid;
                var item = _mapper.Map<InkColor>(model);
                item.Status = 1;
                item.SupplierGuid = supplierGUID;
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

        public override async Task<OperationResult> UpdateAsync(InkColorDto model)
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
                var item = _mapper.Map<InkColor>(model);
                if (string.IsNullOrEmpty(item.SupplierGuid))
                {
                    var supplierID = _repoInk.FindAll(x => x.Guid == model.InkGuid).FirstOrDefault().SupplierId;
                    var supplierGUID = _repoSupplier.FindByID(supplierID).Guid;
                    item.SupplierGuid = supplierGUID;

                }
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

        public override async Task<List<InkColorDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<InkColorDto>(_configMapper);

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

        public async Task<object> LoadData(string colorGuid)
        {
            var inkColor = await _repo.FindAll(x => x.Status == 1 && x.ColorGuid == colorGuid)
            .OrderByDescending(x => x.Id).ToListAsync();
            var ink = await _repoInk.FindAll(x => x.IsShow).ToListAsync();
            var datasource = (from x in inkColor
                              join y in ink on x.InkGuid equals y.Guid
                              select new
                              {
                                  x.Id,
                                  x.Guid,
                                  x.InkGuid,
                                  x.ColorGuid,
                                  x.Percentage,
                                  x.Unit,
                                  y.Name
                              }).ToList();

            return datasource;
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
    }
}
