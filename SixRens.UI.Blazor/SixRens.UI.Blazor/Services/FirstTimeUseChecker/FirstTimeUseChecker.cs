using IndexedDB.Blazor;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
        public async Task<bool> HasUsed(string version)
        {
            var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.FirstTimeUse);
            return db.UsedVersions.Any(item => item.Value == version);
        }

        public async Task SetUsed(string version)
        {
            var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.FirstTimeUse);
            db.UsedVersions.Add(new() { Value = version });
        }

        private readonly IIndexedDbFactory dbFactory;

        public FirstTimeUseChecker(IIndexedDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
    }
}
