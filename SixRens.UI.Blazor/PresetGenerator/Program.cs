using SixRens.Core.插件管理.预设管理;
using System.Diagnostics;

var presetManager = new 预设管理器(new Saver());

foreach (var property in typeof(PresetGenerator.Presets).GetProperties())
{
    var preset = presetManager.新增预设(property.Name);
    Debug.Assert(preset is not null);
    var setter = property.GetValue(null) as PresetGenerator.Presets.PresetSetter;
    Debug.Assert(setter is not null);
    setter(preset);
}

internal class Saver : I预设管理器储存器
{
    private HashSet<string> printed = new HashSet<string>();

    public void 储存预设文件(string 预设名, string 内容)
    {
        var file = new FileInfo($"{预设名}.bin");
        File.WriteAllText(file.FullName, 内容);
        if (this.printed.Add(预设名))
            Console.WriteLine(file.FullName);
    }

    public bool 新建预设文件(string 预设名) { return true; }

    public void 移除预设文件(string 预设名) { }

    public IEnumerable<(string 预设名, string 内容)> 获取所有预设文件() { yield break; }
}