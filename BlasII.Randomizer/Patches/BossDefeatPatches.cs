﻿using BlasII.ModdingAPI;
using BlasII.Randomizer.Models;
using HarmonyLib;
using Il2CppTGK.Game.Components.Animation.AnimatorManagement;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components.UI;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game.PlayerSpawn;
using UnityEngine;

namespace BlasII.Randomizer.Patches;

/// <summary>
/// When attempting to load a post-boss room, reload the same room instead
/// </summary>
[HarmonyPatch(typeof(PlayerSpawnManager), nameof(PlayerSpawnManager.TeleportPlayer), typeof(SceneEntryID), typeof(bool), typeof(PlaybackKey))]
class PlayerSpawnManager_TeleportPlayer_Patch1
{
    public static void Prefix(ref SceneEntryID sceneEntry)
    {
        ModLog.Info($"Teleporting1 to: {sceneEntry.scene} ({sceneEntry.entryId})");
        string currentScene = CoreCache.Room.CurrentRoom?.Name;

        if (!sceneEntry.scene.StartsWith("Z15"))
            return;

        if (!Main.Randomizer.ExtraInfoStorage.TryGetBossTeleportInfo(currentScene, out BossTeleportInfo info))
            return;

        // Trying to teleport out of a boss room to a dream room
        Main.Randomizer.SetQuestValue("ST00", "DREAM_RETURN", true);
        sceneEntry = new SceneEntryID()
        {
            scene = info.EntryScene,
            entryId = info.EntryDoor
        };
    }
}
[HarmonyPatch(typeof(PlayerSpawnManager), nameof(PlayerSpawnManager.TeleportPlayer), typeof(SceneEntryID), typeof(float), typeof(float), typeof(bool), typeof(PlaybackKey))]
class PlayerSpawnManager_TeleportPlayer_Patch2
{
    public static void Prefix(ref SceneEntryID sceneEntry)
    {
        ModLog.Info($"Teleporting2 to: {sceneEntry.scene} ({sceneEntry.entryId})");
        string currentScene = CoreCache.Room.CurrentRoom?.Name;

        if (!sceneEntry.scene.StartsWith("Z15"))
            return;

        if (!Main.Randomizer.ExtraInfoStorage.TryGetBossTeleportInfo(currentScene, out BossTeleportInfo info))
            return;

        // Trying to teleport out of a boss room to a dream room
        Main.Randomizer.SetQuestValue("ST00", "DREAM_RETURN", true);
        sceneEntry = new SceneEntryID()
        {
            scene = info.EntryScene,
            entryId = info.EntryDoor
        };
    }
}

/// <summary>
/// When reloading a boss room after the fight, force deactivate it to prevent camera lock
/// </summary>
[HarmonyPatch(typeof(RoomManager), nameof(RoomManager.ChangeRoom))]
class Room_Change_Patch
{
    public static void Prefix(int roomHash, ref bool forceDeactivate)
    {
        if (!Main.Randomizer.ExtraInfoStorage.TryGetBossTeleportInfo(roomHash, out BossTeleportInfo info))
            return;

        ModLog.Info("Force deactivating boss room: " + info.ForceDeactivate);
        forceDeactivate = info.ForceDeactivate;
    }
}

/// <summary>
/// Always replace fade current color with black - prevents fade being locked to white after boss defeat
/// </summary>
[HarmonyPatch(typeof(FadeWindowLogic), nameof(FadeWindowLogic.FadeToCurrentColorAsync))]
class FadeWindowLogic_FadeToCurrentColorAsync_Patch
{
    public static void Prefix(FadeWindowLogic __instance)
    {
        __instance.colorImage.color = new Color(0, 0, 0, __instance.colorImage.color.a);
    }
}
