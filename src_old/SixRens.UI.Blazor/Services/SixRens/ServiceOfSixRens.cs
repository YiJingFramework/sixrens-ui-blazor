using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using System.Collections.Generic;
using System.Diagnostics;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens : IDisposable
    {
        public static IEnumerable<StoreSchema> IndexedDbStoreSchemas
        {
            get
            {
                yield return PluginPackageSaver.IndexedDbStoreSchema;
                yield return PresetSaver.IndexedDbStoreSchema;
            }
        }

        private 插件包管理器? pluginPackageManager;
        public async Task<插件包管理器> GetPluginPackageManager(bool clearWhenError = false)
        {
            if (this.pluginPackageManager is null)
            {
                var saver = await PluginPackageSaver.Create(this.dBManager);
                try
                {
                    this.pluginPackageManager = await 插件包管理器.创建插件包管理器(saver);
                }
                catch
                {
                    if (!clearWhenError)
                        throw;
                    await saver.ClearPluginsPackages();
                    this.pluginPackageManager = await 插件包管理器.创建插件包管理器(saver);
                }
            }
            return this.pluginPackageManager;
        }

        private 预设管理器? presetManager;
        public async ValueTask<预设管理器> GetPresetManager()
        {
            if (this.presetManager is null)
            {
                var saver = await PresetSaver.Create(this.dBManager);
                this.presetManager = await 预设管理器.创建预设管理器(saver);
            }
            return this.presetManager;
        }

        public async ValueTask<bool> InstallDefaultPlugins()
        {
            using var s = await
                httpClient.GetStreamAsync("plugin-packages/SixRens.DefaultPlugins-1.14.1.srspg");
            using MemoryStream ms = new();
            await s.CopyToAsync(ms);
            ms.Position = 0;
            var manager = await GetPluginPackageManager();
            var (_, failed) = await manager.从外部加载插件包(ms);
            return !failed;
        }

        public async ValueTask InstallDefaultPresets()
        {
            var s = await
               httpClient.GetStringAsync("default-presets/DefaultPreset.bin");
            var manager = await GetPresetManager();
            var p = await manager.导入预设文件内容("默认预设", s);
            for (; p is null;)
            {
                p = await manager.导入预设文件内容($"默认预设（{Guid.NewGuid()}）", s);
            }
        }

        public void Dispose()
        {
            this.pluginPackageManager?.Dispose();
        }

        private readonly IndexedDBManager dBManager;
        private readonly HttpClient httpClient;
        private readonly ILogger<ServiceOfSixRens> logger;
        public ServiceOfSixRens(
            IndexedDBManager dBManager,
            HttpClient httpClient, 
            ILogger<ServiceOfSixRens> logger)
        {
            this.dBManager = dBManager;
            this.httpClient = httpClient;
            this.logger = logger;
        }
    }
}
