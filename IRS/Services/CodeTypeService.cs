using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IRS.Services
{
    public interface ICodeTypeService : IServiceBase<CodeType, CodeTypeDto>
    {
        Task<object> GetCodeTypes();
        Task<object> GetCodeTypes(string codeType, int top, int skip, string filter, string selected, string lang);
        Task<object> LoadData(DataManager data);
        Task<object> LoadData(DataManager data, string codeType);
        Task<object> GetAudit(object id);
        Task<object> GetDataDropdownlist(DataManager data, string lang, string codeType);
    }
    public class CodeTypeService : ServiceBase<CodeType, CodeTypeDto>, ICodeTypeService
    {
        private readonly IRepositoryBase<CodeType> _repo;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISequenceService _sequenceService;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _currentEnvironment;

        public CodeTypeService(
            IRepositoryBase<CodeType> repo,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            ISequenceService sequenceService,
            IMapper mapper,
            MapperConfiguration configMapper,
            IHttpContextAccessor httpContextAccessor,
            IWebHostEnvironment currentEnvironment
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _sequenceService = sequenceService;
            _mapper = mapper;
            _configMapper = configMapper;
            _httpContextAccessor = httpContextAccessor;
            _currentEnvironment = currentEnvironment;
        }
 public async Task<object> GetDataDropdownlist(DataManager data, string lang, string codeType)
        {
              var datasource = _repo.FindAll(x=> x.Status == "Y" && x.CodeType1 == codeType).Select(x => new 
            {
               Guid = x.CodeNo,
               Name = lang == Languages.EN ? string.IsNullOrEmpty(x.CodeNameEn) ? x.CodeName : x.CodeNameEn  
               : lang == Languages.VI ? string.IsNullOrEmpty(x.CodeNameVn) ? x.CodeName : x.CodeNameVn 
               : lang == Languages.CN ? string.IsNullOrEmpty(x.CodeNameCn) ? x.CodeName : x.CodeNameCn : x.CodeName
            }).AsQueryable();
           
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
               var itemNo = new List<dynamic>() {
                   new {
                    Guid = "",
                    Name = "No Item"
                }
                };
                return (await datasource.Select(x => new
                {
                    Guid = x.Guid,
                    Name = x.Name
                }).ToListAsync()).Union(itemNo).OrderBy(x => x.Guid);
        }

        public async Task<object> GetCodeTypes()
        {
            var query = await _repo.FindAll(x => x.Status == "Y").AsNoTracking().Select(x =>
               x.CodeType1
            ).Distinct().ToListAsync();
            return query.Select(x => new { ID = x, Name = x });
        }

        /// <summary>
        /// Add account sau do add CodeTypeGroupCodeType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(CodeTypeDto model)
        {
            try
            {
                var checkKey = await _repo.FindAll(x => x.CodeType1 == model.CodeType1 && x.CodeNo == model.CodeNo && x.Status == "Y").AsNoTracking().AnyAsync();
                if (checkKey)
                {
                    return new OperationResult
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "The CODE_NO already exists",
                        Success = false,
                        Data = model
                    };
                }
                var item = _mapper.Map<CodeType>(model);
                item.Status = "Y";
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

        /// <summary>
        /// Add account sau do add CodeTypeGroupCodeType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> UpdateAsync(CodeTypeDto model)
        {
            try
            {
                var checkKey = await _repo.FindAll(x => x.CodeType1 == model.CodeType1 && x.CodeNo == model.CodeNo && x.Status == "Y").AsNoTracking().FirstOrDefaultAsync();
                if (checkKey != null && model.CodeNo != checkKey.CodeNo)
                {
                    return new OperationResult
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "CODE_NO_ALREADY_EXISTED",
                        Success = false,
                        Data = model
                    };
                }
                var item = _mapper.Map<CodeType>(model);
                item.Status = item.Status;

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
        public override async Task<List<CodeTypeDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == "Y")
                .ProjectTo<CodeTypeDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }
        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            item.Status = "";
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

        public async Task<object> GetCodeTypes(string codeType, int top, int skip, string filter, string selected, string lang)
        {
           
            if (string.IsNullOrEmpty(selected) == false) {
            return await _repo.FindAll(x => x.CodeType1 == codeType && x.Status == "Y" && x.CodeNo == selected).Select(x => new
                        {
                            Guid = x.CodeNo,
                            Name = lang == Languages.EN ? x.CodeNameEn ?? x.CodeName : lang == Languages.VI ? x.CodeNameVn ?? x.CodeName : lang == Languages.CN ? x.CodeNameCn ?? x.CodeName : x.CodeName

                        }).ToListAsync();
            }
            if (string.IsNullOrEmpty(filter) == true)
            {
                var query = _repo.FindAll(x => x.CodeType1 == codeType && x.Status == "Y")
                    .OrderBy(x => x.Sort)
                    .Skip(skip)
                    .Take(top)
                    .Select(x => new
                    {
                        Guid = x.CodeNo,
                        Name = lang == Languages.EN ? x.CodeNameEn ?? x.CodeName : lang == Languages.VI ? x.CodeNameVn ?? x.CodeName : lang == Languages.CN ? x.CodeNameCn ?? x.CodeName : x.CodeName

                    });

                var data = await query.ToListAsync();

                var list = new List<dynamic>();
                if (skip == 0)
                {
                    var itemNo = new
                    {
                        Guid = "",
                        Name = "No Item"
                    };
                    list.Add(itemNo);
                }
                list.AddRange(data.ToList());
                return list;
            }
            else
            {
                var query = _repo.FindAll(x => x.CodeName.Contains(filter) && x.CodeType1 == codeType && x.Status == "Y").OrderBy(x => x.Sort).Skip(skip).Take(top).Select(x => new
                {
                    Guid = x.CodeNo,
                    Name = lang == Languages.EN ? x.CodeNameEn ?? x.CodeName : lang == Languages.VI ? x.CodeNameVn ?? x.CodeName : lang == Languages.CN ? x.CodeNameCn ?? x.CodeName : x.CodeName

                });

                var data = await query.ToListAsync();
                var list = new List<dynamic>();
                if (skip == 0)
                {
                    var itemNo = new
                    {
                        Guid = "",
                        Name = "No Item"
                    };
                    list.Add(itemNo);
                }
                list.AddRange(data.ToList());
                return list;
            }
        }
        public async Task<object> LoadData(DataManager data, string codeType)
        {
            if (string.IsNullOrEmpty(codeType))
            {
                return await LoadData(data);
            }
            if (codeType == "All")
            {
                return await LoadData(data);
            }
            IQueryable<CodeTypeDto> datasource = _repo.FindAll(x => x.Status == "Y" && codeType == x.CodeType1)
                .OrderByDescending(x => x.Id)
                .ProjectTo<CodeTypeDto>(_configMapper);
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

        public async Task<object> LoadData(DataManager data)
        {
            IQueryable<CodeTypeDto> datasource = _repo.FindAll(x => x.Status == "Y")
                .OrderByDescending(x => x.Id)
                .ProjectTo<CodeTypeDto>(_configMapper);
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
    }
}
