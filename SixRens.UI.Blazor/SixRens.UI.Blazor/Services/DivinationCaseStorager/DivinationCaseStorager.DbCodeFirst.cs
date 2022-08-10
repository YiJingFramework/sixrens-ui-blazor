using IndexedDB.Blazor;
using Microsoft.JSInterop;
using SixRens.Core.占例存取;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.Json.Serialization;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
        private sealed class DbCodeFirst : IndexedDb
        {
            public sealed class Item
            {
                [Key]
                [JsonPropertyName(nameof(Id))]
                public long? Id { get; set; }

                [JsonPropertyName(nameof(Name))]
                public string? Name { get; set; }

                [JsonPropertyName(nameof(Group))]
                public string? Group { get; set; }

                [JsonPropertyName(nameof(Content))]
                public string? Content { get; set; }
            }

#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。
            public DbCodeFirst(IJSRuntime jSRuntime, string name, int version)
                : base(jSRuntime, name, version) { }
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑声明为可以为 null。

            public IndexedSet<Item> Cases { get; set; }
        }
    }
}
