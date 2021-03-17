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

namespace Planetside
{
	public class ApacheBullet : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "apache_bullet";
		private static tk2dSpriteCollectionData ApacheBulleet;
		public static GameObject shootpoint;
		public static void Init()
		{
			ApacheBullet.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Apache Bullet", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 800;
				companion.aiActor.MovementSpeed = 2.5f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = true;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = false;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(40f);
				companion.aiActor.CollisionKnockbackStrength = 0f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(40f, null, false);
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
					ManualHeight = 16,
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
					ManualOffsetY = 15,
					ManualWidth = 17,
					ManualHeight = 16,
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
				bool flag3 = ApacheBulleet == null;
				if (flag3)
				{
					ApacheBulleet = SpriteBuilder.ConstructCollection(prefab, "An3sBullet_Collection");
					UnityEngine.Object.DontDestroyOnLoad(ApacheBulleet);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], ApacheBulleet);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ApacheBulleet, new List<int>
					{

					0,
					1,
					2,
					3

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ApacheBulleet, new List<int>
					{

					0,
					1,
					2,
					3


					}, "idle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ApacheBulleet, new List<int>
					{

					0,
					1,
					2,
					3


					}, "run_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ApacheBulleet, new List<int>
					{

					0,
					1,
					2,
					3


					}, "run_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ApacheBulleet, new List<int>
					{

				 4,
				 5,
				 6,
				 7
				 
				 



					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ApacheBulleet, new List<int>
					{


				 4,
				 5,
				 6,
				 7
				 
				 

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
					BulletScript = new CustomBulletScriptSelector(typeof(SkellScript)),
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

				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("psog:apache_bullet", companion.aiActor);
				PlanetsideModule.Strings.Enemies.Set("#APACHE", "Apache Thunder");
				companion.aiActor.OverrideDisplayName = "#APACHE";
				companion.aiActor.ActorName = "#APACHE";
				companion.aiActor.name = "#APACHE";

			}
		}



		private static string[] spritePaths = new string[]
		{

			"Planetside/ModderBullets/apache/apachebullet_idle_001.png",
			"Planetside/ModderBullets/apache/apachebullet_idle_002.png",
			"Planetside/ModderBullets/apache/apachebullet_idle_003.png",
			"Planetside/ModderBullets/apache/apachebullet_idle_004.png",


			//death
			"Planetside/ModderBullets/apache/apachebullet_die_001.png",
			"Planetside/ModderBullets/apache/apachebullet_die_002.png",
			"Planetside/ModderBullets/apache/apachebullet_die_003.png",
			"Planetside/ModderBullets/apache/apachebullet_die_004.png",




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
		public class SkellScript : Script 
		{
			protected override IEnumerator Top() 
			{
				PlayerController player = GameManager.Instance.PrimaryPlayer;
				for (int j = 0; j < 2; j++)
                {
					RoomHandler currentRoom = player.CurrentRoom;
					AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_002");
					this.Mines_Cave_In = assetBundle.LoadAsset<GameObject>("Mines_Cave_In");
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Mines_Cave_In, player.sprite.WorldCenter + new Vector2(UnityEngine.Random.Range(-4, 4), UnityEngine.Random.Range(-4, 4)), Quaternion.identity);
					HangingObjectController RockSlideController = gameObject.GetComponent<HangingObjectController>();
					RockSlideController.triggerObjectPrefab = null;
					GameObject[] additionalDestroyObjects = new GameObject[]
					{
				RockSlideController.additionalDestroyObjects[1]
					};
					RockSlideController.additionalDestroyObjects = additionalDestroyObjects;
					UnityEngine.Object.Destroy(gameObject.transform.Find("Sign").gameObject);
					RockSlideController.ConfigureOnPlacement(currentRoom);
					//yield return new WaitForSeconds(0.5f);
					yield return Wait(30);
					RockSlideController.Interact(player);
				}
				yield return new WaitForSeconds(3f);
				yield break; 
			}
			private GameObject Mines_Cave_In;
		}
	}
}








