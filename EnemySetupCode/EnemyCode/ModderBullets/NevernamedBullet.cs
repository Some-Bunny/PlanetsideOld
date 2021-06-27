﻿using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;

namespace Planetside
{
	public class NevernamedBullet : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "nevernamed_bullet";
		private static tk2dSpriteCollectionData NevernamedBullete;
		public static GameObject shootpoint;
		public static void Init()
		{
			NevernamedBullet.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				//float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = EnemyBuilder.BuildPrefab("Nevernamed Bullet", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 1.5f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = true;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(20f);
				companion.aiActor.CollisionKnockbackStrength = 0f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(20f, null, false);
				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider


				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 16,
					ManualHeight = 22,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0
				});
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider
				{

					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyHitBox,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 0,
					ManualOffsetY = 0,
					ManualWidth = 16,
					ManualHeight = 22,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
					name = "die",
					anim = new DirectionalAnimation
						{
							Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
							Flipped = new DirectionalAnimation.FlipType[2],
							AnimNames = new string[]
							{

						   "die_left",
						   "die_right"

							}

						}
					}
				};
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
					{
						"idle_left",
						"idle_right"
					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					Flipped = new DirectionalAnimation.FlipType[2],
					AnimNames = new string[]
						{
						"run_left",
						"run_right"
						}
				};
				bool flag3 = NevernamedBullete == null;
				if (flag3)
				{
					NevernamedBullete = SpriteBuilder.ConstructCollection(prefab, "NevernamedBullet_Collection");
					UnityEngine.Object.DontDestroyOnLoad(NevernamedBullete);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], NevernamedBullete);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, NevernamedBullete, new List<int>
					{

					0,
					1,
					2,
					3

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, NevernamedBullete, new List<int>
					{

					0,
					1,
					2,
					3


					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, NevernamedBullete, new List<int>
					{

					0,
					1,
					2,
					3


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, NevernamedBullete, new List<int>
					{

					0,
					1,
					2,
					3


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, NevernamedBullete, new List<int>
					{

				 4,
				 5,
				 6,
				 7,
				 8
				 



					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, NevernamedBullete, new List<int>
					{


				 4,
				 5,
				 6,
				 7,
				 8
				 

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				prefab.GetComponent<ObjectVisibilityManager>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("fuck");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("fuck").gameObject;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = true,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f,
				}
			};
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootBehavior() {
					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new CustomBulletScriptSelector(typeof(SalamanderScript)),
					LeadAmount = 0f,
					AttackCooldown = 5f,
					InitialCooldown = 1f,
					RequiresLineOfSight = true,
					StopDuring = ShootBehavior.StopType.Attack,
					Uninterruptible = true,

				}
				};
				bs.MovementBehaviors = new List<MovementBehaviorBase>
			{
				new SeekTargetBehavior
				{
					StopWhenInRange = true,
					CustomRange = 7f,
					LineOfSight = false,
					ReturnToSpawn = false,
					SpawnTetherDistance = 0f,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 0f,
					MaxActiveRange = 0f
				}
			};
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("6e972cd3b11e4b429b888b488e308551").behaviorSpeculator;
				//Tools.DebugInformation(load);
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("psog:nevernamed_bullet", companion.aiActor);

				PlanetsideModule.Strings.Enemies.Set("#ANGRYTENTACLEMANTHATSCARESME", "Nevernamed");
				companion.aiActor.OverrideDisplayName = "#ANGRYTENTACLEMANTHATSCARESME";
				companion.aiActor.ActorName = "#ANGRYTENTACLEMANTHATSCARESME";
				companion.aiActor.name = "#ANGRYTENTACLEMANTHATSCARESME";
			}
		}



		private static string[] spritePaths = new string[]
		{

			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_idle_001.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_idle_002.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_idle_003.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_idle_004.png",
			//death
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_die_001.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_die_002.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_die_003.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_die_004.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_die_005.png",
			"Planetside/Resources/Enemies/ModderBullets/nevernamed/nevernamedbullet_die_006.png",


		};

		public class EnemyBehavior : BraveBehaviour
		{

			private RoomHandler m_StartRoom;
			private void Update()
			{
				if (!base.aiActor.HasBeenEngaged) { CheckPlayerRoom(); }
			}
			private void CheckPlayerRoom()
			{

				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					base.aiActor.HasBeenEngaged = true;
				}

			}
			private void Start()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					AkSoundEngine.PostEvent("Play_ENM_highpriest_blast_01", base.aiActor.gameObject);
				};
			}


		}

		public class SalamanderScript : Script 
		{
			protected override IEnumerator Top()
			{
				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.GetBullet("default"));
				}
				float startingDirection = UnityEngine.Random.Range(-45f, 45f);
				Vector2 targetPos = base.GetPredictedTargetPosition((float)(((double)UnityEngine.Random.value >= 0.5) ? 1 : 0), 12f);
				for (int j = 0; j < 7; j++)
				{
					base.Fire(new Direction(startingDirection, DirectionType.Absolute, -1f), new Speed(11f, SpeedType.Absolute), new SalamanderScript.SnakeBullet(j * 3, targetPos));
				}
				yield break;
			}

			private const int NumSnakes = 10;
			private const int NumBullets = 5;
			private const int BulletSpeed = 12;
			private const float SnakeMagnitude = 0.75f;
			private const float SnakePeriod = 3f;
			public class SnakeBullet : Bullet
			{
				public SnakeBullet(int delay, Vector2 target) : base("default", false, false, false)
				{
					this.delay = delay;
					this.target = target;
				}

				protected override IEnumerator Top()
				{
					base.ManualControl = true;
					yield return base.Wait(this.delay);
					Vector2 truePosition = base.Position;
					for (int i = 0; i < 360; i++)
					{
						float offsetMagnitude = Mathf.SmoothStep(-0.9f, 0.9f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
						if (i > 20 && i < 60)
						{
							float num = (this.target - truePosition).ToAngle();
							float value = BraveMathCollege.ClampAngle180(num - this.Direction);
							this.Direction += Mathf.Clamp(value, -8f, 8f);
						}
						truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
						base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
						yield return base.Wait(1);
					}
					base.Vanish(false);
					yield break;
				}

				private int delay;
				private Vector2 target;
			}
		}


		public class WallBullet : Bullet
		{
			public WallBullet() : base("default", false, false, false)
			{
			}
			protected override IEnumerator Top()
			{
				yield return this.Wait(60);
				base.ChangeSpeed(new Speed(16f, SpeedType.Absolute), 60);
				yield break;
			}
		}
	}
}








