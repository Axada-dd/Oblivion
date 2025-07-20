using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;

public abstract class BLMFunction
{
    public static void 移动处理()
    {
        if (!Core.Me.IsCasting)
        {
            if (GCDHelper.GetGCDCooldown() < 100)
            {
                if (MoveHelper.IsMoving())
                {
                    if (BLMHelper.可用瞬发数() > 0 && !Helper.可瞬发())
                    {
                        BattleData.Instance.需要瞬发gcd = true;
                    }                
                }
            }

            if (GCDHelper.GetGCDCooldown() < 800)
            {
                
                if (BLMHelper.可用瞬发数() == 0 && MoveHelper.IsMoving() && !QT.Instance.GetQt(QTkey.关闭即刻三连的移动判断))
                {
                    BattleData.Instance.需要即刻 = true;
                }
            }
        }
    }

    public static void 重置火状态()
    {
        if (Core.Me.IsDead || BLMHelper.冰状态)
        {
            BattleData.Instance.火状态gcd = new HashSet<uint>();
        }
    }

    public static void 重置冰状态()
    {
        if (Core.Me.IsDead || BLMHelper.火状态)
        {
            BattleData.Instance.三冰针进冰 = false;
            BattleData.Instance.Aoe循环填充 = false;
            BattleData.Instance.冰状态gcd = new HashSet<uint>();
        }
    }

    public static void 记录gcd()
    {
        if (BLMHelper.冰状态)
        {
            BattleData.Instance.冰状态gcd.Add(BattleData.Instance.前一gcd);
        }
        if (BLMHelper.火状态)
        {
            BattleData.Instance.火状态gcd.Add(BattleData.Instance.前一gcd);
        }
    }

    public static void 三冰针进冰()
    {
        if (BLMHelper.冰状态 && BLMHelper.冰针 == 3)
        {
            if (!BattleData.Instance.冰状态gcd.Contains(Skill.玄冰)
                && !BattleData.Instance.冰状态gcd.Contains(Skill.冰澈))
            {
                BattleData.Instance.三冰针进冰 = true;
            }
        }
    }

    public static void 重置三冰针进冰()
    {
        if (BattleData.Instance.三冰针进冰 && BLMHelper.冰状态)
        {
            if(BattleData.Instance.前一gcd == Skill.玄冰 || BattleData.Instance.前一gcd == Skill.冰澈)
            {
                BattleData.Instance.三冰针进冰 = false;
            }
        }
    }
    public static void Run()
    {
        移动处理();
        重置火状态();
        重置冰状态();
        记录gcd();
        三冰针进冰();
        重置三冰针进冰();
    }
}