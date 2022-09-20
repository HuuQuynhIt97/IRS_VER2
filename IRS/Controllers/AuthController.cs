using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using IRS.DTO;
using IRS.DTO.auth;
using IRS.Helpers;
using IRS.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IRS.Controllers
{
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService
            )
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenViewModel model)
        {
            return StatusCodeResult(await _authService.RefreshTokenAsync(model.token, model.refreshToken));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] UserForLoginDto model)
        {
            return StatusCodeResult(await _authService.LoginAsync(model));
        }

        [HttpPost]
        public async Task<IActionResult> LoginRememberAsync([FromBody] UserForLoginRememberDto request)
        {
            return StatusCodeResult(await _authService.LoginAsync(request.ID));
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            return Ok(await _authService.ForgotPassword(email));
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto reset)
        {
            return StatusCodeResult(await _authService.ResetPassword(reset));
        }
        [HttpPost]
        public async Task<IActionResult> ForgotUsername(ForgotUsernameDto forgot)
        {
            return StatusCodeResult(await _authService.ForgotUsername(forgot.email));
        }
        [HttpPost]
        public async Task<IActionResult> LogOutAsync()
        {
            await _authService.LogOut();
            return NoContent();
        }

    }

}
