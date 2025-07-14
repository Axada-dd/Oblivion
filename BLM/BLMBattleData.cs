namespace Oblivion.BLM;

public class BattleData
{
    public static BattleData Instance = new();

    public int 瞬发层数 { get; set; } = 0;
    public uint 前一能力技 { get; set; } = 0;
    public uint 前一gcd { get; set; } = 0;
    public bool 已使用耀星 { get; set; } = false;
    public bool 已使用瞬发 { get; set; } = false;
    public bool 存在黑魔纹 { get; set; } = false;
    public bool 已使用黑魔纹 { get; set; } = false;
    public bool 火循环剩余gcd小于3 { get; set; } = false;
    public int 火循环剩余gcd { get; set; } = 0;
    public int 冰循环剩余gcd { get; set; } = 0;
    public int 复唱时间 { get; set; } = 0;
    public bool 双星灵墨泉短 { get; set; } = false;
    public bool 双星灵墨泉长 { get; set; } = false;
    public bool IsInnerOpener = false;
    public bool 启动aoe { get; set; } = false;
    public bool 能使用耀星 { get; set; } = false;
    public bool 已使用绝望 { get; set; } = false;
    public float 三连cd { get; set; } = 0;
    public int 能使用的火四个数 { get; set; } = 0;
    
    public bool 使用三连转冰 { get; set; } = false;
    public bool HotkeyUseHighPrioritySlot = false; // 热键使用高优先级队列

}