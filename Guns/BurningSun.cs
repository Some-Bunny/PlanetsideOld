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
	// Token: 0x0200002D RID: 45
	public class BurningSun : GunBehaviour
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000D468 File Offset: 0x0000B668
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Burning Sun", "burningsun");
			Game.Items.Rename("outdated_gun_mods:burning_sun", "psog:burning_sun");
			var behav = gun.gameObject.AddComponent<BurningSun>();
			//behav.preventNormalReloadAudio = true;
			//behav.overrideNormalReloadAudio = "Play_BOSS_doormimic_appear_01";
			GunExt.SetShortDescription(gun, "Ring Of Fire");
			GunExt.SetLongDescription(gun, "A powerful cannon that fires miniature suns. The heat radiated from it deals great damage to foes.");
			GunExt.SetupSprite(gun, null, "burningsun_idle_001", 8);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 12);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 4);
			GunExt.SetAnimationFPS(gun, gun.chargeAnimation, 5);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 3);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(146) as Gun, true, false);
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Charged;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 3f;
			gun.DefaultModule.cooldownTime = 1f;
			gun.DefaultModule.numberOfShotsInClip = 1;
			gun.SetBaseMaxAmmo(50);
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.MEDIUM_BLASTER;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_wpn_chargelaser_shot_01";
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].eventAudio = "Play_OBJ_metalskin_end_01";
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].triggerEvent = true;
			gun.barrelOffset.transform.localPosition = new Vector3(2.0f, 0.4375f, 0f);
			gun.carryPixelOffset += new IntVector2((int)4f, (int)0f);
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).wrapMode = tk2dSpriteAnimationClip.WrapMode.LoopSection;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.chargeAnimation).loopStart = 2;
			gun.encounterTrackable.EncounterGuid = "Here comes the sun, dudududu";
			Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(146) as Gun).DefaultModule.projectiles[0]);
			projectile2.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile2);
			gun.DefaultModule.projectiles[0] = projectile2;
			projectile2.baseData.damage = 30f;
			projectile2.baseData.speed *= 0.2f;
			projectile2.baseData.force *= 1f;
			projectile2.baseData.range = 100f;
			projectile2.AdditionalScaleMultiplier *= 1.33f;
			projectile2.transform.parent = gun.barrelOffset;
			projectile2.HasDefaultTint = true;
			AoEDamageComponent Values = projectile2.gameObject.AddComponent<AoEDamageComponent>();
			Values.DamageperDamageEvent = 1;
			Values.Radius = 4;
			Values.TimeBetweenDamageEvents = 0.33f;
			Values.DealsDamage = false;
			Values.InflictsFire = true;
			ProjectileModule.ChargeProjectile item2 = new ProjectileModule.ChargeProjectile
			{
				Projectile = projectile2,
				ChargeTime = 1.33f
			};
			gun.DefaultModule.chargeProjectiles = new List<ProjectileModule.ChargeProjectile>
			{
				item2
			};
			gun.quality = PickupObject.ItemQuality.A;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}

		private bool HasReloaded;

		public Vector3 projectilePos;
		public override void OnPostFired(PlayerController player, Gun flakcannon)
		{
			gun.PreventNormalFireAudio = true;
		}
		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_OBJ_metalskin_end_01", base.gameObject);
			}
		}

		public Texture _gradTexture;

		public override void PostProcessProjectile(Projectile projectile)
		{


			TrailRenderer tr;
			var tro = projectile.gameObject.AddChild("trail object");
			tro.transform.position = projectile.transform.position;
			tro.transform.localPosition = new Vector3(0f, 0f, 0f);

			tr = tro.AddComponent<TrailRenderer>();
			tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			tr.receiveShadows = false;
			var mat = new Material(Shader.Find("Sprites/Default"));
			mat.mainTexture = _gradTexture;
			mat.SetColor("_Color", new Color(5f, 1f, 0f, 2f));
			tr.material = mat;
			tr.time = 1f;
			tr.minVertexDistance = 0.1f;
			tr.startWidth = 0.7f;
			tr.endWidth = 0f;
			tr.startColor = Color.white;
			tr.endColor = new Color(5f, 1f, 1f, 0f);
			
			PlayerController player = projectile.Owner as PlayerController;
			this.ShockRing(projectile);
            projectile.OnDestruction += this.EndRingEffect;
		}

		private void EndRingEffect(Projectile projectile)
		{
			PlayerController player = this.gun.CurrentOwner as PlayerController;
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDefinition = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDefinition);
			goopManagerForGoopType.TimedAddGoopCircle(projectile.sprite.WorldCenter, 4f, 0.8f, false);
		}
		private IEnumerator RiskOfRain(Projectile projectile)
		{
			if (projectile != null)
            {
				yield return new WaitForSeconds(0.5f);
                {
					AkSoundEngine.PostEvent("Play_BOSS_doormimic_flame_01", base.gameObject);
					PlayerController player = this.gun.CurrentOwner as PlayerController;
					BulletStatusEffectItem Firecomponent = PickupObjectDatabase.GetById(295).GetComponent<BulletStatusEffectItem>();
					GameActorFireEffect gameActorFire = Firecomponent.FireModifierEffect;
					List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
					Vector2 centerPosition = projectile.sprite.WorldCenter;
					foreach (AIActor aiactor in activeEnemies)
					{
						if (activeEnemies != null)
                        {
							bool flag = Vector2.Distance(aiactor.CenterPosition, centerPosition) < 5f && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && player != null;
							if (flag)
							{
								aiactor.ApplyEffect(gameActorFire, 1f, null);
							}
						}
					}
				}
				projectile.StartCoroutine(RiskOfRain(projectile));
				yield break;
			}
			yield break;
		}

		private void ShockRing(Projectile projectile)
		{
			PlayerController player = projectile.Owner as PlayerController;
			this.m_radialIndicator = ((GameObject)UnityEngine.Object.Instantiate(ResourceCache.Acquire("Global VFX/HeatIndicator"), projectile.sprite.WorldCenter, Quaternion.identity, projectile.transform)).GetComponent<HeatIndicatorController>();
			this.m_radialIndicator.CurrentColor = Color.red.WithAlpha(0f);
			this.m_radialIndicator.IsFire = true;
			this.m_radialIndicator.CurrentRadius = 4f;
			
		}

        private HeatIndicatorController m_radialIndicator;

        protected void Update()
		{
			if (gun.CurrentOwner)
			{
					
				if (!gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!gun.IsReloading && !HasReloaded)
				{
					this.HasReloaded = true;
				}
			}
		}
	}
}
//this was for bug-testing leave me alone
/*
namespace Planetside
{
	class AutoShotgun : AdvancedGunBehavior
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Auto Shotgun", "polarity");
			Game.Items.Rename("outdated_gun_mods:auto_shotgun", "cel:auto_shotgun");
			gun.gameObject.AddComponent<AutoShotgun>();
			gun.SetShortDescription("Become Decease");
			gun.SetLongDescription("An automatic shotgun. Prefered by those who want the coverage of a shotgun paired with the speed of an assault rifle.");
			gun.SetupSprite(null, "polarity_idle_001", 8);
			gun.SetAnimationFPS(gun.shootAnimation, 10);
			gun.SetAnimationFPS(gun.reloadAnimation, 5);
			for (int i = 0; i < 4; i++)
			{
				GunExt.AddProjectileModuleFrom(gun, "ak-47", true, false);
			}
			foreach (ProjectileModule projectileModule in gun.Volley.projectiles)
			{
				projectileModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
				Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(projectileModule.projectiles[0]);
				projectile.gameObject.SetActive(false);
				FakePrefab.MarkAsFakePrefab(projectile.gameObject);
				UnityEngine.Object.DontDestroyOnLoad(projectile);
				projectileModule.projectiles[0] = projectile;
				projectileModule.angleVariance = 12;
				projectile.transform.parent = gun.barrelOffset;
				gun.DefaultModule.projectiles[0] = projectile;
				projectile.baseData.damage *= .72f;
				projectile.baseData.speed *= 1f;
				projectile.AdditionalScaleMultiplier = 3f;
				projectileModule.numberOfShotsInClip = 8;
				projectileModule.cooldownTime = .4f;
				projectile.baseData.range = 35f;
				projectileModule.ammoType = GameUIAmmoType.AmmoType.CUSTOM;

				bool flag = projectileModule == gun.DefaultModule;
				if (flag)
				{
					projectileModule.ammoCost = 1;
				}
				else
				{
					projectileModule.ammoCost = 0;
				}

			}
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(51) as Gun).gunSwitchGroup;
			gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(51) as Gun).muzzleFlashEffects;
			gun.Volley.UsesShotgunStyleVelocityRandomizer = true;
			gun.Volley.DecreaseFinalSpeedPercentMin = -10f;
			gun.Volley.IncreaseFinalSpeedPercentMax = 10f;
			gun.SetBaseMaxAmmo(360);

			gun.quality = PickupObject.ItemQuality.C;
			gun.encounterTrackable.EncounterGuid = "fricc u spapi imma use guids all i want >:c";
			gun.sprite.IsPerpendicular = true;
			gun.gunClass = GunClass.FULLAUTO;
			gun.reloadTime = 2.2f;
			ETGMod.Databases.Items.Add(gun, null, "ANY");
		}
		private bool HasReloaded;
		protected override void Update()
		{
			base.Update();
			if (gun.CurrentOwner)
			{

				if (gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!gun.IsReloading && !HasReloaded)
				{
					this.HasReloaded = true;
				}
			}
		}

		protected override void OnPickedUpByPlayer(PlayerController player)
		{
			base.OnPickedUpByPlayer(player);
		}
		protected override void OnPostDroppedByPlayer(PlayerController player)
		{
			base.OnPostDroppedByPlayer(player);
		}

		public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				HasReloaded = false;
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				base.OnReloadPressed(player, gun, bSOMETHING);
			}
		}

		public override void OnPostFired(PlayerController player, Gun gun)
		{
			base.OnPostFired(player, gun);

		}

		public AutoShotgun()
		{

		}
	}
}
*/