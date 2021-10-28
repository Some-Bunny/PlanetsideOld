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

namespace Planetside
{
	public class LockOnGun : AdvancedGunBehavior
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("T4-GTR", "lockongun");
			Game.Items.Rename("outdated_gun_mods:t4-gtr", "psog:t4-gtr");
			gun.gameObject.AddComponent<LockOnGun>();
			gun.SetShortDescription("Brain With Brawn");
			gun.SetLongDescription("Fires mini-rockets towards the in-built target system, reloading on a full clip locks on to the currently targeted enemy.\n\nA shoulder-mounted rocket launcher made to end all brain versus brawn discussions.");
			GunExt.SetupSprite(gun, null, "lockongun_idle_001", 11);
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_wpn_voidcannon_shot_01";
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 30);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 6);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 2);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(345) as Gun, true, false);
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(593) as Gun).gunSwitchGroup;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 2.2f;
			gun.DefaultModule.cooldownTime = .7f;
			gun.DefaultModule.numberOfShotsInClip = 4;
			gun.SetBaseMaxAmmo(120);
			gun.quality = PickupObject.ItemQuality.A;
			gun.DefaultModule.burstCooldownTime = 0.0833f;

			gun.DefaultModule.angleVariance = 11f;
			gun.DefaultModule.burstShotCount = 4;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage = 10f;
			projectile.baseData.speed *= 0.9f;
			projectile.shouldRotate = true;
			projectile.pierceMinorBreakables = true;
			projectile.gameObject.AddComponent<LockOnGunProjectile>();
			projectile.SetProjectileSpriteRight("lockonprojectile", 9, 7, false, tk2dBaseSprite.Anchor.MiddleCenter, 9, 7);

			ExplosiveModifier explosiveModifier = projectile.gameObject.AddComponent<ExplosiveModifier>();
			explosiveModifier.doExplosion = true;
			explosiveModifier.explosionData = StaticExplosionDatas.explosiveRoundsExplosion;

			gun.gunClass = GunClass.EXPLOSIVE;

			PierceProjModifier spook = projectile.gameObject.AddComponent<PierceProjModifier>();
			spook.penetration = 0;
			spook.penetratesBreakables = true;
			gun.encounterTrackable.EncounterGuid = "haha funny big shot";
			ETGMod.Databases.Items.Add(gun, null, "ANY");

			GameObject gameObject = SpriteBuilder.SpriteFromResource("Planetside/Resources/VFX/LockOnVFX/lockonvfx1", null, true);
			gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(gameObject);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			GameObject gameObject2 = new GameObject("Lock On Gun VFX");
			tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
			tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);

			LockOnGun.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/LockOnVFX/lockonvfx1", tk2dSprite.Collection));
			LockOnGun.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/LockOnVFX/lockonvfx2", tk2dSprite.Collection));





			tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
			tk2dSprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
			tk2dSprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
			tk2dSprite.renderer.material.SetFloat("_EmissivePower", 30);
			tk2dSprite.renderer.material.SetFloat("_EmissiveColorPower", 2);

			LockOnGun.spriteIds.Add(tk2dSprite.spriteId);
			gameObject2.SetActive(false);

			tk2dSprite.SetSprite(LockOnGun.spriteIds[0]); //Unlocked
			tk2dSprite.SetSprite(LockOnGun.spriteIds[1]); //Locked

			FakePrefab.MarkAsFakePrefab(gameObject2);
			UnityEngine.Object.DontDestroyOnLoad(gameObject2);
			LockOnGun.LockOnPrefab = gameObject2;



			Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			mat.SetColor("_EmissiveColor", new Color32(107, 255, 135, 255));
			mat.SetFloat("_EmissiveColorPower", 1.55f);
			mat.SetFloat("_EmissivePower", 30);
			mat.SetFloat("_EmissiveThresholdSensitivity", 0.15f);
			MeshRenderer component = gun.GetComponent<MeshRenderer>();
			if (!component)
			{
				return;
			}
			Material[] sharedMaterials = component.sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				if (sharedMaterials[i].shader == mat)
				{
					return;
				}
			}
			Array.Resize<Material>(ref sharedMaterials, sharedMaterials.Length + 1);
			Material material = new Material(mat);
			material.SetTexture("_MainTex", sharedMaterials[0].GetTexture("_MainTex"));
			sharedMaterials[sharedMaterials.Length - 1] = material;
			component.sharedMaterials = sharedMaterials;

			List<string> AAA = new List<string>
			{
				"psog:t4-gtr",
			};

			List<string> AAA1= new List<string>
			{
				"homing_bullets",
				"remote_bullets"
			};
			CustomSynergies.Add("No Virus Included", AAA, AAA1, true);

			List<string> AAA2 = new List<string>
			{
				"stinger",
				"rpg",
				"yari_launcher"
			};
			CustomSynergies.Add("The Mighty Budget", AAA, AAA2, true);


			List<string> yes = new List<string>
			{
				"psog:t4-gtr",
				"rc_rocket"
			};
			CustomSynergies.Add("Double Trouble!", yes, null, true);


			LockOnGun.LockOnGunID = gun.PickupObjectId;
			ItemIDs.AddToList(gun.PickupObjectId);
		}
		public static int LockOnGunID;

		public static GameObject LockOnPrefab;
		public static List<int> spriteIds = new List<int>();
		public static GameObject LockOnInstance;
		public static AIActor LockedOnEnemy;
		public bool IsLockedOn;

		private Vector2 aimpoint;
		private float maxDistance = 15;
		private float m_currentAngle;
		private float m_currentDistance;


		public override void PostProcessProjectile(Projectile projectile)
		{
			
		}
		
		private bool HasReloaded;


		protected override void Update()
		{
			base.Update();

			if (gun.CurrentOwner as PlayerController)
			{
				gun.DefaultModule.burstShotCount = gun.DefaultModule.numberOfShotsInClip;
				PlayerController player = gun.CurrentOwner as PlayerController;


				float clip = (player.stats.GetStatValue(PlayerStats.StatType.AdditionalClipCapacityMultiplier));
				float num = (int)(4);
				if (player.PlayerHasActiveSynergy("The Mighty Budget"))
				{
					num *= 2;
				}
				float clipsize = num * clip;
				gun.DefaultModule.numberOfShotsInClip = (int)clipsize;

				if (LockOnInstance != null)
				{
					if (IsLockedOn == true && LockOnInstance.GetComponent<tk2dBaseSprite>().spriteId != LockOnGun.spriteIds[1])
					{
						LockOnInstance.GetComponent<tk2dBaseSprite>().SetSprite(LockOnGun.spriteIds[1]);
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.SetFloat("_EmissivePower", 30);
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.SetFloat("_EmissiveColorPower", 2);
					}
					else if (IsLockedOn == false && LockOnInstance.GetComponent<tk2dBaseSprite>().spriteId != LockOnGun.spriteIds[0])
					{
						LockOnInstance.GetComponent<tk2dBaseSprite>().SetSprite(LockOnGun.spriteIds[0]);
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.SetFloat("_EmissivePower", 30);
						LockOnInstance.GetComponent<tk2dBaseSprite>().renderer.material.SetFloat("_EmissiveColorPower", 2);
					}
					List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
					if (activeEnemies != null)
					{
						foreach (AIActor aiactor in activeEnemies)
						{
							if (aiactor == null)
                            {
								LockOnInstance.transform.position = aimpoint;
							}
							bool ae = Vector2.Distance(aiactor.CenterPosition, aimpoint) < 2f && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && player != null && IsLockedOn == false;
							if (ae)
							{
								LockedOnEnemy = aiactor;
							}
						}
					}
				}
				else
				{
					tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(LockOnPrefab, player.transform).GetComponent<tk2dSprite>();
					component.PlaceAtPositionByAnchor(aimpoint, tk2dBaseSprite.Anchor.MiddleCenter);
					component.GetComponent<tk2dBaseSprite>().SetSprite(LockOnGun.spriteIds[0]);
					component.HeightOffGround = -5;
					component.renderer.material.shader = ShaderCache.Acquire("Brave/LitTk2dCustomFalloffTintableTiltedCutoutEmissive");
					component.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_ON");
					component.renderer.material.SetFloat("_EmissivePower", 30);
					component.renderer.material.SetFloat("_EmissiveColorPower", 2);
					LockOnInstance = component.gameObject;
				}
				if (LockedOnEnemy != null && Vector2.Distance(LockedOnEnemy.CenterPosition, aimpoint) < 2f && LockedOnEnemy.healthHaver.GetMaxHealth() > 0f && LockedOnEnemy != null && LockedOnEnemy.specRigidbody != null && player != null)
                {
					LockOnInstance.transform.position = LockedOnEnemy.sprite.WorldCenter- new Vector2(0.625f, 0.625f);
				}
				else if(IsLockedOn == true && LockedOnEnemy != null)
				{
					LockOnInstance.transform.position = LockedOnEnemy.sprite.WorldCenter - new Vector2(0.625f, 0.625f);
				}
				else
                {
					LockOnInstance.transform.position = aimpoint;
					IsLockedOn = false;
				}

				if (BraveInput.GetInstanceForPlayer(player.PlayerIDX).IsKeyboardAndMouse(false))
				{
					aimpoint = player.unadjustedAimPoint.XY();
					if (LockOnInstance != null) 
					{
						Vector2 vector2 = aimpoint - LockOnInstance.GetComponent<tk2dSprite>().GetBounds().extents.XY();
						aimpoint = vector2;
					}

				}
				else
				{
					BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(player.PlayerIDX);
					Vector2 vector3 = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
					vector3 += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
					this.m_currentAngle = BraveMathCollege.Atan2Degrees(vector3 - player.CenterPosition);
					this.m_currentDistance = Vector2.Distance(vector3, player.CenterPosition);
					this.m_currentDistance = Mathf.Min(this.m_currentDistance, this.maxDistance);
					vector3 = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
					aimpoint = vector3;
					if (LockOnInstance != null)
                    {
						Vector2 vector4 = vector3 - LockOnInstance.GetComponent<tk2dSprite>().GetBounds().extents.XY();
						aimpoint = vector4;

					}
				}


				if (!gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!gun.IsReloading && !HasReloaded)
				{
					this.HasReloaded = true;
				}
			}
			else 
            {
				if (LockOnInstance!=null) { Destroy(LockOnInstance); LockedOnEnemy = null; }
			}

		}

		public override void OnReloadPressed(PlayerController player, Gun bruhgun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_plasmacell_reload_01", gameObject);
			}
			base.OnReloadPressed(player, gun, bSOMETHING);
			bool flag = gun.ClipCapacity == gun.ClipShotsRemaining || gun.CurrentAmmo == gun.ClipShotsRemaining;
			if (flag)
			{
				if (IsLockedOn != true && LockedOnEnemy != null)
				{
					AkSoundEngine.PostEvent("Play_OBJ_supplydrop_activate_01", gameObject);
					IsLockedOn = true;
				}
				else
				{
					AkSoundEngine.PostEvent("Play_OBJ_purchase_unable_01", gameObject);
					LockedOnEnemy = null;
					IsLockedOn = false;
				}
			}
		}

		protected override void OnPickup(PlayerController player)
		{
			player.GunChanged += this.OnGunChanged;
			base.OnPickup(player);
			if (LockOnInstance != null) { Destroy(LockOnInstance); LockedOnEnemy = null; }


		}

		protected override void OnPostDrop(PlayerController player)
		{
			player.GunChanged -= this.OnGunChanged;
			base.OnPostDrop(player);
			if (LockOnInstance != null) { Destroy(LockOnInstance); LockedOnEnemy = null; }

		}
		private void OnGunChanged(Gun oldGun, Gun newGun, bool arg3)
		{
			if (this.gun && this.gun.CurrentOwner)
			{
				if (newGun != this.gun)
				{
					if (LockOnInstance != null) { Destroy(LockOnInstance); }

				}
			}
		}
	}

}