using Live_Bidding_System_App.Helper;
using Live_Bidding_System_App.Repositories.User.DTO;

namespace Live_Bidding_System_App.Repositories.User
{
    public interface IUserAccountRepository
    {
        public Task<OperationResult<string>> Register(RegisterDto registerDto);
        public Task<OperationResult<string>> Login(LoginDto loginDto);
    }
}
