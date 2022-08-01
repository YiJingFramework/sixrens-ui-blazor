using SixRens.Core.插件管理.预设管理;

namespace PresetGenerator
{
    public static class Presets
    {
        public delegate void PresetSetter(预设 preset);

        public static PresetSetter DefaultPreset { get; } = (preset) => {
            preset.地盘插件 = new Guid("A9B377C9-8A25-4476-95C9-7BA90D075A5E");
            preset.天盘插件 = new Guid("7D33B8B0-5CC2-4E78-AB44-077C69908249");
            preset.四课插件 = new Guid("6BAD32BC-0951-417D-B01B-53623FA98D8D");
            preset.三传插件 = new Guid("620CC386-F3C3-4658-ADED-F1A9E4903B9F");
            preset.天将插件 = new Guid("006CD940-0597-4E02-A707-0D54D4216C1A");
            preset.年命插件 = new Guid("56892793-4951-495B-98F0-F9683B3F2AF5");
            preset.神煞插件.Add(new Guid("59375F38-6BE3-46AC-8C46-F4BBF0C03382"));
            preset.课体插件.Add(new Guid("ABDFB6DD-90B9-4B4A-A8A4-4D60996C948D"));
        };
    }
}
