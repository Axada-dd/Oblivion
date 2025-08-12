using AEAssist.Function;
using AEAssist.MemoryApi;
using Oblivion.BLM.QtUI;

namespace Oblivion.BLM;

/// <summary>
/// 黑魔法师(BLM)职业的事件处理类，实现了IRotationEventHandler接口
/// 负责处理黑魔法师战斗中的各种事件和状态更新
/// </summary>
public class BLMEvetHandle : IRotationEventHandler
{
    /// <summary>
    /// 黑魔法师GCD技能ID集合，用于识别和处理GCD技能的使用
    /// 包含所有冰系、火系、雷系以及其他GCD技能
    /// </summary>
    private readonly HashSet<uint> _gcdSpellIds = new HashSet<uint>
    {
        Skill.冰一,Skill.冰三,Skill.冰冻.GetActionChange(),Skill.冰澈,Skill.玄冰,
        Skill.火一,Skill.火三,Skill.火二.GetActionChange(),Skill.火四,Skill.核爆,Skill.绝望,Skill.耀星,
        Skill.异言,Skill.悖论,Skill.秽浊,Skill.雷一.GetActionChange(),Skill.雷二.GetActionChange(),Skill.崩溃,Skill.灵极魂
    };

    /// <summary>
    /// 黑魔法师能力技(oGCD)技能ID集合，用于识别和处理非GCD技能的使用
    /// 包含黑魔纹、三连咏唱、墨泉等辅助技能
    /// </summary>
    private readonly HashSet<uint> _ogcdSpellIds = new HashSet<uint>
    {
        Skill.黑魔纹,Skill.三连,Skill.墨泉,Skill.即刻,Skill.星灵移位,Skill.醒梦,Skill.详述
    };


    /// <summary>
    /// 战斗前准备事件处理方法
    /// 当进入战斗前调用，可以用于设置战斗前的准备工作
    /// </summary>
    /// <returns>异步任务</returns>
    public async Task OnPreCombat()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 重置战斗状态事件处理方法
    /// 当战斗结束或需要重置战斗状态时调用，重置所有战斗相关数据
    /// </summary>
    public void OnResetBattle()
    {
        // 创建新的战斗数据实例并重置所有状态标志
        BattleData.Instance = new BattleData();
        BattleData.Instance.IsInnerOpener = false;
        BattleData.Instance.需要即刻 = false;
        BattleData.Instance.需要瞬发gcd = false;
        BattleData.Instance.正在特殊循环中 = false;
        // 重置QT状态
        QT.Reset();
    }

    /// <summary>
    /// 无目标事件处理方法
    /// 当没有战斗目标时调用，处理无目标状态下的技能使用和状态维护
    /// </summary>
    /// <returns>异步任务</returns>
    public async Task OnNoTarget()
    {
        // 战斗时间小于10秒时不处理
        if (AI.Instance.BattleData.CurrBattleTimeInMs < 10 * 1000) return;
        
        // 重置技能状态标志
        BattleData.Instance.需要即刻 = false;
        BattleData.Instance.需要瞬发gcd = false;
        BattleData.Instance.正在特殊循环中 = false;
        
        // 处理Boss上天特殊情况
        if (QT.Instance.GetQt("Boss上天"))
        {
            // 火状态下使用星灵移位
            if (BLMHelper.火状态) await Skill.星灵移位.GetSpell(SpellTargetType.Self).Cast();
            
            // 冰状态且条件满足时使用灵极魂
            if (BLMHelper.冰状态 && (BLMHelper.冰层数 < 3 || BLMHelper.冰针 < 3 || Core.Me.CurrentMp < 10000))
                await Skill.灵极魂.GetSpell(SpellTargetType.Self).Cast();
        }
    }

    /// <summary>
    /// 技能施放成功事件处理方法
    /// 当成功施放技能后调用，更新相关状态数据
    /// </summary>
    /// <param name="slot">技能槽位</param>
    /// <param name="spell">施放的技能</param>
    public void OnSpellCastSuccess(Slot slot, Spell spell)
    {
        // 处理GCD技能施放成功
        if (_gcdSpellIds.Contains(spell.Id))
        {
            // 记录前一个GCD技能ID
            BattleData.Instance.前一gcd = spell.Id;
            // 重置GCD技能计数
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;
        }
        
        // 特殊处理耀星技能
        if (spell.Id == Skill.耀星)
        {
            BattleData.Instance.已使用耀星 = true;
        }
    }

    /// <summary>
    /// 技能施放后事件处理方法
    /// 在技能施放完成后调用，处理技能施放后的状态更新和逻辑
    /// </summary>
    /// <param name="slot">技能槽位</param>
    /// <param name="spell">施放的技能</param>
    public void AfterSpell(Slot slot, Spell spell)
    {
        // 处理GCD技能施放后的状态
        if (_gcdSpellIds.Contains(spell.Id))
        {
            // 记录前一个GCD技能ID
            BattleData.Instance.前一gcd = spell.Id;
            // 重置GCD技能计数
            AI.Instance.BattleData.CurrGcdAbilityCount = 1;
            // 根据GCD冷却时间判断是否使用了瞬发技能
            // 有咏速buff时阈值为1500ms，否则为1700ms
            BattleData.Instance.已使用瞬发 = GCDHelper.GetGCDCooldown() >= (Core.Me.HasAura(Buffs.咏速Buff) ? 1500 : 1700);
        }

        // 处理冰状态下的AOE循环
        if (BLMHelper.冰状态 && (BLMHelper.双目标aoe() || BLMHelper.三目标aoe()))
        {
            // 如果使用了雷系技能或秽浊，标记AOE循环填充
            if (spell.Id == Skill.雷二 || spell.Id == Skill.雷一 || spell.Id == Skill.秽浊)
            {
                BattleData.Instance.Aoe循环填充 = true;
            }
        }
        
        // 处理能力技(oGCD)施放后的状态
        if (_ogcdSpellIds.Contains(spell.Id))
        {
            // 记录前一个能力技ID
            BattleData.Instance.前一能力技 = spell.Id;
        }
        
        // 处理瞬发技能使用后的状态
        if (BattleData.Instance.已使用瞬发)
        {
            // 重置瞬发GCD需求标志
            BattleData.Instance.需要瞬发gcd = false;
            // 更新GCD技能计数
            AI.Instance.BattleData.CurrGcdAbilityCount = 2;
            // 特殊处理耀星技能
            if (spell.Id == Skill.耀星)
            {
                BattleData.Instance.已使用耀星 = false;
            }
        }
    }

    /// <summary>
    /// 战斗更新事件处理方法
    /// 在战斗中定期调用，更新战斗状态和数据
    /// </summary>
    /// <param name="currTimeInMs">当前战斗时间(毫秒)</param>
    public void OnBattleUpdate(int currTimeInMs)
    {
        // 运行黑魔法师功能逻辑
        BLMFunction.Run();
        
        // 可瞬发状态下不需要即刻
        if (Helper.可瞬发()) BattleData.Instance.需要即刻 = false;
        
        // 处理角色发呆状态
        if (BLMHelper.在发呆())
        {
            // 有可用瞬发技能时设置需要瞬发GCD标志
            if (BLMHelper.可用瞬发() != 0)
                BattleData.Instance.需要瞬发gcd = true;
        }
        
        // 正在施法时重置相关状态
        if (Core.Me.IsCasting)
        {
            BattleData.Instance.需要即刻 = false;
            BattleData.Instance.已使用瞬发 = false;
            BattleData.Instance.需要瞬发gcd = false;
        }

        // 更新三连咏唱转冰状态
        BattleData.Instance.三连转冰 = BLMHelper.三连转冰();

        // 更新GCD复唱时间
        BattleData.Instance.复唱时间 = GCDHelper.GetGCDDuration();

        // 处理耀星使用状态
        if (BattleData.Instance.已使用耀星)
        {
            // 冰状态或最近使用了墨泉时重置耀星使用状态
            if (BLMHelper.冰状态 || Skill.墨泉.RecentlyUsed(300)) BattleData.Instance.已使用耀星 = false;
        }

        // 处理特殊循环状态
        if (!QT.Instance.GetQt(QTkey.使用特供循环)) BattleData.Instance.正在特殊循环中 = false;

        // 更新各种战斗状态数据
        BattleData.Instance.已存在黑魔纹 = Helper.有buff(737);
        BattleData.Instance.能使用耀星 = BLMHelper.能使用耀星();
        BattleData.Instance.能使用的火四个数 = BLMHelper.能使用的火四个数();
        BattleData.Instance.火循环剩余gcd = BLMHelper.火循环gcd();
        BattleData.Instance.冰循环剩余gcd = BLMHelper.冰循环gcd();
        BattleData.Instance.能星灵转冰 = BLMHelper.能星灵转冰();
    }

    /// <summary>
    /// 进入循环事件处理方法
    /// 当进入战斗循环时调用，进行初始化和设置检查
    /// </summary>
    public void OnEnterRotation()
    {
        // 输出欢迎信息
        LogHelper.Print(
            "欢迎使用OVO的黑魔acr，反馈请到：");
        LogHelper.Print("建议设置提前使用gcd时间为50，使用fuckanime三插，DR能力技动画减少");

        // 检查全局设置并给出建议
        if (Helper.GlobalSettings.NoClipGCD3)
            LogHelper.PrintError("建议不要在acr全局设置中勾选【全局能力技不卡GCD】选项");
        
        // 重置开场标志
        BattleData.Instance.IsInnerOpener = false;
        
        // 更新时间轴(当前已注释)
        /*if (BLMSetting.Instance.AutoUpdataTimeLines)
            TimeLineUpdater.UpdateFiles(Helper.DncTimeLineUrl);*/
    }

    /// <summary>
    /// 退出循环事件处理方法
    /// 当退出战斗循环时调用，保存设置和状态
    /// </summary>
    public void OnExitRotation()
    {
        // 保存黑魔法师设置
        BLMSetting.Instance.Save();
        // 保存QT状态
        BLMSetting.Instance.SaveQtStates(QT.Instance);
    }

    /// <summary>
    /// 区域变更事件处理方法
    /// 当玩家切换区域时调用，重置相关状态
    /// </summary>
    public void OnTerritoryChanged()
    {
        // 重置QT状态
        QT.Reset();
    }
}