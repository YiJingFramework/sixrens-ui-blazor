using IndexedDB.Blazor;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private sealed class DataSaver : I插件包管理器储存器, I预设管理器储存器
        {
            private readonly DbCodeFirst db;
            private ILogger logger;

            public static async Task<DataSaver> CreateDataSaver(IIndexedDbFactory dbFactory, ILogger logger)
            {
                return new(await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.SixRensPluginsAndPresets), logger);
            }

            private DataSaver(DbCodeFirst db, ILogger logger)
            {
                this.db = db;
                this.logger = logger;
            }

            public void 储存插件包文件(string 插件包文件名, Stream 插件包)
            {
                using var memory = new MemoryStream();
                插件包.CopyTo(memory);
                this.db.PluginPackages.Add(new() { Name = 插件包文件名, Value = memory.ToArray() });
                _ = this.db.SaveChanges();
            }

            public void 储存预设文件(string 预设名, string 内容)
            {
                logger.LogWarning($"{预设名}{内容}");
                var item = new DbCodeFirst.NamedDbItem<string>() {
                    Name = 预设名,
                    Value = 内容
                };
                this.db.Presets.Remove(item);
                this.db.Presets.Add(item);
                _ = this.db.SaveChanges();
            }

            public bool 新建预设文件(string 预设名)
            {
                if (this.db.Presets.Any(x => x.Name == 预设名))
                    return false;
                this.db.Presets.Add(new() { Name = 预设名, Value = string.Empty });
                _ = this.db.SaveChanges();
                return true;
            }

            public string 生成新的插件包文件名()
            {
                for (; ; )
                {
                    var randomName = Path.GetRandomFileName();
                    randomName = Path.GetFileNameWithoutExtension(randomName);
                    if (!this.db.PluginPackages.Any(x => x.Name == randomName))
                        return randomName;
                }
            }

            public void 移除插件包文件(string 插件包文件名)
            {
                _ = this.db.PluginPackages.Remove(new() {
                    Name = 插件包文件名,
                    Value = null! // 移除操作只需要提供键
                });
                _ = this.db.SaveChanges();
            }

            public void 移除预设文件(string 预设名)
            {
                _ = this.db.Presets.Remove(new() {
                    Name = 预设名,
                    Value = null! // 移除操作只需要提供键
                });
                _ = this.db.SaveChanges();
            }

            public IEnumerable<(string 插件包文件名, Stream 插件包)> 获取所有插件包文件()
            {
                return this.db.PluginPackages
                    .Select(item => (item.Name, (Stream)new MemoryStream(item.Value)));
            }

            public IEnumerable<(string 预设名, string 内容)> 获取所有预设文件()
            {
                return this.db.Presets
                    .Select(item => (item.Name, item.Value));
            }

            public Stream? 获取插件包文件(string 插件包文件名)
            {
                var item = this.db.PluginPackages
                    .Where(item => item.Name == 插件包文件名)
                    .SingleOrDefault(defaultValue: null);
                if (item is null)
                    return null;
                return new MemoryStream(item.Value);
            }
        }
    }
}
