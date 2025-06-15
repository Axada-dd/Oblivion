using AEAssist.CombatRoutine.Module;

namespace Oblivion.BLM;

public class BattleData
{
    public static BattleData Instance = new();

    public int 瞬发层数 { get; set; } = 0;
    public bool 可瞬发 { get; set; } = false;
    public uint 前一GCD { get; set; } = 0;
    public bool 已使用耀星 { get; set; } = false;
}