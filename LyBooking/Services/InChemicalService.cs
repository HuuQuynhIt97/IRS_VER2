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
    public interface IInChemicalService : IServiceBase<InChemical, InChemicalDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task<object> OutOfStock(string inChemicalGuid);

    }
    public class InChemicalService : ServiceBase<InChemical, InChemicalDto>, IInChemicalService
    {
        private readonly IRepositoryBase<InChemical> _repo;
        private readonly IRepositoryBase<Chemical2> _repoChemical;
        private readonly IRepositoryBase<Supplier> _repoSupplier;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public InChemicalService(
            IRepositoryBase<InChemical> repo,
            IRepositoryBase<Chemical2> repoChemical,
            IRepositoryBase<Supplier> repoSupplier,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoChemical = repoChemical;
            _repoSupplier = repoSupplier;
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

        public override async Task<OperationResult> AddAsync(InChemicalDto model)
        {
            try
            {
                //var check = await IsExistKey(model.Name);
                //if (!check.Success) return check;

                var chemical = _repoChemical.FindAll(x => x.Code.Equals(model.QrCode)).FirstOrDefault();
                var sup_name = _repoSupplier.FindAll(x => x.Id == chemical.SupplierID).FirstOrDefault();

                var item = _mapper.Map<InChemical>(model);
                item.ChemicalGuid = chemical.Guid;
                item.Status = 1;
                item.SupplierGuid = sup_name.Guid;
                item.Code = chemical.Code;
                item.Name = chemical.Name;
                item.Deliver = "0";
                item.OutOfStock = "Y";
                item.Unit = chemical.Unit.ToString();
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

        public override async Task<OperationResult> UpdateAsync(InChemicalDto model)
        {
            try
            {
                var item = _mapper.Map<InChemical>(model);
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

        public override async Task<List<InChemicalDto>> GetAllAsync()
        {
            var Start = DateTime.Now;
            var End = DateTime.Now;
            var inChemical = await _repo.FindAll(x => x.Status == 1).ToListAsync();
            var supplier = await _repoSupplier.FindAll(x => x.IsShow == true).ToListAsync();
            var chemical = await _repoChemical.FindAll(x => x.isShow).ToListAsync();
            var datasource = (from x in inChemical
                              join y in chemical on x.ChemicalGuid equals y.Guid
                              join z in supplier on x.SupplierGuid equals z.Guid
                              select new InChemicalDto
                              {
                                  Id = x.Id,
                                  Guid = x.Guid,
                                  ChemicalGuid = x.ChemicalGuid,
                                  SupplierGuid = x.SupplierGuid,
                                  Name = y.Name,
                                  Percentage = x.Percentage,
                                  Status = x.Status,
                                  CreateBy = x.CreateBy,
                                  UpdateBy = x.UpdateBy,
                                  UpdateDate = x.UpdateDate,
                                  DeleteBy = x.DeleteBy,
                                  DeleteDate = x.DeleteDate,
                                  Code = y.Code,
                                  CreateDate = x.CreateDate,
                                  Unit = x.Unit,
                                  OutOfStock = x.OutOfStock,
                                  Supplier = z.Name,
                                  Deliver = x.Deliver
                              }).Where(x => x.CreateDate.Value.Date == Start.Date && x.CreateDate.Value.Year == Start.Year).ToList();

            return datasource;

        }

        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
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
            throw new NotImplementedException();
            //var datasource = _repo.FindAll(x => x.IsShow).Include(x => x.Supplier).Include(x => x.Process)
            //.OrderByDescending(x=> x.Id)
            //.Select(x => new InkDto {
            //    ID = x.Id,
            //    Code = x.Code,
            //    Name = x.Name,
            //    NameEn = x.NameEn,
            //    CreatedDate = x.CreatedDate,
            //    MaterialNO = x.MaterialNo,
            //    VOC = x.Voc,
            //    Unit = x.Unit,
            //    Color = x.Process.Color,
            //    Supplier = x.Supplier.Name,
            //    Process = x.Process.Name,
            //    DaysToExpiration = x.DaysToExpiration,
            //    SupplierID = x.SupplierId,
            //    ProcessID = x.ProcessId,
            //    Percentage = x.Percentage,
            //    CreatedBy = x.CreatedBy,
            //    Guid = x.Guid,
            //    ModifiedDate = x.ModifiedDate
            //});
            //var count = await datasource.CountAsync();
            //if (data.Where != null)// for filtering
            //    datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            //if (data.Sorted != null)// for sorting
            //    datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            //if (data.Search != null)// for sorting
            //    datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);

            //count = await datasource.CountAsync();

            //if (data.Skip >= 0)// for paging
            //    datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            //if (data.Take > 0)// for paging
            //    datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            //return new
            //{
            //    Result = await datasource.ToListAsync(),
            //    Count = count
            //};
        }

        public async Task<object> GetAudit(object id)
        {
            throw new NotImplementedException();
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
        }

        public async Task<object> LoadDataBySite(string siteID)
        {
            var query = _repo.FindAll().Select(x => new { 
                x.Name,
                x.Code
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }

        public async Task<object> OutOfStock(string inChemicalGuid)
        {
            try
            {
                var checkKey = await _repo.FindAll(x => x.Guid == inChemicalGuid).AsNoTracking().FirstOrDefaultAsync();
                if (checkKey != null)
                {
                    checkKey.OutOfStock = "N";

                }
                var item = _mapper.Map<InChemical>(checkKey);
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
                    Success = true,
                    Data = checkKey
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
    }
}
