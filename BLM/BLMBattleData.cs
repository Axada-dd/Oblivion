namespace Oblivion.BLM;

public class BattleData
{
    public static BattleData Instance = new();

    public uint 前一能力技 { get; set; } = 0;
    public uint 前一gcd { get; set; } = 0;
    public bool 已使用瞬发 { get; set; } = false;
    public bool IsInnerOpener = false;
    public bool 正在特殊循环中 { get; set; } = false;
    public bool 正在双星灵墨泉 { get; set; } = false;

    public bool 需要即刻{ get; set; } = false;

    public bool 需要瞬发gcd { get; set; } = false;
    public bool 三冰针进冰 { get; set; } = false;

    public HashSet<uint> 冰状态gcd = new HashSet<uint>();
    public HashSet<uint> 火状态gcd = new HashSet<uint>();
    
    public bool HotkeyUseHighPrioritySlot = false; // 热键使用高优先级队列
    

}