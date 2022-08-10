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

            private readonly Dictionary<string, string> items;
            private readonly IndexedDBManager dbManager;
            private PresetSaver(IndexedDBManager dbManager, Dictionary<string, string> items)
            {
                this.dbManager = dbManager;
                this.items = items;
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
                var records = await dbManager.GetRecords<Item>(Names.IndexedDb.SixRensPlugins);
                return new(dbManager, records.ToDictionary(item => item.name, item => item.content));
            }

            public IEnumerable<(string 预设名, string 内容)> 获取所有预设文件()
            {
                foreach (var item in this.items)
                    yield return (item.Key, item.Value);
            }

            public bool 新建预设文件(string 预设名)
            {
                return this.items.TryAdd(预设名, string.Empty);
            }

            public void 储存预设文件(string 预设名, string 内容)
            {
                this.items[预设名] = 内容;
                _ = this.dbManager.AddRecord(new StoreRecord<Item>() {
                    Storename = Names.IndexedDb.SixRensPresets,
                    Data = new(预设名, 内容)
                }).ContinueWith(task => {
                    if (task.IsFaulted)
                    {
#warning 怎么处理？
                    }
                });
            }

            public void 移除预设文件(string 预设名)
            {
                _ = this.items.Remove(预设名);
                _ = this.dbManager.DeleteRecord(Names.IndexedDb.SixRensPlugins, 预设名)
                    .ContinueWith(task => {
                        if (task.IsFaulted)
                        {
#warning 怎么处理？
                        }
                    });
            }
        }
    }
}
