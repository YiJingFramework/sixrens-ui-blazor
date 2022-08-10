using BlazorIndexedDbJs;
using Microsoft.JSInterop;
using SixRens.Core.占例存取;
using System.Diagnostics;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
        public sealed class DbSchema : IDBDatabase
        {
            public Cases CasesStore { get; }

            public DbSchema(IJSRuntime jsRuntime) : base(jsRuntime)
            {
                Name = Names.IndexedDb.DivinationCases;
                Version = 1;
                CasesStore = new Cases(this);
            }

            public sealed class Cases : IDBObjectStore
            {
#pragma warning disable IDE1006 // 命名样式
                public sealed record CaseItem(long? id, string name, string group, string content);
#pragma warning restore IDE1006 // 命名样式

                public Cases(IDBDatabase database) : base(database)
                {
                    Name = nameof(Cases);
                    KeyPath = nameof(CaseItem.id);
                    AutoIncrement = true;
                }
            }
        }
    }
}
