using SixRens.Core.占例存取;
using System.Diagnostics;
using static SixRens.UI.Blazor.Services.DivinationCaseStorager.DivinationCaseStorager.DbSchema.Cases;

namespace SixRens.UI.Blazor.Services.DivinationCaseStorager
{
    public sealed partial class DivinationCaseStorager
    {
        private readonly DbSchema db;
        public DivinationCaseStorager(DbSchema db)
        {
            this.db = db;
        }
        public static void InjectAsService(IServiceCollection services)
        {
            _ = services.AddScoped<DbSchema>();
            _ = services.AddScoped<DivinationCaseStorager>();
        }

        public async Task AddCase(string name, string group, 占例 dCase)
        {
            await db.Open();
            await db.CasesStore.Add(new CaseItem(null, name, group, dCase.序列化()));
        }

        public async Task UpdateCase(long id, string name, string group, 占例 dCase)
        {
            await db.Open();
            await db.CasesStore.Put(new CaseItem(id, name, group, dCase.序列化()));
        }

        public async Task<(string name, string group, 占例 dCase)?> GetCase(long id)
        {
            await db.Open();
            var result = await db.CasesStore.Get<long, CaseItem>(id);
            if (result is null)
                return null;
            return (result.name, result.group, 占例.反序列化(result.content));
        }

        public async Task<IEnumerable<(long id, string name, string group)>> ListCases()
        {
            IEnumerable<(long id, string name, string group)> ToReturnType(
                IEnumerable<CaseItem> items)
            {
                foreach (var item in items)
                {
                    Debug.Assert(item.id.HasValue);
                    yield return (item.id.Value, item.name, item.group);
                }
            }
            await db.Open();
            var result = await db.CasesStore.GetAll<CaseItem>();
            return ToReturnType(result);
        }

        public async Task RemoveCase(long id)
        {
            await db.Open();
            await db.CasesStore.Delete(id);
        }
    }
}
