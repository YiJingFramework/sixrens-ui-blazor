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
            var query = new StoreIndexQuery<string> {
                Storename = Names.IndexedDb.FirstTimeUse,
                IndexName = nameof(Item.usedVersion),
                QueryValue = version
            };
            var result = await this.dbManager.GetRecords<Item>(Names.IndexedDb.FirstTimeUse);
            return result.Any(item => item.usedVersion == version);
        }

        public async Task SetUsed(string version)
        {
            var record = new StoreRecord<Item> {
                Storename = Names.IndexedDb.FirstTimeUse,
                Data = new Item(version)
            };
            await this.dbManager.AddRecord(record);
        }

        private readonly IndexedDBManager dbManager;
        private readonly ILogger<FirstTimeUseChecker> logger;
        public FirstTimeUseChecker(IndexedDBManager dbManager, ILogger<FirstTimeUseChecker> logger)
        {
            this.dbManager = dbManager;
            this.logger = logger;
        }

        public static StoreSchema IndexedDbStoreSchema
            => new StoreSchema() {
                Name = Names.IndexedDb.FirstTimeUse,
                PrimaryKey = new() {
                    Name = nameof(Item.usedVersion),
                    KeyPath = nameof(Item.usedVersion),
                    Unique = true,
                    Auto = false
                }
            };
    }
}
