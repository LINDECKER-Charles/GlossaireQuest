using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TechQuiz.Api.Models;
using TechQuiz.Api.Dtos;

namespace TechQuiz.Api.Factory
{
    public class QuizzFactory
    {
        public static Quizz CreateQuizz(QuizzRequest request, User user)
        {
            var quizz = new Quizz
            {
                Name = request.Name,
                Description = request.Description,
                UserId = user.Id,
                User = user
            };

            quizz.Questions = request.Questions.Select(q =>
            {
                var question = new Question
                {
                    Name = q.Name,
                    Description = q.Description,
                    Point = q.Point,
                    Type = q.Type,
                    Quizz = quizz,
                    QuizzId = quizz.Id
                };

                question.Choices = q.Choices.Select(c => new Choice
                {
                    Name = c.Name,
                    IsCorrect = c.IsCorrect,
                    Question = question,
                    QuestionId = question.Id
                }).ToList();

                return question;
            }).ToList();

            return quizz;
        }
    }
}
