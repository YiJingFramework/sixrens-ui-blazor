using IndexedDB.Blazor;
using SixRens.Core.占例存取;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
        public async Task<bool> AddCase(string name, 占例 divinationCase)
        {
            using var db = await this.GetDb();
            if (db.DivinationCases.Any(item => item.Name == name))
                return false;
            var item = new DbCodeFirst.CaseItem() {
                Name = name,
                Content = divinationCase.序列化()
            };
            db.DivinationCases.Add(item);
            await db.SaveChanges();
            return true;
        }

        public async Task UpdateCase(string name, 占例 divinationCase)
        {
            using var db = await this.GetDb();
            var item = new DbCodeFirst.CaseItem() {
                Name = name,
                Content = divinationCase.序列化()
            };
            _ = db.DivinationCases.Remove(item);
            db.DivinationCases.Add(item);
            await db.SaveChanges();
        }

        public async Task<占例?> GetCase(string name)
        {
            using var db = await this.GetDb();
            var item = db.DivinationCases
                .Where(item => item.Name == name)
                .SingleOrDefault((DbCodeFirst.CaseItem?)null);
            if (item is null)
                return null;
            return 占例.反序列化(item.Content);
        }

        public async Task<IEnumerable<string>> ListCases()
        {
            using var db = await this.GetDb();
            return db.DivinationCases.Select(item => item.Name);
        }

        public async Task RemoveCase(string name)
        {
            using var db = await this.GetDb();
            var item = new DbCodeFirst.CaseItem() {
                Name = name,
                Content = null! // 删除只需要主键
            };
            _ = db.DivinationCases.Remove(item);
            await db.SaveChanges();
        }

        private async Task<DbCodeFirst> GetDb()
        {
            return await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.DivinationCases);
        }

        private readonly IIndexedDbFactory dbFactory;
        public DivinationCaseStorager(IIndexedDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }
    }
}
