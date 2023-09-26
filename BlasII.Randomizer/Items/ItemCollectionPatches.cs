using HarmonyLib;
using Il2CppPlaymaker.Characters;
using Il2CppPlaymaker.Inventory;
using Il2CppPlaymaker.Loot;
using Il2CppPlaymaker.PrieDieu;
using Il2CppPlaymaker.UI;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.Interactables;
using Il2CppTGK.Game.Inventory.PlayMaker;

namespace BlasII.Randomizer.Items
{
    // ===============
    // Item collection
    // ===============

    [HarmonyPatch(typeof(LootInteractable), nameof(LootInteractable.UseLootByInteractor))]
    class LootInteractable_Use_Patch
    {
        public static void Prefix(LootInteractable __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.l{__instance.transform.GetSiblingIndex()}";
            Main.Randomizer.LogError("LootInteractable.UseLootByInteractor - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.loot = null;
            }
        }
    }
    [HarmonyPatch(typeof(AddItem), nameof(AddItem.OnEnter))]
    class PlayMaker_AddItem_Patch
    {
        public static bool Prefix(AddItem __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.i{__instance.owner.transform.GetSiblingIndex()}";
            locationId = CalculateSpecialLocationId(locationId, __instance.itemID.name);
            Main.Randomizer.LogError($"AddItem.OnEnter - {locationId} ({__instance.itemID.name})");

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }

        private static string CalculateSpecialLocationId(string locationId, string originalItem)
        {
            // Battle arenas
            if (locationId.StartsWith("Z15"))
            {
                return originalItem switch
                {
                    "FG27" => "Z1501.s0",
                    "FG28" => "Z1501.s1",
                    "QI49" => "Z1501.s2",
                    "QI46" => "Z1501.s3",
                    "FG32" => "Z1501.s4",
                    _ => string.Empty
                };
            }

            // Palace elders
            if (locationId.StartsWith("Z0722"))
            {
                return originalItem switch
                {
                    "QI09" => "Z0722.s0",
                    "QI10" => "Z0722.s1",
                    _ => string.Empty
                };
            }

            return locationId;
        }
    }

    // =================
    // Defeat boss marks
    // =================

    [HarmonyPatch(typeof(GiveReward), nameof(GiveReward.OnEnter))]
    class PlayMaker_GiveReward_Patch
    {
        public static bool Prefix(GiveReward __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.r{__instance.owner.transform.GetSiblingIndex()}";
            Main.Randomizer.LogError("GiveReward.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    [HarmonyPatch(typeof(ShowOrbsRewardPopup), nameof(ShowOrbsRewardPopup.OnEnter))]
    class Marks_Skip_Patch
    {
        public static bool Prefix(ShowOrbsRewardPopup __instance)
        {
            __instance.Finish();
            return false;
        }
    }

    // =====================
    // Weapon unlock statues
    // =====================

    [HarmonyPatch(typeof(UnlockWeapon), nameof(UnlockWeapon.OnEnter))]
    class PlayMaker_UnlockWeapon_Patch
    {
        public static bool Prefix(UnlockWeapon __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.w0";
            Main.Randomizer.LogError("UnlockWeapon.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    [HarmonyPatch(typeof(ShowWeaponPopup), nameof(ShowWeaponPopup.OnEnter))]
    class WeaponFind_Skip_Patch
    {
        public static bool Prefix(ShowWeaponPopup __instance)
        {
            __instance.Finish();
            return false;
        }
    }

    // ======================
    // Weapon upgrade statues
    // ======================

    [HarmonyPatch(typeof(UpgradeWeaponTier), nameof(UpgradeWeaponTier.OnEnter))]
    class PlayMaker_UpgradeWeapon_Patch
    {
        public static bool Prefix(UpgradeWeaponTier __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.w0";
            Main.Randomizer.LogError("UpgradeWeaponTier.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    [HarmonyPatch(typeof(ShowWeaponTierPopup), nameof(ShowWeaponTierPopup.OnEnter))]
    class WeaponUpgrade_Skip_Patch
    {
        public static bool Prefix(ShowWeaponTierPopup __instance)
        {
            __instance.Finish();
            return false;
        }
    }

    // ======================
    // Ability unlock statues
    // ======================

    [HarmonyPatch(typeof(UnlockAbility), nameof(UnlockAbility.OnEnter))]
    class PlayMaker_UnlockAbility_Patch
    {
        public static bool Prefix(UnlockAbility __instance)
        {
            string locationId = $"{CoreCache.Room.CurrentRoom.Name}.a0";
            Main.Randomizer.LogError("UnlockAbility.OnEnter - " + locationId);

            if (Main.Randomizer.ItemHandler.IsLocationRandomized(locationId))
            {
                Main.Randomizer.ItemHandler.GiveItemAtLocation(locationId);
                __instance.Finish();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    [HarmonyPatch(typeof(ShowUnlockAbilityPopup), nameof(ShowUnlockAbilityPopup.OnEnter))]
    class Ability_Skip_Patch
    {
        public static bool Prefix(ShowUnlockAbilityPopup __instance)
        {
            __instance.Finish();
            return false;
        }
    }
}