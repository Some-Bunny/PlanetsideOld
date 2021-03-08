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

//Garbage Code Incoming
namespace Planetside
{
    public class ShellsnakeOil : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Shell-snake Oil";
            string resourceName = "Planetside/Resources/gunsnakeoil.png";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<ShellsnakeOil>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Works 103%!";
            string longDesc = "Harvested from elusive shell-snakes, This wonderful elixir is SURE to boost your firepower and strength in combat." +
                "\n\nSource: Trust us! We're completely honest!";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psog");
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.KnockbackMultiplier, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.PlayerBulletScale, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			item.quality = PickupObject.ItemQuality.C;

		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);	
			return result;
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}
	}
}


