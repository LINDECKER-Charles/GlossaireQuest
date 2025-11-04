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

        public static Question CreateQuestion(QuestionRequest request, Quizz quiz)
        {
            var question = new Question
            {
                Name = request.Name,
                Description = request.Description,
                Point = request.Point,
                Type = request.Type,
                Quizz = quiz,
                QuizzId = quiz.Id
            };

            question.Choices = request.Choices.Select(c => new Choice
            {
                Name = c.Name,
                IsCorrect = c.IsCorrect,
                Question = question,
                QuestionId = question.Id
            }).ToList();

            return question;
        }

        public static Choice CreateChoice(ChoiceRequest request, Question question)
        {
            var choice = new Choice
            {
                Name = request.Name,
                IsCorrect = request.IsCorrect,
                Question = question,
                QuestionId = question.Id
            };

            return choice;
        }

        public static Try CreateTry(TryRequest request, Quizz quiz, User user)
        {
            var tryEntry = new Try
            {
                Result = request.Result,
                Quizz = quiz,
                QuizzId = quiz.Id,
                User = user,
                UserId = user.Id
            };

            return tryEntry;
        }
    }
}
