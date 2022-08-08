using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
        private sealed record Item(string UsedVersion);

        public async Task<bool> HasUsed(string version)
        {
            await EnsureStore();
            var query = new StoreIndexQuery<string> {
                Storename = Names.IndexedDb.FirstTimeUse,
                IndexName = nameof(Item.UsedVersion),
                QueryValue = version
            };
            var result = await dbManager.GetRecordByIndex<string, Item>(query);
            return result is not null;
        }

        public async Task SetUsed(string version)
        {
            await EnsureStore();
            var record = new StoreRecord<Item> {
                Storename = Names.IndexedDb.FirstTimeUse,
                Data = new Item(version)
            };
            await dbManager.AddRecord(record);
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
            if (storeEnsured)
                return;
            storeEnsured = true;
            var storeSchema = new StoreSchema {
                Name = Names.IndexedDb.FirstTimeUse,
                PrimaryKey = new() {
                    Name = nameof(Item.UsedVersion),
                    KeyPath = nameof(Item.UsedVersion),
                    Unique = true,
                    Auto = false
                }
            };
            await dbManager.AddNewStore(storeSchema);
        }
    }
}
