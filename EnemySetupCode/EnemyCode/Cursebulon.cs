﻿using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
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
	public class Cursebulon : AIActor
	{
		public static GameObject prefab;
		public static readonly string guid = "cursebulon";
		private static tk2dSpriteCollectionData CurseblobCollection;
		public static GameObject shootpoint;
		public static void Init()
		{
			Cursebulon.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			//
			bool flag = prefab != null || EnemyBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				prefab = EnemyBuilder.BuildPrefab("Cursebulon", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false);
				var companion = prefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 150;
				companion.aiActor.MovementSpeed = 4.75f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(25f);
				companion.aiActor.CollisionKnockbackStrength = 0f;
				companion.aiActor.procedurallyOutlined = true;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.healthHaver.SetHealthMaximum(25f, null, false);
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
					ManualWidth = 20,
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
					ManualWidth = 24,
					ManualHeight = 23,
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
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
					{
						"idle_back_left",
						"idle_front_right",
						"idle_front_left",
						"idle_back_right",

					}
				};
				aiAnimator.MoveAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.FourWay,
					Flipped = new DirectionalAnimation.FlipType[4],
					AnimNames = new string[]
						{
						"run_back_left",
						"run_front_right",
						"run_front_left",
						"run_back_right",

						}
				};
				bool flag3 = CurseblobCollection == null;
				if (flag3)
				{
					CurseblobCollection = SpriteBuilder.ConstructCollection(prefab, "FodderBoi_Collection");
					UnityEngine.Object.DontDestroyOnLoad(CurseblobCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], CurseblobCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,


					}, "idle_back_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{
					6,
					7,
					8,
					9,
					10,
					11


					}, "idle_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{

					12,
					13,
					14,
					15,
					16,
				    17

					}, "idle_front_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{
					18,
					19,
					20,
					21,
					22,
					23


					}, "idle_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{

					12,
					13,
					14,
					15,
					16,
					17


					}, "run_front_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{

					18,
					19,
					20,
					21,
					22,
					23


					}, "run_front_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{
					0,
					1,
					2,
					3,
					4,
					5,



					}, "run_back_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{

				    6,
					7,
					8,
					9,
					10,
					11


					}, "run_back_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{

				 32,
				 33,
				 34,
				 35,
				 36,
				 37,
				 38,
				 39




					}, "die_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, CurseblobCollection, new List<int>
					{

				 24,
				 25,
				 26,
				 27,
				 28,
				 29,
				 30,
				 31

					}, "die_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 13f;

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
					SearchInterval = 0.2f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.2f,
				}
			};
				bs.MovementBehaviors = new List<MovementBehaviorBase>() {
				new SeekTargetBehavior() {
					StopWhenInRange = false,
					CustomRange = 6,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = -0.25f,
					MaxActiveRange = 0
				}
				};	
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("psog:cursebulon", companion.aiActor);

				SpriteBuilder.AddSpriteToCollection("Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_006", SpriteBuilder.ammonomiconCollection);
				if (companion.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(companion.GetComponent<EncounterTrackable>());
				}
				companion.encounterTrackable = companion.gameObject.AddComponent<EncounterTrackable>();
				companion.encounterTrackable.journalData = new JournalEntry();
				companion.encounterTrackable.EncounterGuid = "psog:cursebulon";
				companion.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				companion.encounterTrackable.journalData.SuppressKnownState = false;
				companion.encounterTrackable.journalData.IsEnemy = true;
				companion.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				companion.encounterTrackable.ProxyEncounterGuid = "";
				companion.encounterTrackable.journalData.AmmonomiconSprite = "Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_006";
				companion.encounterTrackable.journalData.enemyPortraitSprite = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\Ammocom\\cursebulonsmmonomiconportrait.png");
				PlanetsideModule.Strings.Enemies.Set("#THE_CURSEBULON", "Cursebulon");
				PlanetsideModule.Strings.Enemies.Set("#THE_CURSEBULON_SHORTDESC", "Jelly Or Jammed?");
				PlanetsideModule.Strings.Enemies.Set("#THE_CURSEBULON_LONGDESC", "A blobuloid that has consumed so many cursed objects the curse has become one with it.\n\nSlaying one of these lets free the curses it holds inside.");
				companion.encounterTrackable.journalData.PrimaryDisplayName = "#THE_CURSEBULON";
				companion.encounterTrackable.journalData.NotificationPanelDescription = "#THE_CURSEBULON_SHORTDESC";
				companion.encounterTrackable.journalData.AmmonomiconFullEntry = "#THE_CURSEBULON_LONGDESC";
				EnemyBuilder.AddEnemyToDatabase(companion.gameObject, "psog:cursebulon");
				EnemyDatabase.GetEntry("psog:cursebulon").ForcedPositionInAmmonomicon = 44;
				EnemyDatabase.GetEntry("psog:cursebulon").isInBossTab = false;
				EnemyDatabase.GetEntry("psog:cursebulon").isNormalEnemy = true;
			}
		}



		private static string[] spritePaths = new string[]
		{

			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_left_001.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_left_002.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_left_003.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_left_004.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_left_005.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_left_006.png",

			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_right_001.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_right_002.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_right_003.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_right_004.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_right_005.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_back_right_006.png",

			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_left_001.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_left_002.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_left_003.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_left_004.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_left_005.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_left_006.png",

			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_001.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_002.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_003.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_004.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_005.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_idle_front_right_006.png",

			//death
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_001.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_002.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_003.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_004.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_005.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_006.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_007.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_left_008.png",

			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_001.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_002.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_003.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_004.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_005.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_006.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_007.png",
			"Planetside/Resources/Enemies/Cursebulon/cursebulon_die_right_008.png",


		};

		public class EnemyBehavior : BraveBehaviour
		{
			private RoomHandler m_StartRoom;

			public void Update()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				if (!base.aiActor.HasBeenEngaged)
				{
					CheckPlayerRoom();
				}
			}
			private void CheckPlayerRoom()
			{
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					GameManager.Instance.StartCoroutine(LateEngage());
				}
				else
				{
					base.aiActor.HasBeenEngaged = false;
				}
			}
			private IEnumerator LateEngage()
			{
				yield return new WaitForSeconds(0.5f);
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					base.aiActor.HasBeenEngaged = true;
				}
				yield break;
			}
			private void Start()
			{
				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.bulletBank.GetBullet("default"));
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					float RAD = 4.5f;
					if (base.aiActor.IsBlackPhantom)
                    {
						RAD += 1.5f;
                    }
					PlayerController player = (GameManager.Instance.PrimaryPlayer);
					AkSoundEngine.PostEvent("Play_OBJ_trashbag_burst_01", base.aiActor.gameObject);
					AkSoundEngine.PostEvent("Play_CHR_shadow_curse_01", null);
					player.CurrentRoom.ApplyActionToNearbyEnemies(base.aiActor.CenterPosition, RAD, new Action<AIActor, float>(this.ProcessEnemy));
					base.aiActor.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, true, false, false);

				};
			}
			private void ProcessEnemy(AIActor target, float distance)
			{
				bool jamnation = target.IsBlackPhantom;
				if (!jamnation)
				{
					target.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Curse") as GameObject, Vector3.zero, true, false, false);
					target.BecomeBlackPhantom();
				}
			}

		}
	}
}





