using Items;
using System;
using System.Collections.Generic;

public static class Constants {

    // Tags
    public const string PlayerTag = "Player";
    public const string EnemyTag = "Enemy";

    // Scene Names
    public const string SnMenu = "Menu";
    public const string SnGame = "Game";

    // Event Names
    public const string ShowTipEvent = "ShowTip";
    public const string HideTipEvent = "HideTip";
    public const string InventoryKeyPressedEvent = "InventoryKeyPressed";
    public const string PauseKeyPressedEvent = "PauseKeyPressed";

    // Numerical Constants
    public const float MaxReputation = 100.0f;
    public const int InventoryMaxSize = 20;

    // In-Game Text
    public const string OpenContainerText = "Loot";
    public const string CloseContainerText = "Close";
    public const string InteractWithText = "Use";
    public const string CollectItemText = "Take";
    public const string PlayerContainerName = "Player";

    public const string ContainerTake = "Take";
    public const string ContainerStash = "Stash";

    public static readonly Dictionary<Type, string> ItemTypeToUseBtnText = new Dictionary<Type, string> {
        { typeof(Weapon), "Equip" },
        { typeof(Armor), "Wear" },
        { typeof(Consumable), "Consume" }
    };

}