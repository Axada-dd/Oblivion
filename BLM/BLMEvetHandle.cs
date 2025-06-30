using AEAssist.Function;
using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;

public class BLMEvetHandle : IRotationEventHandler
{
    private readonly HashSet<uint> _gcdSpellIds = new HashSet<uint>
    {
        Spells.冰一,Spells.冰三,Spells.冰冻.GetActionChange(),Spells.冰澈,Spells.玄冰,
        Spells.火一,Spells.火三,Spells.火二.GetActionChange(),Spells.火四,Spells.核爆,Spells.绝望,Spells.耀星,
        Spells.异言,Spells.悖论,Spells.秽浊,Spells.雷一.GetActionChange(),Spells.雷二.GetActionChange(),Spells.崩溃
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
        if (BLMHelper.火状态) await Spells.星灵移位.GetSpell(SpellTargetType.Self).Cast();
        if (BLMHelper.冰状态 && BLMHelper.冰层数 <3 && BLMHelper.冰针<3 && Core.Me.CurrentMp < 10000) await Spells.灵极魂.GetSpell(SpellTargetType.Self).Cast();
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
        if (_gcdSpellIds.Contains(spell.Id))
        {
            BattleData.Instance.前一gcd = spell.Id;
        }
        if (spell.Id == Spells.耀星)
        {
            BattleData.Instance.已使用耀星 = true;
        }
        if (spell.Id == Spells.绝望) BattleData.Instance.已使用绝望 = true;
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
        if (_gcdSpellIds.Contains(spell.Id))
        {
            BattleData.Instance.前一gcd = spell.Id;
            BattleData.Instance.已使用瞬发 =  GCDHelper.GetGCDCooldown() >= (Core.Me.HasAura(Buffs.咏速Buff) ? 1200 : 1500);
        }

        if (BattleData.Instance.已使用瞬发)
        {
            if (spell.Id == Spells.耀星)
            {
                BattleData.Instance.已使用耀星 = true;
            }
        }

        if (spell.Id == Spells.黑魔纹) BattleData.Instance.已使用黑魔纹 = true;
        if (spell.Id == Spells.绝望) BattleData.Instance.已使用绝望 = true;
    }
    
    public void OnBattleUpdate(int currTimeInMs)
    {
        
        if (Spells.三连.GetSpell().Charges > 1)
        {
            BattleData.Instance.三连cd = 60-(Spells.三连.GetSpell().Charges - 1) * 60;
        }
        else BattleData.Instance.三连cd = 60-Spells.三连.GetSpell().Charges * 60;

        if (BattleData.Instance.能使用耀星 && Spells.三连.GetSpell().Charges > 1 && !BattleData.Instance.已使用耀星 &&
            !QT.Instance.GetQt("三连用于走位") && (Spells.即刻.GetSpell().Cooldown.TotalSeconds > 3 || Spells.三连.GetSpell().Charges * 60 >= 110 ) && Spells.墨泉.GetSpell().Cooldown.TotalSeconds > 12)
        {
            BattleData.Instance.使用三连转冰 = true;
        }
        if (BLMHelper.冰状态 || Spells.三连.GetSpell().Charges < 1 || Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 12) BattleData.Instance.使用三连转冰 = false;
        BattleData.Instance.复唱时间 = Core.Resolve<MemApiSpell>().GetGCDDuration();
        BattleData.Instance.可瞬发 = Core.Me.HasAura(Buffs.即刻Buff) || Core.Me.HasAura(Buffs.三连Buff);
        if (!QT.Instance.GetQt("aoe"))
            BattleData.Instance.启动aoe = false;
        if (BattleData.Instance.已使用耀星)
        {
            if (BLMHelper.冰状态 || Spells.墨泉.RecentlyUsed(300)) BattleData.Instance.已使用耀星 = false;
        }
        if (BattleData.Instance.已使用绝望)if(BLMHelper.冰状态 || Spells.墨泉.RecentlyUsed(300))  BattleData.Instance.已使用绝望 = false;

        if (BattleData.Instance.已使用黑魔纹)
        {
            BattleData.Instance.已使用黑魔纹 = !Helper.Buff时间小于(Buffs.黑魔纹Buff, 500);
        }

        if (BLMHelper.火状态)
        {
            BattleData.Instance.能使用的火四个数 = 0;
            var mp = (int)(Core.Me.CurrentMp - 2400);
            if (BLMHelper.冰针 > 0)
            {
                mp -= 800*BLMHelper.冰针;
                BattleData.Instance.能使用的火四个数 += BLMHelper.冰针;
            }

            BattleData.Instance.能使用的火四个数 += mp / 1600;
            BattleData.Instance.能使用耀星 = (BattleData.Instance.能使用的火四个数 + BLMHelper.耀星层数) == 6;
        }
        if (BLMHelper.火状态)
        {
            BattleData.Instance.冰循环剩余gcd = 0;
            BattleData.Instance.火循环剩余gcd = 0;
            if (BattleData.Instance.IsInnerOpener)
            {
                BattleData.Instance.火循环剩余gcd = BLMSetting.Instance.核爆起手 ? 18 : 19;
            }
            else
            {
                var 模拟mp = (int)Core.Me.CurrentMp;
                int 火四 = 0;
                if (BLMHelper.悖论指示)
                {
                    模拟mp -= 1600;
                    BattleData.Instance.火循环剩余gcd++;
                }
                if(!BattleData.Instance.已使用耀星&&BattleData.Instance.能使用耀星)BattleData.Instance.火循环剩余gcd++;
                if(BLMHelper.火层数<3)BattleData.Instance.火循环剩余gcd++;//火苗火3
                if (!BattleData.Instance.已使用绝望)//绝望
                {
                    模拟mp -= 800;
                    BattleData.Instance.火循环剩余gcd++;
                }
                if (BLMHelper.冰针 > 0)
                {
                    模拟mp -= 800*BLMHelper.冰针;
                    火四 += BLMHelper.冰针;
                }

                火四 += 模拟mp / 1600;
                BattleData.Instance.火循环剩余gcd += 火四;
                if(Helper.目标Buff时间小于(Buffs.雷一dot, BattleData.Instance.火循环剩余gcd*BattleData.Instance.复唱时间, false) && Core.Me.HasAura(Buffs.雷云))BattleData.Instance.火循环剩余gcd++;
            }
        }

        if (BLMHelper.冰状态)
        {
            BattleData.Instance.冰循环剩余gcd = 0;
            BattleData.Instance.火循环剩余gcd = 0;
            if (BLMHelper.冰层数 < 3) BattleData.Instance.冰循环剩余gcd++;
            if (BLMHelper.冰针 < 3) BattleData.Instance.冰循环剩余gcd++;
            if (BLMHelper.悖论指示) BattleData.Instance.冰循环剩余gcd++;
            if (Helper.目标Buff时间小于(Buffs.雷一dot, BattleData.Instance.冰循环剩余gcd *BattleData.Instance.复唱时间, false) &&
                Core.Me.HasAura(Buffs.雷云)) BattleData.Instance.冰循环剩余gcd++;
        }
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