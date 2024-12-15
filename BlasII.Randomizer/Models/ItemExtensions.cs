﻿using BlasII.ModdingAPI.Assets;
using Il2CppTGK.Game;
using System;
using UnityEngine;

namespace BlasII.Randomizer.Models;

/// <summary>
/// Provides functionality for the Item model
/// </summary>
public static class ItemExtensions
{
    /// <summary>
    /// Retrieves the sprite for this item
    /// </summary>
    public static Sprite GetSprite(this Item item)
    {
        return item.Type switch
        {
            Item.ItemType.RosaryBead => AssetStorage.Beads[item.Id].image,
            Item.ItemType.Prayer => AssetStorage.Prayers[item.Id].image,
            Item.ItemType.Figurine => AssetStorage.Figures[item.Id].image,
            Item.ItemType.QuestItem => AssetStorage.QuestItems[item.Id].image,
            Item.ItemType.Weapon => Main.Randomizer.IconStorage.GetImage(item.Id),
            Item.ItemType.Ability => Main.Randomizer.IconStorage.GetImage(item.Id),
            Item.ItemType.Cherub => Main.Randomizer.IconStorage.GetImage("Cherub"),
            Item.ItemType.Tears => Main.Randomizer.IconStorage.GetImage("Tears"),
            Item.ItemType.Marks => Main.Randomizer.IconStorage.GetImage("Marks"),
            Item.ItemType.PreMarks => Main.Randomizer.IconStorage.GetImage("PreMarks"),

            // Fix!!!
            Item.ItemType.Invalid => Main.Randomizer.IconStorage.GetImage("Invalid"),
            _ => null,
        };
    }

    /// <summary>
    /// Retrieves the display name for this item
    /// </summary>
    public static string GetName(this Item item)
    {
        return item.Type switch
        {
            Item.ItemType.RosaryBead => AssetStorage.Beads[item.Id].caption,
            Item.ItemType.Prayer => AssetStorage.Prayers[item.Id].caption,
            Item.ItemType.Figurine => AssetStorage.Figures[item.Id].caption,
            Item.ItemType.QuestItem => AssetStorage.QuestItems[item.Id].caption,
            Item.ItemType.Weapon => Main.Randomizer.LocalizationHandler.Localize($"{item.Id}.name"),
            Item.ItemType.Ability => Main.Randomizer.LocalizationHandler.Localize($"{item.Id}.name"),
            Item.ItemType.Cherub => Main.Randomizer.LocalizationHandler.Localize("Cherub.name"),
            Item.ItemType.Tears => item.GetAmount() + " " + Main.Randomizer.LocalizationHandler.Localize("Tears.name"),
            Item.ItemType.Marks => item.GetAmount() + " " + Main.Randomizer.LocalizationHandler.Localize("Marks.name"),
            Item.ItemType.PreMarks => item.GetAmount() + " " + Main.Randomizer.LocalizationHandler.Localize("PreMarks.name"),

            Item.ItemType.Invalid => "Invalid Item",
            _ => null,
        };
    }

    /// <summary>
    /// Retrieves the description of this item
    /// </summary>
    public static string GetDescription(this Item item)
    {
        return item.Type switch
        {
            Item.ItemType.RosaryBead => AssetStorage.Beads[item.Id].description,
            Item.ItemType.Prayer => AssetStorage.Prayers[item.Id].description,
            Item.ItemType.Figurine => AssetStorage.Figures[item.Id].description,
            Item.ItemType.QuestItem => AssetStorage.QuestItems[item.Id].description,
            Item.ItemType.Weapon => Main.Randomizer.LocalizationHandler.Localize($"{item.Id}.desc"),
            Item.ItemType.Ability => Main.Randomizer.LocalizationHandler.Localize($"{item.Id}.desc"),
            Item.ItemType.Cherub => Main.Randomizer.LocalizationHandler.Localize("Cherub.desc"),
            Item.ItemType.Tears => Main.Randomizer.LocalizationHandler.Localize("Tears.desc"),
            Item.ItemType.Marks => Main.Randomizer.LocalizationHandler.Localize("Marks.desc"),
            Item.ItemType.PreMarks => Main.Randomizer.LocalizationHandler.Localize("PreMarks.desc"),

            Item.ItemType.Invalid => "You should not see this.",
            _ => null,
        };
    }

    /// <summary>
    /// Retrieves the numeric value of this item
    /// </summary>
    private static int GetAmount(this Item item)
    {
        int leftBracket = item.Id.IndexOf('['), rightBracket = item.Id.IndexOf(']');
        return int.Parse(item.Id.Substring(leftBracket + 1, rightBracket - leftBracket - 1));
    }

    /// <summary>
    /// Grants the item
    /// </summary>
    public static void GiveReward(this Item item)
    {
        switch (item.Type)
        {
            case Item.ItemType.RosaryBead:
                {
                    if (AssetStorage.Beads.TryGetValue(item.Id, out var bead))
                        AssetStorage.PlayerInventory.AddItemAsync(bead, 0, true);
                    break;
                }
            case Item.ItemType.Prayer:
                {
                    if (AssetStorage.Prayers.TryGetValue(item.Id, out var prayer))
                        AssetStorage.PlayerInventory.AddItemAsync(prayer, 0, true);
                    break;
                }
            case Item.ItemType.Figurine:
                {
                    if (AssetStorage.Figures.TryGetValue(item.Id, out var figure))
                        AssetStorage.PlayerInventory.AddItemAsync(figure, 0, true);
                    break;
                }
            case Item.ItemType.QuestItem:
                {
                    if (AssetStorage.QuestItems.TryGetValue(item.Id, out var quest))
                        AssetStorage.PlayerInventory.AddItemAsync(quest, 0, true);
                    break;
                }
            case Item.ItemType.Weapon:
                {
                    var weapon = AssetStorage.Weapons[Enum.Parse<WEAPON_IDS>(item.Id)];

                    if (CoreCache.EquipmentManager.IsUnlocked(weapon))
                    {
                        // Upgrade the weapon
                        CoreCache.WeaponMemoryManager.UpgradeWeaponTier(weapon);
                    }
                    else
                    {
                        // Unlock the weapon and give the switching ability
                        var ability = AssetStorage.Abilities[ABILITY_IDS.ChangeWeapon];
                        CoreCache.EquipmentManager.Unlock(weapon);
                        CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                    }
                    break;
                }
            case Item.ItemType.Ability:
                {
                    var ability = AssetStorage.Abilities[Enum.Parse<ABILITY_IDS>(item.Id)];
                    CoreCache.AbilitiesUnlockManager.SetAbility(ability, true);
                    break;
                }
            case Item.ItemType.Cherub:
                {
                    int currentCherubs = Main.Randomizer.GetQuestInt("ST16", "FREED_CHERUBS");
                    Main.Randomizer.SetQuestValue("ST16", "FREED_CHERUBS", currentCherubs + 1);
                    break;
                }
            case Item.ItemType.Tears:
                {
                    AssetStorage.PlayerStats.AddToCurrentValue(AssetStorage.ValueStats["Tears"], item.GetAmount());
                    break;
                }
            case Item.ItemType.Marks:
                {
                    AssetStorage.PlayerStats.AddToCurrentValue(AssetStorage.ValueStats["Orbs"], item.GetAmount());
                    break;
                }
            case Item.ItemType.PreMarks:

                {
                    AssetStorage.PlayerStats.AddToCurrentValue(AssetStorage.ValueStats["MarksPreceptor"], item.GetAmount());
                    break;
                }
        }

        Main.Randomizer.ItemHandler.SetItemCollected(item.Id);
    }
}
