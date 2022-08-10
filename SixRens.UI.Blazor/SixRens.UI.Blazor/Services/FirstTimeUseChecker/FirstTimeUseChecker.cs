using IndexedDB.Blazor;

namespace SixRens.UI.Blazor.Services.FirstTimeUseChecker
{
    public sealed partial class FirstTimeUseChecker
    {
        private readonly IIndexedDbFactory dbFactory;
        public FirstTimeUseChecker(IIndexedDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task<bool> HasUsed(string version)
        {
            using var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.FirstTimeUse);
            return db.UsedVersions.Any(item => item.VersionName == version);
        }

        public async Task SetUsed(string version)
        {
            using var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.FirstTimeUse);
            db.UsedVersions.Add(new DbCodeFirst.Item() { VersionName = version });
            await db.SaveChanges();
        }
    }
}
