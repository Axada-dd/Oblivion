using System.Threading.Tasks;
using AEAssist.CombatRoutine;
using AEAssist.CombatRoutine.Module;

namespace Oblivion.BLM;

public class BLMEvetHandle:IRotationEventHandler
{
    public Task OnPreCombat()
    {
        throw new System.NotImplementedException();
    }

    public void OnResetBattle()
    {
        throw new System.NotImplementedException();
    }

    public Task OnNoTarget()
    {
        throw new System.NotImplementedException();
    }

    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
        throw new System.NotImplementedException();
    }

    public void AfterSpell(Slot slot, Spell spell)
    {
        throw new System.NotImplementedException();
    }

    public void OnBattleUpdate(int currTimeInMs)
    {
        throw new System.NotImplementedException();
    }

    public void OnEnterRotation()
    {
        throw new System.NotImplementedException();
    }

    public void OnExitRotation()
    {
        throw new System.NotImplementedException();
    }

    public void OnTerritoryChanged()
    {
        throw new System.NotImplementedException();
    }
}