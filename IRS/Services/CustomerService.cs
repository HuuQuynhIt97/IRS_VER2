using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using IRS.Constants;
using IRS.Data;
using IRS.DTO;
using IRS.Helpers;
using IRS.Models;
using IRS.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Syncfusion.JavaScript;
using Syncfusion.JavaScript.DataSources;
namespace IRS.Services
{
    public interface ICustomerService : IServiceBase<Customer, CustomerDto>
    {
        Task<object> GetCustomers(string farmGuid, int top, int skip, string filter, string selected);
        Task<object> LoadData(DataManager data, string farmGuid, string lang);
        Task<object> GetCustomers(string farmGuid);
        Task<object> GetAudit(object id);
    }
    public class CustomerService : ServiceBase<Customer, CustomerDto>, ICustomerService
    {
        private readonly IRepositoryBase<Customer> _repo;
        private readonly IRepositoryBase<CodeType> _repoCodeType;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public CustomerService(
            IRepositoryBase<Customer> repo,
            IRepositoryBase<CodeType> repoCodeType,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoCodeType = repoCodeType;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
         public override async Task<object> GetDataDropdownlist(DataManager data)
        {
              var datasource = _repo.FindAll().Select(x => new 
            {
               Id= x.Id,
               Guid = x.Guid,
               FarmGuid = x.FarmGuid,
                Name = x.CustomerName,
                Status = x.Status
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
            return await datasource.ToListAsync();
        }
        public async Task<object> GetCustomers(string farmGuid, int top, int skip, string filter, string selected)
        {
                var selectedData = await _repo.FindAll(x => x.FarmGuid == farmGuid && x.Status == 1 && x.Guid == selected).Select(x => new 
            {
               Id= x.Id,
               Guid = x.Guid,
                Name = x.CustomerName
            }).ToListAsync();

            if (string.IsNullOrEmpty(filter) == true)
            {

                var query = _repo.FindAll(x => x.FarmGuid == farmGuid && x.Status == 1)
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(top)
                    .Select(x => new 
                    {
                        Id= x.Id,
                        Guid = x.Guid,
                        Name = x.CustomerName
                    });

                var data = await query.ToListAsync();
                 var list = new List<dynamic>();
                 if (skip == 0) {
                     var itemNo = new 
                    {
                         Id= 0,
                        Guid = "",
                        Name = "No Item"
                    };
                    list.Add(itemNo);
                }
                list.AddRange(data.Union(selectedData).ToList());
                return list;
            }
            else
            {
                var query = _repo.FindAll(x => x.CustomerName.Contains(filter) && x.FarmGuid == farmGuid && x.Status == 1).OrderBy(x => x.Id).Skip(skip).Take(top).Select(x => new 
                {
                       Id= x.Id,
                        Guid = x.Guid,
                        Name = x.CustomerName
                });
                
                var data = await query.ToListAsync();
                 var list = new List<dynamic>();
                 if (skip == 0) {
                     var itemNo = new 
                    {
                         Id= 0,
                        Guid = "",
                        Name = "No Item"
                    };
                    list.Add(itemNo);
                }
                list.AddRange(data.Union(selectedData).ToList());
                return list;
            }

        }
        public async Task<object> LoadData(DataManager data, string farmGuid, string lang)
        {
            //IQueryable<CustomerDto> datasource = _repo.FindAll(x=> x.Status == 1&&x.FarmGuid == farmGuid)
            //    .OrderByDescending(x=> x.Id)
            //    .ProjectTo<CustomerDto>(_configMapper);

            var datasource = (from a in _repo.FindAll(x => x.Status == 1)
                              join b in _repoCodeType.FindAll(x => x.CodeType1 == CodeTypeConst.CUSTOMER_SEX && x.Status == "Y") on a.CustomerSex equals b.CodeNo into ab
                              from t in ab.DefaultIfEmpty()

                              join c1 in _repoCodeType.FindAll(x => x.CodeType1 == CodeTypeConst.Auction && x.Status == "Y") on a.CustomerNickname equals c1.CodeNo into ac1
                              from c in ac1.DefaultIfEmpty()
                              select new CustomerDto
                              {
                                  Id = a.Id,
                                  CustomerNo = a.CustomerNo,
                                  CustomerName = a.CustomerName,
                                  CustomerSex = a.CustomerSex,
                                  CustomerBirthday = a.CustomerBirthday,
                                  CustomerNickname = a.CustomerNickname,
                                  CustomerTel = a.CustomerTel,
                                  CustomerMobile = a.CustomerMobile,
                                  CustomerAddress = a.CustomerAddress,
                                  CustomerIdcard = a.CustomerIdcard,
                                  CustomerEmail = a.CustomerEmail,
                                  ContactName = a.ContactName,
                                  ContactTel = a.ContactTel,
                                  ContactMobile = a.ContactMobile,
                                  ContactEmail = a.ContactEmail,
                                  Comment = a.Comment,
                                  CancelFlag = a.CancelFlag,
                                  CreateDate = a.CreateDate,
                                  CreateBy = a.CreateBy,
                                  UpdateDate = a.UpdateDate,
                                  UpdateBy = a.UpdateBy,
                                  FarmGuid = a.FarmGuid,
                                  Status = a.Status,
                                  Guid = a.Guid,
                                  CustomerSexName = t == null ? "" : lang == Languages.EN ? t.CodeNameEn ?? t.CodeName : lang == Languages.VI ? t.CodeNameVn ?? t.CodeName : lang == Languages.CN ? t.CodeNameCn ?? t.CodeName : t.CodeName,
                                  Auction = c == null ? "" : lang == Languages.EN ? c.CodeNameEn ?? c.CodeName : lang == Languages.VI ? c.CodeNameVn ?? c.CodeName : lang == Languages.CN ? c.CodeNameCn ?? c.CodeName : c.CodeName,
                              }).OrderByDescending(x => x.Id).AsQueryable();
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

        public override async Task<List<CustomerDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<CustomerDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }
        public override async Task<OperationResult> AddAsync(CustomerDto model)
        {
            var item = _mapper.Map<Customer>(model);
            item.Status = 1;
            _repo.Add(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
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
    
        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = await _repo.FindByIDAsync(id);
            item.Status = 0;
            item.CancelFlag = "Y";
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

        public async Task<object> GetCustomers(string farmGuid)
        {
            var query = await _repo.FindAll(x => x.FarmGuid == farmGuid && x.Status == 1)
                .Select(x => new
                {
                    x.Guid,
                    Name = x.CustomerName
                }).ToListAsync();
            return query;
        }
        public async Task<object> GetAudit(object id)
        {
            var data = await _repo.FindAll(x => x.Id.Equals(id)).AsNoTracking().Select(x=> new {x.UpdateBy, x.CreateBy, x.UpdateDate, x.CreateDate }).FirstOrDefaultAsync();
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
                var updateAudit = await _repoXAccount.FindAll(x => x.AccountId == data.UpdateBy).AsNoTracking().Select(x=> new { x.Uid }).FirstOrDefaultAsync();
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
