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
    public interface IOCService : IServiceBase<Oc, OCDto>
    {
        Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView();
        Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView(int famrID);
        Task<object> CreateMainOC(OCDto model);
        Task<object> CreateSubOC(OCDto model);

        Task<object> CreateMainOCForm(OCDto model);
        Task<object> CreateSubOCForm(OCDto model);
        Task<object> DeleteUploadFile(int key);
        Task<object> LoadData(DataManager dm, int farmID);
        Task<object> GetFarms();

    }
    public class OCService : ServiceBase<Oc, OCDto>, IOCService
    {
        private readonly IRepositoryBase<Oc> _repo;
        private readonly IRepositoryBase<Account> _repoAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;

        public OCService(
            IRepositoryBase<Oc> repo,
            IRepositoryBase<Account> repoAccount,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            MapperConfiguration configMapper,
            IWebHostEnvironment currentEnvironment

            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoAccount = repoAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _configMapper = configMapper;
            _currentEnvironment = currentEnvironment;
        }

        public async Task<bool> Add(OCDto model)
        {
            var building = _mapper.Map<Oc>(model);
            _repo.Add(building);
            return await _unitOfWork.SaveChangeAsync() > 0;
        }

        public Task<object> CreateMainOC(OCDto model)
        {
            throw new NotImplementedException();
        }

        public Task<object> CreateMainOCForm(OCDto model)
        {
            throw new NotImplementedException();
        }

        public Task<object> CreateSubOC(OCDto model)
        {
            throw new NotImplementedException();
        }

        public Task<object> CreateSubOCForm(OCDto model)
        {
            throw new NotImplementedException();
        }

        public Task<object> DeleteUploadFile(int key)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView(int famrID)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetFarms()
        {
            throw new NotImplementedException();
        }

        public Task<object> LoadData(DataManager dm, int farmID)
        {
            throw new NotImplementedException();
        }


        //public override async Task<OperationResult> DeleteAsync(object id)
        //{
        //    var OC = _repo.FindByID(id);
        //    var data = _repo.FindAll(x => x.Status == true).ToList().AsHierarchy(x => x.ID, y => y.ParentID, id).ToList();
        //    var da = data.Flatten(x => x.ChildNodes).ToList();
        //    var list = new List<OC>();
        //    foreach (var item in da)
        //    {
        //        item.Entity.Status = false;
        //        list.Add(item.Entity);
        //    }
        //    _repo.UpdateRange(list);
        //    try
        //    {
        //        await _unitOfWork.SaveChangeAsync();
        //        operationResult = new OperationResult
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Message = MessageReponse.DeleteSuccess,
        //            Success = true,
        //            Data = OC
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        operationResult = ex.GetMessageError();
        //    }
        //    return operationResult;
        //}
        //public override async Task<OperationResult> UpdateAsync(OCDto model)
        //{
        //    FileExtension fileExtension = new FileExtension();
        //    var item = await _repo.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.ID == model.ID);
        //    var update = _mapper.Map<OC>(model);
        //    update.PhotoPath = model.PhotoPath;

        //    // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
        //    var avatarUniqueFileName = string.Empty;
        //    var avatarFolderPath = "FileUploads\\images\\farm\\avatar";
        //    string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

        //    if (model.File != null)
        //    {
        //        IFormFile filesAvatar = model.File.FirstOrDefault();
        //        if (filesAvatar != null)
        //        {
        //            if (!item.PhotoPath.IsNullOrEmpty())
        //                fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.PhotoPath.Replace("/", "\\").Replace("/", "\\")}");
        //            avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //            update.PhotoPath = $"/FileUploads/images/farm/avatar/{avatarUniqueFileName}";
        //        }

        //    }

        //    _repo.Update(update);
        //    try
        //    {
        //        await _unitOfWork.SaveChangeAsync();
        //        operationResult = new OperationResult
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Message = MessageReponse.UpdateSuccess,
        //            Success = true,
        //            Data = item
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!avatarUniqueFileName.IsNullOrEmpty())
        //            fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //        operationResult = ex.GetMessageError();
        //    }

        //    return operationResult;
        //}


        //public OCDto GetById(object id)
        //{
        //    return _mapper.Map<OC, OCDto>(_repo.FindByID(id));
        //}

        //public async Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView()
        //{
        //    var data = await _repo.FindAll(x => x.Status == true)
        //        .ProjectTo<OCDto>(_configMapper)
        //        .OrderByDescending(x => x.ID).ToListAsync();
        //    var lists = data.OrderBy(x => x.Name).AsHierarchy(x => x.ID, y => y.ParentID);
        //    return lists;
        //}


        //public async Task<object> CreateMainOC(OCDto model)
        //{
        //    if (model.ID == 0)
        //    {
        //        var item = _mapper.Map<OC>(model);
        //        item.Level = 1;
        //        item.ParentID = null;
        //        _repo.Add(item);
        //        try
        //        {
        //            return new { status = await _unitOfWork.SaveChangeAsync(), building = item };
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //            return new { status = false };
        //        }

        //    }
        //    else
        //    {
        //        var item = _repo.FindByID(model.ID);
        //        var update = _mapper.Map<OC>(model);
        //        _repo.Update(update);
        //        try
        //        {
        //            return new { status = await _unitOfWork.SaveChangeAsync(), building = item };
        //        }
        //        catch (Exception)
        //        {
        //            return new { status = false };
        //        }

        //    }


        //}

        //public async Task<object> CreateSubOC(OCDto buildingDto)
        //{
        //    var item = _mapper.Map<OC>(buildingDto);
        //    var itemParent = _repo.FindByID(buildingDto.ParentID);
        //    item.Level = itemParent.Level + 1;
        //    item.ParentID = buildingDto.ParentID;
        //    _repo.Add(item);

        //    try
        //    {
        //        return new { status = await _unitOfWork.SaveChangeAsync() > 0, building = item };
        //    }
        //    catch (Exception)
        //    {
        //        return new { status = false };
        //    }
        //}
        //public override async Task<List<OCDto>> GetAllAsync()
        //{
        //    var query = _repo.FindAll(x => x.Status == true)
        //        .ProjectTo<OCDto>(_configMapper);

        //    var data = await query.ToListAsync();
        //    return data;

        //}

        //public async Task<object> CreateMainOCForm(OCDto model)
        //{

        //    if (model.ID == 0)
        //    {
        //        FileExtension fileExtension = new FileExtension();
        //        var avatarUniqueFileName = string.Empty;
        //        var avatarFolderPath = "FileUploads\\images\\farm\\avatar";
        //        string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);
        //        if (model.File != null)
        //        {
        //            IFormFile files = model.File.FirstOrDefault();
        //            if (!files.IsNullOrEmpty())
        //            {
        //                avatarUniqueFileName = await fileExtension.WriteAsync(files, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //                model.PhotoPath = $"/FileUploads/images/farm/avatar/{avatarUniqueFileName}";
        //            }
        //        }
        //        var item = _mapper.Map<OC>(model);
        //        item.Level = 1;
        //        item.ParentID = null;
        //        _repo.Add(item);
        //        try
        //        {
        //            return new { status = await _unitOfWork.SaveChangeAsync(), building = item };
        //        }
        //        catch
        //        {
        //            if (!avatarUniqueFileName.IsNullOrEmpty())
        //                fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //            return new { status = false };
        //        }

        //    }
        //    else
        //    {
        //        FileExtension fileExtension = new FileExtension();
        //        var item = await _repo.FindByIDAsync(model.ID);
        //        var update = _mapper.Map<OC>(model);
        //        update.PhotoPath = model.PhotoPath;

        //        // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
        //        var avatarUniqueFileName = string.Empty;
        //        var avatarFolderPath = "FileUploads\\images\\farm\\avatar";
        //        string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

        //        IFormFile filesAvatar = model.File.FirstOrDefault();

        //        if (filesAvatar != null)
        //        {
        //            if (!item.PhotoPath.IsNullOrEmpty())
        //                fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.PhotoPath.Replace("/", "\\").Replace("/", "\\")}");
        //            avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //            update.PhotoPath = $"/FileUploads/images/farm/avatar/{avatarUniqueFileName}";
        //        }

        //        _repo.Update(update);
        //        try
        //        {
        //            return new { status = await _unitOfWork.SaveChangeAsync() > 0, building = item };
        //        }
        //        catch (Exception)
        //        {
        //            if (!avatarUniqueFileName.IsNullOrEmpty())
        //                fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //            return new { status = false };
        //        }

        //    }

        //}

        //public async Task<object> CreateSubOCForm(OCDto model)
        //{
        //    FileExtension fileExtension = new FileExtension();
        //    var avatarUniqueFileName = string.Empty;
        //    var avatarFolderPath = "FileUploads\\images\\farm\\avatar";
        //    string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);
        //    if (model.File != null)
        //    {
        //        IFormFile files = model.File.FirstOrDefault();
        //        if (!files.IsNullOrEmpty())
        //        {
        //            avatarUniqueFileName = await fileExtension.WriteAsync(files, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //            model.PhotoPath = $"/FileUploads/images/farm/avatar/{avatarUniqueFileName}";
        //        }
        //    }
        //    var item = _mapper.Map<OC>(model);
        //    var itemParent = _repo.FindByID(model.ParentID);
        //    item.Level = itemParent.Level + 1;
        //    item.ParentID = model.ParentID;
        //    _repo.Add(item);

        //    try
        //    {
        //        return new { status = await _unitOfWork.SaveChangeAsync() > 0, building = item };
        //    }
        //    catch (Exception)
        //    {
        //        if (!avatarUniqueFileName.IsNullOrEmpty())
        //            fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");
        //        return new { status = false };
        //    }
        //}

        //public async Task<object> DeleteUploadFile(int key)
        //{
        //    try
        //    {
        //        var item = await _repo.FindByIDAsync(key);
        //        if (item != null)
        //        {
        //            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, item.PhotoPath);
        //            FileExtension fileExtension = new FileExtension();
        //            var avatarUniqueFileName = item.PhotoPath;
        //            if (!avatarUniqueFileName.IsNullOrEmpty())
        //            {
        //                var result = fileExtension.Remove($"{_currentEnvironment.WebRootPath}\\{item.PhotoPath}");
        //                if (result)
        //                {
        //                    item.PhotoPath = string.Empty;
        //                    _repo.Update(item);
        //                    await _unitOfWork.SaveChangeAsync();
        //                }
        //            }
        //        }


        //        return new { status = true };
        //    }
        //    catch (Exception)
        //    {

        //        return new { status = true };
        //    }
        //}

        //public async Task<IEnumerable<HierarchyNode<OCDto>>> GetAllAsTreeView(int farmID)
        //{
        //    string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    var accountId = JWTExtensions.GetDecodeTokenByID(token).ToInt();
        //    var farms = await _repoFarmAccount.FindAll(x => x.AccountID == accountId).Select(x => x.FarmID).ToListAsync();
        //    var item = await _repoAccount.FindAll(x => x.Id == accountId).FirstOrDefaultAsync();
        //    if (item.AccountGroupID != null)
        //    {
        //        if (item.AccountGroup.GroupNO == "ADMIN")
        //        {
        //            var data = await _repo.FindAll(x => x.Status == true)
        //    .ProjectTo<OCDto>(_configMapper)
        //    .OrderByDescending(x => x.ID).ToListAsync();
        //            var lists = data.OrderBy(x => x.Name).AsHierarchy(x => x.ID, y => y.ParentID);
        //            return lists;
        //        }
        //        else
        //        {
        //            var data = await _repo.FindAll(x => x.Status == true)
        //     .ProjectTo<OCDto>(_configMapper)
        //     .OrderByDescending(x => x.ID).ToListAsync();
        //            var lists = data.OrderBy(x => x.Name).AsHierarchy(x => x.ID, y => y.ParentID).Where(x => farms.Contains(x.Entity.ID));
        //            return lists;
        //        }

        //    }
        //    else
        //    {

        //        return new List<HierarchyNode<OCDto>>();
        //    }

        //}
        //public async Task<object> LoadData(DataManager data, int farmID)
        //{
        //    string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    var accountId = JWTExtensions.GetDecodeTokenByID(token).ToInt();
        //    var item = await _repoAccount.FindAll(x => x.ID == accountId).Include(x => x.AccountGroup).FirstOrDefaultAsync();
        //    if (item.AccountGroupID != null && item.AccountGroup.GroupNO == "ADMIN")
        //    {
        //        IQueryable<OCDto> datasource = _repo.FindAll(x => x.Status == true).ProjectTo<OCDto>(_configMapper);
        //        var count = await datasource.CountAsync();
        //        if (data.Where != null) // for filtering
        //            datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
        //        if (data.Sorted != null)//for sorting
        //            datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
        //        if (data.Search != null)
        //            datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
        //        count = await datasource.CountAsync();
        //        if (data.Skip >= 0)//for paging
        //            datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
        //        if (data.Take > 0)//for paging
        //            datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
        //        return new
        //        {
        //            Result = await datasource.ToListAsync(),
        //            Count = count
        //        };
        //    } else
        //    {
        //        IQueryable<OCDto> datasource = _repo.FindAll(x => x.Status == true).ProjectTo<OCDto>(_configMapper);
        //        var count = await datasource.CountAsync();
        //        if (data.Where != null) // for filtering
        //            datasource = QueryableDataOperations.PerformWhereFilter(datasource, data.Where, data.Where[0].Condition);
        //        if (data.Sorted != null)//for sorting
        //            datasource = QueryableDataOperations.PerformSorting(datasource, data.Sorted);
        //        if (data.Search != null)
        //            datasource = QueryableDataOperations.PerformSearching(datasource, data.Search);
        //        count = await datasource.CountAsync();
        //        if (data.Skip >= 0)//for paging
        //            datasource = QueryableDataOperations.PerformSkip(datasource, data.Skip);
        //        if (data.Take > 0)//for paging
        //            datasource = QueryableDataOperations.PerformTake(datasource, data.Take);
        //        return new
        //        {
        //            Result = await datasource.ToListAsync(),
        //            Count = count
        //        };
        //    }


        //}

        //public async Task<object> GetFarms()
        //{
        //    string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    var accountId = JWTExtensions.GetDecodeTokenByID(token).ToInt();
        //    var farms = await _repoFarmAccount.FindAll(x => x.AccountID == accountId).Select(x => x.FarmID).ToListAsync();
        //    var query = _repo.FindAll(x => x.Level == 1 && x.Status == true && farms.Contains(x.ID));

        //    var data = await query.Select(x => new
        //    {
        //        x.Name,
        //        x.ID
        //    }).ToListAsync();
        //    return data;
        //}
    }
}
