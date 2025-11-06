
namespace TechQuiz.Api.Dtos
{
    public record QuizzRequest(
        string Name,
        string Description,
        List<QuestionRequest>? Questions
    );

    public record QuestionRequest(
        string Name,
        string Description,
        int Point,
        string Type,
        List<ChoiceRequest>? Choices
    );

    public record ChoiceRequest(
        string Name,
        bool IsCorrect
    );

    public record TryRequest(
        int Result,
        int QuizzId
    );
}