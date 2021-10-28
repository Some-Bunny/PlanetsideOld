﻿
using GungeonAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static GungeonAPI.OldShrineFactory;
using Dungeonator;
using System.Reflection;
using MonoMod.RuntimeDetour;
using Brave.BulletScript;
using System.Collections;
using SaveAPI;
using Gungeon;

namespace Planetside
{

	public static class RedShrine
	{

		public static void Add()
		{
			OldShrineFactory iei = new OldShrineFactory
			{
				name = "RedShrine",
				modID = "psog",
				text = "A red pedestal with a hole in it.",
				spritePath = "Planetside/Resources/Shrines/SWMines/RedShrineEmpty.png",
				acceptText = "Insert The Red Casing.",
				declineText = "Leave It.",
				OnAccept = Accept,
				OnDecline = null,

				CanUse = CanUse,
				offset = new Vector3(0, 0, 0),
				talkPointOffset = new Vector3(0, 3, 0),
				isToggle = false,
				isBreachShrine = false,

			};
			GameObject self = iei.Build();
			SpriteID = ItemAPI.SpriteBuilder.AddSpriteToCollection("Planetside/Resources/Shrines/SWMines/RedShrine.png", self.GetComponent<tk2dBaseSprite>().Collection);
		}
		private static int SpriteID;
		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			return shrine.GetComponent<CustomShrineController>().numUses == 0 && player.HasPickupID(RedThing.RedCasingID);
		}

		public static void Accept(PlayerController player, GameObject shrine)
		{
			if (AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.HAS_COMPLETED_SOMETHING_WICKED) != true)
            {
				OtherTools.Notify("A RIFT IN TIME", "Opens in the distant future.", "Planetside/Resources/ShrineIcons/DarknessIcon");
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.HAS_COMPLETED_SOMETHING_WICKED, true);
				
				
			}
			for (int i = 0; i < EncounterDatabase.Instance.Entries.Count; i++)
			{
				if (EncounterDatabase.Instance.Entries[i].journalData.PrimaryDisplayName == "#WICKEDNESS")
				{
					GameStatsManager.Instance.HandleEncounteredObjectRaw(EncounterDatabase.Instance.Entries[i].myGuid);

				}
			}
			OtherTools.Notify("THE EXIT", "Unblocks.", "Planetside/Resources/ShrineIcons/DarknessIcon");
			AkSoundEngine.PostEvent("Play_OBJ_lock_unlock_01", shrine.gameObject);
			player.RemovePassiveItem(RedThing.RedCasingID);
			shrine.GetComponent<CustomShrineController>().numUses++;
			shrine.GetComponent<CustomShrineController>().GetRidOfMinimapIcon();
			SimpleShrine shrineonj = shrine.GetComponent<SimpleShrine>();
			shrineonj.text = "The red casing sits in the hole snugly. You feel like you can leave now.";
			shrineonj.acceptText = "Kneel.";
			shrineonj.declineText = "Leave, and head for the Exit.";
			tk2dSprite sprite = shrine.GetComponent<tk2dSprite>();
			sprite.GetComponent<tk2dBaseSprite>().SetSprite(SpriteID);
			LootEngine.DoDefaultPurplePoof(sprite.WorldCenter);
			foreach(GameObject obj in Thing.ExtantShrines)
            {
				UnityEngine.GameObject.Destroy(obj);
			}
			Thing.ExtantShrines.Clear();
		}
	}
}



