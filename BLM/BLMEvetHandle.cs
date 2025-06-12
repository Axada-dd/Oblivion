using System.Threading.Tasks;
using AEAssist;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;
using AEAssist.Helper;
using AEAssist.MemoryApi;
using Oblivion.Common;

namespace Oblivion.BLM;

public class BLMEvetHandle:IRotationEventHandler
{
    public async Task OnPreCombat()
    {
        await Task.CompletedTask;
    }

    public void OnResetBattle()
    {
        BLMBattleData.Instance = new BattleData();
    }

    public async Task OnNoTarget()
    {
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 10 * 1000) return;
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
    }

    public void OnBattleUpdate(int currTimeInMs)
    {
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