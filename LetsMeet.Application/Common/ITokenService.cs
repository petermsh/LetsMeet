using LetsMeet.Domain.Entities;

namespace LetsMeet.Application.Common;

public interface ITokenService
{
    string CreateAccessToken(AppUser user);
}