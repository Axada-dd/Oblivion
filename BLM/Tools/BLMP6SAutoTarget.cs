using Dalamud.Game.ClientState.Objects.Types;

namespace Oblivion.BLM.Tools;

public class BLMP6SAutoTarget
{
    public static IBattleChara? AutoTarget()
    {
        IBattleChara? currentTarget = ((IBattleChara)(object)Core.Me).GetCurrTarget();
		if (!BLMSetting.Instance.Autotarget)
		{
			return currentTarget;
		}
		if (ECHelper.Objects == null)
		{
			return currentTarget;
		}
		IBattleChara[] enemies = (from IBattleChara c in ((IEnumerable<IGameObject>)ECHelper.Objects).Where((IGameObject obj) => obj is IBattleChara)
			where c != null && ((IGameObject?)(object)c).DistanceToPlayer() < 25f && c.CanAttack()
			orderby ((IGameObject?)(object)c).DistanceToPlayer()
			select c).ToArray();
		if (enemies.Length == 0)
		{
			return currentTarget;
		}
		IBattleChara? 无目标鱼 = ((IEnumerable<IBattleChara?>)enemies).FirstOrDefault((Func<IBattleChara?, bool>)((IBattleChara c) => ((IGameObject)c).DataId == 18346 && ((IGameObject)c).TargetObject == null));
		IBattleChara? 有目标鱼 = ((IEnumerable<IBattleChara?>)enemies).FirstOrDefault((Func<IBattleChara?, bool>)((IBattleChara c) => ((IGameObject)c).DataId == 18346));
		bool 自己有鱼 = enemies.Any((IBattleChara c) => ((IGameObject)c).DataId == 18346 && ((IGameObject)c).TargetObject != null && ((IGameObject)c).TargetObject.IsMe());
		IBattleChara? 哈基米 = ((IEnumerable<IBattleChara?>)enemies).FirstOrDefault((Func<IBattleChara?, bool>)((IBattleChara c) => ((IGameObject)c).DataId == 18347));
		IBattleChara? 羊 = ((IEnumerable<IBattleChara?>)enemies).FirstOrDefault((Func<IBattleChara?, bool>)((IBattleChara c) => ((IGameObject)c).DataId == 18344));
		IBattleChara? 马 = ((IEnumerable<IBattleChara?>)enemies).FirstOrDefault((Func<IBattleChara?, bool>)((IBattleChara c) => ((IGameObject)c).DataId == 18345));
		IBattleChara? boss = ((IEnumerable<IBattleChara?>)enemies).FirstOrDefault((Func<IBattleChara?, bool>)((IBattleChara c) => ((IGameObject)c).DataId == 18335));
		if (!自己有鱼 && 无目标鱼 != null && AI.Instance?.BattleData != null)
		{
			long battleTime = AI.Instance.BattleData.CurrBattleTimeInMs;
			if (BLMSetting.Instance.AutoTargetMode == 3 || (BLMSetting.Instance.AutoTargetMode == 2 && battleTime > 300000) || (BLMSetting.Instance.AutoTargetMode == 1 && battleTime > 250000 && battleTime < 300000))
			{
				return 无目标鱼;
			}
		}
		if (马 != null)
		{
			return 马;
		}
		if (哈基米 != null)
		{
			return 哈基米;
		}
		if (有目标鱼 != null)
		{
			return 有目标鱼;
		}
		if (羊 != null)
		{
			return 羊;
		}
		if (boss != null)
		{
			return boss;
		}
		return currentTarget;
    }
}