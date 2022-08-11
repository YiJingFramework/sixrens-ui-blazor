using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using System.Collections.Generic;
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
        public async Task<插件包管理器> GetPluginPackageManager()
        {
            if (this.pluginPackageManager is null)
            {
                var saver = await PluginPackageSaver.Create(this.dBManager);
                this.pluginPackageManager = new(saver);
            }
            return this.pluginPackageManager;
        }

        private 预设管理器? presetManager;
        public async Task<预设管理器> GetPresetManager()
        {
            if (this.presetManager is null)
            {
                var saver = await PresetSaver.Create(this.dBManager);
                this.presetManager = new(saver);
            }
            return this.presetManager;
        }

        public async Task<bool> InstallDefaultPlugins()
        {
            using var s = await
                httpClient.GetStreamAsync("plugin-packages/SixRens.DefaultPlugins-1.14.1.srspg");
            using MemoryStream ms = new();
            await s.CopyToAsync(ms);
            ms.Position = 0;
            var manager = await GetPluginPackageManager();
            logger.LogError("BEOFRE");
            var (_, 未加入) = manager.从外部加载插件包(ms);
            logger.LogError("AFTER");
            return !未加入;
        }

        public async Task InstallDefaultPresets()
        {
            var s = await
               httpClient.GetStringAsync("default-presets/DefaultPreset.bin");
            var manager = await GetPresetManager();
            var p = manager.导入预设文件内容("默认预设", s);
            for (; p is null;)
            {
                p = manager.导入预设文件内容($"默认预设（{Guid.NewGuid()}）", s);
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
