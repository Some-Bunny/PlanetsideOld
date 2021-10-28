﻿using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;
using SaveAPI;
using Brave.BulletScript;

namespace Planetside
{
	public class DiamondChamber : PassiveItem
	{
		public static void Init()
		{
			string name = "Diamond Chamber";
			string resourcePath = "Planetside/Resources/brokenchamberfixed.png";
			GameObject gameObject = new GameObject(name);
			DiamondChamber chamber = gameObject.AddComponent<DiamondChamber>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Purity";
			string longDesc = "A chamber made of pure diamond. It glows with a shine unseen.\n\nYou feel at peace.";
			ItemBuilder.SetupItem(chamber, shortDesc, longDesc, "psog");
			chamber.quality = PickupObject.ItemQuality.S;
			chamber.SetupUnlockOnCustomFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED, true);
			DiamondChamber.DiamondChamberID = chamber.PickupObjectId;
			ItemIDs.AddToList(chamber.PickupObjectId);
		}
		public static int DiamondChamberID;

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			return result;
		}
		protected override void Update()
		{
			base.Update();
			bool flag = base.Owner != null;
			if (flag)
			{
				this.CalculateStats(base.Owner);
				bool Q = SaveAPIManager.GetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED);
				bool W = SaveAPIManager.GetFlag(CustomDungeonFlags.BEAT_LOOP_1);
				bool E = SaveAPIManager.GetFlag(CustomDungeonFlags.BEAT_A_BOSS_UNDER_A_SECOND);
				bool R = SaveAPIManager.GetFlag(CustomDungeonFlags.BULLETBANK_DEFEATED);
				bool T = SaveAPIManager.GetFlag(CustomDungeonFlags.HIGHER_CURSE_DRAGUN_KILLED);
				bool Y = SaveAPIManager.GetFlag(CustomDungeonFlags.SHELLRAX_DEFEATED);
				bool U = SaveAPIManager.GetFlag(CustomDungeonFlags.JAMMED_GUARD_DEFEATED);
				bool I = SaveAPIManager.GetFlag(CustomDungeonFlags.DEFEAT_FUNGANNON);
				bool O = SaveAPIManager.GetFlag(CustomDungeonFlags.DEFEAT_OPHANAIM);
				bool P = SaveAPIManager.GetFlag(CustomDungeonFlags.DEFEAT_ANNIHICHAMBER);
				bool A = SaveAPIManager.GetFlag(CustomDungeonFlags.DECURSE_HELL_SHRINE_UNLOCK);
				bool S = SaveAPIManager.GetFlag(CustomDungeonFlags.HAS_COMPLETED_SOMETHING_WICKED);
				if (Q == true && W == true && E == true && R == true && T == true && Y == true && U == true && I == true && O == true && P == true && A == true && S == true)
                {
					AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.PLATE_DIAMOND_CHAMBER, true);
					base.Owner.RemovePassiveItem(DiamondChamber.DiamondChamberID);
					LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items["Netherite Chamber"].gameObject, base.Owner, true);

				}
			}
		}
		private void CalculateStats(PlayerController player)
		{
			this.currentItems = player.passiveItems.Count;
			bool flag = this.currentItems != this.lastItems;
			if (flag)
			{
				this.RemoveStat(PlayerStats.StatType.Health);
				this.RemoveStat(PlayerStats.StatType.Damage);
				foreach (PassiveItem passiveItem in player.passiveItems)
				{
					bool flag2 = passiveItem is BasicStatPickup && (passiveItem as BasicStatPickup).IsMasteryToken;
					if (flag2)
					{
						this.AddStat(PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
					}
					string encounterNameOrDisplayName = passiveItem.EncounterNameOrDisplayName;
					bool fucker = encounterNameOrDisplayName.Contains("Chamber") || encounterNameOrDisplayName.Contains("chamber");
					if (fucker)
					{
						this.AddStat(PlayerStats.StatType.Damage, .2f, StatModifier.ModifyMethod.ADDITIVE);
					}
				}

					this.lastItems = this.currentItems;
				player.stats.RecalculateStats(player, true, false);
			}
		}
		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			StatModifier statModifier = new StatModifier
			{
				amount = amount,
				statToBoost = statType,
				modifyType = method
			};
			bool flag = this.passiveStatModifiers == null;
			if (flag)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier
				};
			}
			else
			{
				this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
				{
					statModifier
				}).ToArray<StatModifier>();
			}
		}
		private void RemoveStat(PlayerStats.StatType statType)
		{
			List<StatModifier> list = new List<StatModifier>();
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				bool flag = this.passiveStatModifiers[i].statToBoost != statType;
				if (flag)
				{
					list.Add(this.passiveStatModifiers[i]);
				}
			}
			this.passiveStatModifiers = list.ToArray();
		}
		private int currentItems;
		private int lastItems;
	}
}
