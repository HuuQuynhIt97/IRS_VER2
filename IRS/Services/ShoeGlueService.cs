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
    public interface IShoeGlueService : IServiceBase<ShoeGlue, ShoeGlueDto>
    {
        Task<object> LoadData(string glueGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task<object> GetMenuPageSetting();
        Task<object> GetRecipePageSetting();

    }
    public class ShoeGlueService : ServiceBase<ShoeGlue, ShoeGlueDto>, IShoeGlueService
    {
        private readonly IRepositoryBase<ShoeGlue> _repo;
        private readonly IRepositoryBase<Chemical> _repoChemical;
        private readonly IRepositoryBase<CodeType> _repoCodeType;
        private readonly IRepositoryBase<Glue> _repoGlue;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ShoeGlueService(
            IRepositoryBase<ShoeGlue> repo,
            IRepositoryBase<CodeType> repoCodeType,
            IRepositoryBase<Chemical> repoChemical,
            IRepositoryBase<Glue> repoGlue,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoGlue = repoGlue;
            _repoCodeType = repoCodeType;
            _repoChemical = repoChemical;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public override async Task<OperationResult> AddAsync(ShoeGlueDto model)
        {
            try
            {
                var item = _mapper.Map<ShoeGlue>(model);
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

        public override async Task<List<ShoeGlueDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<ShoeGlueDto>(_configMapper);

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

        public async Task<object> LoadData(string shoeGuid)
        {
            var shoeGlue = await _repo.FindAll(x => x.Status == 1 && x.ShoesGuid == shoeGuid)
            .OrderByDescending(x => x.Id).ToListAsync();
            var glue = await _repoGlue.FindAll().ToListAsync();
            var datasource = (from x in shoeGlue
                              join y in glue on x.GlueGuid equals y.Guid
                              select new
                              {
                                  x.Id,
                                  x.Guid,
                                  x.ShoesGuid,
                                  x.GlueGuid,
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
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }

        public async Task<object> GetMenuPageSetting()
        {
            var pageCount = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Count).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Count).FirstOrDefault().CodeNo : "5";

            var pageSize = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Size).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Size).FirstOrDefault().CodeNo : "5";

            var pageSizes = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Sizes).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Menu_PageSetting_Sizes).Select(x => x.CodeNo).ToList() : null;

            return new
            {
                pageCount,
                pageSize,
                pageSizes
            };
        }

        public async Task<object> GetRecipePageSetting()
        {
            var pageCount = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Count).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Count).FirstOrDefault().CodeNo : "5";

            var pageSize = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Size).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Size).FirstOrDefault().CodeNo : "5";

            var pageSizes = _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Sizes).FirstOrDefault() != null ?
                _repoCodeType.FindAll(x => x.CodeType1 == Constants.CodeTypeConst.Recipe_PageSetting_Sizes).Select(x => x.CodeNo).ToList() : null;

            return new
            {
                pageCount,
                pageSize,
                pageSizes
            };
        }
    }
}
