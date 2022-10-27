using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private sealed class PresetSaver : I预设管理器储存器
        {
#pragma warning disable IDE1006 // 命名样式
            private sealed record Item(string name, string content);
#pragma warning restore IDE1006 // 命名样式

            private readonly IndexedDBManager dbManager;
            private PresetSaver(IndexedDBManager dbManager)
            {
                this.dbManager = dbManager;
            }
            public static StoreSchema IndexedDbStoreSchema
                => new StoreSchema {
                    Name = Names.IndexedDb.SixRensPresets,
                    PrimaryKey = new() {
                        Name = nameof(Item.name),
                        KeyPath = nameof(Item.name),
                        Unique = true,
                        Auto = false
                    }
                };
            public static async Task<PresetSaver> Create(IndexedDBManager dbManager)
            {
                return new(dbManager);
            }

            public async ValueTask<IEnumerable<(string 预设名, string 内容)>> 获取所有预设文件()
            {
                var records = await dbManager.GetRecords<Item>(Names.IndexedDb.SixRensPresets);
                return records.Select(item => (item.name, item.content));
            }

            public async ValueTask<bool> 新建预设文件(string 预设名)
            {
                var contains = await this.dbManager.GetRecordById<string, Item>(
                    Names.IndexedDb.SixRensPresets, 预设名);
                return contains is null;
            }

            public async ValueTask 储存预设文件(string 预设名, string 内容)
            {
                await this.dbManager.UpdateRecord(new StoreRecord<Item>() {
                    Storename = Names.IndexedDb.SixRensPresets,
                    Data = new(预设名, 内容)
                });
            }

            public async ValueTask 移除预设文件(string 预设名)
            {
                await this.dbManager.DeleteRecord(Names.IndexedDb.SixRensPresets, 预设名);
            }
        }
    }
}
