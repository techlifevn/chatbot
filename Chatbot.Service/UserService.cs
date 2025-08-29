using Chatbot.Common;
using Chatbot.Common.Result;
using Chatbot.Data.EF;
using Chatbot.Data.Entity;
using Chatbot.Model.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chatbot.Service
{
    public interface IUserService
    {
        Task<PagedResult<UserVm>> GetPaging(GetPagingRequest request);
        Task<List<UserVm>> GetAll();
        Task<UserVm> GetById(string id);
        Task<Result<bool>> Create(UserCreateRequest request);
        Task<Result<bool>> Update(UserUpdateRequest request);
        Task<Result<bool>> Delete(string id);
        Task<Result<bool>> UpdateStatus(string id);
        Task<Result<string>> Login(UserSignRequest request);
    }

    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IDbConnectionService _dbConnection;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;

        public UserService(DataContext context
            , UserManager<User> userManager
            , SignInManager<User> signInManager
            , IDbConnectionService dbConnection
            , IConfiguration configuration
            , ILogger<UserService> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _dbConnection = dbConnection;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<Result<bool>> Create(UserCreateRequest request)
        {
            try
            {
                string userName = request.UserName.Trim().ToLower();
                string email = request.Email.Trim().ToLower();

                if (await _userManager.FindByNameAsync(userName) != null)
                    return new ErrorResult<bool>("UserName đã tồn tại");

                if (await _userManager.FindByEmailAsync(email) != null)
                    return new ErrorResult<bool>("Email đã tồn tại");

                if (string.Compare(request.Password, request.ConfirmPassword, StringComparison.OrdinalIgnoreCase) != 0)
                    return new ErrorResult<bool>("Mật khẩu xác nhận không trùng khớp");

                var entity = new User
                {
                    FirstName = request.FirstName.Trim(),
                    LastName = request.LastName.Trim(),
                    FullName = request.FirstName.Trim() + ' ' + request.LastName.Trim(),
                    Email = email,
                    UserName = userName,
                    IsDeleted = false,
                    IsStatus = false,
                    CreatedAt = DateTime.Now,
                };

                var result = await _userManager.CreateAsync(entity, request.ConfirmPassword);

                if (result.Succeeded)
                    return new SuccessResult<bool> { IsSuccessed = true, Message = "Tạo tài khoản thành công" };

                return new ErrorResult<bool> { IsSuccessed = false, Message = "Tạo tài khoản thất bại" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<bool>> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) return new ErrorResult<bool>("Tài khoản không tồn tại");

                var result = await _userManager.DeleteAsync(user);

                if (result.Succeeded) return new SuccessResult<bool> { IsSuccessed = true, Message = "Xóa thành công" };

                return new ErrorResult<bool> { IsSuccessed = false, Message = "Xóa không thành công" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<UserVm>> GetAll()
        {
            try
            {
                var data = await _context.Users
                    .Where(x => x.IsStatus)
                    .Select(x => new UserVm
                    {
                        Id = x.Id.ToString(),
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        FullName = x.FullName,
                        UserName = x.UserName
                    }).ToListAsync();

                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<UserVm> GetById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) return null;

                return new UserVm
                {
                    Id = id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    UserName = user.UserName
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagedResult<UserVm>> GetPaging(GetPagingRequest request)
        {
            try
            {
                string keyword = !string.IsNullOrWhiteSpace(request.Keyword) ? request.Keyword.Trim().ToLower() : "";

                var query = _context.Users.AsQueryable();

                if (!string.IsNullOrWhiteSpace(request.Keyword))
                    query = query.Where(x => x.UserName.ToLower().Contains(keyword)
                     || x.FullName.ToLower().Contains(keyword));

                int totalRow = await query.CountAsync();

                var data = await query
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new UserVm
                    {
                        Id = x.Id.ToString(),
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        FullName = x.FullName,
                        UserName = x.UserName
                    }).ToListAsync();

                return new PagedResult<UserVm>
                {
                    Items = data,
                    Keyword = keyword,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = totalRow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<string>> Login(UserSignRequest request)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.UserName);

                if (user == null || !user.IsStatus) return new ErrorResult<string> { IsSuccessed = false, Message = "Tài khoản hoặc mật khẩu không đúng" };

                var result = await _signInManager.PasswordSignInAsync(user, request.Password, true, true);

                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                        return new ErrorResult<string>("Tài khoản đang tạm khóa, Vui lòng quay trở lại sau!");
                    else
                        return new ErrorResult<string>("Tài khoản hoặc mật khẩu không đúng!");
                }

                var claims = new List<Claim>
                {
                    new("Id", user.Id.ToString()),
                    new(ClaimTypes.Name, user.UserName),
                    new(ClaimTypes.Sid, ""),
                    new("UserName", user.UserName),
                    new("FullName", user.FullName),
                    new("FirstName", user.FirstName),
                    new("LastName", user.LastName),
                    new("PhoneNumber", user.PhoneNumber ?? ""),
                    new("Email", user.Email ?? "")
                };

                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var role in userRoles)
                {
                    claims.Add(new(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstants.AppSettings.JWTSecurityKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.Aes128CbcHmacSha256);
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(SystemConstants.AppSettings.ExpiresUtcMinutes),
                    signingCredentials: creds
                );

                return new SuccessResult<string>(new JwtSecurityTokenHandler().WriteToken(token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<bool>> Update(UserUpdateRequest request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id);

                if (user == null) return new ErrorResult<bool>("Tài khoản không tồn tại");

                user.FirstName = request.FirstName.Trim();
                user.LastName = request.LastName.Trim();
                user.FullName = request.FirstName.Trim() + " " + request.LastName.Trim();
                user.UpdatedAt = DateTime.Now;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return new ErrorResult<bool>("Cập nhật thành công");

                return new ErrorResult<bool>("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<Result<bool>> UpdateStatus(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null) return new ErrorResult<bool>("Tài khoản không tồn tại");

                user.IsStatus = !user.IsStatus;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded) return new SuccessResult<bool>("Cập nhật thành công");

                return new ErrorResult<bool>("Cập nhật không thành công");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
