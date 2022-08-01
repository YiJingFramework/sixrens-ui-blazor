using IndexedDB.Blazor;
using Microsoft.JSInterop;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
        private class DbCodeFirst : IndexedDb
        {
#nullable disable
            public DbCodeFirst(IJSRuntime jSRuntime, string name, int version)
                : base(jSRuntime, name, version) { }
#nullable restore

            public sealed class StringItem
            {
#nullable disable
                public StringItem() { }
#nullable restore
                [System.ComponentModel.DataAnnotations.Key]
                public string Value { get; set; }
            }
            public IndexedSet<StringItem> UsedVersions { get; set; }
        }
    }
}
