using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.Blazor.Extensions;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private sealed class PresetSaver : I预设管理器储存器
        {
            private sealed record Item(string Name, string Content);

            private readonly Dictionary<string, string> items;
            private readonly IndexedDBManager dbManager;
            private PresetSaver(IndexedDBManager dbManager, Dictionary<string, string> items)
            {
                this.dbManager = dbManager;
                this.items = items;
            }
            public static async Task<PresetSaver> Create(IndexedDBManager dbManager)
            {
                var storeSchema = new StoreSchema {
                    Name = Names.IndexedDb.SixRensPresets,
                    PrimaryKey = new() {
                        Name = nameof(Item.Name),
                        KeyPath = nameof(Item.Name),
                        Unique = true,
                        Auto = false
                    }
                };
                await dbManager.AddNewStore(storeSchema);
                var records = await dbManager.GetRecords<Item>(Names.IndexedDb.SixRensPlugins);
                return new(dbManager, records.ToDictionary(item => item.Name, item => item.Content));
            }

            public IEnumerable<(string 预设名, string 内容)> 获取所有预设文件()
            {
                foreach (var item in items)
                    yield return (item.Key, item.Value);
            }

            public bool 新建预设文件(string 预设名)
            {
                return items.TryAdd(预设名, string.Empty);
            }

            public void 储存预设文件(string 预设名, string 内容)
            {
                items[预设名] = 内容;
                _ = dbManager.AddRecord(new StoreRecord<Item>() {
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
                _ = items.Remove(预设名);
                _ = dbManager.DeleteRecord(Names.IndexedDb.SixRensPlugins, 预设名)
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
