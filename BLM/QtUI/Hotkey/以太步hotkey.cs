using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;

namespace Oblivion.BLM.QtUI.Hotkey;

public static class 以太步hotkeywindow
{
    public static HotkeyWindow BLMPartnerPanel;
    public static JobViewSave myJobViewSave;

    public static void Build(JobViewWindow instance)
    {
        instance.SetUpdateAction((() =>
        {
            PartyHelper.UpdateAllies();
            if(PartyHelper.Party.Count<1)return;
            myJobViewSave = new JobViewSave
            {
                QtHotkeySize = new Vector2(BLMSetting.Instance.以太步IconSize),
                ShowHotkey = BLMSetting.Instance.以太步窗口显示
            };
            BLMPartnerPanel = new HotkeyWindow(myJobViewSave,"BLMPartnerPanel")
            {
                HotkeyLineCount = 1
            };
            for (var i = 1; i < PartyHelper.Party.Count; i++)
            {
                var index = i;
                BLMPartnerPanel?.AddHotkey("以太步: " + PartyHelper.Party[i].Name, new 以太步hotkey(index));
            }
        }));
    }
}
public class 以太步hotkey(int index):IHotkeyResolver
{
    private const uint 以太步 = Spells.以太步;

    public void Draw(Vector2 size)
    {
        HotkeyHelper.DrawSpellImage(size, 以太步);
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        if (_check() >= 0)
        {
            if (isActive)
            {
                HotkeyHelper.DrawActiveState(size);
            }
            else
            {
                HotkeyHelper.DrawGeneralState(size);
            }
        }
        else
        {
            HotkeyHelper.DrawDisabledState(size);
        }
        HotkeyHelper.DrawCooldownText(以太步.GetSpell(), size);
    }
    public int Check()
    {
        return _check();
    }
    
    private int _check()
    {
        if (以太步.GetSpell().Cooldown.TotalMilliseconds > 0 ||
            !PartyHelper.Party[index].IsTargetable ||
            PartyHelper.Party[index].IsDead() ||
            Core.Me.Distance(PartyHelper.Party[index]) > SettingMgr.GetSetting<GeneralSettings>().AttackRange + 27)
            return -2;
        return 0;
    }

    public void Run()
    {
        var partyMembers = PartyHelper.Party;
        if (partyMembers.Count < index + 1)return;

        if (GCDHelper.GetGCDCooldown() <= 0)
        {
            AI.Instance.BattleData.NextSlot ??= new Slot();
            AI.Instance.BattleData.NextSlot.Add(new Spell(以太步, partyMembers[index]));
        }
        else
        {
            AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(new Slot(new Spell(以太步, partyMembers[index]),2500));
        }
    }
}