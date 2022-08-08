using Microsoft.EntityFrameworkCore;
using IRS.Data;
using IRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IRS.Helpers;
using IRS.DTO;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using System.Net;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using IRS.DTO.auth;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using IRS.Constants;

namespace IRS.Services
{
    public interface IAuthService
    {
        Task<XAccount> Login(string username, string password);
        Task LogOut();
        Task<bool> CheckLock(string username);
        Task<OperationResult> ResetPassword(ResetPasswordDto reset);
        Task<OperationResult> ForgotPassword(string email);
        Task<OperationResult> ForgotUsername(string email);
        Task<OperationResult> LoginAsync(UserForLoginDto loginDto);
        Task<OperationResult> LoginAsync(decimal ID);
        Task<OperationResult> RefreshTokenAsync(string token, string refreshToken);
    }
    public class AuthService : IAuthService
    {
        private readonly IRepositoryBase<XAccount> _repo;
        private readonly IRepositoryBase<CodeType> _repoCodeType;
        private readonly IRepositoryBase<XAccountGroup> _repoXAccountGroup;
        private readonly IRepositoryBase<Employee> _repoEmployee;
        private readonly IRepositoryBase<RefreshToken> _repoRefreshToken;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtSettings _jwtSettings;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly IConfiguration _configuration;

        public AuthService(
            IRepositoryBase<XAccount> repo,
            IRepositoryBase<CodeType> repoCodeType,
            IRepositoryBase<XAccountGroup> repoXAccountGroup,
            IRepositoryBase<Employee> repoEmployee,
            IRepositoryBase<RefreshToken> repoRefreshToken,
            IUnitOfWork unitOfWork,
            JwtSettings jwtSettings,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            TokenValidationParameters tokenValidationParameters,
            IWebHostEnvironment currentEnvironment,
            IConfiguration configuration
            )
        {
            _repo = repo;
            _repoCodeType = repoCodeType;
            _repoXAccountGroup = repoXAccountGroup;
            _repoEmployee = repoEmployee;
            _repoRefreshToken = repoRefreshToken;
            _unitOfWork = unitOfWork;
            _jwtSettings = jwtSettings;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _tokenValidationParameters = tokenValidationParameters;
            _currentEnvironment = currentEnvironment;
            _configuration = configuration;
        }

        public async Task<bool> CheckLock(string username)
        {
            var account = await _repo.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Uid == username);

            if (account == null)
                return false;

            return true;

        }

        public async Task<XAccount> Login(string username, string password)
        {
            var account = await _repo.FindAll().FirstOrDefaultAsync(x => x.Uid == username);

            if (account == null)
                return null;
            if (account.Upwd == password)
                return account;
            return null;

        }

        public async Task<OperationResult> LoginAsync(UserForLoginDto loginDto)
        {
            var account = await _repo.FindAll(x => x.Uid == loginDto.Username)
                .FirstOrDefaultAsync();
            if (account == null)
                return new OperationResult
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "The account name is not available!",
                Success = false
            };
            if (account.Upwd == loginDto.Password)
            {

                return await GenerateOperationResultForUserAsync(account, loginDto.Password);
            }

            return new OperationResult
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "The account name or password is incorrect!",
                Success = false
            };

        }

        public async Task<OperationResult> LoginAsync(decimal ID)
        {
            var account = await _repo.FindAll().FirstOrDefaultAsync(x => x.AccountId == ID);
            if (account != null)
                return await GenerateOperationResultForUserAsync(account,"");

            return new OperationResult
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = "",
                Success = false
            };

        }
        public async Task LogOut()
        {
            string token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
            var accountId = JWTExtensions.GetDecodeTokenByID(token);
            var account = await _repo.FindByIDAsync(accountId.ToDecimal());
            account.LastLoginDate = DateTime.Now;
            try
            {
                _repo.Update(account);
                await _unitOfWork.SaveChangeAsync();
            }
            catch 
            {
            }

        }

        public async Task<OperationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new OperationResult { Message = "Invalid token!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            var expiryDateUnix = (validatedToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value).ToLong();

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.Now)
            {
                return new OperationResult { Message = "Unexpired token!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _repoRefreshToken.FindAll().FirstOrDefaultAsync(x => x.JwtId == refreshToken);

            if (storedRefreshToken == null)
            {
                return new OperationResult { Message = "Token does not existed!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            if (DateTime.Now > storedRefreshToken.ExpiryDate)
            {
                return new OperationResult { Message = "Token has expired!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            if (storedRefreshToken.Invalidated)
            {
                return new OperationResult { Message = "Token is invalidated!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            if (storedRefreshToken.Used)
            {
                return new OperationResult { Message = "Token is used!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new OperationResult { Message = "Token does not match!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            storedRefreshToken.Used = true;
            _repoRefreshToken.Update(storedRefreshToken);
            await _unitOfWork.SaveChangeAsync();

            var user = await _repo.FindByIDAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);

            return await GenerateOperationResultForUserAsync(user, "");
        }

        public async Task<OperationResult> ResetPassword(ResetPasswordDto reset)
        {
            var validatedToken = GetPrincipalFromToken(reset.token);

            if (validatedToken == null)
            {
                return new OperationResult { Message = "Invalid token!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }

            var expiryDateUnix = (validatedToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value).ToLong();

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc < DateTime.Now)
            {
                return new OperationResult { Message = "Unexpired token!", StatusCode = HttpStatusCode.BadRequest, Success = false };
            }
            var email = (validatedToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value).ToSafetyString();
            if (email == null)
                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Your email does not exist"
                };
            var employee = await _repoEmployee.FindAll(x => x.Status == 1).AsNoTracking().Select(x=> new {
            x.Email,
            x.Guid
            }).FirstOrDefaultAsync(x => x.Email == email);
            if (employee == null)
                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Your email does not exist"
                };
            var account = await _repo.FindAll(x => x.Status == "1").AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeGuid == employee.Guid);
            if (account == null)
                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Your account does not exist"
                };
            account.Upwd = reset.NewPassword;
            try
            {
                _repo.Update(account);

                await _unitOfWork.SaveChangeAsync();
                return new OperationResult
                {
                    Success = true,
                    Data = null,
                    Message = "Reset password successfully!"
                };
            }
            catch (Exception)
            {

                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Can not reset password!"
                };
            }
        }

        private async Task<OperationResult> GenerateOperationResultForUserAsync(XAccount user, string password)
        {
            var claims = new[]
            {
                            new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Add(_jwtSettings.TokenLifetime),
                //Expires = DateTime.Now.Add(TimeSpan.FromSeconds(15)),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenValue = tokenHandler.WriteToken(token);
            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                AccountId = user.AccountId.ToInt(),
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMonths(6),
                Token = tokenValue
            };

            _repoRefreshToken.Add(refreshToken);
            user.LastLoginDate = DateTime.Now;
            await _unitOfWork.SaveChangeAsync();
            var userResponse = _mapper.Map<UserForDetailDto>(user);
            var pageSizeSetting = await _repoCodeType.FindAll(x => x.CodeNo == user.PageSizeSetting && CodeTypeConst.PageSize_Setting == x.CodeType1 && x.Status == "Y").AsNoTracking().Select(x => x.CodeName).FirstOrDefaultAsync();
            if (pageSizeSetting != null)
            {
                userResponse.PageSizeSettingValue = pageSizeSetting;
                userResponse.PageSizeSetting = user.PageSizeSetting;
            }
            var pages = await _repoCodeType.FindAll(x => CodeTypeConst.PageSize_Setting == x.CodeType1 && x.Status == "Y").AsNoTracking().Select(x => x.CodeName).ToListAsync();
            userResponse.PageSizeSettingList = pages;
             var employee = await _repoEmployee.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Guid == user.EmployeeGuid);
            if (employee != null)
            {
               
                userResponse.NickName = employee.NickName;
            }
            var xaccountGroup = await _repoXAccountGroup.FindAll().AsNoTracking().FirstOrDefaultAsync(x => x.Guid == user.AccountGroup);

            if (xaccountGroup != null)
            {
                var groupNO = xaccountGroup.GroupNo;
                var groupID = xaccountGroup.Id;
                userResponse.GroupCode = groupNO;
                userResponse.GroupID = groupID;
            }
            return new OperationResult
            {
                Success = true,
                Data = new
                {
                    Token = tokenValue,
                    RefreshToken = refreshToken.JwtId,
                    User = userResponse
                }
            };
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<OperationResult> GenerateOperationResultForUserForGotPasswordAsync(XAccount user, string email)
        {

            var claims = new[]
            {
                             new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                            new Claim(ClaimTypes.Email, email.ToString())
                        };
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.Add(TimeSpan.FromHours(24)),
                SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenValue = tokenHandler.WriteToken(token);
           var path = "EmailTemplate\\forgot-password.html";
            string fogotPasswordHtml = Path.Combine(_currentEnvironment.WebRootPath, path);
            string html = File.ReadAllText(fogotPasswordHtml);
            string urlRedirect = $"{_configuration["MailSettings:AngularUrl"]}/reset-password?token={tokenValue}";
            html = html.Replace("{{HREF}}", urlRedirect);
            
            var check = await  _emailService.SendAsync(email, "Forgot Password", html);
            if (check == "")
            return new OperationResult
            {
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Data = null
            };
            return new OperationResult
            {
                Success = false,
                StatusCode = HttpStatusCode.OK,
                Data = check
            };
        }

        public async Task<OperationResult> ForgotPassword(string email)
        {
            var employee = await _repoEmployee.FindAll(x=> x.Status == 1).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

            if (employee == null)
                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Your email does not exist"
                };
            var account = await _repo.FindAll(x => x.Status == "1").AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeGuid == employee.Guid);

            if (account == null)
                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Your account does not exist"
                };
            return await GenerateOperationResultForUserForGotPasswordAsync(account,email);

        }

        public async Task<OperationResult> ForgotUsername(string email)
        {
            var employee = await _repoEmployee.FindAll(x => x.Status == 1).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

            if (employee == null)
                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Your email does not exist"
                };
            var account = await _repo.FindAll(x => x.Status == "1").AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeGuid == employee.Guid);

            if (account == null)
                return new OperationResult
                {
                    Success = false,
                    Data = null,
                    Message = "Your account does not exist"
                };

            var path = "EmailTemplate\\forgot-username.html";
            string fogotPasswordHtml = Path.Combine(_currentEnvironment.WebRootPath, path);
            string html = File.ReadAllText(fogotPasswordHtml);
            string urlRedirect = $"{_configuration["MailSettings:AngularUrl"]}/login";
            html = html.Replace("{{HREF}}", urlRedirect);
            html = html.Replace("{{USERNAME}}", account.Uid);

            var check = await _emailService.SendAsync(email, "Forgot Username", html);
            if (check == "")
                return new OperationResult
                {
                    Success = true,
                    StatusCode = HttpStatusCode.OK,
                    Message = "Success! An email has been sent. Please check your inbox!"
                };
            return new OperationResult
            {
                Success = false,
                StatusCode = HttpStatusCode.OK,
                Data = check
            };
        }
    }
}
