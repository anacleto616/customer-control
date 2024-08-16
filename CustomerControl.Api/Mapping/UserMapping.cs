using CustomerControl.Api.Dtos;
using CustomerControl.Api.Entities;

namespace CustomerControl.Api.Mapping;

public static class UserMapping
{
    public static User ToEntity(this UserRegisterDto user)
    {
        return new User()
        {
            Name = user.Name,
            Email = user.Email,
            Password = user.Password,
        };
    }
}
