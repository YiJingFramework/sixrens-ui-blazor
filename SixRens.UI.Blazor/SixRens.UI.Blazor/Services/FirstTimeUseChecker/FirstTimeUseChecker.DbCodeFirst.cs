using IndexedDB.Blazor;
using Microsoft.JSInterop;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
        private sealed class DbCodeFirst : IndexedDb
        {
            public sealed class Item
            {
                [Key]
                [JsonPropertyName(nameof(VersionName))]
                public string? VersionName { get; set; }
            }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            public DbCodeFirst(IJSRuntime jSRuntime, string name, int version)
                : base(jSRuntime, name, version) { }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

            public IndexedSet<Item> UsedVersions { get; set; }
        }
    }
}
