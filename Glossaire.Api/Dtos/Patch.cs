
namespace TechQuiz.Api.Dtos
{
    public record QuizzPatch(
        string? Name,
        string? Description
    );
    public record QuestionPatch(
        string? Name,
        string? Description,
        int? Point,
        string? Type
    );
    public record ChoicePatch(
        string? Name,
        bool? IsCorrect
    );
}