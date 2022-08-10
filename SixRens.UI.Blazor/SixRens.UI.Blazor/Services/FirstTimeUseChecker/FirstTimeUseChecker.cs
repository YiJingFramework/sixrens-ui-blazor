using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
#pragma warning disable IDE1006 // 命名样式
        private sealed record Item(string usedVersion);
#pragma warning restore IDE1006 // 命名样式

        public async Task<bool> HasUsed(string version)
        {
            await this.EnsureStore();
            var query = new StoreIndexQuery<string> {
                Storename = Names.IndexedDb.FirstTimeUse,
                IndexName = nameof(Item.usedVersion),
                QueryValue = version
            };
            var result = await this.dbManager.GetRecordByIndex<string, Item>(query);
            return result is not null;
        }

        public async Task SetUsed(string version)
        {
            await this.EnsureStore();
            var record = new StoreRecord<Item> {
                Storename = Names.IndexedDb.FirstTimeUse,
                Data = new Item(version)
            };
            await this.dbManager.AddRecord(record);
        }

        private readonly IndexedDBManager dbManager;
        public FirstTimeUseChecker(IndexedDBManager dbManager)
        {
            this.dbManager = dbManager;
            this.storeEnsured = false;
        }

        private bool storeEnsured;
        private async Task EnsureStore()
        {
            if (this.storeEnsured)
                return;
            this.storeEnsured = true;
            var storeSchema = new StoreSchema {
                Name = Names.IndexedDb.FirstTimeUse,
                PrimaryKey = new() {
                    Name = nameof(Item.usedVersion),
                    KeyPath = nameof(Item.usedVersion),
                    Unique = true,
                    Auto = false
                }
            };
            await this.dbManager.AddNewStore(storeSchema);
        }
    }
}
