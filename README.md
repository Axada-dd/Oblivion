# ISlotResolver类Check方法返回值说明

## 技能分类说明
以下表格详细列出了不同技能类 `Check` 方法的返回值及其对应的条件，按 Ability（非公共冷却技能）和 GCD（公共冷却技能） 进行分类。

## Ability（非公共冷却技能）
### 详述类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.详述.GetSpell().IsReadyWithCanCast() 为 false                 |
| -2     | BLMHelper.通晓层数 >= 3 或 (BLMHelper.耀星层数 == 2 且 BLMHelper.通晓剩余时间 < 4000) |
| 1      | 以上条件均不满足                                                     |

### 即刻类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.即刻.GetSpell().IsReadyWithCanCast() 为 false                 |
| -2     | !QT.Instance.GetQt("即刻")                                           |
| -3     | BattleData.Instance.可瞬发                                           |
| 1      | BLMHelper.火状态 为 true 且 BattleData.Instance.已使用耀星 且 BattleData.Instance.已使用瞬发 且 Core.Me.CurrentMp < 800 且 (Spells.墨泉.GetSpell().Cooldown.TotalSeconds >= 12 && !Spells.墨泉.RecentlyUsed(1500)) |
| 2      | BLMHelper.冰状态 为 true 且 BLMHelper.冰层数 < 3 且 BattleData.Instance.已使用瞬发 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds >= 12 |
| 3      | Helper.IsMove 且 GCDHelper.Is2ndAbilityTime() 且 Spells.三连.GetSpell().Charges < 1.4 |
| -99    | 以上条件均不满足                                                     |

### 醒梦类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.醒梦.GetSpell().IsReadyWithCanCast()                         |
| -2     | !QT.Instance.GetQt("醒梦")                                           |
| 1      | !BattleData.Instance.可瞬发 且 BLMHelper.冰状态 且 Core.Me.CurrentMp < 800 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 12 且 BLMHelper.冰层数 < 3 |
| -99    | 以上条件均不满足                                                     |

### 星灵移位类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.星灵移位.GetSpell().IsReadyWithCanCast()                     |
| -2     | !BLMHelper.冰状态 且 !BLMHelper.火状态                               |
| -5     | Core.Me.Level < 90                                                   |
| -6     | BLMHelper.火状态 为 true 且 Core.Me.CurrentMp < 800 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 2 |
| 1      | BLMHelper.冰状态 为 true 且 Core.Me.HasAura(Buffs.火苗) 且 BLMHelper.冰层数 == 3 且 BLMHelper.冰针 == 3 且 !BLMHelper.悖论指示 |
| 5      | BLMHelper.冰状态 为 true 且 !Core.Me.HasAura(Buffs.火苗) 且 BLMHelper.冰层数 == 3 且 BLMHelper.冰针 == 3 且 !BLMHelper.悖论指示 |
| 4      | BLMHelper.冰状态 为 true 且 BLMHelper.冰层数 < 3 且 !BLMHelper.悖论指示 且 Core.Me.CurrentMp >= 800 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 10 |
| 3      | BLMHelper.火状态 为 true 且 Core.Me.CurrentMp < 800 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 12 且 BLMHelper.耀星层数 != 6 |
| 6      | BLMHelper.火状态 为 true 且 Core.Me.CurrentMp < 800 且 (BattleData.Instance.可瞬发 或 Spells.即刻.GetSpell().Cooldown.TotalSeconds < 0.3 ) 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds > 2 且 BLMHelper.耀星层数 != 6 且 !Spells.墨泉.RecentlyUsed(1500) |

### 三连咏唱类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.三连.GetSpell().IsReadyWithCanCast()                         |
| -2     | !QT.Instance.GetQt("三连咏唱")                                       |
| -3     | BLMHelper.火状态 为 true 且 BattleData.Instance.火循环剩余gcd < 2 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 10 |
| -4     | BattleData.Instance.可瞬发                                           |
| -5     | GCDHelper.Is2ndAbilityTime()                                         |
| 2      | Helper.IsMove 且 BLMHelper.通晓层数 < 2 且 !(new 悖论().Check() > 0 或 new 绝望().Check() > 0 或 new 雷1().Check() > 0 或 new 雷2().Check() > 0 或 new 异言().Check() > 0) |
| 3      | BattleData.Instance.火循环剩余gcd < 2 且 (new 雷1().Check() < 0 或 new 雷2().Check() < 0) 且 Spells.即刻.GetSpell().Cooldown.TotalSeconds > 3 且 BLMHelper.火状态 且 !QT.Instance.GetQt("三连用于走位") |
| 4      | BattleData.Instance.使用三连转冰 且 !BLMHelper.悖论指示 且 Core.Me.CurrentMp < 800 |
| -99    | 以上条件均不满足                                                     |

### 黑魔纹类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.黑魔纹.GetSpell().IsReadyWithCanCast()                       |
| -2     | !BattleData.Instance.已使用瞬发                                      |
| -3     | BattleData.Instance.已使用黑魔纹                                     |
| -5     | !QT.Instance.GetQt("黑魔纹")                                         |
| 1      | 以上条件均不满足                                                     |

### 爆发药类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -2     | !QT.Instance.GetQt("爆发药")                                         |
| -3     | !ItemHelper.CheckCurrJobPotion()                                     |
| -1     | 以上条件均不满足                                                     |

### 墨泉类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.墨泉.GetSpell().IsReadyWithCanCast()                         |
| -2     | !BLMHelper.火状态                                                   |
| -3     | Core.Me.CurrentMp > 800                                              |
| -4     | !BattleData.Instance.已使用瞬发                                      |
| -5     | !QT.Instance.GetQt("墨泉")                                           |
| -6     | BLMHelper.耀星层数 == 6                                              |
| 1      | 以上条件均不满足                                                     |

## GCD（公共冷却技能）
### 火
#### 耀星类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.耀星.GetSpell().IsReadyWithCanCast() 为 false                 |
| -6     | BLMHelper.火状态 为 false                                           |
| 1      | BLMHelper.耀星层数 == 6 且 Helper.IsMove 为 false                    |
| 2      | BLMHelper.耀星层数 == 6 且 Helper.IsMove 为 true 且 BattleData.Instance.可瞬发 |
| -99    | 以上条件均不满足                                                     |

#### 火三类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | 以上条件均不满足                                                     |
| -2     | !new Spell(Spells.火三, SpellTargetType.Target).IsReadyWithCanCast() |
| -3     | nearbyEnemyCount >= 3 且 QT.Instance.GetQt("AOE")                    |
| -5     | BLMHelper.火状态 为 true 且 !Core.Me.HasAura(Buffs.火苗)              |
| 1      | BLMHelper.冰状态 为 true 且 !Core.Me.HasAura(Buffs.火苗) 且 BLMHelper.冰层数 >= 3 且 BLMHelper.冰针 >= 3 且 Core.Me.CurrentMp >= 10000 |
| 2      | !BLMHelper.火状态 且 !BLMHelper.冰状态 且 Core.Me.CurrentMp >= 10000 |
| 3      | BLMHelper.火状态 为 true 且 Core.Me.HasAura(Buffs.火苗) 且 BLMHelper.火层数 < 3 |
| 4      | BLMHelper.火状态 为 true 且 Core.Me.HasAura(Buffs.火苗) 且 BLMHelper.火层数 < 3 |

#### 绝望类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -2     | !new Spell(Spells.绝望, SpellTargetType.Target).IsReadyWithCanCast() |
| -3     | nearbyEnemyCount >= 2 且 QT.Instance.GetQt("核爆") 且 QT.Instance.GetQt("AOE") |
| -4     | !QT.Instance.GetQt("绝望") 或 (BattleData.Instance.使用三连转冰 且 BLMHelper.悖论指示) |
| -6     | !BLMHelper.火状态                                                   |
| 1      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 == 3 且 BLMHelper.耀星层数 == 6 且 Core.Me.CurrentMp <= 2400 且 Helper.IsMove 且 !BattleData.Instance.可瞬发 |
| 2      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 <= 3 且 Core.Me.CurrentMp < 2400 且 BLMHelper.耀星层数 < 6 且 !Core.Me.HasAura(Buffs.火苗) |
| 3      | BLMHelper.火层数 == 3 且 BattleData.Instance.已使用耀星 且 Core.Me.CurrentMp <= 2400 |
| 5      | BattleData.Instance.使用三连转冰 且 Core.Me.CurrentMp < 2400         |
| -99    | 以上条件均不满足                                                     |

#### 火4类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -2     | !new Spell(Spells.火四, SpellTargetType.Target).IsReadyWithCanCast() |
| -3     | nearbyEnemyCount >= 2 且 QT.Instance.GetQt("核爆") 且 QT.Instance.GetQt("AOE") |
| -6     | !BLMHelper.火状态                                                   |
| 1      | BLMHelper.火层数 >= 3 且 BLMHelper.耀星层数 < 6 且 Core.Me.CurrentMp > 2400L 且 !Helper.IsMove |
| 2      | BLMHelper.火层数 >= 3 且 BLMHelper.耀星层数 < 6 且 Core.Me.CurrentMp > 2400L 且 BattleData.Instance.可瞬发 |
| -99    | 以上条件均不满足                                                     |

#### 核爆类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | 示例：Spells.核爆.GetSpell().IsReadyWithCanCast() 为 false           |
| -2     | 示例：BLMHelper.火状态 为 false 且 Core.Me.CurrentMp < 5000          |
| 1      | 示例：BLMHelper.火状态 为 true 且 BLMHelper.火层数 == 3 且 nearbyEnemyCount >= 2 |
| -99    | 以上条件均不满足                                                     |

### 冰
#### 冰澈类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.冰澈.GetSpell().IsReadyWithCanCast()                         |
| -6     | !BLMHelper.冰状态                                                   |
| 1      | BLMHelper.冰层数 == 3 且 BLMHelper.冰针 < 3 且 !Helper.IsMove          |
| 2      | BLMHelper.冰层数 == 3 且 BLMHelper.冰针 < 3 且 Helper.IsMove 且 BattleData.Instance.可瞬发 |
| -99    | 以上条件均不满足                                                     |

#### 冰三类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.冰三.GetSpell().IsReadyWithCanCast()                         |
| 1      | BLMHelper.火状态 为 true 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds > 10 且 Core.Me.CurrentMp < 800 且 !BattleData.Instance.可瞬发 且 BLMHelper.耀星层数 != 6 |
| 2      | BLMHelper.冰状态 为 true 且 BLMHelper.冰层数 < 3 且 BattleData.Instance.可瞬发 |
| 4      | !BLMHelper.冰状态 且 !BLMHelper.火状态 且 Core.Me.CurrentMp < 5000   |
| -99    | 以上条件均不满足                                                     |

### 雷
#### 雷2类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.雷二.GetSpell().IsReadyWithCanCast()                         |
| -2     | !QT.Instance.GetQt("雷二")                                           |
| -3     | nearbyEnemyCount < 2                                                 |
| -4     | QT.Instance.GetQt("AOE")                                             |
| 1      | Core.Me.HasAura(Buffs.雷云) 且 (Helper.目标Buff时间小于(Buffs.雷二dot, 3000, false) 且 Helper.目标Buff时间小于(Buffs.雷一dot, 5000)) |
| -99    | 以上条件均不满足                                                     |

#### 雷1类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.雷一.GetSpell().IsReadyWithCanCast()                         |
| -2     | enemyCount >= 2 且 QT.Instance.GetQt("AOE")                           |
| -3     | !QT.Instance.GetQt("雷一")                                           |
| 1      | (Helper.目标Buff时间小于(Buffs.雷一dot, 3000, false) 且 Helper.目标Buff时间小于(Buffs.雷二dot, 3000, false)) 且 Core.Me.HasAura(Buffs.雷云) |
| -99    | 以上条件均不满足                                                     |

### 通晓
#### 悖论类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | Spells.悖论.GetSpell().IsReadyWithCanCast() 为 false                 |
| -6     | BLMHelper.悖论指示 为 false                                          |
| 1      | BLMHelper.冰状态 为 true 且 BLMHelper.冰层数 == 3 且 BLMHelper.冰针 == 3 |
| 2      | BLMHelper.冰状态 为 true 且 Helper.IsMove 为 true 且 !BattleData.Instance.可瞬发 |
| 3      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 == 3 且 (BattleData.Instance.已使用耀星 || (BattleData.Instance.使用三连转冰&&BLMHelper.耀星层数 == 6)) 且 Core.Me.CurrentMp >= 2400L |
| 5      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 == 3 且 Helper.IsMove 且 !BattleData.Instance.可瞬发 |
| 6      | BLMHelper.冰状态 为 true 且 !BattleData.Instance.可瞬发 且 BLMHelper.冰层数 < 3 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 10 |
| 7      | BLMHelper.火状态 为 true 且 BLMHelper.火层数 < 3 且 !Core.Me.HasAura(Buffs.火苗) |
| -99    | 以上条件均不满足                                                     |

#### 异言类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.异言.GetSpell().IsReadyWithCanCast()                         |
| -2     | !QT.Instance.GetQt("异言")                                           |
| -3     | nearbyEnemyCount >= 2 且 QT.Instance.GetQt("AOE")                    |
| -4     | !BLMHelper.火状态                                                   |
| -5     | BLMHelper.火状态 为 true 且 BLMHelper.耀星层数 != 6 且 Core.Me.CurrentMp < 800 且 Spells.即刻.GetSpell().Cooldown.TotalMilliseconds < 2200 |
| 6      | BLMHelper.火状态 为 true 且 Core.Me.CurrentMp < 800 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 2 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds > 0 且 BLMHelper.耀星层数 != 6 |
| 7      | BLMHelper.火状态 为 true 且 Core.Me.CurrentMp < 800 且 Spells.即刻.GetSpell().Cooldown.TotalSeconds < 2 且 Spells.即刻.GetSpell().Cooldown.TotalSeconds > 0 且 BLMHelper.耀星层数 != 6 |
| -99    | 以上条件均不满足                                                     |

#### 秽浊类
| 返回值 | 条件                                                                 |
|--------|----------------------------------------------------------------------|
| -1     | !Spells.秽浊.GetSpell().IsReadyWithCanCast()                         |
| -2     | nearbyEnemyCount < 2 或 !QT.Instance.GetQt("AOE")                    |
| -5     | !QT.Instance.GetQt("秽浊")                                           |
| 999    | TTKHelper.IsTargetTTK(Core.Me.GetCurrTarget(), BLMSetting.Instance.TTK阈值, false) |
| 666    | QT.Instance.GetQt("倾泻资源")                                        |
| 5      | BLMHelper.火状态 为 true 且 Core.Me.CurrentMp < 800 且 Spells.墨泉.GetSpell().Cooldown.TotalSeconds < 4 |
| 2      | BLMHelper.通晓层数 == 3 且 BLMHelper.通晓剩余时间 <= 6000             |
| 3      | BLMHelper.通晓层数 == 3 且 Spells.详述.GetSpell().Cooldown.TotalMilliseconds < 6000 |
| 1      | MoveHelper.IsMoving() 且 !BattleData.Instance.可瞬发                 |
| 4      | new 悖论().Check() == 6                                              |
| -99    | 以上条件均不满足                                                     |
