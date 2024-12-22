﻿using BlasII.Framework.Menus;
using BlasII.Framework.Menus.Options;
using UnityEngine;

namespace BlasII.Randomizer.Services;

public class RandomizerMenu : ModMenu
{
    /// <inheritdoc/>
    protected override int Priority { get; } = 100;

    /// <summary>
    /// Stores or loads the entire settings menu into or from a settings object
    /// </summary>
    private RandomizerSettings MenuSettings
    {
        get
        {
            return new RandomizerSettings()
            {
                Seed = _setSeed.CurrentNumericValue == 0 ? RandomizerSettings.RANDOM_SEED : _setSeed.CurrentNumericValue,
                LogicType = 1,
                RequiredKeys = _setRequiredKeys.CurrentOption,
                StartingWeapon = _setStartingWeapon.CurrentOption,
                ShuffleLongQuests = _setShuffleLongQuests.Toggled,
                ShuffleShops = _setShuffleShops.Toggled,
            };
        }
        set
        {
            _setLogicDifficulty.CurrentOption = 0;
            _setRequiredKeys.CurrentOption = value.RequiredKeys;
            _setStartingWeapon.CurrentOption = value.StartingWeapon;
            _setShuffleLongQuests.Toggled = value.ShuffleLongQuests;
            _setShuffleShops.Toggled = value.ShuffleShops;

            _setSeed.CurrentValue = string.Empty;
        }
    }

    /// <summary>
    /// Set the menu options to default when opening it
    /// </summary>
    public override void OnStart()
    {
        MenuSettings = RandomizerSettings.DEFAULT;
    }

    /// <summary>
    /// Update the randomizer settings when closing it
    /// </summary>
    public override void OnFinish()
    {
        Main.Randomizer.CurrentSettings = MenuSettings;
    }

    protected override void CreateUI(Transform ui)
    {
        var toggle = new ToggleCreator(this)
        {
            BoxSize = 55,
            TextColor = SILVER,
            TextSize = TEXT_SIZE,
        };

        var arrow = new ArrowCreator(this)
        {
            ArrowSize = 55,
            TextColor = SILVER,
            TextColorAlt = YELLOW,
            TextSize = TEXT_SIZE,
        };

        var text = new TextCreator(this)
        {
            LineSize = 150,
            TextColor = SILVER,
            TextColorAlt = YELLOW,
            TextSize = TEXT_SIZE,
        };

        _setSeed = text.CreateOption("Seed", ui, new Vector2(0, 300), "option/seed", true, false, 6);

        _setLogicDifficulty = arrow.CreateOption("LD", ui, new Vector2(-300, 80), "option/logic", new string[]
        {
            "option/logic/normal",
        });

        _setRequiredKeys = arrow.CreateOption("RQ", ui, new Vector2(-300, -80), "option/keys", new string[]
        {
            "option/random",
            "option/keys/zero",
            "option/keys/one",
            "option/keys/two",
            "option/keys/three",
            "option/keys/four",
            "option/keys/five",
        });

        _setStartingWeapon = arrow.CreateOption("SW", ui, new Vector2(-300, -240), "option/weapon", new string[]
        {
            "option/random",
            "option/weapon/censer",
            "option/weapon/rosary",
            "option/weapon/rapier",
            "option/weapon/meaculpa",
        });

        _setShuffleLongQuests = toggle.CreateOption("SL", ui, new Vector2(150, 70), "option/long");

        _setShuffleShops = toggle.CreateOption("SS", ui, new Vector2(150, -10), "option/shops");
    }

    private const int TEXT_SIZE = 55;
    private readonly Color SILVER = new Color32(192, 192, 192, 255);
    private readonly Color YELLOW = new Color32(255, 231, 65, 255);

    private TextOption _setSeed;

    private ArrowOption _setLogicDifficulty;
    private ArrowOption _setRequiredKeys;
    private ArrowOption _setStartingWeapon;

    private ToggleOption _setShuffleLongQuests;
    private ToggleOption _setShuffleShops;
}
