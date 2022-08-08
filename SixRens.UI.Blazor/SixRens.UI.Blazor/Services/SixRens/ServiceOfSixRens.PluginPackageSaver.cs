using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using SixRens.UI.Blazor.Extensions;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private sealed class PluginPackageSaver : I插件包管理器储存器
        {
            private sealed record Item(string name, byte[] content);

            private readonly Dictionary<string, byte[]> items;
            private readonly IndexedDBManager dbManager;
            private PluginPackageSaver(IndexedDBManager dbManager, Dictionary<string, byte[]> items)
            {
                this.dbManager = dbManager;
                this.items = items;
            }
            public static async Task<PluginPackageSaver> Create(IndexedDBManager dbManager)
            {
                var storeSchema = new StoreSchema {
                    Name = Names.IndexedDb.SixRensPlugins,
                    PrimaryKey = new() {
                        Name = nameof(Item.name),
                        KeyPath = nameof(Item.name),
                        Unique = true,
                        Auto = false
                    }
                };
                await dbManager.AddNewStore(storeSchema);
                var records = await dbManager.GetRecords<Item>(Names.IndexedDb.SixRensPlugins);
                return new(dbManager, records.ToDictionary(item => item.name, item => item.content));
            }

            public string 储存插件包文件(Stream 插件包)
            {
                var bytes = 插件包.ReadAsBytes();
                for (; ; )
                {
                    var randName = Path.GetRandomFileName();
                    randName = Path.GetFileNameWithoutExtension(randName);
                    if (items.TryAdd(randName, bytes))
                    {
                        _ = dbManager.AddRecord(new StoreRecord<Item>() {
                            Storename = Names.IndexedDb.SixRensPlugins,
                            Data = new(randName, 插件包.ReadAsBytes())
                        }).ContinueWith(task => {
                            if (task.IsFaulted)
                            {
#warning 怎么处理？
                            }
                        });
                    }
                }
            }

            public void 移除插件包文件(string 插件包文件名)
            {
                _ = items.Remove(插件包文件名);
                _ = dbManager.DeleteRecord(Names.IndexedDb.SixRensPlugins, 插件包文件名)
                    .ContinueWith(task => {
                        if (task.IsFaulted)
                        {
#warning 怎么处理？
                        }
                    });
            }

            public IEnumerable<(string 插件包本地识别码, Stream 插件包)> 获取所有插件包文件()
            {
                foreach(var item in items)
                    yield return (item.Key, new MemoryStream(item.Value));
            }

            public Stream? 获取插件包文件(string 插件包文件名)
            {
                if (items.TryGetValue(插件包文件名, out var value))
                    return new MemoryStream(value);
                return null;
            }
        }
    }
}
