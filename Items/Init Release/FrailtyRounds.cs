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
using SaveAPI;

//Garbage Code Incoming
namespace Planetside
{
    public class FrailtyRounds : PassiveItem
    {
        public static void Init()
        {
            string itemName = "Frailty Rounds";
            string resourceName = "Planetside/Resources/frailtyrounds.png";
            GameObject obj = new GameObject(itemName);
            var item = obj.AddComponent<FrailtyRounds>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Annealling";
            string longDesc = "These bullets have been tipped with a very exotic toxin that erodes at the enemies resistance to damage." +
                "\n\nHarvested from cute, but rare species of poison round frogs that reside in a hidden chamber of the Gungeon.";
            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psog");
			item.quality = PickupObject.ItemQuality.B;
			item.SetupUnlockOnCustomFlag(CustomDungeonFlags.HIGHER_CURSE_DRAGUN_KILLED, true);

			FrailtyRounds.FrailtyRoundsID = item.PickupObjectId;
			ItemIDs.AddToList(item.PickupObjectId);

		}
		public static int FrailtyRoundsID;
		private void PostProcessProjectile(Projectile sourceProjectile, float effectChanceScalar)
		{
			try
			{
				sourceProjectile.AppliesPoison = true;
				sourceProjectile.PoisonApplyChance = 0.2f;
				sourceProjectile.healthEffect = DebuffLibrary.Frailty;
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
			}
		}

		private void PostProcessBeamTick(BeamController beam, SpeculativeRigidbody hitRigidBody, float tickrate)
		{
			/*
            foreach (Component item in beam.GetComponentsInChildren(typeof(Component)))
            {
                ETGModConsole.Log(item.GetType().ToString());
            }
			*/
			float procChance = 0.25f; //Chance per second or some shit idk
			GameActor gameActor = hitRigidBody.gameActor;
			if (!gameActor)
			{
				return;
			}
			if (UnityEngine.Random.value < BraveMathCollege.SliceProbability(procChance, tickrate))
			{
				hitRigidBody.gameActor.ApplyEffect(DebuffLibrary.Frailty);
			}
		}

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
			player.PostProcessProjectile += this.PostProcessProjectile;
			player.PostProcessBeamTick += this.PostProcessBeamTick;
		}
		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			player.PostProcessProjectile -= this.PostProcessProjectile;
			player.PostProcessBeamTick -= this.PostProcessBeamTick;

			return result;
		}

		protected override void OnDestroy()
		{
			if (base.Owner != null)
            {
				base.Owner.PostProcessProjectile -= this.PostProcessProjectile;
				base.Owner.PostProcessBeamTick -= this.PostProcessBeamTick;
			}
			base.OnDestroy();
		}
	}
}


