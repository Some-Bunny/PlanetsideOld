﻿using System;
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

namespace Planetside
{
	public class TinyPlanetBullets : PassiveItem
	{
		public static void Init()
		{
			string itemName = "Corrupt Bullets";
			string resourceName = "Planetside/Resources/sirpaler.png";
			GameObject obj = new GameObject(itemName);
			var item = obj.AddComponent<TinyPlanetBullets>();
			ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
			string shortDesc = "Downwards Spiral, Downwards Spiral...";
			string longDesc = "Warped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\nWarped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\nWarped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\nWarped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\nWarped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\nWarped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\nWarped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\nWarped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\n." +
				"Warped by a fracture in reality, these bullets no longer adhere to normal standards of movement.\n\n";
			ItemBuilder.SetupItem(item, shortDesc, longDesc, "psog");
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.Damage, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			ItemBuilder.AddPassiveStatModifier(item, PlayerStats.StatType.RangeMultiplier, 5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			item.quality = PickupObject.ItemQuality.D;
		}
		private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			try
			{
				sourceProjectile.OverrideMotionModule = new TinyPlanetMotionModule();
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
				ETGModConsole.Log("If you see this pop up, write a message in the comment section of ModWorkShop AND include a list of all items/guns you have/had during the run.");
			}
		}
		private void PostProcessBeam(BeamController obj)
		{

			try
			{
				obj.projectile.OverrideMotionModule = new TinyPlanetMotionModule();
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
				ETGModConsole.Log("If you see this pop up, write a message in the comment section of ModWorkShop AND include a list of all items/guns you have/had during the run.");
			}
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeam -= this.PostProcessBeam;
			return result;
		}
		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
			player.PostProcessBeam += this.PostProcessBeam;
		}

		protected override void OnDestroy()
		{
			base.Owner.PostProcessProjectile -= this.PostProcessProjectile;
			base.Owner.PostProcessBeam -= this.PostProcessBeam;
			base.OnDestroy();
		}
	}
}