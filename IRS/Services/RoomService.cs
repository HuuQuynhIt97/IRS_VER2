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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace IRS.Services
{
    public interface IRoomService : IServiceBase<Room, RoomDto>
    {
        Task<object> GetRooms(string farmGuid, int top, int skip, string filter, string selected);
        Task<object> LoadData(DataManager data, string siteGuid, string hallGuid);
        Task<object> GetAudit(object id);
        Task<object> GetRoomBySiteAndHall(string siteID, string hallID);
        Task<object> Search(RoomFilterDto filter);
        Task<OperationResult> AddFormAsync(RoomDto model);
        Task<OperationResult> UpdateFormAsync(RoomDto model);
        Task<object> DeleteUploadFile(decimal key);
        Task<object> GetRoomByid(int ID);
    }
    public class RoomService : ServiceBase<Room, RoomDto>, IRoomService
    {
        private readonly IRepositoryBase<Room> _repo;
        private readonly IRepositoryBase<Room> _repoRoom;
        private readonly IRepositoryBase<Hall> _repoHall;
        private readonly IRepositoryBase<XAccount> _repoXAccount;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;
        public RoomService(
            IRepositoryBase<Room> repo,
            IRepositoryBase<Room> repoRoom,
            IRepositoryBase<Hall> repoHall,
            IRepositoryBase<XAccount> repoXAccount,
            IWebHostEnvironment currentEnvironment,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _repoRoom = repoRoom;
            _repoHall = repoHall;
            _repoXAccount = repoXAccount;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
            _currentEnvironment = currentEnvironment;
        }

        public override async Task<object> GetDataDropdownlist(DataManager data)
        {
            var datasource = _repo.FindAll().AsQueryable();

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

            var results = (await datasource.Select(x => new
            {
                Id = x.Id,
                Guid = x.Guid,
                FarmGuid = x.HallGuid,
                Name = $"{x.RoomNo} - {x.RoomName}",
                Status = x.Status
            }).Select(x => new
            {
                Guid = x.Guid,
                Name = $"{x.Name}"

            }).ToListAsync());
            if (data.Skip == 0)
            {
                var itemNo = new
                {
                    Guid = "",
                    Name = "No Item",
                };
                results.Insert(0, itemNo);
                return results;
            }
            return results;
        }

        public async Task<object> GetRooms(string farmGuid, int top, int skip, string filter, string selected)
        {
            var selectedData = await _repo.FindAll(x => x.HallGuid == farmGuid && x.Status == 1 && x.Guid == selected).Select(x => new
            {
                x.Id,
                x.Guid,
                x.RoomNo,
                Name = $"{x.RoomNo} - {x.RoomName}"
            }).ToListAsync();

            if (string.IsNullOrEmpty(filter) == true)
            {

                var query = _repo.FindAll(x => x.HallGuid == farmGuid && x.Status == 1)
                    .OrderBy(x => x.Id)
                    .Skip(skip)
                    .Take(top)
                    .Select(x => new
                    {
                        x.Id,
                        x.Guid,
                        x.RoomNo,
                        Name = $"{x.RoomNo} - {x.RoomName}"
                    });

                var data = await query.ToListAsync();


                var list = new List<dynamic>();
                if (skip == 0)
                {
                    var itemNo = new
                    {
                        Id = 0,
                        Guid = "",
                        RoomNo = "",
                        Name = "No Item"
                    };
                    list.Add(itemNo);
                }
                list.AddRange(data.Union(selectedData).ToList());
                return list;
            }
            else
            {
                var query = _repo.FindAll(x => (x.RoomName.Contains(filter) || x.RoomNo.Contains(filter)) && x.HallGuid == farmGuid && x.Status == 1).OrderBy(x => x.Id).Skip(skip).Take(top).Select(x => new
                {
                    x.Id,
                    x.Guid,
                    x.RoomNo,
                    Name = $"{x.RoomNo} - {x.RoomName}"
                });

                var data = await query.ToListAsync();
                var list = new List<dynamic>();
                if (skip == 0)
                {
                    var itemNo = new
                    {
                        Id = 0,
                        Guid = "",
                        RoomNo = "",
                        Name = "No Item"
                    };
                    list.Add(itemNo);
                }
                list.AddRange(data.Union(selectedData).ToList());
                return list;
            }


        }

        public override async Task<OperationResult> AddAsync(RoomDto model)
        {
            try
            {
                var item = _mapper.Map<Room>(model);
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
        public override async Task<List<RoomDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == 1).ProjectTo<RoomDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }


        public async Task<object> Search(RoomFilterDto filter)
        {
            var query = new List<Room>();
            if (!string.IsNullOrEmpty(filter.SiteGuidFilter) && string.IsNullOrEmpty(filter.HallGuidFilter))
            {
                query = await _repo.FindAll(x => x.Status == 1 && x.SiteGuid == filter.SiteGuidFilter).ToListAsync();
            }

            if (string.IsNullOrEmpty(filter.SiteGuidFilter) && string.IsNullOrEmpty(filter.HallGuidFilter))
            {
                query = await _repo.FindAll(x => x.Status == 1).ToListAsync();
            }

            if (!string.IsNullOrEmpty(filter.SiteGuidFilter) && !string.IsNullOrEmpty(filter.HallGuidFilter))
            {
                query = await _repo.FindAll(x => x.Status == 1 && x.SiteGuid == filter.SiteGuidFilter && x.HallGuid == filter.HallGuidFilter).ToListAsync();
            }
            return query;
        }
        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            // item.CancelFlag = "Y";
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
        public async Task<object> LoadData(DataManager data, string siteGuid, string hallGuid)
        {
            var datasource = _repo.FindAll(x => x.Status == 1 && x.HallGuid == hallGuid && x.SiteGuid == siteGuid)
                .ProjectTo<RoomDto>(_configMapper)
                .OrderByDescending(x => x.Id)
                .Select(x => new
                {
                    x.Id,
                    x.RoomName,
                    x.PriceMember,
                    x.Price,
                    x.PriceSpecial,
                    x.RoomPhoto,
                    x.RoomNo,
                    x.Guid,
                    x.RoomPhoto1,
                    x.RoomPhoto2,
                    x.RoomPhoto3,
                    x.RoomPhoto4,
                    x.RoomPhoto5
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
        
        public async Task<OperationResult> CheckExistRoomName(string roomName)
        {
            var item = await _repo.FindAll(x => x.RoomName == roomName).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "The room name already existed!", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }
        public async Task<OperationResult> CheckExistRoomNo(string roomNo)
        {
            var item = await _repo.FindAll(x => x.RoomNo == roomNo).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "The room NO already existed!", Success = false };
            }
            operationResult = new OperationResult
            {
                StatusCode = HttpStatusCode.OK,
                Success = true,
                Data = item
            };
            return operationResult;
        }
        public async Task<OperationResult> AddFormAsync(RoomDto model)
        {
            var check = await CheckExistRoomName(model.RoomName);
            if (!check.Success) return check;
            var checkAccountNo = await CheckExistRoomNo(model.RoomNo);
            if (!checkAccountNo.Success) return checkAccountNo;
            FileExtension fileExtension = new FileExtension();
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\room\\image";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);
            if (model.File != null)
            {
                IFormFile files = model.File.FirstOrDefault();
                if (!files.IsNullOrEmpty())
                {
                    avatarUniqueFileName = await fileExtension.WriteAsync(files, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                    model.RoomPhoto = $"/FileUploads/images/room/image/{avatarUniqueFileName}";
                }
            }
            List<string> galleries = new List<string>();
            if (model.RoomGallery != null)
            {
                    int id = 1;
                    model.RoomGallery.ForEach(async item =>
                    {
                        string roomPhoto = await fileExtension.WriteAsync(item, $"{uploadAvatarFolder}\\{string.Empty}");
                        switch (id)
                        {
                            case 1:
                                model.RoomPhoto1 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 2:
                                model.RoomPhoto2 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 3:
                                model.RoomPhoto3 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 4:
                                model.RoomPhoto4 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 5:
                                model.RoomPhoto5 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            default:
                                break;
                        }
                        galleries.Add(roomPhoto);
                        id++;
                    });
            }
            try
            {
                var item = _mapper.Map<Room>(model);
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

                if (galleries != null)
                {
                    galleries.ForEach(item =>
                    {
                        fileExtension.Remove($"{uploadAvatarFolder}\\{item}");
                    });
                }

                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        public async Task<OperationResult> UpdateFormAsync(RoomDto model)
        {
            FileExtension fileExtension = new FileExtension();
            var itemModel = await _repo.FindAll(x => x.Id == model.Id).AsNoTracking().FirstOrDefaultAsync();
            if (itemModel.RoomName != model.RoomName)
            {
                var check = await CheckExistRoomName(model.RoomName);
                if (!check.Success) return check;
            }

            if (itemModel.RoomNo != model.RoomNo)
            {
                var checkAccountNo = await CheckExistRoomNo(model.RoomNo);
                if (!checkAccountNo.Success) return checkAccountNo;
            }
            var item = _mapper.Map<Room>(model);

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\room\\image";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            if (model.File != null)
            {
                IFormFile filesAvatar = model.File.FirstOrDefault();
                if (!filesAvatar.IsNullOrEmpty())
                {
                    if (!item.RoomPhoto.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.RoomPhoto.Replace("/", "\\").Replace("/", "\\")}");
                    avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                    item.RoomPhoto = $"/FileUploads/images/room/image/{avatarUniqueFileName}";
                }
            }
            List<string> galleries = new List<string>();
            if (model.RoomGallery != null)
            {
                    if (!item.RoomPhoto1.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.RoomPhoto1.Replace("/", "\\").Replace("/", "\\")}");
                    if (!item.RoomPhoto2.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.RoomPhoto2.Replace("/", "\\").Replace("/", "\\")}");
                    if (!item.RoomPhoto3.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.RoomPhoto3.Replace("/", "\\").Replace("/", "\\")}");
                    if (!item.RoomPhoto4.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.RoomPhoto4.Replace("/", "\\").Replace("/", "\\")}");
                    if (!item.RoomPhoto5.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.RoomPhoto5.Replace("/", "\\").Replace("/", "\\")}");

                    item.RoomPhoto1 = item.RoomPhoto2 = item.RoomPhoto3 = item.RoomPhoto4 = item.RoomPhoto5 = null;

                    int id = 1;
                    model.RoomGallery.ForEach(async file =>
                    {
                        string roomPhoto = await fileExtension.WriteAsync(file, $"{uploadAvatarFolder}\\{string.Empty}");
                        switch (id)
                        {
                            case 1:
                                item.RoomPhoto1 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 2:
                                item.RoomPhoto2 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 3:
                                item.RoomPhoto3 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 4:
                                item.RoomPhoto4 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            case 5:
                                item.RoomPhoto5 = $"/FileUploads/images/room/image/{roomPhoto}";
                                break;
                            default:
                                break;
                        }
                        galleries.Add(roomPhoto);
                        id++;
                    });
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

                if (galleries != null)
                {
                    galleries.ForEach(item =>
                    {
                        fileExtension.Remove($"{uploadAvatarFolder}\\{item}");
                    });
                }

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
                    string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, item.RoomPhoto);
                    FileExtension fileExtension = new FileExtension();
                    var avatarUniqueFileName = item.RoomPhoto;
                    if (!avatarUniqueFileName.IsNullOrEmpty())
                    {
                        var result = fileExtension.Remove($"{_currentEnvironment.WebRootPath}\\{item.RoomPhoto}");
                        if (result)
                        {
                            item.RoomPhoto = string.Empty;
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

        public async Task<object> GetRoomBySiteAndHall(string siteID, string hallID)
        {
            var query = _repo.FindAll(x => x.Status == 1 && x.SiteGuid == siteID && x.HallGuid == hallID).Select(x => new {
                x.RoomName,
                x.Guid
            });

            var data = await query.ToListAsync();
            return data;
        }

        public async Task<object> GetRoomByid(int ID)
        {

            var x = _repoRoom.FindByID(Convert.ToDecimal(ID));
            var y = _repoHall.FindAll(y => y.Guid == x.HallGuid).FirstOrDefault();

            var result = new
            {
                x.Id,
                x.Type,
                y.HallName,
                HallGuid = y.Guid,
                RoomGuid = x.Guid,
                y.HallNo,
                x.RoomNo,
                x.RoomName,
                x.RoomSize,
                x.RoomDescript,
                x.Price,
                x.PriceMember,
                x.PriceSpecial,
                x.RoomLocation,
                x.RoomPhoto,
                x.RoomPhoto1,
                x.RoomPhoto2,
                x.RoomPhoto3,
                x.RoomPhoto4,
                x.RoomPhoto5,
            };

            return result;
        }
    }
}
