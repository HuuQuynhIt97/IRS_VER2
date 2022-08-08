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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IRS.Services
{
    public interface IAccountService : IServiceBase<Account, AccountDto>
    {
        Task<OperationResult> LockAsync(int id);
        Task<AccountDto> GetByUsername(string username);
        Task<object> GetAccounts();

        Task<OperationResult> AddFormAsync(AccountDto model);
        Task<OperationResult> UpdateFormAsync(AccountDto model);
        Task<OperationResult> ChangePassword(ChangePasswordDto model);
        Task<object> DeleteUploadFile(int key);
        Task<object> ShowPassword(int key);
    }
    public class AccountService : ServiceBase<Account, AccountDto>, IAccountService
    {
        private readonly IRepositoryBase<Account> _repo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        private readonly IWebHostEnvironment _currentEnvironment;

        public AccountService(
            IRepositoryBase<Account> repo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            MapperConfiguration configMapper,
            IWebHostEnvironment currentEnvironment
            )
            : base(repo, unitOfWork, mapper, configMapper)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configMapper = configMapper;
            _currentEnvironment = currentEnvironment;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> AddAsync(AccountDto model)
        {
            try
            {
                //var item = _mapper.Map<Account>(model);
                //item.Password = item.Password.ToEncrypt();
                //_repo.Add(item);
                //var accountID = await _sequenceService.GetPigFarmNewID();
                //var farmAccountList = new List<FarmAccount>();
                //foreach (var farmID in model.OCIDList)
                //{
                //    farmAccountList.Add(new FarmAccount(farmID, accountID));
                //}
                //_repoFarmAccount.AddRange(farmAccountList);
                //await _unitOfWork.SaveChangeAsync();

                //operationResult = new OperationResult
                //{
                //    StatusCode = HttpStatusCode.OK,
                //    Message = MessageReponse.AddSuccess,
                //    Success = true,
                //    Data = model
                //};
            }
            catch (Exception ex)
            {
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override async Task<OperationResult> UpdateAsync(AccountDto model)
        {
            try
            {
                var item = await _repo.FindByIDAsync(model.ID);
                if (model.Password.IsBase64() == false)
                    item.Password = model.Password.ToEncrypt();
                item.Username = model.Username;
                item.Name = model.Name;
                item.No = model.NO;
                item.Rfid = model.RFID;
                item.EmployeeId = model.EmployeeID;
                item.AccountGroupId = model.AccountGroupID;
                item.AccountRole = model.AccountRole;
                item.AccountTypeId = model.AccountTypeID;
                item.Comment = model.Comment;

                //_repo.Update(item);
                //var accountID = item.Id;
                //var removeList = await _repoFarmAccount.FindAll(x => x.AccountID == accountID).ToListAsync();
                //_repoFarmAccount.RemoveMultiple(removeList);

                //var farmAccountList = new List<FarmAccount>();
                //foreach (var farmID in model.OCIDList)
                //{
                //    farmAccountList.Add(new FarmAccount(farmID, accountID));
                //}
                //_repoFarmAccount.AddRange(farmAccountList);
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
        public override async Task<List<AccountDto>> GetAllAsync()
        {
            var query = _repo.FindAll(x => x.Status == true)
                .ProjectTo<AccountDto>(_configMapper);

            var data = await query.ToListAsync();
            return data;

        }
        public override async Task<OperationResult> DeleteAsync(object id)
        {
            var item = _repo.FindByID(id);
            item.Status = false;
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

        public async Task<OperationResult> LockAsync(int id)
        {
            var item = await _repo.FindByIDAsync(id);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Không tìm thấy tài khoản này!", Success = false };
            }
            try
            {
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.UnlockSuccess,
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

        public async Task<AccountDto> GetByUsername(string username)
        {
            var result = await _repo.FindAll(x => x.Username.ToLower() == username.ToLower()).ProjectTo<AccountDto>(_configMapper).FirstOrDefaultAsync();
            return result;
        }
        public async Task<OperationResult> CheckExistUsername(string userName)
        {
            var item = await _repo.FindAll(x => x.Username == userName).AnyAsync();
            if (item)
            {
                return new OperationResult { StatusCode = HttpStatusCode.OK, Message = "Username already exists", Success = false };
            }
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Success = true,
                    Data = item
                };
            return operationResult;
        }
        public async Task<object> GetAccounts()
        {
            var query = await _repo.FindAll(x => x.Status == true)
                .Select(x => new
                {
                    x.Username,
                    x.Id
                }).ToListAsync();
            return query;
        }

        public async Task<OperationResult> UploadAvatar(UploadAvatarRequest request)
        {
            //string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            //var accountId = JWTExtensions.GetDecodeTokenByID(token).ToInt();
            var item = await _repo.FindAll(x => x.Id == request.Key).FirstOrDefaultAsync();
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Not Found!", Success = false };
            }

            FileExtension fileExtension = new FileExtension();

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\account\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            IFormFile filesAvatar = request.File;

            if (filesAvatar != null)
            {
                if (!item.PhotoPath.IsNullOrEmpty())
                    fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.PhotoPath.Replace("/", "\\").Replace("/", "\\")}");
                avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                item.PhotoPath = $"/FileUploads/images/account/avatar/{avatarUniqueFileName}";
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
                    Data = item
                };
            }
            catch (Exception ex)
            {
                // Nếu tạo ra file rồi mã lưu db bị lỗi thì xóa file vừa tạo đi
                if (!avatarUniqueFileName.IsNullOrEmpty())
                    fileExtension.Remove($"{uploadAvatarFolder}\\{avatarUniqueFileName}");

                // Không thêm được thì xóa file vừa tạo đi
                operationResult = ex.GetMessageError();
            }
            return operationResult;
        }

        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OperationResult> AddFormAsync(AccountDto model)
        {
            var check = await CheckExistUsername(model.Username);
            if (!check.Success) return check;
            FileExtension fileExtension = new FileExtension();
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\account\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);
            if (model.File != null)
            {
                IFormFile files = model.File.FirstOrDefault();
                if (!files.IsNullOrEmpty())
                {
                    avatarUniqueFileName = await fileExtension.WriteAsync(files, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                    model.PhotoPath = $"/FileUploads/images/account/avatar/{avatarUniqueFileName}";
                }
            }
            try
            {
                var item = _mapper.Map<Account>(model);
                item.Password = item.Password.ToEncrypt();
                _repo.Add(item);
                await _unitOfWork.SaveChangeAsync();

                //var accountID = await _sequenceService.GetPigFarmNewID();
                var accountID = item.Id;
                //var farmAccountList = new List<FarmAccount>();
                //foreach (var farmID in model.OCIDList)
                //{
                //    farmAccountList.Add(new FarmAccount(farmID, accountID));
                //}
                //_repoFarmAccount.AddRange(farmAccountList);
                //await _unitOfWork.SaveChangeAsync();

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
        /// <summary>
        /// Add account sau do add AccountGroupAccount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<OperationResult> UpdateFormAsync(AccountDto model)
        {
            
            FileExtension fileExtension = new FileExtension();
            var item = await _repo.FindByIDAsync(model.ID);
            if (item.Username != model.Username)
            {
                var check = await CheckExistUsername(model.Username);
                if (!check.Success) return check;
            }
            item.PhotoPath = model.PhotoPath;

            // Nếu có đổi ảnh thì xóa ảnh cũ và thêm ảnh mới
            var avatarUniqueFileName = string.Empty;
            var avatarFolderPath = "FileUploads\\images\\account\\avatar";
            string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, avatarFolderPath);

            if (model.File != null)
            {
                IFormFile filesAvatar = model.File.FirstOrDefault();
                if (!filesAvatar.IsNullOrEmpty())
                {
                    if (!item.PhotoPath.IsNullOrEmpty())
                        fileExtension.Remove($"{_currentEnvironment.WebRootPath}{item.PhotoPath.Replace("/", "\\").Replace("/", "\\")}");
                    avatarUniqueFileName = await fileExtension.WriteAsync(filesAvatar, $"{uploadAvatarFolder}\\{avatarUniqueFileName}");
                    item.PhotoPath = $"/FileUploads/images/account/avatar/{avatarUniqueFileName}";
                }
            }

            try
            {
                if (model.Password.IsBase64() == false)
                    item.Password = model.Password.ToEncrypt();
                item.Username = model.Username;
                item.Name = model.Name;
                item.No = model.NO;
                item.Rfid = model.RFID;
                item.EmployeeId = model.EmployeeID;
                item.AccountGroupId = model.AccountGroupID;
                item.AccountRole = model.AccountRole;
                item.AccountTypeId = model.AccountTypeID;
                item.Comment = model.Comment;
                _repo.Update(item);
                var accountID = item.Id;
                //var removeList = await _repoFarmAccount.FindAll(x => x.AccountID == accountID).ToListAsync();
                //_repoFarmAccount.RemoveMultiple(removeList);

                //var farmAccountList = new List<FarmAccount>();
                //foreach (var farmID in model.OCIDList)
                //{
                //    farmAccountList.Add(new FarmAccount(farmID, accountID));
                //}
                //_repoFarmAccount.AddRange(farmAccountList);
                //await _unitOfWork.SaveChangeAsync();

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

        public async Task<object> DeleteUploadFile(int key)
        {
            try
            {
                var item = await _repo.FindByIDAsync(key);
                if (item != null)
                {
                    string uploadAvatarFolder = Path.Combine(_currentEnvironment.WebRootPath, item.PhotoPath);
                    FileExtension fileExtension = new FileExtension();
                    var avatarUniqueFileName = item.PhotoPath;
                    if (!avatarUniqueFileName.IsNullOrEmpty())
                    {
                        var result = fileExtension.Remove($"{_currentEnvironment.WebRootPath}\\{item.PhotoPath}");
                        if (result)
                        {
                            item.PhotoPath = string.Empty;
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

        public async Task<object> ShowPassword(int key)
        {
            var item = await _repo.FindByIDAsync(key);
            if (item != null)
            {
                return new
                {
                    PasswordDecrypt = item.Password.ToDecrypt(),
                    PasswordEncrypt = item.Password,
                };
            }
            return new
            {
                PasswordDecrypt = "",
                PasswordEncrypt = "",
            };
        }

        public async Task<OperationResult> ChangePassword(ChangePasswordDto model)
        {
            var item = await _repo.FindByIDAsync(model.ID);
            if (item == null)
            {
                return new OperationResult { StatusCode = HttpStatusCode.NotFound, Message = "Account Not found !", Success = false };
            }
            try
            {
                item.Password = model.Upwd.ToEncrypt();
                _repo.Update(item);
                await _unitOfWork.SaveChangeAsync();
                operationResult = new OperationResult
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = MessageReponse.ChangePasswordSuccess,
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
    }
}
