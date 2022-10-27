using SixRens.Core.插件管理.预设管理;

namespace PresetGenerator
{
    public static class Presets
    {
        public delegate void PresetSetter(预设 preset);

        public static PresetSetter DefaultPreset { get; } = (preset) => {
            preset.三传插件 = new Guid("620CC386-F3C3-4658-ADED-F1A9E4903B9F");
            preset.天将插件 = new Guid("006CD940-0597-4E02-A707-0D54D4216C1A");
            preset.神煞插件.Add(new Guid("59375F38-6BE3-46AC-8C46-F4BBF0C03382"));
            preset.课体插件.Add(new Guid("ABDFB6DD-90B9-4B4A-A8A4-4D60996C948D"));
            preset.参考插件.Add(new Guid("76C9F1AA-6E3E-40CB-90C6-A3B146529043"));
        };
    }
}
