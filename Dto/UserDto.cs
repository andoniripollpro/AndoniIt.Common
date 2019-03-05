using System.Collections.Generic;

namespace MovistarPlus.Common.Dto
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<ProfileDto> Profiles { get; set; } = new List<ProfileDto>();        
    }
}