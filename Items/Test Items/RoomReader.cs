using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Reflection;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using MonoMod.RuntimeDetour;
using MonoMod;
using System.Collections.ObjectModel;

using UnityEngine.Serialization;
using MonoMod.Utils;
using Brave.BulletScript;
using GungeonAPI;
using SaveAPI;

using System.IO;
using Planetside;
using FullInspector.Internal;

namespace Planetside
{
    public class RoomReader : PlayerItem
    {
        public static void Init()
        {
            string itemName = "RoomReader";
            string resourceName = "Planetside/Resources/blashshower.png";
            GameObject obj = new GameObject(itemName);
            TestActiveItem testActive = obj.AddComponent<TestActiveItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Reads Rooms";
            string longDesc = "Prints The Name Of The Current Room Into The F2 Console";
            testActive.SetupItem(shortDesc, longDesc, "psog");
            testActive.SetCooldownType(ItemBuilder.CooldownType.Timed, 1f);
            testActive.consumable = false;
            ItemBuilder.AddPassiveStatModifier(testActive, PlayerStats.StatType.AdditionalItemCapacity, 1f, StatModifier.ModifyMethod.ADDITIVE);
            testActive.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        

        protected override void DoEffect(PlayerController user)
        {
            ETGModConsole.Log(user.CurrentRoom.GetRoomName());
           
        }
    }
}



