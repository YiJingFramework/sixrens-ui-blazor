using SixRens.Core.占例存取;
using System.Diagnostics;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
        private sealed record Item(long? Id, string Name, string Content);

        public async Task AddCase(string name, 占例 dCase)
        {
            await EnsureStore();
            await dbManager.AddRecord(new StoreRecord<Item>() {
                Storename = Names.IndexedDb.DivinationCases,
                Data = new(null, name, dCase.序列化())
            });
        }

        public async Task UpdateCase(long id, string name, 占例 dCase)
        {
            await EnsureStore();
            await dbManager.UpdateRecord(new StoreRecord<Item>() {
                Storename = Names.IndexedDb.DivinationCases,
                Data = new(id, name, dCase.序列化()),
            });
        }

        public async Task<(string name, 占例 dCase)> GetCase(long id)
        {
            await EnsureStore();
            var result = await dbManager.GetRecordById<long, Item>(Names.IndexedDb.DivinationCases, id);
            return (result.Name, 占例.反序列化(result.Content));
        }

        public async Task<IEnumerable<(long id, string name)>> ListCases()
        {
            IEnumerable<(long id, string name)> ToReturnType(List<Item> items)
            {
                foreach(var item in items)
                {
                    Debug.Assert(item.Id.HasValue);
                    yield return (item.Id.Value, item.Name);
                }
            }
            await EnsureStore();
            var result = await dbManager.GetRecords<Item>(Names.IndexedDb.DivinationCases);
            return ToReturnType(result);
        }

        public async Task RemoveCase(long id)
        {
            await EnsureStore();
            await dbManager.DeleteRecord(Names.IndexedDb.DivinationCases, id);
        }

        private readonly IndexedDBManager dbManager;
        public DivinationCaseStorager(IndexedDBManager dbManager)
        {
            this.dbManager = dbManager;
            this.storeEnsured = false;
        }

        private bool storeEnsured;
        private async Task EnsureStore()
        {
            if (storeEnsured)
                return;
            storeEnsured = true;
            var storeSchema = new StoreSchema {
                Name = Names.IndexedDb.DivinationCases,
                PrimaryKey = new() {
                    Name = nameof(Item.Id),
                    KeyPath = nameof(Item.Id),
                    Unique = true,
                    Auto = true
                },
                Indexes = new() {
                    new() {
                        Name = nameof(Item.Name),
                        KeyPath = nameof(Item.Name),
                        Unique = false,
                        Auto = false
                    }
                }
            };
            await dbManager.AddNewStore(storeSchema);
        }
    }
}
