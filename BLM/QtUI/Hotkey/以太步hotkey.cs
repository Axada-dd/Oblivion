using System.Numerics;
using AEAssist.CombatRoutine.View.JobView;
using AEAssist.MemoryApi;
using Dalamud.Interface.Textures.TextureWraps;
using ImGuiNET;

namespace Oblivion.BLM.QtUI.Hotkey;

public class 以太步HotkeyResolver : IHotkeyResolver
{
    private uint SpellId;
    private int Index;

    public 以太步HotkeyResolver(int index)
    {
        this.SpellId = Skill.以太步;
        this.Index = index;
    }
    
    public void Draw(Vector2 size)
    {
        uint id = SpellId;
        Vector2 size1 = size * 0.8f;
        ImGui.SetCursorPos(size * 0.1f);
        IDalamudTextureWrap textureWrap;
        if (!Core.Resolve<MemApiIcon>().GetActionTexture(id, out textureWrap))
            return;
        ImGui.Image(textureWrap.ImGuiHandle, size1);
        
        if (SpellId.GetSpell().Cooldown.TotalMilliseconds > 0)
        {
            // Use ImGui.GetItemRectMin() and ImGui.GetItemRectMax() for exact icon bounds
            Vector2 overlayMin = ImGui.GetItemRectMin();
            Vector2 overlayMax = ImGui.GetItemRectMax();

            // Draw a grey overlay over the icon
            ImGui.GetWindowDrawList().AddRectFilled(
                overlayMin, 
                overlayMax, 
                ImGui.ColorConvertFloat4ToU32(new Vector4(0, 0, 0, 0.5f))); // 50% transparent grey
        }
        
        var cooldownRemaining = SpellId.GetSpell().Cooldown.TotalMilliseconds / 1000;
        if (cooldownRemaining > 0)
        {
            // Convert cooldown to seconds and format as string
            string cooldownText = Math.Ceiling(cooldownRemaining).ToString();

            // 计算文本位置，向左下角偏移
            Vector2 textPos = ImGui.GetItemRectMin();
            textPos.X -= 1; // 向左移动一点
            textPos.Y += size1.Y - ImGui.CalcTextSize(cooldownText).Y + 5; // 向下移动一点

            // 绘制冷却时间文本
            ImGui.GetWindowDrawList().AddText(textPos, ImGui.ColorConvertFloat4ToU32(new Vector4(1, 1, 1, 1)), cooldownText);
        }
    }

    public void DrawExternal(Vector2 size, bool isActive)
    {
        SpellHelper.DrawSpellInfo(Core.Resolve<MemApiSpell>().CheckActionChange(this.SpellId).GetSpell(), size, isActive);
    }

    public int Check()
    {
        if (!Skill.以太步.IsUnlockWithCDCheck())
            return -1;
        return 0;
    }

    public void Run()
    {
        ClosedPosition(index: this.Index);
        return;
    }
    
    private void ClosedPosition(int index)
    {
        var partyMembers = PartyHelper.Party;
        if (partyMembers.Count < index + 1)
            return;
        if (!Skill.以太步.IsUnlockWithCDCheck())
            return;
        
        if (!BattleData.Instance.HotkeyUseHighPrioritySlot)
        {
            if (AI.Instance.BattleData.NextSlot == null)
                AI.Instance.BattleData.NextSlot = new Slot(); 
            AI.Instance.BattleData.NextSlot.Add(new Spell(Skill.以太步,
                    partyMembers[index]));
        }
        else
        {
            Slot slot = new Slot();
            slot.Add(new Spell(Skill.以太步, partyMembers[index]));
            AI.Instance.BattleData.HighPrioritySlots_OffGCD.Enqueue(slot);
        }
    }
}