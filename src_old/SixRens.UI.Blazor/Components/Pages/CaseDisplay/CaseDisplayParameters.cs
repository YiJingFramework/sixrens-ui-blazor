using SixRens.Core.占例存取;

namespace SixRens.UI.Blazor.Components.Pages.CaseDisplay
{
    public sealed record CaseDisplayParameters(
        占例 Case,
        string? CaseName)
    { }
}
