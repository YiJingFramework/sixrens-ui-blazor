using IndexedDB.Blazor;
using SixRens.Core.插件管理.插件包管理;
using SixRens.Core.插件管理.预设管理;

namespace SixRens.UI.Blazor.Services.SixRens
{
    public sealed partial class ServiceOfSixRens
    {
        private 插件包管理器? pluginPackageManager;
        public async Task<插件包管理器> GetPluginPackageManager()
        {
            if (this.pluginPackageManager is null)
            {
                var saver = await GetDataSaver();
                this.pluginPackageManager = new(saver);
            }
            return this.pluginPackageManager;
        }

        private 预设管理器? presetManager;
        public async Task<预设管理器> GetPresetManager()
        {
            if (this.presetManager is null)
            {
                var saver = await GetDataSaver();
                this.presetManager = new(saver);
            }
            return this.presetManager;
        }


        private DataSaver? dataSaver;
        private async Task<DataSaver> GetDataSaver()
        {
            if(this.dataSaver is null)
                dataSaver = await DataSaver.CreateDataSaver(this.dbFactory, logger);
            return dataSaver;
        }

        private readonly IIndexedDbFactory dbFactory;
        private readonly ILogger logger;

        public ServiceOfSixRens(IIndexedDbFactory dbFactory, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ServiceOfSixRens>();
            this.dbFactory = dbFactory;
        }
    }
}
