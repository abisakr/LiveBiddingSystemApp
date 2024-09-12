using Live_Bidding_System_App.Repositories.User.DTO;

namespace Live_Bidding_System_App.Repositories.User
{
    public interface IUserAccountRepository
    {
        public Task<string> Register(RegisterDto registerDto);
        public Task<string> Login(LoginDto loginDto);
    }
}
