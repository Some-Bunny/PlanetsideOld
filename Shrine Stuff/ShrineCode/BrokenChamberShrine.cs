
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

	public static class BrokenChamberShrine
	{

		public static void Add()
		{
			float Weight = 0.5f;
			bool Shrine = SaveAPIManager.GetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED);
			if (Shrine != true)
            {
				Weight *= 10;
				OldShrineFactory iei = new OldShrineFactory
				{
					name = "Broken Chamber Shrine",
					modID = "psog",
					text = "A shrine with a half-broken chamber on it. It's seems loose...",
					spritePath = "Planetside/Resources/Shrines/brokenchambershrine.png",
					room = RoomFactory.BuildFromResource("Planetside/ShrineRooms/BrokenChamberRoom.room").room,
					RoomWeight = Weight,
					acceptText = "Lift the remnant.",
					declineText = "Leave.",
					OnAccept = Accept,
					OnDecline = null,
					CanUse = CanUse,
					offset = new Vector3(0, 0, 0),
					talkPointOffset = new Vector3(0, 3, 0),
					isToggle = false,
					isBreachShrine = false,

				};
				iei.Build();

			}
			else
            {
				Weight = 1;
				OldShrineFactory iei = new OldShrineFactory
				{
					name = "End Of Everything Shrine",
					modID = "psog",
					text = "A shrine with 4 engravings carved onto it. Although the engravings shift, you can slightly make out what they are...",
					spritePath = "Planetside/Resources/Shrines/EOEShrine.png",
					room = RoomFactory.BuildFromResource("Planetside/ShrineRooms/EOEShrineRoom.room").room,
					RoomWeight = Weight,
					acceptText = "Kneel.",
					declineText = "Leave.",
					OnAccept = Accept,
					OnDecline = null,
					CanUse = CanUse,
					offset = new Vector3(0, 0, 0),
					talkPointOffset = new Vector3(0, 3, 0),
					isToggle = false,
					isBreachShrine = false,

				};
				iei.Build();
			}


		}
		public static bool CanUse(PlayerController player, GameObject shrine)
		{
			return shrine.GetComponent<CustomShrineController>().numUses == 0;
		}

		public static void Accept(PlayerController player, GameObject shrine)
		{
			bool Shrine = SaveAPIManager.GetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED);
			if (Shrine != true)
            {
				AkSoundEngine.PostEvent("Play_ENM_darken_world_01", shrine);
				LootEngine.TryGivePrefabToPlayer(ETGMod.Databases.Items["Broken Chamber"].gameObject, player, true);
				shrine.GetComponent<CustomShrineController>().numUses++;
				shrine.GetComponent<CustomShrineController>().GetRidOfMinimapIcon();
			}
			else
            {
				Gun gun = PickupObjectDatabase.GetByEncounterName(InitialiseGTEE.GunIDForEOE) as Gun;
				int StoredGunID = gun.PickupObjectId;

				//PickupObject Item1 = PickupObjectDatabase.GetByName(InitialiseGTEE.HOneToFireIt);
				int Item1ID = Game.Items[InitialiseGTEE.HOneToFireIt].PickupObjectId;
				int Item2ID = Game.Items[InitialiseGTEE.HOneToPrimeIt].PickupObjectId;
				int Item3ID = Game.Items[InitialiseGTEE.HOneToHoldIt].PickupObjectId;

				string encounterNameOrDisplayName1 = (PickupObjectDatabase.GetById(StoredGunID) as Gun).EncounterNameOrDisplayName;
				string encounterNameOrDisplayName2 = (PickupObjectDatabase.GetById(Item1ID)).EncounterNameOrDisplayName;
				string encounterNameOrDisplayName3 = (PickupObjectDatabase.GetById(Item2ID)).EncounterNameOrDisplayName;
				string encounterNameOrDisplayName4 = (PickupObjectDatabase.GetById(Item3ID)).EncounterNameOrDisplayName;

				string header;
				string text;
				
				header = encounterNameOrDisplayName1 + " / " + encounterNameOrDisplayName2;
				text = "Filler.";
				BrokenChamberShrine.Notify(header, text);
				
				
				header = encounterNameOrDisplayName3 + " `/ " + encounterNameOrDisplayName4;
				text = "Filler.";
				BrokenChamberShrine.Notify(header, text);
				

				shrine.GetComponent<CustomShrineController>().numUses++;
				shrine.GetComponent<CustomShrineController>().GetRidOfMinimapIcon();
			}
		}
		private static void Notify(string header, string text)
		{
			tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
			int spriteIdByName = encounterIconCollection.GetSpriteIdByName("Planetside/Resources/shellheart");
			GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, null, spriteIdByName, UINotificationController.NotificationColor.PURPLE, true, true);
		}
	}
}



