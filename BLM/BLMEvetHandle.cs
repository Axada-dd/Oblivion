using AEAssist.Function;
using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;

public class BLMEvetHandle : IRotationEventHandler
{
    private readonly HashSet<uint> _gcdSpellIds = new HashSet<uint>
    {
        Skill.冰一,Skill.冰三,Skill.冰冻.GetActionChange(),Skill.冰澈,Skill.玄冰,
        Skill.火一,Skill.火三,Skill.火二.GetActionChange(),Skill.火四,Skill.核爆,Skill.绝望,Skill.耀星,
        Skill.异言,Skill.悖论,Skill.秽浊,Skill.雷一.GetActionChange(),Skill.雷二.GetActionChange(),Skill.崩溃,Skill.灵极魂
    };

    private readonly HashSet<uint> _ogcdSpellIds = new HashSet<uint>
    {
        Skill.黑魔纹,Skill.三连,Skill.墨泉,Skill.即刻,Skill.星灵移位,Skill.醒梦,Skill.详述
    };
    public async Task OnPreCombat()
    {
        await Task.CompletedTask;
    }

    public void OnResetBattle()
    {
        BattleData.Instance = new BattleData();
        BattleData.Instance.IsInnerOpener = false;
    }

    public async Task OnNoTarget()
    {
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 10 * 1000) return;
        if (!QT.Instance.GetQt("Boss上天")) return;
        if (BLMHelper.火状态) await Skill.星灵移位.GetSpell(SpellTargetType.Self).Cast();
        if (BLMHelper.冰状态 && (BLMHelper.冰层数 < 3 || BLMHelper.冰针 < 3 || Core.Me.CurrentMp < 10000)) await Skill.灵极魂.GetSpell(SpellTargetType.Self).Cast();
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
        if (_gcdSpellIds.Contains(spell.Id))
        {
            BattleData.Instance.前一gcd = spell.Id;
        }
        if (spell.Id == Skill.耀星)
        {
            BattleData.Instance.已使用耀星 = true;
        }
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
        if (_gcdSpellIds.Contains(spell.Id))
        {
            BattleData.Instance.前一gcd = spell.Id;
            BattleData.Instance.已使用瞬发 =  GCDHelper.GetGCDCooldown() >= (Core.Me.HasAura(Buffs.咏速Buff) ? 1500 : 1700);
        }

        if (_ogcdSpellIds.Contains(spell.Id))
        {
            BattleData.Instance.前一能力技 = spell.Id;
        }
        if (BattleData.Instance.已使用瞬发)
        {
            BattleData.Instance.需要瞬发gcd = false;
            if (spell.Id == Skill.耀星)
            {
                BattleData.Instance.已使用耀星 = true;
            }
        }

        if (spell.Id == Skill.黑魔纹) BattleData.Instance.已使用黑魔纹 = true;
    }
    
    public void OnBattleUpdate(int currTimeInMs)
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
                
                if (BLMHelper.可用瞬发数() == 0 && MoveHelper.IsMoving() && !QT.Instance.GetQt("关闭即刻三连的移动判断"))
                {
                    BattleData.Instance.需要即刻 = true;
                }
            }
        }
        
        if (Helper.可瞬发()) BattleData.Instance.需要即刻 = false;
        if (BLMHelper.在发呆()) BattleData.Instance.需要瞬发gcd = true;
        if (Skill.三连.GetSpell().Charges > 1)
        {
            BattleData.Instance.三连cd = 60-(Skill.三连.GetSpell().Charges - 1) * 60;
        }
        else BattleData.Instance.三连cd = 60-Skill.三连.GetSpell().Charges * 60;
        if (Core.Me.IsCasting)
        {
            BattleData.Instance.已使用瞬发 = false;
        }   
        BattleData.Instance.三连转冰 = BLMHelper.三连转冰();

        BattleData.Instance.复唱时间 = Core.Resolve<MemApiSpell>().GetElapsedGCD();

        if (BattleData.Instance.已使用耀星)
        {
            if (BLMHelper.冰状态 || Skill.墨泉.RecentlyUsed(300)) BattleData.Instance.已使用耀星 = false;
        }

        if (BattleData.Instance.已使用黑魔纹)
        {
            BattleData.Instance.已使用黑魔纹 = !Helper.Buff时间小于(Buffs.黑魔纹Buff, 500);
        }
        BattleData.Instance.能使用耀星 = BLMHelper.能使用耀星();
        BattleData.Instance.能使用的火四个数 = BLMHelper.能使用的火四个数();
        BattleData.Instance.火循环剩余gcd = BLMHelper.火循环gcd();
        BattleData.Instance.冰循环剩余gcd = BLMHelper.冰循环gcd();
        BattleData.Instance.能星灵转冰 = BLMHelper.能星灵转冰();
    }

    public void OnEnterRotation()
    {
        LogHelper.Print(
            "欢迎使用OVO的黑魔acr，反馈请到：");
        LogHelper.Print("建议设置提前使用gcd时间为50，使用fuckanime三插，DR能力技动画减少");


        //检查全局设置
        if (Helper.GlobalSettings.NoClipGCD3)
            LogHelper.PrintError("建议不要在acr全局设置中勾选【全局能力技不卡GCD】选项");
        BattleData.Instance.IsInnerOpener = false;
        //更新时间轴
        /*if (BLMSetting.Instance.AutoUpdataTimeLines)
            TimeLineUpdater.UpdateFiles(Helper.DncTimeLineUrl);*/
    }

    public void OnExitRotation()
    {
        BLMSetting.Instance.Save();
    }

    public void OnTerritoryChanged()
    {
    }
}