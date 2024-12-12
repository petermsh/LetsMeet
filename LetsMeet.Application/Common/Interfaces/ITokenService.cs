using LetsMeet.Domain.Entities;

namespace LetsMeet.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateAccessToken(AppUser user);
}