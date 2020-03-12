using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Services.Interface;
using Data.Model;
using Data.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly RoleManager<IdentityUserRole> _roleManager;

        private readonly IConfiguration _configuration;
        IUserService _userService;
        ITokenService _tokenService;

        public UsersController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            IUserService userService,
            ITokenService tokenService
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        //public ActionResult<IdentityUser> Login(UserVM userVM)
        public async Task<IActionResult> Login(LoginVM loginViewModel)
        {
            if (ModelState.IsValid)
            {
                //var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, false, false);
                //if (result.Succeeded)
                //{
                    var user = await _userManager.FindByNameAsync(loginViewModel.Username);
                    //var kk = _roleManager.FindByNameAsync("Seller").Result;
                    //if (kk.UserId == user.Id)
                    //{

                    //}
                    //if (user != null)
                    //{
                        #region Create Token
                        //var claims = new List<Claim> {
                        //    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        //    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString())
                        //};

                        //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                        //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        //var token = new JwtSecurityToken(
                        //    _configuration["Jwt:Issuer"],
                        //    _configuration["Jwt:Audience"],
                        //    claims,
                        //    expires: DateTime.UtcNow.AddMinutes(10),
                        //    signingCredentials: signIn
                        //);
                        //var idtoken = new JwtSecurityTokenHandler().WriteToken(token);
                        //return Ok(idtoken + "..." + user.Email + "..." + user.Name);
                        #endregion
                        try
                        {
                            #region Create Token & Refresh Token
                            //var authClaim = new List<Claim>
                            //{
                            //new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                            //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                            //};
                            //var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                            //var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                            //var acctoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                            //    _configuration["Jwt:Audience"],
                            //    authClaim,
                            //    expires: DateTime.UtcNow.AddMinutes(40),
                            //    signingCredentials: signIn);
                            //var accessToken = new JwtSecurityTokenHandler().WriteToken(acctoken);
                            //var expirationToken = DateTime.UtcNow.AddMinutes(40).Ticks;
                            //var refreshToken = GenerateRefreshToken();
                            //var expirationRefreshToken = DateTime.UtcNow.AddMinutes(140).Ticks;

                            //_tokenService.Insert(new TokenVM
                            //{
                            //    Username = userVM.Username,
                            //    AccessToken = accessToken,
                            //    ExpireToken = DateTime.UtcNow.AddMinutes(40).Ticks,
                            //    RefreshToken = refreshToken,
                            //    ExpireRefreshToken = expirationRefreshToken
                            //});

                            //return Ok(new
                            //{
                            //    Email = user.Email,
                            //    Name = user.Name,
                            //    Username = userVM.Username,
                            //    AccessToken = accessToken,
                            //    ExpireToken = expirationToken,
                            //    RefreshToken = refreshToken,
                            //    ExpireRefreshToken = expirationRefreshToken
                            //}
                            //);
                            #endregion
                            TokenVM tokenVM = new TokenVM();
                            tokenVM.Username = loginViewModel.Username;
                            TokenVM generate = await GenerateToken(tokenVM);
                            return Ok(generate);
                        }
                        catch (Exception) { }
                //    }
                //}
                return BadRequest(new { message = "Username or password is invalid" });
            }
            else
            {
                return BadRequest("Failed");
            }
        }

        public async Task<TokenVM> GenerateToken(TokenVM tokenVM)
        {
            var user = await _userManager.FindByNameAsync(tokenVM.Username);
            TokenVM response = _tokenService.Get(tokenVM.Username);

            var authClaim = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var acctoken = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                authClaim,
                expires: DateTime.UtcNow.AddMinutes(40),
                signingCredentials: signIn);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(acctoken);
            var expirationToken = DateTime.UtcNow.AddMinutes(40).Ticks;
            var refreshToken = GenerateRefreshToken();
            var expirationRefreshToken = DateTime.UtcNow.AddMinutes(140).Ticks;
            TokenVM generate = new TokenVM
            {
                Email = user.Email,
                Name = user.Name,
                Username = user.UserName,
                AccessToken = accessToken,
                ExpireToken = expirationToken,
                RefreshToken = refreshToken,
                ExpireRefreshToken = expirationRefreshToken
            };

            if (response == null)
            {
                _tokenService.Insert(new TokenVM
                {
                    Username = generate.Username,
                    AccessToken = generate.AccessToken,
                    ExpireToken = generate.ExpireToken,
                    RefreshToken = generate.RefreshToken,
                    ExpireRefreshToken = generate.ExpireRefreshToken
                });
            }
            else
            {
                _tokenService.Update(new TokenVM
                {
                    Username = generate.Username,
                    AccessToken = generate.AccessToken,
                    ExpireToken = generate.ExpireToken,
                    RefreshToken = generate.RefreshToken,
                    ExpireRefreshToken = generate.ExpireRefreshToken
                });
            }
            return generate;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> Refresh(TokenVM tokenViewModel)
        {
            try
            {
                TokenVM refToken = _tokenService.Get(tokenViewModel.Username);
                if (refToken.ExpireRefreshToken < DateTime.UtcNow.Ticks)
                {
                    return Unauthorized();
                }
                if (refToken.RefreshToken == tokenViewModel.RefreshToken)
                {
                    TokenVM generate = await GenerateToken(tokenViewModel);
                    return Ok(generate);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex);
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        [HttpGet("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Logged out");
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserVM userVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User { };
                    user.Id = Guid.NewGuid().ToString();
                    user.Name = userVM.Name;
                    user.Email = userVM.Email;
                    user.Password = userVM.Password;
                    user.UserName = userVM.Username;
                    user.CreateDate = DateTime.Now;

                    var result = await _userManager.CreateAsync(user, userVM.Password);
                    //var result = await _userService.Register(userVM);
                    if (result.Succeeded)
                    {
                        return Ok("Register succes");
                    }
                }
                catch (Exception)
                {

                    throw;
                }
                //AddErrors(result);
            }

            return BadRequest(ModelState);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}