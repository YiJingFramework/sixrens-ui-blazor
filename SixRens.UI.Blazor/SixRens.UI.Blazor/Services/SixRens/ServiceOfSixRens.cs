using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;
using System.Collections.Generic;
using TG.Blazor.IndexedDB;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
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

        private readonly IndexedDBManager dBManager;
        public ServiceOfSixRens(IndexedDBManager dBManager)
        {
            this.dBManager = dBManager;
        }
    }
}
