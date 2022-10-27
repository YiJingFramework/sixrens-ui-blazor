using SixRens.Core.占例存取;
using System.Diagnostics;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
#pragma warning disable IDE1006 // 命名样式
        private sealed record Item(long? id, string name, string group, string content);
#pragma warning restore IDE1006 // 命名样式

        public async ValueTask AddCase(string name, string group, 占例 dCase)
        {
            await this.dbManager.AddRecord(new StoreRecord<Item>() {
                Storename = Names.IndexedDb.DivinationCases,
                Data = new(null, name, group, dCase.序列化())
            });
        }

        public async ValueTask UpdateCase(long id, string name, string group, 占例 dCase)
        {
            await this.dbManager.UpdateRecord(new StoreRecord<Item>() {
                Storename = Names.IndexedDb.DivinationCases,
                Data = new(id, name, group, dCase.序列化()),
            });
        }

        public async ValueTask<(string name, string group, 占例 dCase)> GetCase(long id)
        {
            var result = await this.dbManager.GetRecordById<long, Item>(Names.IndexedDb.DivinationCases, id);
            return (result.name, result.group, 占例.反序列化(result.content));
        }

        public async ValueTask<IEnumerable<(long id, string name, string group)>> ListCases()
        {
            IEnumerable<(long id, string name, string group)> ToReturnType(List<Item> items)
            {
                foreach (var item in items)
                {
                    Debug.Assert(item.id.HasValue);
                    yield return (item.id.Value, item.name, item.group);
                }
            }
            var result = await this.dbManager.GetRecords<Item>(Names.IndexedDb.DivinationCases);
            return ToReturnType(result);
        }

        public async ValueTask RemoveCase(long id)
        {
            await this.dbManager.DeleteRecord(Names.IndexedDb.DivinationCases, id);
        }

        private readonly IndexedDBManager dbManager;
        public DivinationCaseStorager(IndexedDBManager dbManager)
        {
            this.dbManager = dbManager;
        }

        public static StoreSchema IndexedDbStoreSchema
            => new StoreSchema() {
                Name = Names.IndexedDb.DivinationCases,
                PrimaryKey = new() {
                    Name = nameof(Item.id),
                    KeyPath = nameof(Item.id),
                    Unique = true,
                    Auto = true
                },
                Indexes = new() {
                    new() {
                        Name = nameof(Item.name),
                        KeyPath = nameof(Item.name),
                        Unique = false,
                        Auto = false
                    }
                }
            };
    }
}
