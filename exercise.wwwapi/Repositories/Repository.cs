using exercise.wwwapi.Data;
using exercise.wwwapi.Data.DTO;
using exercise.wwwapi.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using workshop.webapi.Services;

namespace exercise.wwwapi.Repositories
{
    public interface IRepository
    {
        Task<ResponseObject<string>> CreateUser(CreateUserDTO cDTO);
        Task<ResponseObject<string>> LoginUser(LoginUserDTO lDTO);
        Task<ICollection<GetDataDTO>> GetData();
    }
    public class Repository : IRepository
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<ApiUser> _userManager;
        private readonly TokenService _tokenService;

        public Repository(DatabaseContext dbContext, UserManager<ApiUser> userManager, TokenService tokenService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ResponseObject<string>> CreateUser(CreateUserDTO cDTO)
        {
            ResponseObject<string> responseObject = new();
            ApiUser? dbUser = await _userManager.FindByEmailAsync(cDTO.Email);
            if (dbUser != null)
            {
                responseObject.Status = ResponseStatus.Failure;
                responseObject.ErrorMessage = "Email already exists! Did you mean to use /login ?";
                return responseObject;
            }

            var result = await _userManager.CreateAsync(new ApiUser() { Email = cDTO.Email, UserName = cDTO.UserName }, cDTO.Password!);
            if (result.Errors.Any())
            {
                responseObject.Status = ResponseStatus.Failure;
                responseObject.ErrorMessage = result.ToString();
                return responseObject;
            }
            await _dbContext.SaveChangesAsync();
            responseObject.Data = "User created successfully!";
            return responseObject;
        }

        public async Task<ResponseObject<string>> LoginUser(LoginUserDTO lDTO)
        {
            ResponseObject<string> responseObject = new();
            ApiUser? dbUser = await _userManager.FindByEmailAsync(lDTO.Email);
            if (dbUser == null)
            {
                responseObject.Status = ResponseStatus.Failure;
                responseObject.ErrorMessage = "User with email not found, did you mean to /register ?";
                return responseObject;
            }
            bool isValidPassword = await _userManager.CheckPasswordAsync(dbUser, lDTO.Password);
            if (!isValidPassword)
            {
                responseObject.Status = ResponseStatus.Failure;
                responseObject.ErrorMessage = "Wrong password for user";
                return responseObject;
            }
            string token = _tokenService.CreateToken(dbUser);
            await _dbContext.SaveChangesAsync();
            responseObject.Data = token;
            return responseObject;
        }

        public async Task<ICollection<GetDataDTO>> GetData()
        {
            return await _dbContext.DbData.Select(x => new GetDataDTO() { Id = x.Id, Description = x.Description }).ToListAsync();
        }
    }
}
