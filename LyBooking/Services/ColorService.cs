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
    public interface IColorService : IServiceBase<Color, ColorDto>
    {
        Task<object> LoadData(DataManager data, string colorGuid);
        Task<object> GetAudit(object id);
        Task<object> LoadDataBySite(string siteID);
        Task ImportExcel(List<ColorUploadDto> dto);

    }
    public class ColorService : ServiceBase<Color, ColorDto>, IColorService
    {
        private readonly IRepositoryBase<Color> _repo;
        private readonly IRepositoryBase<Supplier> _repoSupplier;
        private readonly IRepositoryBase<Ink> _repoInk;
        private readonly IRepositoryBase<Chemical2> _repoChemical;
        private readonly IRepositoryBase<InkColor> _repoInkColor;
        private readonly IRepositoryBase<ChemicalColor> _repoChemicalColor;
        private readonly IRepositoryBase<Shoe> _repoShoe;
        private readonly IRepositoryBase<Models.Schedule> _repoSchedule;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public ColorService(
            IRepositoryBase<Color> repo,
            IRepositoryBase<Supplier> repoSupplier,
            IRepositoryBase<Ink> repoInk,
            IRepositoryBase<Chemical2> repoChemical,
            IRepositoryBase<InkColor> repoInkColor,
            IRepositoryBase<ChemicalColor> repoChemicalColor,
            IRepositoryBase<Shoe> repoShoe,
            IRepositoryBase<Models.Schedule> repoSchedule,
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
            _repoChemical = repoChemical;
            _repoInkColor = repoInkColor;
            _repoChemicalColor = repoChemicalColor;
            _repoShoe = repoShoe;
            _repoSchedule = repoSchedule;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<OperationResult> IsExistKey(string key , string version)
        {
            var item = await _repo.FindAll(x => x.Status == 1 && x.Name == key ).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "COLOR_NAME_ALREADY_EXISTED", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }
        public override async Task<OperationResult> AddAsync(ColorDto model)
        {
            try
            {
                var check = await IsExistKey(model.Name, model.Name);
                if (!check.Success) return check;
                var item = _mapper.Map<Color>(model);
                item.Name = model.Name.Trim();
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

        public override async Task<OperationResult> UpdateAsync(ColorDto model)
        {
            try
            {
                
                var checkKey_pre = await _repo.FindAll(x => x.Name == model.Name && x.Status == 1 ).AsNoTracking().FirstOrDefaultAsync();
                var checkKey = await _repo.FindAll(x => x.Id == model.Id && x.Status == 1).AsNoTracking().FirstOrDefaultAsync();
                if (checkKey != null && checkKey_pre != null)
                {
                    if (checkKey.Name != model.Name )
                    {
                        var check = await IsExistKey(model.Name, model.Name);
                        if (!check.Success) return check;
                    }
                }
                var item = _mapper.Map<Color>(model);
                item.Name = model.Name.Trim();
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

        public override async Task<List<ColorDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<ColorDto>(_configMapper);

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

        public async Task<object> LoadData(DataManager data, string colorGuid)
        {
            var result = _repo.FindAll(x => x.Status == 1)
            .OrderByDescending(x=> x.Id)
            .Select(x => new ColorShoeModelDto {
                Id = x.Id,
                Guid = x.Guid,
                Name = x.Name,
                ModelNo = ""
            }).ToList();

            result.ForEach(item =>
            {
                string ShoeModel = "";
                var listShoeModel = new List<ShoeModelNoDto>();
                var listSchedule = _repoSchedule.FindAll().Where(x => x.ColorGuid == item.Guid)
                                                .Select(x => new
                                                {
                                                    x.ShoesGuid
                                                }).DistinctBy(x => x.ShoesGuid);

                foreach (var itemSchedule in listSchedule)
                {
                    string shoeModelNo = _repoShoe.FindAll().Where(x => x.Guid == itemSchedule.ShoesGuid).FirstOrDefault().ModelNo;
                    string shoeArticle = _repoShoe.FindAll().Where(x => x.Guid == itemSchedule.ShoesGuid).FirstOrDefault().Article1;
                    var newShoeModel = new ShoeModelNoDto();
                    newShoeModel.ModelNo = shoeModelNo;
                    newShoeModel.ArticelNo = shoeArticle;
                    listShoeModel.Add(newShoeModel);
                }
                var newlist = listShoeModel.DistinctBy(x => new { x.ModelNo, x.ArticelNo });

                foreach (var itemShoe in newlist)
                {
                    if (ShoeModel == string.Empty)
                    {
                        ShoeModel = ShoeModel + itemShoe.ModelNo + " - " + itemShoe.ArticelNo;
                    }
                    else
                    {
                        ShoeModel = ShoeModel + ", " + itemShoe.ModelNo + " - " + itemShoe.ArticelNo;
                    }
                }

                item.ModelNo = ShoeModel;
            });

            var queryable = result.AsQueryable();
            var datasource = queryable
            .Select(x => new {
                x.Id,
                x.Guid,
                x.Name,
                x.ModelNo
            });
            var count = datasource.Count();
            if (data.Where != null) // for filtering
                datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
            if (data.Sorted != null)//for sorting
                datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
            if (data.Search != null)
                datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
            count = datasource.Count();
            if (data.Skip >= 0)//for paging
                datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
            if (data.Take > 0)//for paging
                datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
            return new
            {
                Result = datasource,
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
                x.Name,
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;
            //throw new NotImplementedException();
        }

        public async Task ImportExcel(List<ColorUploadDto> res)
        {
            var result = res.DistinctBy(x => new { 
                x.Name , 
                x.ChemicalID,
                x.InkID
            }).GroupBy(x => x.Name)
               .Select(x => new
               {

                   Name = x.First().Name,
                   GlueID = x.First().GlueID,
                   Ink = x.Select(y => new {
                       y.InkID,
                       y.Percentage
                   }),
                   Chemical = x.Select(y => new {
                       y.ChemicalID,
                       y.Percentage
                   }),
                   Percentage = x.First().Percentage,
               });
            try
            {
                foreach (var item in result)
                {
                    //add color truoc
                    var color_add = new Color();
                    color_add.Name = item.Name;
                    var item_color = _mapper.Map<Color>(color_add);
                    item_color.Status = 1;
                    item_color.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                    _repo.Add(item_color);
                    await _unitOfWork.SaveChangeAsync();

                    //add tiep Ink_color - Chemical_color

                    foreach (var item_ink in item.Ink)
                    {
                        if (item_ink.InkID > 0 )
                        {
                            //tim inkGuid
                            var inkGuid = _repoInk.FindAll(x => x.Id == item_ink.InkID && x.IsShow).FirstOrDefault() != null 
                                ? _repoInk.FindAll(x => x.Id == item_ink.InkID && x.IsShow).FirstOrDefault().Guid 
                                : null;
                            if (inkGuid != null)
                            {
                                var supplierID = _repoInk.FindAll(x => x.Guid == inkGuid).FirstOrDefault().SupplierId;
                                var supplierGUID = _repoSupplier.FindByID(supplierID).Guid;

                                //add ink_color
                                var Inkcolor_add = new InkColor();
                                Inkcolor_add.ColorGuid = item_color.Guid;
                                Inkcolor_add.InkGuid = inkGuid;
                                Inkcolor_add.Percentage = item_ink.Percentage;
                                Inkcolor_add.Status = 1;
                                Inkcolor_add.SupplierGuid = supplierGUID;
                                Inkcolor_add.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                                var item_inkColor = _mapper.Map<InkColor>(Inkcolor_add);
                                _repoInkColor.Add(item_inkColor);

                                await _unitOfWork.SaveChangeAsync();
                            }
                        }
                    }

                    foreach (var item_chemical in item.Chemical)
                    {
                        if (item_chemical.ChemicalID > 0 && item_chemical.ChemicalID >= 1188)
                        {
                            //tim inkGuid
                            var chemicalGuid = _repoChemical.FindAll(x => x.ID == item_chemical.ChemicalID && x.isShow).FirstOrDefault() != null 
                                ? _repoChemical.FindAll(x => x.ID == item_chemical.ChemicalID && x.isShow).FirstOrDefault().Guid 
                                : null;
                            if (chemicalGuid != null)
                            {
                                var supplierID = _repoChemical.FindAll(x => x.Guid == chemicalGuid).FirstOrDefault().SupplierID;
                                var supplierGUID = _repoSupplier.FindByID(supplierID).Guid;

                                //add ink_color
                                var Chemicalcolor_add = new ChemicalColor();
                                Chemicalcolor_add.ColorGuid = item_color.Guid;
                                Chemicalcolor_add.ChemicalGuid = chemicalGuid;
                                Chemicalcolor_add.Status = 1;
                                Chemicalcolor_add.Percentage = item_chemical.Percentage;
                                Chemicalcolor_add.SupplierGuid = supplierGUID;
                                Chemicalcolor_add.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();
                                var item_chemicalColor = _mapper.Map<ChemicalColor>(Chemicalcolor_add);
                                _repoChemicalColor.Add(item_chemicalColor);

                                await _unitOfWork.SaveChangeAsync();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            
            //throw new NotImplementedException();
        }
    }
}
