using IndexedDB.Blazor;
using Microsoft.JSInterop;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
        private class DbCodeFirst : IndexedDb
        {
#nullable disable
            public DbCodeFirst(IJSRuntime jSRuntime, string name, int version)
                : base(jSRuntime, name, version) { }
#nullable restore

            public sealed class CaseItem
            {
#nullable disable
                public CaseItem() { }
#nullable restore
                [System.ComponentModel.DataAnnotations.Key]
                public string Name { get; set; }

                public string Content { get; set; }
            }
            public IndexedSet<CaseItem> DivinationCases { get; set; }
        }
    }
}
