using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Extension;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Oblivion.BLM.SlotResolver.Data;
using Oblivion.Common;

namespace Oblivion.BLM;

public class BLMEvetHandle : IRotationEventHandler
{
    private readonly HashSet<uint> _GcdSpellIds = new HashSet<uint>
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
    }

    public async Task OnNoTarget()
    {
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 10 * 1000) return;
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
        if (_GcdSpellIds.Contains(spell.Id))
        {
            BattleData.Instance.前一GCD = spell.Id;
        }
        if (spell.Id == Spells.耀星)
        {
            BattleData.Instance.已使用耀星 = true;
        }
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
        if (_GcdSpellIds.Contains(spell.Id))
        {
            BattleData.Instance.前一GCD = spell.Id;
            BattleData.Instance.已使用瞬发 = GCDHelper.GetGCDCooldown() >= (Core.Me.HasAura(Buffs.咏速Buff) ? 800 : 1500);
        }

        if (BattleData.Instance.已使用瞬发)
        {
            if (spell.Id == Spells.耀星)
            {
                BattleData.Instance.已使用耀星 = true;
            }
        }

        if (spell.Id == Spells.黑魔纹) BattleData.Instance.已使用黑魔纹 = true;
    }

    public void OnBattleUpdate(int currTimeInMs)
    {
        BattleData.Instance.可瞬发 = Core.Me.HasAura(Buffs.即刻Buff) || Core.Me.HasAura(Buffs.三连Buff);
        if (BattleData.Instance.已使用耀星)
        {
            if (BLMHelper.冰状态||Spells.墨泉.RecentlyUsed(300)) BattleData.Instance.已使用耀星 = false;
        }

        if (BattleData.Instance.已使用黑魔纹)
        {
            BattleData.Instance.已使用黑魔纹 = !Helper.Buff时间小于(Buffs.黑魔纹Buff, 500);
        }
    }

    public void OnEnterRotation()
    {
        LogHelper.Print(
            "欢迎使用嗨呀的黑魔acr，反馈请到：");
        Core.Resolve<MemApiChatMessage>()
            .Toast2("跟我念：傻逼riku和souma的妈死干净咯！", 1, 5000);


        //检查全局设置
        if (!Helper.GlobalSettings.NoClipGCD3)
            LogHelper.PrintError("建议在acr全局设置中勾选【全局能力技不卡GCD】选项");

        //更新时间轴
        /*if (BLMSetting.Instance.AutoUpdataTimeLines)
            TimeLineUpdater.UpdateFiles(Helper.DncTimeLineUrl);*/
    }

    public void OnExitRotation()
    {
    }

    public void OnTerritoryChanged()
    {
    }
}