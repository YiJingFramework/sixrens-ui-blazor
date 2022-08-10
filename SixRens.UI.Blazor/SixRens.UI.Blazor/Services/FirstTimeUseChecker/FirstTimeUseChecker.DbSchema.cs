using BlazorIndexedDbJs;
using Microsoft.JSInterop;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
        public sealed class DbSchema : IDBDatabase
        {
            public Records RecordsStore { get; }

            public DbSchema(IJSRuntime jsRuntime) : base(jsRuntime)
            {
                Name = Names.IndexedDb.FirstTimeUse;
                Version = 1;
                RecordsStore = new Records(this);
            }

            public sealed class Records : IDBObjectStore
            {
#pragma warning disable IDE1006 // 命名样式
                public sealed record RecordItem(string used_version);
#pragma warning restore IDE1006 // 命名样式
                public Records(IDBDatabase database) : base(database)
                {
                    Name = nameof(Records);
                    KeyPath = nameof(RecordItem.used_version);
                    AutoIncrement = false;
                }
            }
        }
    }
}
