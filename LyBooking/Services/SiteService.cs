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
using System.IO;
using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Hosting;

namespace IRS.Services
{
    public interface ISiteService : IServiceBase<Site, SiteDto>
    {
        Task<object> LoadData(DataManager data, string farmGuid);
        Task<object> GetAudit(object id);
        Task<OperationResult> AddFormAsync(SiteDto model);
        Task<OperationResult> UpdateFormAsync(SiteDto model);
        Task<object> DeleteUploadFile(decimal key);
        Task<object> GetSitesByAccount();

    }
    public class SiteService : ServiceBase<Site, SiteDto>, ISiteService
    {
        private readonly IRepositoryBase<Site> _repo;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IRepositoryBase<XAccountGroup> _repoXAccountGroup;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SiteService(
            IRepositoryBase<Site> repo,
            IRepositoryBase<XAccount> repoXAccount,
            IRepositoryBase<XAccountGroup> repoXAccountGroup,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IWebHostEnvironment currentEnvironment,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _repoXAccountGroup = repoXAccountGroup;
            _mapper = mapper;
            _configMapper = configMapper;
            _currentEnvironment = currentEnvironment;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> GetSitesByAccount()
        {
            var accessToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            int accountID = JWTExtensions.GetDecodeTokenByID(accessToken);


            var account = await _repoXAccount.FindAll(x => x.AccountId == accountID).Select(x => new { x.FarmGuid, x.AccountGroup }).FirstOrDefaultAsync();
            if (account == null)
                return new List<dynamic>();

            var group = await _repoXAccountGroup.FindAll(x => x.Guid == account.AccountGroup).Select(x => new { x.GroupNo }).FirstOrDefaultAsync();

            if (group.GroupNo == SystemAccountGroup.Admin)
            {
                var datasource = await _repo.FindAll(x => x.Status == 1).Select(x => new
                {
                    x.Id,
                    x.SiteName,
                    x.Guid
                }).ToListAsync();
                return datasource;
            }
            else
            {
                if (account.FarmGuid == null)
                    return new List<dynamic>();
                var datasource = await _repo.FindAll(x => x.Status == 1 && x.Guid == account.FarmGuid).Select(x => new
                {
                    x.Id,
                    x.SiteName,
                    x.Guid
                }).ToListAsync();
                return datasource;
            }
        }
        public override async Task<OperationResult> AddAsync(SiteDto model)
        {
            try
            {
                var item = _mapper.Map<Site>(model);
                item.Status = 1;
                //item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff").ToUpper();

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
        public override async Task<List<SiteDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<SiteDto>(_configMapper);

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
            var datasource = _repo.FindAll(x => x.Status == 1 && farmGuid == x.Guid)
            .OrderByDescending(x=> x.Id)
            .Select(x => new {
                x.Id,
                x.Guid
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
        public async Task<OperationResult> CheckExistSitename(string siteName)
        {
            var item = await _repo.FindAll(x => x.SiteName == siteName).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "The site name already existed!", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }
        public async Task<OperationResult> CheckExistSiteNo(string siteNo)
        {
            var item = await _repo.FindAll(x => x.SiteNo == siteNo).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "The site NO already existed!", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }
        public async Task<OperationResult> AddFormAsync(SiteDto model)
        {
            var check = await CheckExistSitename(model.SiteName);
            if (!check.Success) return check;
            var checkAccountNo = await CheckExistSiteNo(model.SiteNo);
            if (!checkAccountNo.Success) return checkAccountNo;
            FileExtension fileExtension = new FileExtension();
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\site\\image";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);
            if (model.File != null)
            {
                IFormFile files = model.File.FirstOrDefault();
                if (!files.IsNullOrEmpty())
                {
                    avatarUniqueFileName = await fileExtension.WriteAsync(files, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                    model.SitePhoto = $"/FileUploads/images/site/image/{avatarUniqueFileName}";
                }
            }
            try
            {
                var item = _mapper.Map<Site>(model);
                item.Guid = Guid.NewGuid().ToString("N") + DateTime.Now.ToString("ssff");
                item.Status = 1;
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
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<object> DeleteUploadFile(decimal key)
        {
            try
            {
                var item = await _repo.FindByIDAsync(key);
                if (item != null)
                {
                    string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, item.SitePhoto);
                    FileExtension fileExtension = new FileExtension();
                    var avatarUniqueFileName = item.SitePhoto;
                    if (!avatarUniqueFileName.IsNullOrEmpty())
                    {
                        var result = fileExtension.Remove($"{_currentEnvironment.WebRootPath}\\{item.SitePhoto}");
                        if (result)
                        {
                            item.SitePhoto = string.Empty;
                            _repo.Update(item);
                            await _unitOfWork.SaveChangeAsync();
                        }
                    }
                }


                return new { status = true };
            }
            catch (Exception)
            {

                return new { status = true };
            }
        }

        public async Task<OperationResult> UpdateFormAsync(SiteDto model)
        {
            FileExtension fileExtension = new FileExtension();
            var itemModel = await _repo.FindAll(x => x.Id == model.Id).AsNoTracking().FirstOrDefaultAsync();
            if (itemModel.SiteName != model.SiteName)
            {
                var check = await CheckExistSitename(model.SiteName);
                if (!check.Success) return check;
            }

            if (itemModel.SiteNo != model.SiteNo)
            {
                var checkAccountNo = await CheckExistSiteNo(model.SiteNo);
                if (!checkAccountNo.Success) return checkAccountNo;
            }
            var item = _mapper.Map<Site>(model);

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\site\\image";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            if (model.File != null)
            {
                IFormFile filesAvatar = model.File.FirstOrDefault();
                if (!filesAvatar.IsNullOrEmpty())
                {
                    if (!item.SitePhoto.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.SitePhoto.Replace("/", "\\").Replace("/", "\\")}");
                    avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                    item.SitePhoto = $"/FileUploads/images/site/image/{avatarUniqueFileName}";
                }
            }

            try
            {
                
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
            {   // Nếu tạo ra file rồi mã lưu db bị lỗi thì xóa file vừa tạo đi
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        
    }
}
