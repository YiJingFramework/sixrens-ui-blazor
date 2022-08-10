using static SixRens.UI.Blazor.Services.FirstTimeUseChecker.FirstTimeUseChecker.DbSchema.Records;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
        private readonly DbSchema db;
        public FirstTimeUseChecker(DbSchema db)
        {
            this.db = db;
        }

        public static void InjectAsService(IServiceCollection services)
        {
            _ = services.AddScoped<DbSchema>();
            _ = services.AddScoped<FirstTimeUseChecker>();
        }

        public async Task<bool> HasUsed(string version)
        {
            await db.Open();
            var result = await db.RecordsStore.Get<string, RecordItem>(version);
            return result is not null;
        }

        public async Task SetUsed(string version)
        {
            await db.Open();
            await db.RecordsStore.Add(new RecordItem(version));
        }
    }
}
