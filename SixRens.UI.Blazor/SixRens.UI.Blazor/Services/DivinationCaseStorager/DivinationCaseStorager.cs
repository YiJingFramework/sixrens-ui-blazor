using IndexedDB.Blazor;
using SixRens.Core.占例存取;
using System.Diagnostics;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
        private readonly IIndexedDbFactory dbFactory;
        public DivinationCaseStorager(IIndexedDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public async Task AddCase(string name, string group, 占例 dCase)
        {
            using var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.DivinationCases);
            db.Cases.Add(new() {
                Id = null,
                Name = name,
                Group = group,
                Content = dCase.序列化()
            });
            await db.SaveChanges();
        }

        public async Task UpdateCase(long id, string name, string group, 占例 dCase)
        {
            using var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.DivinationCases);
            var item = db.Cases.Single(item => item.Id == id);
            item.Name = name;
            item.Group = group;
            item.Content = dCase.序列化();
            await db.SaveChanges();
        }

        public async Task<(string name, string group, 占例 dCase)> GetCase(long id)
        {
            using var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.DivinationCases);
            var item = db.Cases.Single(item => item.Id == id);
            Debug.Assert(item.Name is not null);
            Debug.Assert(item.Group is not null);
            Debug.Assert(item.Content is not null);
            return (item.Name, item.Group, 占例.反序列化(item.Content));
        }

        public async Task<IEnumerable<(long id, string name, string group)>> ListCases()
        {
            IEnumerable<(long id, string name, string group)> ToReturnType(
                IEnumerable<DbCodeFirst.Item> items)
            {
                foreach (var item in items)
                {
                    Debug.Assert(item.Id.HasValue);
                    Debug.Assert(item.Name is not null);
                    Debug.Assert(item.Group is not null);
                    yield return (item.Id.Value, item.Name, item.Group);
                }
            }
            using var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.DivinationCases);
            return ToReturnType(db.Cases);
        }

        public async Task RemoveCase(long id)
        {
            using var db = await dbFactory.Create<DbCodeFirst>(Names.IndexedDb.DivinationCases);
            _ = db.Cases.Remove(new DbCodeFirst.Item() {
                Id = id
            });
            await db.SaveChanges();
        }
    }
}
