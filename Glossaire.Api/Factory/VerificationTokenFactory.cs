using TechQuiz.Api.Models;
using TechQuiz.Api.Services;

namespace TechQuiz.Api.Factory
{
    public class VerifyTokenFactory
    {
        public static VerificationToken CreateVerifyToken(User user)
        {
            var verifyToken = new VerificationToken
            {
                Token = TokenService.GenerateHexToken(32),
                UserId = user.Id,
                User = user
            };
            return verifyToken;
        }
    }
}
