using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Colors;
using Dalamud.Interface.Internal;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace WhatIHaveInFFXIVCN10thAnniv.Windows;

public unsafe class ConfigWindow() : Window("我到底在 10 周年庆活动里缺了啥", ImGuiWindowFlags.AlwaysAutoResize), IDisposable
{
    private static readonly Dictionary<uint, Award?> BuddyEquip = new()
    {
        { 36, null }
    };
    
    private static readonly Dictionary<uint, Award?> Pet = new()
    {
        { 78, null },
        { 51, null },
        { 161, null },
        { 182, null },
        { 251, null },
        { 317, null },
        { 320, null },
        { 416, null },
        { 417, null },
    };

    static ConfigWindow()
    {
        foreach (var kvp in BuddyEquip)
        {
            BuddyEquip[kvp.Key] = new()
                { 
                    ID = kvp.Key, 
                    Name = Service.Data.GetExcelSheet<BuddyEquip>().GetRow(kvp.Key).Name.RawString,
                    Icon = Service.Texture.GetIcon(Service.Data.GetExcelSheet<BuddyEquip>().GetRow(kvp.Key).IconBody)
                };
        }

        foreach (var kvp in Pet)
        {
            Pet[kvp.Key] = new()
            {
                ID   = kvp.Key,
                Name = Service.Data.GetExcelSheet<Companion>().GetRow(kvp.Key).Singular.RawString,
                Icon = Service.Texture.GetIcon(Service.Data.GetExcelSheet<Companion>().GetRow(kvp.Key).Icon)
            };
        }
    }

    public override void Draw()
    {
        ImGui.TextColored(ImGuiColors.DalamudOrange, "未解锁的奖励如下:");
        
        ImGui.SameLine();
        if (ImGui.SmallButton("刷新")) Refresh();

        ImGui.Spacing();

        using var table = ImRaii.Table("AwardsTable", 4, ImGuiTableFlags.NoBordersInBody);
        if (!table) return;
        foreach (var award in BuddyEquip.Values.Concat(Pet.Values))
        {
            if (award.IsGet) continue;

            ImGui.TableNextColumn();

            using (ImRaii.Group())
            {
                var columnWidth = ImGui.GetColumnWidth();
                var imagePosX   = (columnWidth - award.Icon.Size.X) * 0.5f;
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + imagePosX);
                ImGui.Image(award.Icon.ImGuiHandle, award.Icon.Size);
            
                var textWidth = ImGui.CalcTextSize(award.Name).X;
                var textPosX  = (columnWidth - textWidth) * 0.5f;
                ImGui.SetCursorPosX(ImGui.GetCursorPosX() + textPosX);
                ImGui.Text(award.Name);
            }
        }
    }
    private static void Refresh()
    {
        foreach (var award in BuddyEquip.Values)
        {
            BuddyEquip[award.ID].IsGet = UIState.Instance()->Buddy.CompanionInfo.IsBuddyEquipUnlocked(award.ID);
        }
        
        foreach (var award in Pet.Values)
        {
            Pet[award.ID].IsGet = UIState.Instance()->IsCompanionUnlocked(award.ID);
        }
    }
    
    public void Dispose() { }

    private class Award : IEquatable<Award>
    {
        public uint                 ID    { get; set; }
        public string?              Name  { get; set; }
        public IDalamudTextureWrap? Icon  { get; set; }
        public bool                 IsGet { get; set; } = true;

        public bool Equals(Award? other)
        {
            if(other is null) return false;
            if(ReferenceEquals(this, other)) return true;
            return ID == other.ID;
        }
    }
}
