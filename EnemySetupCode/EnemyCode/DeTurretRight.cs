using System;
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
using Pathfinding;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Planetside
{
	public class DeTurretRight : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "deturret_enemy";
		private static tk2dSpriteCollectionData DeturretColection;
		public static GameObject shootpoint;
		public static void Init()
		{
			DeTurretRight.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				//float AttackAnimationThingAMaWhatIts = 0.5f;
				prefab = EnemyBuilder.BuildPrefab("Deturret Enemy", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 10000;
				companion.aiActor.MovementSpeed = 0f;
				companion.aiActor.healthHaver.PreventAllDamage = true;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = true;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = false;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(100f);
				companion.aiActor.CollisionKnockbackStrength = 0f;
				companion.aiActor.procedurallyOutlined = true;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(100f, null, false);
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
					ManualWidth = 15,
					ManualHeight = 20,
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
					ManualWidth = 15,
					ManualHeight = 20,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("43426a2e39584871b287ac31df04b544").CorpseObject;
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
				bool flag3 = DeturretColection == null;
				if (flag3)
				{
					DeturretColection = SpriteBuilder.ConstructCollection(prefab, "DeturretRight_Collection");
					UnityEngine.Object.DontDestroyOnLoad(DeturretColection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], DeturretColection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DeturretColection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DeturretColection, new List<int>
					{
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9

					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DeturretColection, new List<int>
					{
					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DeturretColection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6,
					7,
					8,
					9


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DeturretColection, new List<int>
					{

                    10




					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, DeturretColection, new List<int>
					{
                     10

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;

				}
				var bs = prefab.GetComponent<BehaviorSpeculator>();
				prefab.GetComponent<ObjectVisibilityManager>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("43426a2e39584871b287ac31df04b544").behaviorSpeculator;
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
				bs.OtherBehaviors = new List<BehaviorBase>() {
				new CustomSpinBulletsBehavior() {
					ShootPoint = m_CachedGunAttachPoint,
					OverrideBulletName = "default",
					NumBullets = 5,
					BulletMinRadius = 1,
					BulletMaxRadius = 4,
					BulletCircleSpeed = 72,
					BulletsIgnoreTiles = false,
					RegenTimer = 0.02f,
					AmountOFLines = 4,
				}
				};
				/*
				bs.AttackBehaviors = new List<AttackBehaviorBase>() {
				new ShootBeamBehavior() {
					//ShootPoint = m_CachedGunAttachPoint,

					firingTime = 3f,
					stopWhileFiring = true,
					//beam
					//BulletScript = new CustomBulletScriptSelector(typeof(SkellScript)),
					//LeadAmount = 0f,
					AttackCooldown = 5f,
					//InitialCooldown = 4f,
					RequiresLineOfSight = true,
					//StopDuring = ShootBehavior.StopType.Attack,
					//Uninterruptible = true,

				}
				};
				*/
				//BehaviorSpeculator load = EnemyDatabase.GetOrLoadByGuid("43426a2e39584871b287ac31df04b544").behaviorSpeculator;
				//Tools.DebugInformation(load);

				
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("psog:deturret_right", companion.aiActor);


			}
		}



		private static string[] spritePaths = new string[]
		{

			"Planetside/Resources/Enemies/Deturret/deturret_idle_001.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_002.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_003.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_004.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_005.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_006.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_007.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_008.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_009.png",
			"Planetside/Resources/Enemies/Deturret/deturret_idle_010.png",

			//death
			"Planetside/Resources/Enemies/Deturret/deturret_idle_001.png",
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
					base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1a4872dafdb34fd29fe8ac90bd2cea67").bulletBank.bulletBank.GetBullet("default"));
					base.aiActor.HasBeenEngaged = true;
				}
				else
                {
					base.aiActor.bulletBank.Bullets.Remove(EnemyDatabase.GetOrLoadByGuid("1a4872dafdb34fd29fe8ac90bd2cea67").bulletBank.bulletBank.GetBullet("default"));
				}

			}
			private void Start()
			{
				this.aiActor.knockbackDoer.SetImmobile(true, "nope.");
				//base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.bulletBank.GetBullet("default"));
				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));

				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				//base.aiActor.HasBeenEngaged = true;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{ //new CustomBulletScriptSelector(typeof(EatPants));		
				  //AkSoundEngine.PostEvent("Play_BOSS_mineflayer_bellshot_01", base.aiActor.gameObject);

				};
			}

		}

		public class EatPants : Script // This BulletScript is just a modified version of the script BulletManShroomed, which you can find with dnSpy.
		{
			protected override IEnumerator Top() // This is just a simple example, but bullet scripts can do so much more.
			{

				if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
				{
					base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("sweep"));
				}
				for (int i = 0; i <= 12; i++)
				{
					this.Fire(new Direction(i * 30, DirectionType.Aim, -1f), new Speed(1f, SpeedType.Absolute), new SkellBullet());
				}
				yield break;
			}
		}


		public class SkellBullet : Bullet
		{
			public SkellBullet() : base("sweep", false, false, false)
			{

			}
			protected override IEnumerator Top()
			{
				base.ChangeSpeed(new Speed(20f, SpeedType.Absolute), 60);


				yield break;
			}
		}

	}
}


/*
 * 			private void FireLine(float startingAngle)
			{
				float num = 9f;
				for (int i = 0; i < 11; i++)
				{
					float num2 = Mathf.Atan((-45f + (float)i * num) / 45f) * 57.29578f;
					float num3 = Mathf.Cos(num2 * 0.0174532924f);
					float num4 = ((double)Mathf.Abs(num3) >= 0.0001) ? (1f / num3) : 1f;
					base.Fire(new Direction(num2 + startingAngle, DirectionType.Absolute, -1f), new Speed(num4 * 14, SpeedType.Absolute), new FleshCubeSlam.SpinOneWay(this, startingAngle + (float)i * num));
					//base.Fire(new Direction(num2 + startingAngle, DirectionType.Absolute, -1f), new Speed(num4 * 8, SpeedType.Absolute), new FleshCubeSlam.SpinOneWay(this, startingAngle + (float)i * num));
					//base.Fire(new Direction(num2 + startingAngle, DirectionType.Absolute, -1f), new Speed(num4 * 9f, SpeedType.Absolute), new FleshCubeSlam.SpinOtherWay(base.Position, num));
				}
			}
			public static int RandomValue = 0;
			public class SpinOneWay : Bullet
			{
				// Token: 0x06000A99 RID: 2713 RVA: 0x000085A7 File Offset: 0x000067A7
				public SpinOneWay(FleshCubeSlam parent, float angle = 0f)  : base(null, false, false, false)
				{
					this.m_parent = parent;
					this.m_angle = angle;
				}
				protected override IEnumerator Top()
				{
					base.ManualControl = true;
					Vector2 centerPosition = base.Position;
					float radius = 0f;
					this.m_spinSpeed = 40f;
					for (int i = 0; i < 1200; i++)
					{
						//base.StartTask(this.ChangeSpinSpeedTask(180f, 240));
						radius += 0.075f;
						//base.UpdateVelocity();
						centerPosition += this.Velocity / 60f;
						this.m_angle += this.m_spinSpeed / 60f;
						base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.m_angle, radius);
						yield return base.Wait(1);
					}
				}
				private IEnumerator ChangeSpinSpeedTask(float newSpinSpeed, int term)
				{
					float delta = (newSpinSpeed - this.m_spinSpeed) / (float)term;
					for (int i = 0; i < term; i++)
					{
						this.m_spinSpeed += delta;
						yield return base.Wait(1);
					}
					yield break;
				}
				private float m_spinSpeed;
				private float m_angle;
				private FleshCubeSlam m_parent;
			}*/



