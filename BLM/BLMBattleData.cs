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

    public List<uint> 冰状态gcd = [];
    public List<uint> 火状态gcd = [];
    public List<uint> 上一轮循环 = [];
    public bool HotkeyUseHighPrioritySlot = false; // 热键使用高优先级队列
    
    
    //循环控制
    public bool 三连走位 = false;
    public bool 即刻三连无移动判断 = false;
    public bool 起手 = false;
    public bool aoe火二 = false;
    public bool 特供循环 = false;
    public bool 压缩冰悖论 = false;
    public bool 压缩火悖论 = false;
    public bool 核爆收尾 = false;


}