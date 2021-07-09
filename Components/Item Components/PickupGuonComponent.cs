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
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using Brave.BulletScript;
using GungeonAPI;
using SpriteBuilder = ItemAPI.SpriteBuilder;
using DirectionType = DirectionalAnimation.DirectionType;
using static DirectionalAnimation;

namespace Planetside
{
	public class PickupGuonComponent : MonoBehaviour
	{
		public PickupGuonComponent()
		{
			this.HitsBeforeDeath = 10;
			this.player = GameManager.Instance.PrimaryPlayer;
			this.Hits = 0;

			this.IsAmmo = false;
			this.IsHalfAmmo = false;
			this.IsHeart = false;
			this.IsHalfHeart = false;
			this.IsBlank = false;
			this.IsKey = false;
			this.IsArmor = false;

		}

		public void Awake()
		{
			this.actor = base.GetComponent<PlayerOrbital>();
			this.player = base.GetComponent<PlayerController>();

		}
		public static Hook guonHook;

		public void Start()
		{
			LootEngine.DoDefaultItemPoof(actor.transform.position, false, false);
			PickupGuonComponent.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(GunknownGuonComponent).GetMethod("GuonInit"));
			PlayerController player = GameManager.Instance.PrimaryPlayer;
			if (this.actor == null)
            {
				this.actor = base.GetComponent<PlayerOrbital>();
			}
			if (this.player == null)
			{
				this.player = base.GetComponent<PlayerController>();
			}
			PlayerOrbital playerOrbital2 = actor;
			SpeculativeRigidbody specRigidbody = playerOrbital2.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));

		}
		public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
		{
			orig(self, player);
		}
		private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
		{

			GameObject silencerVFX = (GameObject)ResourceCache.Acquire("Global VFX/BlankVFX_Ghost");
			PlayerController player = this.player;

			Hits++;
			if (Hits == HitsBeforeDeath)
            {
				LootEngine.DoDefaultItemPoof(actor.sprite.WorldCenter, false, true);
				UnityEngine.Object.Destroy(base.gameObject);
				if (IsBlank == true)
				{
					AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
					GameObject gameObject = new GameObject("silencer");
					SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
					float additionalTimeAtMaxRadius = 0.25f;
					silencerInstance.TriggerSilencer(myRigidbody.sprite.WorldCenter, 25f, 3f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, player, false, false);
				}
			}
			if (IsBlank == true)
			{
				this.random = UnityEngine.Random.Range(0.0f, 1.0f);
				if (random <= 0.05f)
                {
					AkSoundEngine.PostEvent("Play_OBJ_silenceblank_small_01", base.gameObject);
					GameObject gameObject = new GameObject("silencer");
					SilencerInstance silencerInstance = gameObject.AddComponent<SilencerInstance>();
					float additionalTimeAtMaxRadius = 0.25f;
					silencerInstance.TriggerSilencer(myRigidbody.sprite.WorldCenter, 25f, 3f, silencerVFX, 0f, 3f, 3f, 3f, 250f, 5f, additionalTimeAtMaxRadius, player, false, false);
				}
			}
			if (IsAmmo == true | IsHalfAmmo == true)
			{
				Projectile proj = other.GetComponent<Projectile>();
				if (proj != null)
                {
					AIActor actor = proj.Owner as AIActor;
					if (actor != null)
                    {
						float CooldownIncrease = 0.02f;
						if (IsHalfAmmo == true)
                        {
							CooldownIncrease = 0.01f;
                        }
						if (!actor.healthHaver.IsBoss)
                        {
							actor.behaviorSpeculator.CooldownScale += CooldownIncrease;
						}
					}
                }
			}
			if (IsHeart == true | IsHalfHeart == true)
            {
				Projectile proj = other.GetComponent<Projectile>();
				if (proj != null)
				{
					AIActor actor = proj.Owner as AIActor;
					if (actor != null)
					{
						if (!actor.healthHaver.IsBoss)
                        {
							this.random = UnityEngine.Random.Range(0.0f, 1.0f);
							float rng = 0.05f;
							if (IsHalfHeart == true)
							{
								rng = 0.035F;
							}
							if (random <= rng)
							{
								actor.ApplyEffect(GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultPermanentCharmEffect, 1f, null);
								actor.gameObject.AddComponent<KillOnRoomClear>();
								actor.IsHarmlessEnemy = true;
								actor.IgnoreForRoomClear = true;
								bool flag4 = actor.gameObject.GetComponent<SpawnEnemyOnDeath>();
								if (flag4)
								{
									Destroy(actor.gameObject.GetComponent<SpawnEnemyOnDeath>());
								}
							}
						}
					}
				}
			}
			if (IsArmor == true)
			{
				Projectile proj = other.GetComponent<Projectile>();
				if (proj != null)
				{
					AIActor actor = proj.Owner as AIActor;
					if (actor != null)
					{
						this.random = UnityEngine.Random.Range(0.0f, 1.0f);
						if (random <= 0.08f)
						{
							if (!actor.healthHaver.IsBoss)
							{
								actor.behaviorSpeculator.Stun(4 , true);
							}
						}
					}
				}
			}
		}
		public float random;

		public void Update()
		{
			if (this.actor == null)
			{
				this.actor = base.GetComponent<PlayerOrbital>();
			}
			if (this.player == null)
			{
				this.player = base.GetComponent<PlayerController>();
			}
		}

		public bool IsHeart;
		public bool IsHalfHeart;
		public bool IsAmmo;
		public bool IsHalfAmmo;
		public bool IsKey;
		public bool IsArmor;
		public bool IsBlank;


		public PlayerController sourcePlayer;
		public int HitsBeforeDeath = 10;
		private PlayerOrbital actor;
		public PlayerController player;
		public int Hits;
	}
}
