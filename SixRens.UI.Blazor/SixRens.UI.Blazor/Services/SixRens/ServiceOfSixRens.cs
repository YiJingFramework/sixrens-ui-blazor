﻿using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private 插件包管理器? pluginPackageManager;
        public async Task<插件包管理器> GetPluginPackageManager()
        {
            throw new NotImplementedException("没实现呢");
            if (this.pluginPackageManager is null)
            {
                // var saver = await PluginPackageSaver.Create(this.dBManager);
                // this.pluginPackageManager = new(saver);
            }
            return this.pluginPackageManager;
        }

        private 预设管理器? presetManager;
        public async Task<预设管理器> GetPresetManager()
        {
            throw new NotImplementedException("没实现呢");
            if (this.presetManager is null)
            {
                // var saver = await PresetSaver.Create(this.dBManager);
                // this.presetManager = new(saver);
            }
            return this.presetManager;
        }

        public ServiceOfSixRens()
        {
        }
    }
}
