using AutoMapper;
using WordApp.Domain.Interfaces;
using WordApp.Persistence.Interfaces;
using WordApp.Shared.Dtos.AuthDtos;

namespace WordApp.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        public async Task<bool> CheckUsernameUniquenessAsync(string username)
        {
            return await _userRepository.CheckUsernameUniquenessAsync(username);
        }

        public async Task<UserInfo> GetUserInfoById(Guid id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException("User doesn`t exist");
            }
            return _mapper.Map<UserInfo>(user);
        }
    }
}
