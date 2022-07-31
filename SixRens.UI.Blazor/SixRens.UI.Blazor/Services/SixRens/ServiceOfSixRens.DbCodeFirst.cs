using IndexedDB.Blazor;
using Microsoft.JSInterop;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private sealed class DbCodeFirst : IndexedDb
        {
#nullable disable
            public DbCodeFirst(IJSRuntime jSRuntime, string name, int version)
                : base(jSRuntime, name, version) { }
#nullable restore
            public sealed class NamedDbItem<T>
            {
#nullable disable
                public NamedDbItem() { }
#nullable restore
                [System.ComponentModel.DataAnnotations.Key]
                public string Name { get; set; }
                public T Value { get; set; }
            }
            public IndexedSet<NamedDbItem<string>> Presets { get; set; }
            public IndexedSet<NamedDbItem<byte[]>> PluginPackages { get; set; }
        }
    }
}
