using SixRens.Core.插件管理.插件包管理;
using SixRens.UI.Blazor.Extensions;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private sealed class PluginPackageSaver : I插件包管理器储存器
        {
#pragma warning disable IDE1006 // 命名样式
            private sealed record Item(string name, byte[] content);
#pragma warning restore IDE1006 // 命名样式

            private readonly IndexedDBManager dbManager;
            private PluginPackageSaver(IndexedDBManager dbManager)
            {
                this.dbManager = dbManager;
            }

            public static StoreSchema IndexedDbStoreSchema
                => new StoreSchema {
                    Name = Names.IndexedDb.SixRensPlugins,
                    PrimaryKey = new() {
                        Name = nameof(Item.name),
                        KeyPath = nameof(Item.name),
                        Unique = true,
                        Auto = false
                    }
                };

            public static async Task<PluginPackageSaver> Create(IndexedDBManager dbManager)
            {
                return new(dbManager);
            }

            public async ValueTask<string> 储存插件包文件(Stream 插件包)
            {
                for (; ; )
                {
                    var randName = Path.GetRandomFileName();
                    randName = Path.GetFileNameWithoutExtension(randName);

                    var contains = await this.dbManager.GetRecordById<string, Item>(
                        Names.IndexedDb.SixRensPlugins, randName);
                    if (contains is null)
                    {
                        await this.dbManager.AddRecord(new StoreRecord<Item>() {
                            Storename = Names.IndexedDb.SixRensPlugins,
                            Data = new(randName, 插件包.ReadAsBytes())
                        });
                        return randName;
                    }
                }
            }

            public async ValueTask 移除插件包文件(string 插件包文件名)
            {
                await this.dbManager.DeleteRecord(Names.IndexedDb.SixRensPlugins, 插件包文件名);
            }

            public async ValueTask<IEnumerable<(string 插件包本地识别码, Stream 插件包)>> 获取所有插件包文件()
            {
                var packages = await this.dbManager.GetRecords<Item>(Names.IndexedDb.SixRensPlugins);
                return packages.Select(item => (item.name, (Stream)new MemoryStream(item.content)));
            }

            public async ValueTask<Stream?> 获取插件包文件(string 插件包文件名)
            {
                var result = await this.dbManager.GetRecordById<string, Item>(
                    Names.IndexedDb.SixRensPlugins, 插件包文件名);
                if (result is null)
                    return null;
                return new MemoryStream(result.content);
            }
        }
    }
}
