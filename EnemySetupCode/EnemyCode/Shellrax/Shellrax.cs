using System;
using System.Collections.Generic;
using Gungeon;
using ItemAPI;
using UnityEngine;
//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.BossBuilder.AnimationType;
using System.Collections;
using Dungeonator;
using System.Linq;
using Brave.BulletScript;
using GungeonAPI;
using SaveAPI;

namespace Planetside
{
	public class Shellrax : AIActor
	{
		public static GameObject fuckyouprefab;
		public static readonly string guid = "Shellrax";
		private static tk2dSpriteCollectionData ShellraxClooection;
		public static GameObject shootpoint;
		public static GameObject shootpoint1;
		private static Texture2D BossCardTexture = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside/Resources/BossCards/shellrax_bosscard.png");
		public static string TargetVFX;
		public static void Init()
		{

			Shellrax.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = fuckyouprefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				fuckyouprefab = BossBuilder.BuildPrefab("Shellrax", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false, true);
				var companion = fuckyouprefab.AddComponent<EnemyBehavior>();
				companion.aiActor.knockbackDoer.weight = 200;
				companion.aiActor.MovementSpeed = 0f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0.05f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(450f);
				companion.aiActor.healthHaver.SetHealthMaximum(450f);
				companion.aiActor.CollisionKnockbackStrength = 2f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.procedurallyOutlined = true;
				///	
				//PlanetsideModule.Strings.Enemies.Set("#SHELLRAX", "SHELLRAX");
				//PlanetsideModule.Strings.Enemies.Set("#????", "???");
				//PlanetsideModule.Strings.Enemies.Set("#SUBTITLE", "FAILED DEMI-LICH");
				//PlanetsideModule.Strings.Enemies.Set("#QUOTE", "");
				//companion.aiActor.healthHaver.overrideBossName = "#SHELLRAX";
				//companion.aiActor.OverrideDisplayName = "#SHELLRAX";
				//companion.aiActor.ActorName = "#SHELLRAX";
				//companion.aiActor.name = "#SHELLRAX";
				fuckyouprefab.name = companion.aiActor.OverrideDisplayName;
				
				companion.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376").ShadowObject;
				companion.aiActor.HasShadow = true;



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
					ManualWidth = 36,
					ManualHeight = 40,
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
					ManualWidth = 36,
					ManualHeight = 40,
					ManualDiameter = 0,
					ManualLeftX = 0,
					ManualLeftY = 0,
					ManualRightX = 0,
					ManualRightY = 0,



				});
				companion.aiActor.CorpseObject = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").CorpseObject;
				companion.aiActor.PreventBlackPhantom = false;
				AIAnimator aiAnimator = companion.aiAnimator;
				aiAnimator.IdleAnimation = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "idle",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};


				DirectionalAnimation anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"ribopen",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "ribopen",
						anim = anim
					}
				};
				DirectionalAnimation eee = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
	{
						"attackandclose",

	},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attackandclose",
						anim = eee
					}
				};


				DirectionalAnimation anim3 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					AnimNames = new string[]
					{
						"tellfist",

					},
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "tellfist",
						anim = anim3
					}
				};


				DirectionalAnimation Hurray = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "slamfist",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "slamfist",
						anim = Hurray
					}
				};

				DirectionalAnimation TelepertOut = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "TeleportOut",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "TeleportOut",
						anim = TelepertOut
					}
				};


				DirectionalAnimation TelepertIn = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "TeleportIn",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "TeleportIn",
						anim = TelepertIn
					}
				};


				DirectionalAnimation almostdone = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "intro",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "intro",
						anim = almostdone
					}
				};
				DirectionalAnimation done = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					Prefix = "death",
					AnimNames = new string[1],
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "death",
						anim = done
					}
				};
				bool flag3 = ShellraxClooection == null;
				if (flag3)
				{
					ShellraxClooection = SpriteBuilder.ConstructCollection(fuckyouprefab, "Shellrax-Clooection");
					UnityEngine.Object.DontDestroyOnLoad(ShellraxClooection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], ShellraxClooection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{

					6,
					7,
					8,
					9,
					10


					}, "ribopen", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{

					11,
					12,
					13,
					14,
					11,
					12,
					13,
					14,
					11,
					12,
					13,
					14,
					11,
					12,
					13,
					14,
					15,
					16,
					17


					}, "attackandclose", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3.5f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{

					18,
					19,
					20,
					21,
					21,
					22,
					22
					


					}, "tellfist", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{
					23,
					24,
					25,
					26,
					27

					}, "slamfist", tk2dSpriteAnimationClip.WrapMode.Once).fps = 15f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{
					28,
					29,
					30,
					31,
					32,
					33,
					34,
					35,
					36

					}, "TeleportOut", tk2dSpriteAnimationClip.WrapMode.Once).fps = 9f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{
					37,
					38,
					39,
					40,
					41,
					42,
					43,
					44,
					45,
					46,
					47

					}, "TeleportIn", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{
				48,
				49,
				50,
				51,
				52,
				53,
				54,
				55,
				56,
				57,
				58,
				59,
				60,
				61,
				62,
				63,
				64,
				65,
				

					}, "intro", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, ShellraxClooection, new List<int>
					{
				66,
				67,
				68,
				69,
				70,
				71,
				72,
				73,
				74,
				75,
				76,
				77,
				78,
				79,
				80,
				81,
				82,
				83,
				84

					}, "death", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;

				}
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[1].eventAudio = "Play_ENM_shells_gather_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[1].triggerEvent = true;
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[2].eventAudio = "Play_BOSS_lichC_intro_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[2].triggerEvent = true;

				var bs = fuckyouprefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("attach").gameObject;
				
				shootpoint1 = new GameObject("fuck");
				shootpoint1.transform.parent = companion.transform;
				shootpoint1.transform.position = companion.sprite.WorldBottomLeft;
				GameObject m_CachedGunAttachPoint1 = companion.transform.Find("fuck").gameObject;
				bs.TargetBehaviors = new List<TargetBehaviorBase>
			{
				new TargetPlayerBehavior
				{
					Radius = 35f,
					LineOfSight = false,
					ObjectPermanence = true,
					SearchInterval = 0.25f,
					PauseOnTargetSwitch = false,
					PauseTime = 0.25f
				}
			};

				bs.MovementBehaviors = new List<MovementBehaviorBase>() {
				new SeekTargetBehavior() {
					StopWhenInRange = true,
					CustomRange = 6,
					LineOfSight = true,
					ReturnToSpawn = true,
					SpawnTetherDistance = 0,
					PathInterval = 0.5f,
					SpecifyRange = false,
					MinActiveRange = 1,
					MaxActiveRange = 10
				}
			};
				bs.AttackBehaviorGroup.AttackBehaviors = new List<AttackBehaviorGroup.AttackGroupItem>
				{

					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 0.4f,
					Behavior = new ShootBehavior{
					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new CustomBulletScriptSelector(typeof(SemiCirclesOfDoom)),
					LeadAmount = 0f,
					AttackCooldown = 1f,
					Cooldown = 3f,
					TellAnimation = "ribopen",
					FireAnimation = "attackandclose",
					RequiresLineOfSight = true,
					MultipleFireEvents = true,
					//EnabledDuringAttack = new PowderSkullSpinBulletsBehavior(),
					//StopDuring = ShootBehavior.StopType.Attack,
					Uninterruptible = false,
						},
						NickName = "SemiCircles"

					},
					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 0.5f,
					Behavior = new ShootBehavior{
					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new CustomBulletScriptSelector(typeof(DropDownScript)),
					LeadAmount = 0f,
					AttackCooldown = 1f,
					Cooldown = 2f,
					TellAnimation = "tellfist",
					FireAnimation = "slamfist",
					RequiresLineOfSight = true,

					MultipleFireEvents = true,
					//EnabledDuringAttack = new PowderSkullSpinBulletsBehavior(),
					//StopDuring = ShootBehavior.StopType.Attack,
					Uninterruptible = false,
						},
						NickName = "Support"

					},
					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 0.7f,
					Behavior = new ShootBehavior{
					ShootPoint = m_CachedGunAttachPoint1,
					BulletScript = new CustomBulletScriptSelector(typeof(Slammo)),
					LeadAmount = 0f,
					AttackCooldown = 1f,
					Cooldown = 2f,
					TellAnimation = "tellfist",
					FireAnimation = "slamfist",
					RequiresLineOfSight = true,

					MultipleFireEvents = true,
					//EnabledDuringAttack = new PowderSkullSpinBulletsBehavior(),
					//StopDuring = ShootBehavior.StopType.Attack,
					Uninterruptible = false,
						},
						NickName = "SpreadSlam"

					},
					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 0.6f,
					Behavior = new ShootBehavior{
					ShootPoint = m_CachedGunAttachPoint,
					BulletScript = new CustomBulletScriptSelector(typeof(CirclesWithOpenings)),
					LeadAmount = 0f,
					AttackCooldown = 1f,
					Cooldown = 3f,
					TellAnimation = "ribopen",
					FireAnimation = "attackandclose",
					RequiresLineOfSight = true,
					MultipleFireEvents = true,
					//EnabledDuringAttack = new PowderSkullSpinBulletsBehavior(),
					//StopDuring = ShootBehavior.StopType.Attack,
					Uninterruptible = false,
						},
						NickName = "Circles&Snakes"

					},
					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 0.4f,
					Behavior = new TeleportBehavior{
					AttackableDuringAnimation = true,
					AllowCrossRoomTeleportation = false,
					teleportRequiresTransparency = false,
					hasOutlinesDuringAnim = true,
					ManuallyDefineRoom = false,
					MaxHealthThreshold = 1f,
					StayOnScreen = true,
					AvoidWalls = true,
					GoneTime = 2f,
					OnlyTeleportIfPlayerUnreachable = false,
					MinDistanceFromPlayer = 10f,
					MaxDistanceFromPlayer = 5f,
					teleportInAnim = "TeleportIn",
					teleportOutAnim = "TeleportOut",
					AttackCooldown = 1f,
					InitialCooldown = 0f,
					RequiresLineOfSight = false,
					roomMax = new Vector2(0,0),
					roomMin = new Vector2(0,0),
					//teleportInBulletScript = new CustomBulletScriptSelector(typeof(ShellRaxDropCursedAreas)),
					teleportOutBulletScript = new CustomBulletScriptSelector(typeof(ShellRaxDropCursedAreas)),
					GlobalCooldown = 0.5f,
					Cooldown = 6f,

					CooldownVariance = 1f,
					InitialCooldownVariance = 0f,
					goneAttackBehavior = null,
					IsBlackPhantom = false,
					GroupName = null,
					GroupCooldown = 0f,
					MinRange = 0,
					Range = 0,
					MinHealthThreshold = 0,
					
					//MaxEnemiesInRoom = 1,
					MaxUsages = 0,
					AccumulateHealthThresholds = true,
					//shadowInAnim = null,
					//shadowOutAnim = null,
					targetAreaStyle = null,
					HealthThresholds = new float[0],
					MinWallDistance = 0,
					},
					//resetCooldownOnDamage = null,
					//shadowSupport = (TeleportBehavior.ShadowSupport)1,

					},

				};
			
				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("psog:shellrax", companion.aiActor);



				SpriteBuilder.AddSpriteToCollection("Planetside/Resources/Shellrax/shellrax_idle_001", SpriteBuilder.ammonomiconCollection);
				if (companion.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(companion.GetComponent<EncounterTrackable>());
				}
				companion.encounterTrackable = companion.gameObject.AddComponent<EncounterTrackable>();
				companion.encounterTrackable.journalData = new JournalEntry();
				companion.encounterTrackable.EncounterGuid = "psog:shellrax";
				companion.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				companion.encounterTrackable.journalData.SuppressKnownState = false;
				companion.encounterTrackable.journalData.IsEnemy = true;
				companion.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				companion.encounterTrackable.ProxyEncounterGuid = "";
				companion.encounterTrackable.journalData.AmmonomiconSprite = "Planetside/Resources/Shellrax/shellrax_idle_001";
				companion.encounterTrackable.journalData.enemyPortraitSprite = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\Ammocom\\shellraxicon.png");
				PlanetsideModule.Strings.Enemies.Set("#SHELLAD", "Shellrax");
				PlanetsideModule.Strings.Enemies.Set("#SHELLAD_SHORTDESC", "Failed Demi-lich");
				PlanetsideModule.Strings.Enemies.Set("#SHELLAD_LONGDESC", "A prior student of the Lich, Shellrax was destined to become the heir of the Gungeon. However, insanity and power-creep caused Shellrax to retort against his Master to claim the throne before it was due.\n\nAlthough Shellrax was no match for the Lich, impressed with his strength, the Lich spared him, and left him to roam the halls of the Gungeon as a powerful guardian of Bullet Hell.");
				companion.encounterTrackable.journalData.PrimaryDisplayName = "#SHELLAD";
				companion.encounterTrackable.journalData.NotificationPanelDescription = "#SHELLAD_SHORTDESC";
				companion.encounterTrackable.journalData.AmmonomiconFullEntry = "#SHELLAD_LONGDESC";
				EnemyBuilder.AddEnemyToDatabase(companion.gameObject, "psog:shellrax");
				EnemyDatabase.GetEntry("psog:shellrax").ForcedPositionInAmmonomicon = 200;
				EnemyDatabase.GetEntry("psog:shellrax").isInBossTab = false;
				EnemyDatabase.GetEntry("psog:shellrax").isNormalEnemy = true;

				GenericIntroDoer miniBossIntroDoer = fuckyouprefab.AddComponent<GenericIntroDoer>();
				fuckyouprefab.AddComponent<ShellraxIntro>();
				miniBossIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;
				miniBossIntroDoer.initialDelay = 0.15f;
				miniBossIntroDoer.cameraMoveSpeed = 14;
				miniBossIntroDoer.specifyIntroAiAnimator = null;
				miniBossIntroDoer.BossMusicEvent = "Play_MUS_Boss_Theme_Dragun_02";
				//miniBossIntroDoer.BossMusicEvent = "Play_MUS_Lich_Double_01";
				miniBossIntroDoer.PreventBossMusic = false;
				miniBossIntroDoer.InvisibleBeforeIntroAnim = true;
				miniBossIntroDoer.preIntroAnim = string.Empty;
				miniBossIntroDoer.preIntroDirectionalAnim = string.Empty;
				miniBossIntroDoer.introAnim = "intro";
				miniBossIntroDoer.introDirectionalAnim = string.Empty;
				miniBossIntroDoer.continueAnimDuringOutro = false;
				miniBossIntroDoer.cameraFocus = null;
				miniBossIntroDoer.roomPositionCameraFocus = Vector2.zero;
				miniBossIntroDoer.restrictPlayerMotionToRoom = false;
				miniBossIntroDoer.fusebombLock = false;
				miniBossIntroDoer.AdditionalHeightOffset = 0;
				PlanetsideModule.Strings.Enemies.Set("#SHELLRAX", "SHELLRAX");
				PlanetsideModule.Strings.Enemies.Set("#SHELLRAD_SHORTDESC", "FAILED DEMI-LICH");
				PlanetsideModule.Strings.Enemies.Set("#QUOTE", "");

				miniBossIntroDoer.portraitSlideSettings = new PortraitSlideSettings()
				{
					bossNameString = "#SHELLRAX",
					bossSubtitleString = "#SHELLRAD_SHORTDESC",
					bossQuoteString = "#QUOTE",
					bossSpritePxOffset = IntVector2.Zero,
					topLeftTextPxOffset = IntVector2.Zero,
					bottomRightTextPxOffset = IntVector2.Zero,
					bgColor = Color.red
				};
				if (BossCardTexture)
				{
					miniBossIntroDoer.portraitSlideSettings.bossArtSprite = BossCardTexture;
					miniBossIntroDoer.SkipBossCard = false;
					companion.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.SubbossBar;
				}
				else
				{
					miniBossIntroDoer.SkipBossCard = true;
					companion.aiActor.healthHaver.bossHealthBar = HealthHaver.BossBarType.SubbossBar;
				}
				miniBossIntroDoer.SkipFinalizeAnimation = true;
				miniBossIntroDoer.RegenerateCache();
			}
		}
		public static List<int> Lootdrops = new List<int>
		{
			73,
			85,
			120,
			67,
			224,
			600,
			78
		};





		private static string[] spritePaths = new string[]
		{
			
			//idle
			"Planetside/Resources/Shellrax/shellrax_idle_001.png",
			"Planetside/Resources/Shellrax/shellrax_idle_002.png",
			"Planetside/Resources/Shellrax/shellrax_idle_003.png",
			"Planetside/Resources/Shellrax/shellrax_idle_004.png",
			"Planetside/Resources/Shellrax/shellrax_idle_005.png",
			"Planetside/Resources/Shellrax/shellrax_idle_006.png",
			//6
			//open ribs
			"Planetside/Resources/Shellrax/shellrax_ribopen_001.png",
			"Planetside/Resources/Shellrax/shellrax_ribopen_002.png",
			"Planetside/Resources/Shellrax/shellrax_ribopen_003.png",
			"Planetside/Resources/Shellrax/shellrax_ribopen_004.png",
			"Planetside/Resources/Shellrax/shellrax_ribopen_005.png",
			//11

			//ribs
			"Planetside/Resources/Shellrax/shellrax_ribs_001.png",
			"Planetside/Resources/Shellrax/shellrax_ribs_002.png",
			"Planetside/Resources/Shellrax/shellrax_ribs_003.png",
			"Planetside/Resources/Shellrax/shellrax_ribs_004.png",
			//ribsclose
			"Planetside/Resources/Shellrax/shellrax_ribs_005.png",
			"Planetside/Resources/Shellrax/shellrax_ribs_006.png",
			"Planetside/Resources/Shellrax/shellrax_ribs_007.png",
			//18

			//tell
			"Planetside/Resources/Shellrax/shellrax_tell1_001.png",
			"Planetside/Resources/Shellrax/shellrax_tell1_002.png",
			"Planetside/Resources/Shellrax/shellrax_tell1_003.png",
			"Planetside/Resources/Shellrax/shellrax_tell1_004.png",
			"Planetside/Resources/Shellrax/shellrax_tell1_005.png",
			//23

			//slam
			"Planetside/Resources/Shellrax/shellrax_slam_001.png",
			"Planetside/Resources/Shellrax/shellrax_slam_002.png",
			"Planetside/Resources/Shellrax/shellrax_slam_003.png",
			"Planetside/Resources/Shellrax/shellrax_slam_004.png",
			"Planetside/Resources/Shellrax/shellrax_slam_005.png",
			//28
			//Teleport out
			"Planetside/Resources/Shellrax/shellrax_warp_001.png",
			"Planetside/Resources/Shellrax/shellrax_warp_002.png",
			"Planetside/Resources/Shellrax/shellrax_warp_003.png",
			"Planetside/Resources/Shellrax/shellrax_warp_004.png",
			"Planetside/Resources/Shellrax/shellrax_warp_005.png",
			"Planetside/Resources/Shellrax/shellrax_warp_006.png",
			"Planetside/Resources/Shellrax/shellrax_warp_007.png",
			"Planetside/Resources/Shellrax/shellrax_warp_008.png",
			"Planetside/Resources/Shellrax/shellrax_warp_009.png",
			//37
			//Teleport in
			"Planetside/Resources/Shellrax/shellrax_warpin_001.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_002.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_003.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_004.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_005.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_006.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_007.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_008.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_009.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_010.png",
			"Planetside/Resources/Shellrax/shellrax_warpin_011.png",
			//48
			//intro /
			"Planetside/Resources/Shellrax/shellrax_intro_001.png",
			"Planetside/Resources/Shellrax/shellrax_intro_002.png",
			"Planetside/Resources/Shellrax/shellrax_intro_003.png",
			"Planetside/Resources/Shellrax/shellrax_intro_004.png",
			"Planetside/Resources/Shellrax/shellrax_intro_005.png",
			"Planetside/Resources/Shellrax/shellrax_intro_006.png",
			"Planetside/Resources/Shellrax/shellrax_intro_007.png",
			"Planetside/Resources/Shellrax/shellrax_intro_008.png",
			"Planetside/Resources/Shellrax/shellrax_intro_009.png",
			"Planetside/Resources/Shellrax/shellrax_intro_010.png",
			"Planetside/Resources/Shellrax/shellrax_intro_011.png",
			"Planetside/Resources/Shellrax/shellrax_intro_012.png",
			"Planetside/Resources/Shellrax/shellrax_intro_013.png",
			"Planetside/Resources/Shellrax/shellrax_intro_014.png",
			"Planetside/Resources/Shellrax/shellrax_intro_015.png",
			"Planetside/Resources/Shellrax/shellrax_intro_016.png",
			"Planetside/Resources/Shellrax/shellrax_intro_017.png",
			"Planetside/Resources/Shellrax/shellrax_intro_018.png",
			//66

			//die /
			"Planetside/Resources/Shellrax/shellrax_death_001.png",
			"Planetside/Resources/Shellrax/shellrax_death_002.png",
			"Planetside/Resources/Shellrax/shellrax_death_003.png",
			"Planetside/Resources/Shellrax/shellrax_death_004.png",
			"Planetside/Resources/Shellrax/shellrax_death_005.png",
			"Planetside/Resources/Shellrax/shellrax_death_006.png",
			"Planetside/Resources/Shellrax/shellrax_death_007.png",
			"Planetside/Resources/Shellrax/shellrax_death_008.png",
			"Planetside/Resources/Shellrax/shellrax_death_009.png",
			"Planetside/Resources/Shellrax/shellrax_death_010.png",
			"Planetside/Resources/Shellrax/shellrax_death_011.png",
			"Planetside/Resources/Shellrax/shellrax_death_012.png",
			"Planetside/Resources/Shellrax/shellrax_death_013.png",
			"Planetside/Resources/Shellrax/shellrax_death_014.png",
			"Planetside/Resources/Shellrax/shellrax_death_015.png",
			"Planetside/Resources/Shellrax/shellrax_death_016.png",
			"Planetside/Resources/Shellrax/shellrax_death_017.png",
			"Planetside/Resources/Shellrax/shellrax_death_018.png",
			"Planetside/Resources/Shellrax/shellrax_death_019.png",
			//85


		};
	}

	public class DropDownScript : Script 
	{
		protected override IEnumerator Top()
		{
							base.PostWwiseEvent("Play_BOSS_lichC_morph_01", null);
			base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("4d164ba3f62648809a4a82c90fc22cae").bulletBank.GetBullet("big_one"));
			int DropDowns = UnityEngine.Random.Range(3, 7);
			base.PostWwiseEvent("Play_BOSS_RatMech_Stomp_01", null);
			for (int i = 0; i < DropDowns; i++)
			{
				PlayerController player = (GameManager.Instance.PrimaryPlayer);
				IntVector2? vector = player.CurrentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), CellTypes.FLOOR | CellTypes.PIT, false, null);
				Vector2 vector2 = vector.Value.ToVector2();
				base.Fire(Offset.OverridePosition(vector2 + new Vector2(0f, 30f)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new DropDownScript.BigBullet());
				yield return base.Wait(10);
			}


			yield break;
		}

		private class BigBullet : Bullet
		{
			public BigBullet() : base("big_one", false, false, false)
			{
			}

			public override void Initialize()
			{
				this.Projectile.spriteAnimator.StopAndResetFrameToDefault();
				base.Initialize();
			}

			protected override IEnumerator Top()
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
				this.Projectile.specRigidbody.CollideWithTileMap = false;
				this.Projectile.specRigidbody.CollideWithOthers = false;
				yield return base.Wait(60);
				base.PostWwiseEvent("Play_ENM_bulletking_slam_01", null);
				this.Speed = 0f;
				this.Projectile.spriteAnimator.Play();
				base.Vanish(true);
				yield break;
			}

			public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
			{
				if (!preventSpawningProjectiles)
				{
					var list = new List<string> {
				//"shellet",
				"e21ac9492110493baef6df02a2682a0d"
			};
					string guid = BraveUtility.RandomElement<string>(list);
					var Enemy = EnemyDatabase.GetOrLoadByGuid(guid);
					AIActor.Spawn(Enemy.aiActor, this.Projectile.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
					float num = base.RandomAngle();
					float Amount = 12;
					float Angle = 360 / Amount;
					for (int i = 0; i < Amount; i++)
					{
						base.Fire(new Direction(num + Angle * (float)i + 10, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new BurstBullet());
					}
					return;
				}
			}
			public class BurstBullet : Bullet
			{
				public BurstBullet() : base("reversible", false, false, false)
				{
				}
				protected override IEnumerator Top()
				{
					base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 60);
					yield return base.Wait(60);
					base.Vanish(false);
					yield break;
				}
			}

		}
	}


	public class ShellRaxDropCursedAreas : Script
	{
		protected override IEnumerator Top()
		{
			DraGunController dragunController = EnemyDatabase.GetOrLoadByGuid("05b8afe0b6cc4fffa9dc6036fa24c8ec").GetComponent<DraGunController>();
			AkSoundEngine.PostEvent("Play_BOSS_wall_slam_01", base.BulletBank.aiActor.gameObject);
			int DropDowns = UnityEngine.Random.Range(2, 6);
			for (int i = 0; i < DropDowns; i++)
			{
				PlayerController player = (GameManager.Instance.PrimaryPlayer);

				IntVector2? vector = player.CurrentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), CellTypes.FLOOR | CellTypes.PIT, false, null);
				Vector2 vector2 = vector.Value.ToVector2();
				this.FireRocket(dragunController.skyBoulder, vector2);
				yield return base.Wait(10);
			}

			yield return base.Wait(60);
			base.PostWwiseEvent("Play_VO_lichA_chuckle_01", null);
			DropDowns = UnityEngine.Random.Range(1, 4);
			for (int u = 0; u < DropDowns; u++)
			{
				this.m_player = base.BulletBank.GetComponent<PlayerController>();
				PlayerController player = (GameManager.Instance.PrimaryPlayer);
				IntVector2? vector = player.CurrentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), CellTypes.FLOOR | CellTypes.PIT, false, null);
				Vector2 vector2 = vector.Value.ToVector2();
				this.FireRocket(dragunController.skyBoulder, vector2);
				yield return base.Wait(10);
			}
			yield break;
		}
		private void FireRocket(GameObject skyRocket, Vector2 target)
		{
			SkyRocket component = SpawnManager.SpawnProjectile(skyRocket, base.Position, Quaternion.identity, true).GetComponent<SkyRocket>();
			component.TargetVector2 = target;
			tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
			component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
			component.ExplosionData.ignoreList.Add(base.BulletBank.specRigidbody);
		}
		public int NumRockets = 3;
		public PlayerController m_player;
	}


	public class SemiCirclesOfDoom : Script 
	{
		protected override IEnumerator Top()
		{
			if (this.BulletBank && this.BulletBank.aiActor && this.BulletBank.aiActor.TargetRigidbody)
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("ec6b674e0acd4553b47ee94493d66422").bulletBank.GetBullet("bigBullet"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6c43fddfd401456c916089fdd1c99b1c").bulletBank.GetBullet("amuletRing"));
			}
			for (int u = 0; u < 4; u++)
            {
				base.PostWwiseEvent("Play_BOSS_lichB_charge_01", null);
				string BulletType = "amuletRing";
				float radius = UnityEngine.Random.Range(0.03f, 0.09f);
				float delta = 15f;
				float startDirection = AimDirection;
				for (int j = 0; j < 6; j++)
				{
					base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SemiCirclesOfDoom.TheGear(BulletType, this, startDirection + (float)j * delta, radius));
					base.Fire(new Direction(-90+delta, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SemiCirclesOfDoom.TheGear(BulletType, this, (startDirection + (float)j * delta), radius+0.005f));
					base.Fire(new Direction(-90+(delta*2), DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SemiCirclesOfDoom.TheGear(BulletType, this, (startDirection + (float)j * delta), radius+0.01f));
				}
				for (int j = 0; j < 6; j++)
				{
					base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SemiCirclesOfDoom.TheGear(BulletType, this, (startDirection + (float)j * delta)+180, radius));
					base.Fire(new Direction(-90 + delta, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SemiCirclesOfDoom.TheGear(BulletType, this, (startDirection + (float)j * delta) + 180, radius + 0.005f));
					base.Fire(new Direction(-90 + (delta * 2), DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SemiCirclesOfDoom.TheGear(BulletType, this, (startDirection + (float)j * delta) + 180, radius + 0.01f));
				}
				BulletType = "bigBullet";
				base.Fire(new Direction(-90f, DirectionType.Absolute, -1f), new Speed(0f, SpeedType.Absolute), new SemiCirclesOfDoom.TheGear(BulletType, this, startDirection + (float)+0, 0));

				yield return base.Wait(50);
			}
			yield break;
		}
		public class TheGear : Bullet
		{
			public TheGear(string BulletType, SemiCirclesOfDoom parent, float angle = 0f, float aradius = 0) : base(BulletType, false, false, false)
			{
				this.m_parent = parent;
				this.m_angle = angle;
				this.m_radius = aradius;
				this.m_bulletype = BulletType;
				this.SuppressVfx = true;
			}

			protected override IEnumerator Top()
			{
				base.ManualControl = true;
				Vector2 centerPosition = base.Position;
				float radius = 0f;
				this.m_spinSpeed = 40f;
				for (int i = 0; i < 300; i++)
				{
					if (i == 40)
					{
						base.ChangeSpeed(new Speed(18f, SpeedType.Absolute), 120);
						base.ChangeDirection(new Direction(this.m_parent.GetAimDirection(1f, 10f), DirectionType.Absolute, -1f), 20);
						base.StartTask(this.ChangeSpinSpeedTask(180f, 240));
					}
					bool HasDiverted = false;
					if (i > 100 && i < 140 && UnityEngine.Random.value < 0.02f && m_bulletype == "bigBullet" && HasDiverted == false)
					{
						HasDiverted = true;
						float speed = base.Speed;
						this.Direction = base.AimDirection;
						this.Speed = speed * 0.75f;
						base.ManualControl = false;
						yield break;
					}
					base.UpdateVelocity();
					centerPosition += this.Velocity / 60f;
					if (i < 40)
					{
						radius += m_radius;
					}
					this.m_angle += this.m_spinSpeed / 60f;
					base.Position = centerPosition + BraveMathCollege.DegreesToVector(this.m_angle, radius);
					yield return base.Wait(1);
				}
				base.Vanish(false);
				yield break;
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
			private const float ExpandSpeed = 4.5f;
			private const float SpinSpeed = 40f;
			private SemiCirclesOfDoom m_parent;
			private float m_angle;
			private float m_spinSpeed;
			private float m_radius;
			private string m_bulletype;

		}

	}
	public class Slammo : Script
	{
		protected override IEnumerator Top()
		{
			base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("68a238ed6a82467ea85474c595c49c6e").bulletBank.GetBullet("poundSmall"));
			base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("da797878d215453abba824ff902e21b4").bulletBank.GetBullet("snakeBullet"));

			base.PostWwiseEvent("Play_BOSS_RatMech_Stomp_01", null);
			int AAmountToShoot = UnityEngine.Random.Range(20, 40);
			for (int i = 0; i < AAmountToShoot; i++)
			{
				int RNG = UnityEngine.Random.Range(0, 3);
				bool OneInTheChamber = RNG == 1;
				if (OneInTheChamber)
				{
					base.Fire(new Direction(UnityEngine.Random.Range(30 + (i / 4), -30 - (i / 2)), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(6, 10), SpeedType.Absolute), new Slammo.BabyShot());
				}
				else
				{
					base.Fire(new Direction(UnityEngine.Random.Range(30 + (i / 4), -30 - (i / 2)), DirectionType.Aim, -1f), new Speed(UnityEngine.Random.Range(6, 10), SpeedType.Absolute), new Slammo.Yees());

				}
			}
			yield break;
		}
		public class BabyShot : Bullet
		{
			public BabyShot() : base("poundSmall", false, false, false)
			{
				base.SuppressVfx = true;

			}
			protected override IEnumerator Top()
			{
				
				float speed = base.Speed;
				base.ChangeSpeed(new Speed(speed * 2, SpeedType.Absolute), 30);
				yield break;
			}

		}
		public class Yees : Bullet
		{
			public Yees() : base("snakeBullet", false, false, false)
			{
				base.SuppressVfx = true;

			}
			protected override IEnumerator Top()
			{
				float speed = base.Speed;
				base.ChangeSpeed(new Speed(speed * 1.5f, SpeedType.Absolute), 45);
				yield break;
			}

		}
	}

	public class CirclesWithOpenings : Script
	{
		protected override IEnumerator Top()
		{
			base.PostWwiseEvent("Play_BOSS_lichB_spew_01", null);
			base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("465da2bb086a4a88a803f79fe3a27677").bulletBank.GetBullet("homing"));
			base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("da797878d215453abba824ff902e21b4").bulletBank.GetBullet("snakeBullet"));

			int AAmountToShoot = UnityEngine.Random.Range(3, 7);
			for (int i = 0; i < AAmountToShoot; i++)
            {
				base.PostWwiseEvent("Play_ENM_kali_shockwave_01", null);
				this.Direction = base.AimDirection;
				for (int l = 0; l < 50; l++)
				{
					float yah = base.AimDirection;
					base.Fire(new Direction(UnityEngine.Random.Range(180, -180), DirectionType.Absolute, -1f), new Speed(9f+ AAmountToShoot, SpeedType.Absolute), new CirclesWithOpenings.Yees());

				}
				for (int l = 0; l < UnityEngine.Random.Range(1, 3); l++)
				{
					float Aim = UnityEngine.Random.Range(-45, 45);
					for (int u = 0; u < 6; u++)
                    {
						base.Fire(new Direction(Aim, DirectionType.Aim, -1f), new Speed(12f, SpeedType.Absolute), new CirclesWithOpenings.SnakeBullet(u * 3));
					}

				}
				yield return this.Wait(180/AAmountToShoot);
			}
			yield break;
		}

		public class Yees : Bullet
		{
			public Yees() : base("snakeBullet", false, false, false)
			{
				base.SuppressVfx = true;

			}
			protected override IEnumerator Top()
			{
				float speed = base.Speed;
				base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 45);
				this.Direction += 180f;
				yield return this.Wait(90);
				base.ChangeSpeed(new Speed(18f, SpeedType.Absolute), 45);
				yield break;
			}

		}
		public class SnakeBullet : Bullet
		{
			public SnakeBullet(int delay) : base("snakeBullet", false, false, false)
			{
				this.delay = delay;
			}

			protected override IEnumerator Top()
			{
				base.ManualControl = true;
				yield return base.Wait(this.delay);
				Vector2 truePosition = base.Position;
				for (int i = 0; i < 360; i++)
				{
					float offsetMagnitude = Mathf.SmoothStep(-0.5f, 0.5f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
					truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
					base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
					yield return base.Wait(1);
				}
				base.Vanish(false);
				yield break;
			}

			private int delay;
		}
	}






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
			base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("4d164ba3f62648809a4a82c90fc22cae").bulletBank.GetBullet("big_one"));
			base.aiActor.healthHaver.OnPreDeath += (obj) =>
			{
				AkSoundEngine.PostEvent("Play_VO_lichC_death_01", base.gameObject);
			};
			base.healthHaver.healthHaver.OnDeath += (obj) =>
			{
				float itemsToSpawn = UnityEngine.Random.Range(2, 5);
				float spewItemDir = 360 / itemsToSpawn;
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.SHELLRAX_DEFEATED, true);//Done
				for (int i = 0; i < itemsToSpawn; i++)
				{
					int id = BraveUtility.RandomElement<int>(Shellrax.Lootdrops);
					LootEngine.SpawnItem(PickupObjectDatabase.GetById(id).gameObject, base.aiActor.sprite.WorldCenter, new Vector2((spewItemDir * itemsToSpawn)*i, spewItemDir * itemsToSpawn), 2.2f, false, true, false);
				}

				Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
				chest2.IsLocked = false;
				chest2.RegisterChestOnMinimap(chest2.GetAbsoluteParentRoom());


			}; ;
			this.aiActor.knockbackDoer.SetImmobile(true, "nope.");

		}


	}

}







