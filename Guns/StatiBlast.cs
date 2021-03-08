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


namespace Planetside
{
	public class StatiBlast : GunBehaviour
	{
		// Token: 0x06000036 RID: 54 RVA: 0x000037D8 File Offset: 0x000019D8
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("StatiBlast", "statiblast");
			Game.Items.Rename("outdated_gun_mods:statiblast", "psog:statiblast");
			gun.gameObject.AddComponent<StatiBlast>();
			GunExt.SetShortDescription(gun, "Your Reminder");
			GunExt.SetLongDescription(gun, "Fires small electric arcs that burst into lightning. A failed prototype of the BSG, it is unable to contain its own stored energy for long.");
			GunExt.SetupSprite(gun, null, "statiblast_idle_001", 11);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 15);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 10);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 4);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(546) as Gun, true, false);
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(156) as Gun).gunSwitchGroup;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.3f;
			gun.DefaultModule.cooldownTime = .133f;
			gun.DefaultModule.numberOfShotsInClip = 6;
			gun.SetBaseMaxAmmo(200);
			gun.quality = PickupObject.ItemQuality.B;
			gun.DefaultModule.angleVariance = 11f;
			gun.DefaultModule.burstShotCount = 1;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage = 7f;
			projectile.baseData.speed *= 1f;
			projectile.AdditionalScaleMultiplier = 0.5f;
			projectile.shouldRotate = true;
			projectile.pierceMinorBreakables = true;
			PierceProjModifier spook = projectile.gameObject.AddComponent<PierceProjModifier>();
			spook.penetration = 3;
			gun.encounterTrackable.EncounterGuid = "and his music was electric...";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.barrelOffset.transform.localPosition = new Vector3(2.0f, 0.75f, 0f);
			gun.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);
		}

		private bool HasReloaded;


		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
			}
		}
		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.OnDestruction += this.Zzap;
			//projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHit));
		}
		private void HandleHit(Projectile projectile, SpeculativeRigidbody arg2, bool arg3)
		{
			PlayerController player = projectile.Owner as PlayerController;
			bool isInCombat = player.IsInCombat;
			if (isInCombat)
            {
				float num2 = 10f;
				List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag5 = activeEnemies == null | activeEnemies.Count <= 0;
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, projectile.sprite.WorldCenter, out num2, null);
					bool flag8 = nearestEnemy && nearestEnemy != null;
					if (flag8)
					{
						Vector2 worldCenter3 = projectile.sprite.WorldCenter;
						Vector2 unitCenter3 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
						float z3 = BraveMathCollege.Atan2Degrees((unitCenter3 - worldCenter3).normalized);
						Projectile projectile3 = ((Gun)ETGMod.Databases.Items[153]).DefaultModule.projectiles[0];
						GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile3.gameObject, projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z3), true);
						Projectile component3 = gameObject3.GetComponent<Projectile>();
						bool flag15 = component3 != null;
						bool flag16 = flag15;
						if (flag16)
						{
							component3.Owner = this.gun.CurrentOwner;
							component3.baseData.range *= 5f;
							component3.pierceMinorBreakables = true;
							component3.collidesWithPlayer = false;
							PierceProjModifier spook = component3.gameObject.AddComponent<PierceProjModifier>();
							spook.penetration = 10;
							component3.AdditionalScaleMultiplier = 0.5f;
							component3.baseData.damage *= 0.375f;
						}
					}
				}
			}
				
		}
		private void Zzap(Projectile projectile)
        {
			PlayerController player = projectile.Owner as PlayerController;
			bool isInCombat = player.IsInCombat;
			if (isInCombat)
			{
				float num2 = 10f;
				List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag5 = activeEnemies == null | activeEnemies.Count <= 0;
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, projectile.sprite.WorldCenter, out num2, null);
					bool flag8 = nearestEnemy && nearestEnemy != null;
					if (flag8)
					{
						Vector2 worldCenter3 = projectile.sprite.WorldCenter;
						Vector2 unitCenter3 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
						float z3 = BraveMathCollege.Atan2Degrees((unitCenter3 - worldCenter3).normalized);
						Projectile projectile3 = ((Gun)ETGMod.Databases.Items[153]).DefaultModule.projectiles[0];
						GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile3.gameObject, projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z3), true);
						Projectile component3 = gameObject3.GetComponent<Projectile>();
						bool flag15 = component3 != null;
						bool flag16 = flag15;
						if (flag16)
						{
							component3.Owner = this.gun.CurrentOwner;
							component3.baseData.range *= 5f;
							component3.pierceMinorBreakables = true;
							component3.collidesWithPlayer = false;
							PierceProjModifier spook = component3.gameObject.AddComponent<PierceProjModifier>();
							spook.penetration = 10;
							component3.AdditionalScaleMultiplier = 0.5f;
							component3.baseData.damage *= 0.375f;
						}
					}
				}
			}
		}
		public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
		{
			AIActor aiactor = null;
			nearestDistance = float.MaxValue;
			bool flag = activeEnemies == null;
			bool flag2 = flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			bool flag5 = flag4;
			AIActor result;
			if (flag5)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor2 = activeEnemies[i];
					bool flag6 = !aiactor2.healthHaver.IsDead && aiactor2.healthHaver.IsVulnerable;
					bool flag7 = flag6;
					if (flag7)
					{
						bool flag8 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
						bool flag9 = flag8;
						if (flag9)
						{
							float num = Vector2.Distance(position, aiactor2.CenterPosition);
							bool flag10 = num < nearestDistance;
							bool flag11 = flag10;
							if (flag11)
							{
								nearestDistance = num;
								aiactor = aiactor2;
							}
						}
					}
				}
				result = aiactor;
			}
			return result;
		}
		public GameObject ChainLightningVFX;

		// Token: 0x040070E8 RID: 28904
		public CoreDamageTypes ChainLightningDamageTypes;

		public float ChainLightningMaxLinkDistance = 15f;

		public float ChainLightningDamagePerHit = 6f;

		// Token: 0x040070EB RID: 28907
		public float ChainLightningDamageCooldown = 1f;

		public GameObject ChainLightningDispersalParticles;
		public bool AddsChainLightning;
	}
}