using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
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
	public class GameActorBlueFireEffect : GameActorHealthEffect
	{
		public static void Init()
		{
			GameActorBlueFireEffect.BlueFlameVFX = SpriteBuilder.SpriteFromResource(GameActorBlueFireEffect.BlueFirevfx, null, false);
			GameActorBlueFireEffect.BlueFlameVFX.name = GameActorBlueFireEffect.vfxName;
			UnityEngine.Object.DontDestroyOnLoad(GameActorBlueFireEffect.BlueFlameVFX);
			FakePrefab.MarkAsFakePrefab(GameActorBlueFireEffect.BlueFlameVFX);
			GameActorBlueFireEffect.BlueFlameVFX.SetActive(false);
		}


		public override bool ResistanceAffectsDuration
		{
			get
			{
				return true;
			}
		}

		public static RuntimeGameActorEffectData ApplyFlamesToTarget(GameActor actor, GameActorBlueFireEffect sourceEffect)
		{
			return new RuntimeGameActorEffectData
			{
				actor = actor
			};
		}

		public override void OnEffectApplied(GameActor actor, RuntimeGameActorEffectData effectData, float partialAmount = 1f)
		{
			base.OnEffectApplied(actor, effectData, partialAmount);
			effectData.OnActorPreDeath = delegate (Vector2 dir)
			{
				GameActorBlueFireEffect.DestroyFlames(effectData);
			};
			actor.healthHaver.OnPreDeath += effectData.OnActorPreDeath;
			if (GameActorBlueFireEffect.BlueFlameVFX != null)
			{
				if (effectData.vfxObjects == null)
				{
					effectData.vfxObjects = new List<Tuple<GameObject, float>>();
				}
				effectData.OnFlameAnimationCompleted = delegate (tk2dSpriteAnimator spriteAnimator, tk2dSpriteAnimationClip clip)
				{
					if (effectData.destroyVfx || !actor)
					{
						spriteAnimator.AnimationCompleted = (Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip>)Delegate.Remove(spriteAnimator.AnimationCompleted, effectData.OnFlameAnimationCompleted);
						UnityEngine.Object.Destroy(spriteAnimator.gameObject);
						return;
					}
					if (UnityEngine.Random.value < this.flameMoveChance)
					{
						Vector2 a = actor.specRigidbody.HitboxPixelCollider.UnitDimensions / 2f;
						Vector2 b = BraveUtility.RandomVector2(-a + this.flameBuffer, a - this.flameBuffer);
						Vector2 v = actor.specRigidbody.HitboxPixelCollider.UnitCenter + b;
						spriteAnimator.transform.position = v;
					}
					spriteAnimator.Play(clip, 0f, clip.fps * UnityEngine.Random.Range(1f - this.flameFpsVariation, 1f + this.flameFpsVariation), false);
				};
			}
		}

		// Token: 0x06004C9E RID: 19614 RVA: 0x001A2288 File Offset: 0x001A0488
		public override void EffectTick(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			base.EffectTick(actor, effectData);
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && effectData.actor && effectData.actor.specRigidbody.HitboxPixelCollider != null)
			{
				Vector2 unitBottomLeft = effectData.actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
				Vector2 unitTopRight = effectData.actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
				this.m_emberCounter += 30f * BraveTime.DeltaTime;
				if (this.m_emberCounter > 1f)
				{
					int num = Mathf.FloorToInt(this.m_emberCounter);
					this.m_emberCounter -= (float)num;
					GlobalSparksDoer.DoRandomParticleBurst(num, unitBottomLeft, unitTopRight, new Vector3(1f, 1f, 0f), 120f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.DARK_MAGICKS);
				}
			}
			if (actor && actor.specRigidbody)
			{
				Vector2 unitDimensions = actor.specRigidbody.HitboxPixelCollider.UnitDimensions;
				Vector2 a = unitDimensions / 2f;
				int num2 = Mathf.RoundToInt((float)this.flameNumPerSquareUnit * 0.5f * Mathf.Min(30f, Mathf.Min(new float[]
				{
				unitDimensions.x * unitDimensions.y
				})));
				this.m_particleTimer += BraveTime.DeltaTime * (float)num2;
				if (this.m_particleTimer > 1f)
				{
					int num3 = Mathf.FloorToInt(this.m_particleTimer);
					Vector2 vector = actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
					Vector2 vector2 = actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
					PixelCollider pixelCollider = actor.specRigidbody.GetPixelCollider(ColliderType.Ground);
					if (pixelCollider != null && pixelCollider.ColliderGenerationMode == PixelCollider.PixelColliderGeneration.Manual)
					{
						vector = Vector2.Min(vector, pixelCollider.UnitBottomLeft);
						vector2 = Vector2.Max(vector2, pixelCollider.UnitTopRight);
					}
					vector += Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2 -= Vector2.Min(a * 0.15f, new Vector2(0.25f, 0.25f));
					vector2.y -= Mathf.Min(a.y * 0.1f, 0.1f);
					GlobalSparksDoer.DoRandomParticleBurst(num3, vector, vector2, Vector3.zero, 0f, 0f, null, null, null, (!this.IsGreenFire) ? GlobalSparksDoer.SparksType.STRAIGHT_UP_FIRE : GlobalSparksDoer.SparksType.STRAIGHT_UP_GREEN_FIRE);
					this.m_particleTimer -= Mathf.Floor(this.m_particleTimer);
				}
			}
			if (actor.IsGone)
			{
				effectData.elapsed = 10000f;
			}
			if ((actor.IsFalling || actor.IsGone) && effectData.vfxObjects != null && effectData.vfxObjects.Count > 0)
			{
				GameActorBlueFireEffect.DestroyFlames(effectData);
			}
		}

		public override void OnEffectRemoved(GameActor actor, RuntimeGameActorEffectData effectData)
		{
			base.OnEffectRemoved(actor, effectData);
			actor.healthHaver.OnPreDeath -= effectData.OnActorPreDeath;
			GameActorBlueFireEffect.DestroyFlames(effectData);
		}

		// Token: 0x06004CA0 RID: 19616 RVA: 0x001A25F0 File Offset: 0x001A07F0
		public static void DestroyFlames(RuntimeGameActorEffectData effectData)
		{
			if (effectData.vfxObjects == null)
			{
				return;
			}
			if (!effectData.actor.IsFrozen)
			{
				for (int i = 0; i < effectData.vfxObjects.Count; i++)
				{
					GameObject first = effectData.vfxObjects[i].First;
					if (first)
					{
						first.transform.parent = SpawnManager.Instance.VFX;
					}
				}
			}
			effectData.vfxObjects.Clear();
			effectData.destroyVfx = true;
			if (GameManager.Options.ShaderQuality == GameOptions.GenericHighMedLowOption.HIGH && effectData.actor && effectData.actor.healthHaver && effectData.actor.healthHaver.GetCurrentHealth() <= 0f && effectData.actor.specRigidbody.HitboxPixelCollider != null)
			{
				Vector2 unitBottomLeft = effectData.actor.specRigidbody.HitboxPixelCollider.UnitBottomLeft;
				Vector2 unitTopRight = effectData.actor.specRigidbody.HitboxPixelCollider.UnitTopRight;
				float num = (unitTopRight.x - unitBottomLeft.x) * (unitTopRight.y - unitBottomLeft.y);
				GlobalSparksDoer.DoRandomParticleBurst(Mathf.Max(1, (int)(75f * num)), unitBottomLeft, unitTopRight, new Vector3(1f, 1f, 0f), 120f, 0.75f, null, null, null, GlobalSparksDoer.SparksType.EMBERS_SWIRLING);
			}
		}

		// Token: 0x040042A0 RID: 17056
		public const float BossMinResistance = 0.05f;

		// Token: 0x040042A1 RID: 17057
		public const float BossMaxResistance = 0.25f;

		public const float BossResistanceDelta = 0.015f;

		private static string BlueFirevfx = "Planetside/Resources/VFX/BlueFire/bluefire_002.png";

		private static string vfxName = "BlueFlameVFX";


		private static GameObject BlueFlameVFX;

		public int flameNumPerSquareUnit = 14;

		public Vector2 flameBuffer = new Vector2(0.0625f, 0.0625f);

		public float flameFpsVariation = 0.5f;

		public float flameMoveChance = 0.2f;

		public bool IsGreenFire;

		private float m_particleTimer;

		private float m_emberCounter;
	}

}