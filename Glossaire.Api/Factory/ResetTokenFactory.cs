using TechQuiz.Api.Models;
using TechQuiz.Api.Services;

namespace TechQuiz.Api.Factory
{
    public class ResetTokenFactory
    {
        public static ResetToken CreateResetToken(User user)
        {
            var resetToken = new ResetToken
            {
                Token = TokenService.GenerateHexToken(64),
                UserId = user.Id,
                User = user
            };
            return resetToken;
        }
    }
}
