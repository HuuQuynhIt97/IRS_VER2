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

namespace IRS.Services
{
    public interface IXAccountGroupService : IServiceBase<XAccountGroup, XAccountGroupDto>
    {
        Task<object> GetAccountGroup();
        Task<object> GetAudit(object id);
        Task<object> GetPermissionsDropdown(string accountGuid, string lang);
        Task<object> GetPermissions(string accountGuid, string lang);
        Task<OperationResult> StorePermission(StorePermissionDto request);
    }
    public class XAccountGroupService : ServiceBase<XAccountGroup, XAccountGroupDto>, IXAccountGroupService
    {
        private readonly IRepositoryBase<XAccountGroup> _repo;
        private readonly IRepositoryBase<XAccountGroupPermission> _repoXAccountGroupPermission;
        private readonly IRepositoryBase<CodePermission> _repoCodePermission;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public XAccountGroupService(
            IRepositoryBase<XAccountGroup> repo,
            IRepositoryBase<XAccountGroupPermission> repoXAccountGroupPermission,
            IRepositoryBase<CodePermission> repoCodePermission,
            IRepositoryBase<XAccount> repoXAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoXAccountGroupPermission = repoXAccountGroupPermission;
            _repoCodePermission = repoCodePermission;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
        }
        public async Task<object> GetPermissions(string accountGuid, string lang)
        {
            var query = from a in _repoXAccountGroupPermission.FindAll(x => x.UpperGuid == accountGuid).AsNoTracking()
                        select new
                        {
                            a.CodeNo
                        };

            return await query.Select(x => x.CodeNo).Distinct().ToListAsync();
        }
        public async Task<object> GetPermissionsDropdown(string accountGuid, string lang)
        {
            var query = from a in _repoCodePermission.FindAll(x => x.Status == "1").OrderBy(x => x.Sort).AsNoTracking()
                        join b in _repoXAccountGroupPermission.FindAll(x => x.UpperGuid == accountGuid).AsNoTracking() on a.CodeNo equals b.CodeNo into gj
                        from x in gj.DefaultIfEmpty()
                        where x.UpperGuid == null || x.UpperGuid != accountGuid

                        select new
                        {
                            a.Id,
                            a.CodeNo,
                            Code = x.CodeNo,
                            x.UpperGuid,
                            Name = lang == Languages.EN ? (a.CodeNameEn == "" ? a.CodeName : a.CodeNameEn) : lang == Languages.VI ? (a.CodeNameVn == "" ? a.CodeName : a.CodeNameVn) : lang == Languages.TW ? a.CodeName : lang == Languages.CN ? (a.CodeNameCn == "" ? a.CodeName : a.CodeNameCn) : a.CodeName,

                        };
            if (!string.IsNullOrEmpty(accountGuid))
            {
                var query2 = from a in _repoCodePermission.FindAll(x => x.Status == "1").AsNoTracking()
                             join b in _repoXAccountGroupPermission.FindAll().AsNoTracking() on a.CodeNo equals b.CodeNo
                             where b.UpperGuid == accountGuid
                             select new
                             {
                                 a.Id,
                                 a.CodeNo,
                                 Code = b.CodeNo,
                                 b.UpperGuid,
                                 Name = lang == Languages.EN ? (a.CodeNameEn == "" ? a.CodeName : a.CodeNameEn) : lang == Languages.VI ? (a.CodeNameVn == "" ? a.CodeName : a.CodeNameVn) : lang == Languages.TW ? a.CodeName : lang == Languages.CN ? (a.CodeNameCn == "" ? a.CodeName : a.CodeNameCn) : a.CodeName,
                             };
                var data = await query.ToListAsync();
                var data2 = await query2.ToListAsync();
                return data.Union(data2).ToList().DistinctBy(x => x.CodeNo);

            }
            return (await query.ToListAsync()).DistinctBy(x => x.CodeNo);
        }

        public async Task<OperationResult> StorePermission(StorePermissionDto request)
        {
            try
            {
                var ap = await _repoXAccountGroupPermission.FindAll(x => x.UpperGuid == request.Guid).ToListAsync();
                var permissions = await _repoCodePermission.FindAll(x => request.Permissions.Contains(x.CodeNo)).Select(x => x.CodeNo).ToListAsync();

                if (ap.Any())
                {
                    _repoXAccountGroupPermission.RemoveMultiple(ap);
                    var xapList = new List<XAccountGroupPermission>();
                    foreach (var item in permissions)
                    {
                        xapList.Add(new XAccountGroupPermission
                        {
                            CodeNo = item,
                            UpperGuid = request.Guid,
                        });
                    }
                    _repoXAccountGroupPermission.AddRange(xapList);
                }
                else
                {
                    var xapList = new List<XAccountGroupPermission>();
                    foreach (var item in permissions)
                    {
                        xapList.Add(new XAccountGroupPermission
                        {
                            CodeNo = item,
                            UpperGuid = request.Guid
                        });
                    }
                    _repoXAccountGroupPermission.AddRange(xapList);
                }

                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = request.Permissions
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        private async Task<OperationResult> SavePermission(StorePermissionDto request)
        {
            try
            {
                var ap = await _repoXAccountGroupPermission.FindAll(x => x.UpperGuid == request.Guid).ToListAsync();
                var permissions = await _repoCodePermission.FindAll(x => request.Permissions.Contains(x.CodeNo)).Select(x => x.CodeNo).ToListAsync();

                if (ap.Any())
                {
                    _repoXAccountGroupPermission.RemoveMultiple(ap);
                    var xapList = new List<XAccountGroupPermission>();
                    foreach (var item in permissions)
                    {
                        xapList.Add(new XAccountGroupPermission
                        {
                            CodeNo = item,
                            UpperGuid = request.Guid,
                        });
                    }
                    _repoXAccountGroupPermission.AddRange(xapList);
                }
                else
                {
                    var xapList = new List<XAccountGroupPermission>();
                    foreach (var item in permissions)
                    {
                        xapList.Add(new XAccountGroupPermission
                        {
                            CodeNo = item,
                            UpperGuid = request.Guid
                        });
                    }
                    _repoXAccountGroupPermission.AddRange(xapList);
                }

                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.AddSuccess,
                    Success = true,
                    Data = request.Permissions
                };
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        public override async Task<OperationResult> AddAsync(XAccountGroupDto model)
        {
            var item = _mapper.Map<XAccountGroup>(model);
            item.Status = 1;
            _repo.Add(item);
            try
            {
                await _unitOfWork.SaveChangeAsync();
                model.Permissions.Guid = item.Guid;
                var result = await SavePermission(model.Permissions);
                if (result.Success == false) return result;
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
        public override async Task<OperationResult> UpdateAsync(XAccountGroupDto model)
        {
            var item = _mapper.Map<XAccountGroup>(model);
            _repo.Update(item);
            try
            {
               var result= await SavePermission(model.Permissions);
                if (result.Success == false) return result;
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UpdateSuccess,
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
        public override async Task<List<XAccountGroupDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x=> x.Status == 1).ProjectTo<XAccountGroupDto>(_configMapper);

            var data = await query.OrderByDescending(x=>x.Id).ToListAsync();
            return data;

        }
        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id.ToDecimal());
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
        public  async Task<object> GetAccountGroup()
        {
            var query = _repo.FindAll(x => x.Status == 1).Select(x => new {
                x.Id,
                x.GroupName,
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;

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
