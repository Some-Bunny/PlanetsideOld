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
	public class AnnihiChamber : AIActor
	{
		public static GameObject fuckyouprefab;
		public static readonly string guid = "annihichamber";
		private static tk2dSpriteCollectionData AnnihichamberCollection;
		public static GameObject shootpoint;
		public static GameObject shootpoint1;

		public static GameObject Laser1;
		public static GameObject Laser2;
		public static GameObject Laser3;
		public static GameObject Laser4;
		public static GameObject Laser5;
		public static GameObject Laser6;

		private static Texture2D BossCardTexture = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside/Resources/BossCards/annihichamber_bosscard.png");
		public static string TargetVFX;
		public static void Init()
		{

			AnnihiChamber.BuildPrefab();
		}

		public static void BuildPrefab()
		{
			// source = EnemyDatabase.GetOrLoadByGuid("c50a862d19fc4d30baeba54795e8cb93");
			bool flag = fuckyouprefab != null || BossBuilder.Dictionary.ContainsKey(guid);
			bool flag2 = flag;
			if (!flag2)
			{
				fuckyouprefab = BossBuilder.BuildPrefab("Annihi-Chamber", guid, spritePaths[0], new IntVector2(0, 0), new IntVector2(8, 9), false, true);
				var companion = fuckyouprefab.AddComponent<AnnihiChamberBehavior>();
				companion.aiActor.knockbackDoer.weight = 200;
				companion.aiActor.MovementSpeed = 2.5f;
				companion.aiActor.healthHaver.PreventAllDamage = false;
				companion.aiActor.CollisionDamage = 1f;
				companion.aiActor.HasShadow = false;
				companion.aiActor.IgnoreForRoomClear = false;
				companion.aiActor.aiAnimator.HitReactChance = 0f;
				companion.aiActor.specRigidbody.CollideWithOthers = true;
				companion.aiActor.specRigidbody.CollideWithTileMap = true;
				companion.aiActor.PreventFallingInPitsEver = true;
				companion.aiActor.healthHaver.ForceSetCurrentHealth(450f);
				companion.aiActor.healthHaver.SetHealthMaximum(450f);
				companion.aiActor.CollisionKnockbackStrength = 0f;
				companion.aiActor.procedurallyOutlined = false;
				companion.aiActor.CanTargetPlayers = true;
				companion.aiActor.procedurallyOutlined = true;
				companion.aiActor.gameObject.AddComponent<AfterImageTrailController>().spawnShadows = false;
				companion.aiActor.gameObject.AddComponent<tk2dSpriteAttachPoint>();
				companion.aiActor.gameObject.AddComponent<ObjectVisibilityManager>();
				companion.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376").ShadowObject;
				companion.aiActor.HasShadow = true;


				fuckyouprefab.name = companion.aiActor.OverrideDisplayName;

				/*
				OtherTools.EasyTrailOnEnemy trail1 = companion.gameObject.AddComponent<OtherTools.EasyTrailOnEnemy>();
				trail1.TrailPos = new Vector2(0.6875f, 1.5625f);
				trail1.StartColor = Color.white;
				trail1.StartWidth = 0.35f;
				trail1.EndWidth = 0;
				trail1.LifeTime = 1f;
				trail1.BaseColor = Color.red;
				trail1.EndColor = Color.white;
				trail1.castingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				trail1.name = "trail1";

				OtherTools.EasyTrailOnEnemy trail2 = companion.gameObject.AddComponent<OtherTools.EasyTrailOnEnemy>();
				trail2.TrailPos = new Vector2(0.6875f, 3.0625f);
				trail2.StartColor = Color.white;
				trail2.StartWidth = 0.35f;
				trail2.EndWidth = 0;
				trail2.LifeTime = 1f;
				trail2.BaseColor = Color.red;
				trail2.EndColor = Color.white;
				trail2.castingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				trail2.name = "trail2";

				//==============================================


				OtherTools.EasyTrailOnEnemy trail3 = companion.gameObject.AddComponent<OtherTools.EasyTrailOnEnemy>();
				trail3.TrailPos = new Vector2(2.0625f, 0.8125f);
				trail3.StartColor = Color.white;
				trail3.StartWidth = 0.35f;
				trail3.EndWidth = 0;
				trail3.LifeTime = 1f;
				trail3.BaseColor = Color.red;
				trail3.EndColor = Color.white;
				trail3.castingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				trail3.name = "trail3";

				OtherTools.EasyTrailOnEnemy trail4 = companion.gameObject.AddComponent<OtherTools.EasyTrailOnEnemy>();
				trail4.TrailPos = new Vector2(2.0625f, 3.8125f);
				trail4.StartColor = Color.white;
				trail4.StartWidth = 0.35f;
				trail4.EndWidth = 0;
				trail4.LifeTime = 1f;
				trail4.BaseColor = Color.red;
				trail4.EndColor = Color.white;
				trail4.castingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				trail4.name = "trail4";
				//======================================================

				OtherTools.EasyTrailOnEnemy trail5 = companion.gameObject.AddComponent<OtherTools.EasyTrailOnEnemy>();
				trail5.TrailPos = new Vector2(3.5625f, 3.0625f);
				trail5.StartColor = Color.white;
				trail5.StartWidth = 0.35f;
				trail5.EndWidth = 0;
				trail5.LifeTime = 1f;
				trail5.BaseColor = Color.red;
				trail5.EndColor = Color.white;
				trail5.castingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				trail5.name = "trail5";


				OtherTools.EasyTrailOnEnemy trail6 = companion.gameObject.AddComponent<OtherTools.EasyTrailOnEnemy>();
				trail6.TrailPos = new Vector2(3.5625f, 1.5625f);
				trail6.StartColor = Color.white;
				trail6.StartWidth = 0.35f;
				trail6.EndWidth = 0;
				trail6.LifeTime = 1f;
				trail6.BaseColor = Color.red;
				trail6.EndColor = Color.white;
				trail6.castingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
				trail6.name = "trail6";
				companion.aiActor.ShadowObject = EnemyDatabase.GetOrLoadByGuid("4db03291a12144d69fe940d5a01de376").ShadowObject;
				companion.aiActor.HasShadow = true;
				*/
				//companion.aiActor.gameObject.AddComponent<ImprovedAfterImage>().dashColor = Color.red;
				//companion.aiActor.gameObject.AddComponent<ImprovedAfterImage>().spawnShadows = true;


				companion.aiActor.specRigidbody.PixelColliders.Clear();
				companion.aiActor.specRigidbody.PixelColliders.Add(new PixelCollider

				{
					ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual,
					CollisionLayer = CollisionLayer.EnemyCollider,
					IsTrigger = false,
					BagleUseFirstFrameOnly = false,
					SpecifyBagelFrame = string.Empty,
					BagelColliderNumber = 0,
					ManualOffsetX = 7,
					ManualOffsetY = 10,
					ManualWidth = 52,
					ManualHeight = 52,
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
					ManualOffsetX = 7,
					ManualOffsetY = 10,
					ManualWidth = 52,
					ManualHeight = 52,
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
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"idle_left",
						"idle_right"

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};


				DirectionalAnimation anim = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"burp",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "burp",
						anim = anim
					}
				};

				DirectionalAnimation dashprime = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"dashprime",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "dashprime",
						anim = dashprime
					}
				};
				DirectionalAnimation dashdash = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"dashdash",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "dashdash",
						anim = dashdash
					}
				};

				DirectionalAnimation vomitprime = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"vomitprime",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "vomitprime",
						anim = vomitprime
					}
				};
				DirectionalAnimation vomit = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"vomit",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "vomit",
						anim = vomit
					}
				};
				//============================================================
				DirectionalAnimation cloakdash_prime = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"cloakdash_prime",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "cloakdash_prime",
						anim = cloakdash_prime
					}
				};
				DirectionalAnimation cloakdash_charge = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"cloakdash_charge",

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "cloakdash_charge",
						anim = cloakdash_charge
					}
				};




				DirectionalAnimation eee = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
	{
						"charge1",

	},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "charge1",
						anim = eee
					}
				};





				DirectionalAnimation anim3 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.Single,
					AnimNames = new string[]
					{
						"attack1_left",
						"attack1_right"

					},
					Flipped = new DirectionalAnimation.FlipType[1]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "attack1",
						anim = anim3
					}
				};


				DirectionalAnimation uncharge1 = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"uncharge1_left",
						"uncharge1_right"

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "uncharge1",
						anim = uncharge1
					}
				};


				DirectionalAnimation Hurray = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"cloak_left",
						"cloak_right"

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "cloak",
						anim = Hurray
					}
				};

				//companion.aiAnimator.AssignDirectionalAnimation("cloak", Hurray, AnimationType.Other);

				DirectionalAnimation fard = new DirectionalAnimation
				{
					Type = DirectionalAnimation.DirectionType.TwoWayHorizontal,
					AnimNames = new string[]
					{
						"cloakidle_left",
						"cloakidle_right"

					},
					Flipped = new DirectionalAnimation.FlipType[2]
				};
				aiAnimator.OtherAnimations = new List<AIAnimator.NamedDirectionalAnimation>
				{
					new AIAnimator.NamedDirectionalAnimation
					{
						name = "cloakidle",
						anim = fard
					}
				};
				//companion.aiAnimator.AssignDirectionalAnimation("cloakidle", fard, AnimationType.Other);


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

				companion.aiAnimator.AssignDirectionalAnimation("cloakidle_left", fard, AnimationType.Other);
				companion.aiAnimator.AssignDirectionalAnimation("cloakidle_right", fard, AnimationType.Other);

				companion.aiAnimator.AssignDirectionalAnimation("cloak_left", Hurray, AnimationType.Other);
				companion.aiAnimator.AssignDirectionalAnimation("cloak_right", Hurray, AnimationType.Other);
				companion.aiAnimator.AssignDirectionalAnimation("cloak", Hurray, AnimationType.Other);


				bool flag3 = AnnihichamberCollection == null;
				if (flag3)
				{
					AnnihichamberCollection = SpriteBuilder.ConstructCollection(fuckyouprefab, "AnnihichamberCollection");
					UnityEngine.Object.DontDestroyOnLoad(AnnihichamberCollection);
					for (int i = 0; i < spritePaths.Length; i++)
					{
						SpriteBuilder.AddSpriteToCollection(spritePaths[i], AnnihichamberCollection);
					}
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6

					}, "idle_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					0,
					1,
					2,
					3,
					4,
					5,
					6

					}, "idle", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{


					7,
					8,
					9,
					10,
					11,
					12,
					13,
					14


					}, "burp", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
					7,
					8,
					8,
					9,
					9,
					10,
					}, "dashprime", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
					7,
					8,
										7,
					8,
										7,
					8,
										7,
					8,
					9,
					10,
					}, "dashdash", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
					7,
					8,
					9,
					10,
					9,
					10,
					9,
					10,
					}, "vomitprime", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
					7,
					7,
					7,
					8,
					7,
					8,
					8,
					9,
					9,
					10,
					}, "vomit", tk2dSpriteAnimationClip.WrapMode.Once).fps = 7f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					15,
					16,
					17,
					18


					}, "charge1", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3.5f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					19,
					20

					}, "attack1_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					19,
					20

					}, "attack1_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					18,
					17,
					16,
					15


					}, "uncharge1_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3.5f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

						18,
					17,
					16,
					15


					}, "uncharge1_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 3.5f;
					//=======================================================================================================================
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					21,
					22,
					23,
					24,
					25,
					26,
					27


					}, "cloak_left", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
					21,
					22,
					23,
					24,
					25,
					26,
					27


					}, "cloak_right", tk2dSpriteAnimationClip.WrapMode.Loop).fps = 7f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{

					21,
					22,
					22,
					23,
					23,
					23,
					24,
					24,
					24,
					24


					}, "cloakdash_prime", tk2dSpriteAnimationClip.WrapMode.Once).fps = 9f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,
						83,

					}, "cloakdash_charge", tk2dSpriteAnimationClip.WrapMode.Once).fps = 2f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
											0,
					1,
					2,
					3,
					4,
					5,
					6,
					28,
					29,
					30,
					31,
					32,
					33,
					34
					}, "cloakidle_left", tk2dSpriteAnimationClip.WrapMode.Once).fps = 11f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
											0,
					1,
					2,
					3,
					4,
					5,
					6,
					28,
					29,
					30,
					31,
					32,
					33,
					34
					}, "cloakidle_right", tk2dSpriteAnimationClip.WrapMode.Once).fps = 11f;

					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
					35,
					36,
					37,
					38,
					39,
					40

					}, "TeleportOut", tk2dSpriteAnimationClip.WrapMode.Once).fps = 9f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
					41,
					42,
					43,
					44,
					45
					}, "TeleportIn", tk2dSpriteAnimationClip.WrapMode.Once).fps = 8f;


					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
				46,
				47,
				48,
				49,
				50,
				51,
				52,
				53,
				0,
				1,
				2,
				3,
				4,
				5,
				6,
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
				66,
				67


					}, "intro", tk2dSpriteAnimationClip.WrapMode.Once).fps = 11f;
					SpriteBuilder.AddAnimation(companion.spriteAnimator, AnnihichamberCollection, new List<int>
					{
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
				82,
				82

					}, "death", tk2dSpriteAnimationClip.WrapMode.Once).fps = 10f;

				}

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[0].eventAudio = "Play_BOSS_doormimic_vanish_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[0].triggerEvent = true;

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[20].eventAudio = "Play_BOSS_doormimic_lick_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro").frames[20].triggerEvent = true;

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("dashprime").frames[1].eventAudio = "Play_BOSS_dragun_charge_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("dashprime").frames[1].triggerEvent = true;
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("dashdash").frames[0].eventAudio = "Play_ENM_beholster_intro_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("dashdash").frames[0].triggerEvent = true;

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("cloakdash_prime").frames[1].eventAudio = "Play_BOSS_dragun_charge_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("cloakdash_prime").frames[1].triggerEvent = true;

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("cloakdash_charge").frames[0].eventAudio = "Play_ENM_beholster_intro_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("cloakdash_charge").frames[0].triggerEvent = true;

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("TeleportOut").frames[1].eventAudio = "Play_BOSS_lichA_turn_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("TeleportOut").frames[1].triggerEvent = true;

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("TeleportIn").frames[1].eventAudio = "Play_BOSS_lichA_turn_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("TeleportIn").frames[1].triggerEvent = true;

				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("vomit").frames[0].eventAudio = "Play_BOSS_doormimic_vomit_01";
				fuckyouprefab.GetComponent<tk2dSpriteAnimator>().GetClipByName("vomit").frames[0].triggerEvent = true;

				var intro = companion.aiActor.GetComponent<tk2dSpriteAnimator>().GetClipByName("intro");
				intro.frames[17].eventInfo = "lolwhat";
				intro.frames[17].triggerEvent = true;

				var clip1 = companion.aiActor.GetComponent<tk2dSpriteAnimator>().GetClipByName("dashdash");
				clip1.frames[0].eventInfo = "tempgaintrail";
				clip1.frames[0].triggerEvent = true;

				var clip2 = companion.aiActor.GetComponent<tk2dSpriteAnimator>().GetClipByName("cloakdash_charge");
				clip2.frames[0].eventInfo = "tempgaintrail2";
				clip2.frames[0].triggerEvent = true;

				var clip3 = companion.aiActor.GetComponent<tk2dSpriteAnimator>().GetClipByName("death");
				clip3.frames[3].eventInfo = "deathboom";
				clip3.frames[3].triggerEvent = true;
				clip3.frames[5].eventInfo = "deathboom";
				clip3.frames[5].triggerEvent = true;
				clip3.frames[7].eventInfo = "deathboom";
				clip3.frames[7].triggerEvent = true;
				clip3.frames[11].eventInfo = "deathboom";
				clip3.frames[11].triggerEvent = true;

				clip3.frames[14].eventInfo = "eat dicks";
				clip3.frames[14].triggerEvent = true;

				var bs = fuckyouprefab.GetComponent<BehaviorSpeculator>();
				BehaviorSpeculator behaviorSpeculator = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5").behaviorSpeculator;
				bs.OverrideBehaviors = behaviorSpeculator.OverrideBehaviors;
				bs.OtherBehaviors = behaviorSpeculator.OtherBehaviors;
				shootpoint = new GameObject("attach");
				shootpoint.transform.parent = companion.transform;
				shootpoint.transform.position = companion.sprite.WorldCenter;
				GameObject m_CachedGunAttachPoint = companion.transform.Find("attach").gameObject;


				shootpoint1 = new GameObject("Centre");
				shootpoint1.transform.parent = companion.transform;
				shootpoint1.transform.position = new Vector2(2.0625f, 2.375f);
				GameObject Centre = companion.transform.Find("Centre").gameObject;

				Laser1 = new GameObject("Laser1"); //240
				Laser1.transform.parent = companion.transform;
				Laser1.transform.position = companion.sprite.WorldBottomLeft + new Vector2(0.6875f, 1.5625f);
				GameObject LaserOne = companion.transform.Find("Laser1").gameObject;

				Laser2 = new GameObject("Laser2"); //300
				Laser2.transform.parent = companion.transform;
				Laser2.transform.position = companion.sprite.WorldBottomLeft + new Vector2(0.6875f, 3.0625f);
				GameObject LaserTwo = companion.transform.Find("Laser2").gameObject;

				Laser3 = new GameObject("Laser3"); //0
				Laser3.transform.parent = companion.transform;
				Laser3.transform.position = companion.sprite.WorldBottomLeft + new Vector2(2.0625f, 0.8125f);
				GameObject LaserThree = companion.transform.Find("Laser3").gameObject;

				Laser4 = new GameObject("Laser4"); //180
				Laser4.transform.parent = companion.transform;
				Laser4.transform.position = companion.sprite.WorldBottomLeft + new Vector2(2.0625f, 3.8125f);
				GameObject LaserFour = companion.transform.Find("Laser4").gameObject;

				Laser5 = new GameObject("Laser5");
				Laser5.transform.parent = companion.transform;//60
				Laser5.transform.position = companion.sprite.WorldBottomLeft + new Vector2(3.5625f, 3.0625f);
				GameObject LaserFive = companion.transform.Find("Laser5").gameObject;

				Laser6 = new GameObject("Laser6");
				Laser6.transform.parent = companion.transform; //120
				Laser6.transform.position = companion.sprite.WorldBottomLeft + new Vector2(3.5625f, 1.5625f);
				GameObject LaserSix = companion.transform.Find("Laser6").gameObject;

				AIActor actor = EnemyDatabase.GetOrLoadByGuid("4b992de5b4274168a8878ef9bf7ea36b");
				BeholsterController beholsterbeam = actor.GetComponent<BeholsterController>();

				AIBeamShooter2 bholsterbeam1 = companion.gameObject.AddComponent<AIBeamShooter2>();
				bholsterbeam1.beamTransform = m_CachedGunAttachPoint.transform;
				bholsterbeam1.beamModule = beholsterbeam.beamModule;
				bholsterbeam1.beamProjectile = beholsterbeam.projectile;
				bholsterbeam1.firingEllipseCenter = LaserOne.transform.position;
				bholsterbeam1.name = "240";
				bholsterbeam1.northAngleTolerance = 240 + 30;

				AIBeamShooter2 bholsterbeam2 = companion.gameObject.AddComponent<AIBeamShooter2>();
				bholsterbeam2.beamTransform = m_CachedGunAttachPoint.transform;
				bholsterbeam2.beamModule = beholsterbeam.beamModule;
				bholsterbeam2.beamProjectile = beholsterbeam.projectile;
				bholsterbeam2.firingEllipseCenter = LaserTwo.transform.position;
				bholsterbeam2.name = "300";
				bholsterbeam2.northAngleTolerance = 300 + 30;

				AIBeamShooter2 bholsterbeam3 = companion.gameObject.AddComponent<AIBeamShooter2>();
				bholsterbeam3.beamTransform = m_CachedGunAttachPoint.transform;
				bholsterbeam3.beamModule = beholsterbeam.beamModule;
				bholsterbeam3.beamProjectile = beholsterbeam.projectile;
				bholsterbeam3.firingEllipseCenter = LaserThree.transform.position;
				bholsterbeam3.name = "0";
				bholsterbeam3.northAngleTolerance = 0 + 30;

				AIBeamShooter2 bholsterbeam4 = companion.gameObject.AddComponent<AIBeamShooter2>();
				bholsterbeam4.beamTransform = m_CachedGunAttachPoint.transform;
				bholsterbeam4.beamModule = beholsterbeam.beamModule;
				bholsterbeam4.beamProjectile = beholsterbeam.projectile;
				bholsterbeam4.firingEllipseCenter = LaserFour.transform.position;
				bholsterbeam4.name = "180";
				bholsterbeam4.northAngleTolerance = 180 + 30;

				AIBeamShooter2 bholsterbeam5 = companion.gameObject.AddComponent<AIBeamShooter2>();
				bholsterbeam5.beamTransform = m_CachedGunAttachPoint.transform;
				bholsterbeam5.beamModule = beholsterbeam.beamModule;
				bholsterbeam5.beamProjectile = beholsterbeam.projectile;
				bholsterbeam5.firingEllipseCenter = LaserFive.transform.position;
				bholsterbeam5.name = "60";
				bholsterbeam5.northAngleTolerance = 60 + 30;

				AIBeamShooter2 bholsterbeam6 = companion.gameObject.AddComponent<AIBeamShooter2>();
				bholsterbeam6.beamTransform = m_CachedGunAttachPoint.transform;
				bholsterbeam6.beamModule = beholsterbeam.beamModule;
				bholsterbeam6.beamProjectile = beholsterbeam.projectile;
				bholsterbeam6.firingEllipseCenter = LaserSix.transform.position;
				bholsterbeam6.name = "120";
				bholsterbeam6.northAngleTolerance = 120 + 30;


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
				bs.OverrideBehaviors = new List<OverrideBehaviorBase>
			{
				new ModifySpeedBehavior()
				{
					minSpeed = 2.5f,
					minSpeedDistance = 5,
					maxSpeed = 5,
					maxSpeedDistance = 8
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

					Probability = 1f,
					Behavior = new DashBehavior{
					//dashAnim = "wail",
					ShootPoint = Centre,
					dashDistance = 7f,
					dashTime = 0.25f,
					doubleDashChance = 1,
					enableShadowTrail = false,
					Cooldown = 3,
					dashDirection = DashBehavior.DashDirection.PerpendicularToTarget,
					warpDashAnimLength = true,
					hideShadow = true,
					fireAtDashStart = true,
					InitialCooldown = 1f,
					AttackCooldown = 1,
					bulletScript = new CustomBulletScriptSelector(typeof(DashAttack)),
					//BulletScript = new CustomBulletScriptSelector(typeof(Wail)),
					//LeadAmount = 0f,
					//AttackCooldown = 5f,
					//InitialCooldown = 4f,
					//TellAnimation = "wail",
					//FireAnimation = "wail",
					RequiresLineOfSight = false,
					//Uninterruptible = true,
					//FireVfx = ,
					//ChargeVfx = ,
					//	MoveSpeedModifier = 0f,
						},
						NickName = "Phase 1 Dash"

					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
					Probability = 1f,
					Behavior = new CustomBeholsterLaserBehavior{

					InitialCooldown = 8f,
					firingTime = 6f,
					Cooldown = 12,
					AttackCooldown = 1.5f,
					RequiresLineOfSight = false,
					//beamSelection = ShootBeamBehavior.BeamSelection.All,
					FiresDirectlyTowardsPlayer = true,
					UsesCustomAngle = true,

					chargeTime = 1.5f,
					UsesBaseSounds = true,
					//LaserFiringSound = "Play_ENM_deathray_shot_01",
					//StopLaserFiringSound = "Stop_ENM_deathray_loop_01",
					//ChargeAnimation = "charge1",
					//FireAnimation = "attack1",
					//PostFireAnimation = "uncharge1",
					beamSelection = ShootBeamBehavior.BeamSelection.All,
					trackingType = CustomBeholsterLaserBehavior.TrackingType.ConstantTurn,
					//initialAimType = CustomShootBeamBehavior.InitialAimType.Aim,
					BulletScript = new CustomBulletScriptSelector(typeof(BigBall)),
					ShootPoint = Centre.transform,
					maxTurnRate = 12,
					turnRateAcceleration = 12,
					LockInPlaceWhileAttacking = false,
					useUnitOvershoot = true,
					minUnitForOvershoot = 1,

					},
					NickName = "LASERZ"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
					Probability = 1f,
					Behavior = new ChargeBehavior{
						InitialCooldown = 1,
						chargeAcceleration = -1,
						chargeSpeed = 35,
						maxChargeDistance = -1,
						bulletScript = new CustomBulletScriptSelector(typeof(ChargeAttack1Attack)),
						ShootPoint = Centre,
						chargeDamage = 0.5f,
						chargeKnockback = 100,
						collidesWithDodgeRollingPlayers = false,
						primeAnim = "dashprime",
						primeTime = 1f,
						chargeAnim = "dashdash",
						stopDuringPrime = false,
						stoppedByProjectiles = false,
						wallRecoilForce = 50,
						AttackCooldown = 1.5f,
						
					},
					NickName = "Phase 1 CHrge"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
					Probability = 1f,
					Behavior = new ShootBehavior()
					{
							TellAnimation = "vomitprime",
							FireAnimation = "vomit",
							BulletScript = new CustomBulletScriptSelector(typeof(VomitsGutsAndShit)),
							LeadAmount = 0,
							StopDuring = ShootBehavior.StopType.Attack,
							AttackCooldown = 0.875f,
							Cooldown = 4f,

							RequiresLineOfSight = true,
							ShootPoint = Centre,
							CooldownVariance = 0f,
							GlobalCooldown = 0,
							InitialCooldown = 0,
							InitialCooldownVariance = 0,
							GroupName = null,
							MinRange = 0,
							Range = 1000,
							MinWallDistance = 0,
							MaxEnemiesInRoom = -1,
							MinHealthThreshold = 0,
							MaxHealthThreshold = 1,
							HealthThresholds = new float[0],
							AccumulateHealthThresholds = true,
							targetAreaStyle = null,
							IsBlackPhantom = false,
							resetCooldownOnDamage = null,
							MaxUsages = 0,

					},
					NickName = "Vomit B O N E S"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 0f,
					Behavior = new TeleportBehavior{
					AttackableDuringAnimation = true,
					AllowCrossRoomTeleportation = false,
					teleportRequiresTransparency = false,
					hasOutlinesDuringAnim = true,
					ManuallyDefineRoom = false,
					MaxHealthThreshold = 1f,
					StayOnScreen = true,
					AvoidWalls = true,
					GoneTime = 0.5f,
					OnlyTeleportIfPlayerUnreachable = false,
					MinDistanceFromPlayer = 10f,
					MaxDistanceFromPlayer = 5f,
					teleportInAnim = "TeleportIn",
					teleportOutAnim = "TeleportOut",
					AttackCooldown = 0.25f,
					InitialCooldown = 0f,
					RequiresLineOfSight = false,
					roomMax = new Vector2(0,0),
					roomMin = new Vector2(0,0),
					//teleportInBulletScript = new CustomBulletScriptSelector(typeof(ShellRaxDropCursedAreas)),
					//teleportOutBulletScript = new CustomBulletScriptSelector(typeof(ShellRaxDropCursedAreas)),
					GlobalCooldown = 0f,
					Cooldown = 0.5f,
					
					CooldownVariance = 0f,
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
					targetAreaStyle = null,
					HealthThresholds = new float[0],
					MinWallDistance = 0,
					},
					NickName = "ShadePort"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{
					Probability = 0f,
					Behavior = new ChargeBehavior{
						InitialCooldown = 1,
						chargeAcceleration = -1,
						chargeSpeed = 25,
						maxChargeDistance = -1,
						bulletScript = new CustomBulletScriptSelector(typeof(FakeOutCharge)),
						ShootPoint = Centre,
						chargeDamage = 0.5f,
						chargeKnockback = 100,
						collidesWithDodgeRollingPlayers = false,
						primeAnim = "cloakdash_prime",
						primeTime = 0.66f,
						chargeAnim = "cloakdash_charge",
						stopDuringPrime = true,
						stoppedByProjectiles = false,
						Cooldown = 1f,
						AttackCooldown = 0.25f,
						wallRecoilForce = 50,
						
					},
					NickName = "Phase 2 CHrge"
					},
					new AttackBehaviorGroup.AttackGroupItem()
					{

					Probability = 0f,
					Behavior = new DashBehavior{
					//dashAnim = "wail",
					ShootPoint = Centre,
					dashDistance = 5f,
					dashTime = 0.166f,
					doubleDashChance = 0,
					enableShadowTrail = false,
					Cooldown = 2f,
					AttackCooldown = 0.5f,
					dashDirection = DashBehavior.DashDirection.Random,
					warpDashAnimLength = true,
					hideShadow = true,
					fireAtDashStart = true,
					InitialCooldown = 1f,
					
					bulletScript = new CustomBulletScriptSelector(typeof(FakeOut)),
					//BulletScript = new CustomBulletScriptSelector(typeof(Wail)),
					//LeadAmount = 0f,
					//AttackCooldown = 5f,
					//InitialCooldown = 4f,
					//TellAnimation = "wail",
					//FireAnimation = "wail",
					RequiresLineOfSight = false,
					//Uninterruptible = true,
					//FireVfx = ,
					//ChargeVfx = ,
					//	MoveSpeedModifier = 0f,
						},
						NickName = "Phase 2 Dash"

					},
				};


				bs.InstantFirstTick = behaviorSpeculator.InstantFirstTick;
				bs.TickInterval = behaviorSpeculator.TickInterval;
				bs.PostAwakenDelay = behaviorSpeculator.PostAwakenDelay;
				bs.RemoveDelayOnReinforce = behaviorSpeculator.RemoveDelayOnReinforce;
				bs.OverrideStartingFacingDirection = behaviorSpeculator.OverrideStartingFacingDirection;
				bs.StartingFacingDirection = behaviorSpeculator.StartingFacingDirection;
				bs.SkipTimingDifferentiator = behaviorSpeculator.SkipTimingDifferentiator;
				Game.Enemies.Add("psog:annihi-chamber", companion.aiActor);

				GameObject wat = ItemBuilder.AddSpriteToObject("confused", "Planetside/Resources/VFX/ConfusedChamber/confusedchamber1", null);
				FakePrefab.MarkAsFakePrefab(wat);
				UnityEngine.Object.DontDestroyOnLoad(wat);
				tk2dSpriteAnimator yees = wat.AddComponent<tk2dSpriteAnimator>();
				tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
				animationClip.fps = 11;
				animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Once;
				animationClip.name = "start";

				GameObject spriteObject = new GameObject("spriteObject");
				ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/VFX/ConfusedChamber/confusedchamber1", spriteObject);
				tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
				starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
				starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
				tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
				{
				starterFrame
				};
				animationClip.frames = frameArray;
				for (int i = 2; i < 10; i++)
				{
					GameObject spriteForObject = new GameObject("spriteForObject");
					ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/VFX/ConfusedChamber/confusedchamber{i}", spriteForObject);
					tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
					frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
					frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
					animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
				}
				yees.Library = yees.gameObject.AddComponent<tk2dSpriteAnimation>();
				yees.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
				yees.DefaultClipId = yees.GetClipIdByName("start");
				yees.playAutomatically = true;
				yees.ignoreTimeScale = true;
				yees.AlwaysIgnoreTimeScale = true;
				yees.AnimateDuringBossIntros = true;
				ConfusedPrefab = wat;

				SpriteBuilder.AddSpriteToCollection("Planetside/Resources/AnnihiChamber/annihichamber_idle_001.png", SpriteBuilder.ammonomiconCollection);
				if (companion.GetComponent<EncounterTrackable>() != null)
				{
					UnityEngine.Object.Destroy(companion.GetComponent<EncounterTrackable>());
				}
				companion.encounterTrackable = companion.gameObject.AddComponent<EncounterTrackable>();
				companion.encounterTrackable.journalData = new JournalEntry();
				companion.encounterTrackable.EncounterGuid = "psog:annihi-chamber";
				companion.encounterTrackable.prerequisites = new DungeonPrerequisite[0];
				companion.encounterTrackable.journalData.SuppressKnownState = false;
				companion.encounterTrackable.journalData.IsEnemy = true;
				companion.encounterTrackable.journalData.SuppressInAmmonomicon = false;
				companion.encounterTrackable.ProxyEncounterGuid = "";
				companion.encounterTrackable.journalData.AmmonomiconSprite = "Planetside/Resources/AnnihiChamber/annihichamber_idle_001";
				companion.encounterTrackable.journalData.enemyPortraitSprite = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\Ammocom\\annihichamberSheet.png");
				PlanetsideModule.Strings.Enemies.Set("#ANNIHICHAMBER", "Annihi-Chamber");
				PlanetsideModule.Strings.Enemies.Set("#ANNIHICHAMBER_SHORTDESC", "Six Circles Of Hell");

				List<string> PotentialEntries = new List<string>
				{
					"Corroded, waste casts thrown into the depths of Bullet Hell, and taken over by a parasitic will fueled by six souls. Each eye is a separate voice, with a separate story, yearning for something they now hold dear.\n\nEVERY MOVEMENT, EVERY TWITCH SENDS ME INTO A RAGE. I WOULD TEAR EVERYTHING TO PIECES IF IT WERE NOT FOR THIS LIMITED FORM.\n\n...\n\n...\n\n...\n\n...\n\n...",
					"Corroded, waste casts thrown into the depths of Bullet Hell, and taken over by a parasitic will fueled by six souls. Each eye is a separate voice, with a separate story, yearning for something they now hold dear.\n\n...\n\nI miss him...\n\n...\n\n...\n\n...\n\n...",
					"Corroded, waste casts thrown into the depths of Bullet Hell, and taken over by a parasitic will fueled by six souls. Each eye is a separate voice, with a separate story, yearning for something they now hold dear.\n\n...\n\n...\n\nthis was not the afterlife i studied for this was not the afterlife i studied for this was not the afterlife i studied for this was not the afterlife i studied for\n\n...\n\n...\n\n...",
					"Corroded, waste casts thrown into the depths of Bullet Hell, and taken over by a parasitic will fueled by six souls. Each eye is a separate voice, with a separate story, yearning for something they now hold dear.\n\n...\n\n...\n\n...\n\nMY_MACHINE_SPIRIT_CALLS_FOR_THE_COMFORT_OF_MY_OLD_METAL,_EVEN_IF_MY_PURPOSE_WAS_SELF_SACRIFICE._IN_DEATH,_DO_I_VALUE_MY_OLD_LIFE.\n\n...\n\n...",
					"Corroded, waste casts thrown into the depths of Bullet Hell, and taken over by a parasitic will fueled by six souls. Each eye is a separate voice, with a separate story, yearning for something they now hold dear.\n\n...\n\n...\n\n...\n\n...\n\nTo see the Keep once more, oh how much I'd sacrifice... I hope this damned vessel drags itself there one day... May my old guard comrades slay this vessel so my soul is released to chance a ghostly form...\n\n...",
					"Corroded, waste casts thrown into the depths of Bullet Hell, and taken over by a parasitic will fueled by six souls. Each eye is a separate voice, with a separate story, yearning for something they now hold dear.\n\n...\n\n...\n\n...\n\n...\n\n...\n\nI shouldn't have stepped in...I wanted to help the Master, and paid with my life... Would He have spared his own greatest student... even with his insanity?",
					"THEIR SOULS AS MY EYES. I WILL NOT LET THEM DIE"

				};
				System.Random r = new System.Random();
				int index = r.Next(PotentialEntries.Count);
				string randomString = PotentialEntries[index];

				PlanetsideModule.Strings.Enemies.Set("#ANNIHICHAMBER_LONGDESC", randomString);
				companion.encounterTrackable.journalData.PrimaryDisplayName = "#ANNIHICHAMBER";
				companion.encounterTrackable.journalData.NotificationPanelDescription = "#ANNIHICHAMBER_SHORTDESC";
				companion.encounterTrackable.journalData.AmmonomiconFullEntry = "#ANNIHICHAMBER_LONGDESC";
				EnemyBuilder.AddEnemyToDatabase(companion.gameObject, "psog:annihi-chamber");
				EnemyDatabase.GetEntry("psog:annihi-chamber").ForcedPositionInAmmonomicon = 202;
				EnemyDatabase.GetEntry("psog:annihi-chamber").isInBossTab = false;
				EnemyDatabase.GetEntry("psog:annihi-chamber").isNormalEnemy = true;

				GenericIntroDoer miniBossIntroDoer = fuckyouprefab.AddComponent<GenericIntroDoer>();
				fuckyouprefab.AddComponent<AnnihiChamberIntroController>();
				miniBossIntroDoer.triggerType = GenericIntroDoer.TriggerType.PlayerEnteredRoom;
				miniBossIntroDoer.initialDelay = 0.15f;
				miniBossIntroDoer.cameraMoveSpeed = 14;
				miniBossIntroDoer.specifyIntroAiAnimator = null;
				miniBossIntroDoer.BossMusicEvent = "Play_MUS_Boss_Theme_Lich";
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
				PlanetsideModule.Strings.Enemies.Set("#ANNIHICHAMBER_NAME", "ANNIHI-CHAMBER");
				PlanetsideModule.Strings.Enemies.Set("#ANNIHICHAMBER_SHORTNAME", "SIX CIRCLES OF HELL");
				PlanetsideModule.Strings.Enemies.Set("#QUOTE", "");

				miniBossIntroDoer.portraitSlideSettings = new PortraitSlideSettings()
				{
					bossNameString = "#ANNIHICHAMBER_NAME",
					bossSubtitleString = "#ANNIHICHAMBER_SHORTNAME",
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

				//==================
				//Important for not breaking basegame stuff!
				StaticReferenceManager.AllHealthHavers.Remove(companion.aiActor.healthHaver);
				//==================
			}


		}
		private static GameObject ConfusedPrefab;


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

		public class AnnihiChamberBehavior : BraveBehaviour
		{
			private RoomHandler m_StartRoom;

			public void Update()
			{
				m_StartRoom = aiActor.GetAbsoluteParentRoom();
				if (!base.aiActor.HasBeenEngaged)
				{
					CheckPlayerRoom();
				}
				bool flag = base.aiActor && base.aiActor.healthHaver;
				if (flag)
				{
					float maxHealth = base.aiActor.healthHaver.GetMaxHealth();
					float num = maxHealth * 0.35f;
					float currentHealth = base.aiActor.healthHaver.GetCurrentHealth();
					bool flag2 = currentHealth < num;
					if (flag2)
					{
						if (Phase2AnnihiChamberCheck != true)
						{
							StaticReferenceManager.DestroyAllEnemyProjectiles();
							Phase2AnnihiChamberCheck = true;
							base.aiActor.aiAnimator.OverrideIdleAnimation = "cloak";
							base.aiActor.aiAnimator.OverrideMoveAnimation = "cloak";
							base.aiAnimator.PlayUntilFinished("cloakidle_left", true, null, -1f, false);
							base.aiActor.behaviorSpeculator.Interrupt();
							base.aiActor.healthHaver.IsVulnerable = false;
							foreach (OtherTools.EasyTrailOnEnemy c in base.aiActor.gameObject.GetComponents(typeof(OtherTools.EasyTrailOnEnemy)))
							{
								c.SetMode(UnityEngine.Rendering.ShadowCastingMode.On);
								//c.castingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
								//ETGModConsole.Log(c.name);
							}

							if (!base.aiActor.IsBlackPhantom)
							{
								Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
								mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
								mat.SetColor("_EmissiveColor", new Color32(255, 255, 255, 255));
								mat.SetFloat("_EmissiveColorPower", 1.55f);
								mat.SetFloat("_EmissivePower", 100);
								mat.SetFloat("_EmissiveThresholdSensitivity", 0.05f);

								base.aiActor.sprite.renderer.material = mat;
								//yes.dashColor = Color.white;
							}
							else
							{
								//yes.dashColor = Color.red;
							}
							for (int j = 0; j < base.aiActor.behaviorSpeculator.AttackBehaviors.Count; j++)
							{
								if (base.behaviorSpeculator.AttackBehaviors[j] is AttackBehaviorGroup && base.behaviorSpeculator.AttackBehaviors[j] != null)
								{
									this.ProcessAttackGroup(base.behaviorSpeculator.AttackBehaviors[j] as AttackBehaviorGroup);
								}
							}
							base.aiActor.MovementSpeed = 5f;
							m_StartRoom.BecomeTerrifyingDarkRoom(3f, 0.5f, 0.1f, "Play_ENM_darken_world_01");
							GameManager.Instance.StartCoroutine(LoseIFRames());

						}
					}
				}
			}
			private void ProcessAttackGroup(AttackBehaviorGroup attackGroup)
			{
				for (int i = 0; i < attackGroup.AttackBehaviors.Count; i++)
				{
					AttackBehaviorGroup.AttackGroupItem attackGroupItem = attackGroup.AttackBehaviors[i];
					if (attackGroup != null && attackGroupItem.NickName == "Phase 1 Dash")
					{
						attackGroupItem.Probability = 0f;
					}
					if (attackGroup != null && attackGroupItem.NickName == "LASERZ")
					{
						attackGroupItem.Probability = 0f;
					}
					if (attackGroup != null && attackGroupItem.NickName == "Vomit B O N E S")
					{
						attackGroupItem.Probability = 0f;
					}
					if (attackGroup != null && attackGroupItem.NickName == "Phase 1 CHrge")
					{
						attackGroupItem.Probability = 0f;
					}
					else if (attackGroup != null && attackGroupItem.NickName == "ShadePort")
					{
						attackGroupItem.Probability = 1f;
					}
					else if (attackGroup != null && attackGroupItem.NickName == "Phase 2 CHrge")
					{
						attackGroupItem.Probability = 1f;
					}
					else if (attackGroup != null && attackGroupItem.NickName == "Phase 2 Dash")
					{
						attackGroupItem.Probability = 1f;
					}
				}
			}
			public Material PitCausticsMaterial;
			private void Start()
			{
				//Important for not breaking basegame stuff!
				StaticReferenceManager.AllHealthHavers.Remove(base.aiActor.healthHaver);

				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("044a9f39712f456597b9762893fbc19c").bulletBank.bulletBank.GetBullet("gross"));
				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1b5810fafbec445d89921a4efb4e42b7").bulletBank.bulletBank.GetBullet("firehose"));
				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5").bulletBank.GetBullet("teeth_football"));
				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("f905765488874846b7ff257ff81d6d0c").bulletBank.GetBullet("spore2"));

				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("463d16121f884984abe759de38418e48").bulletBank.GetBullet("ball"));
				base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("463d16121f884984abe759de38418e48").bulletBank.GetBullet("link"));

				base.aiActor.spriteAnimator.AnimationEventTriggered += this.AnimationEventTriggered;
				//firehose
				Phase2AnnihiChamberCheck = false;
				base.aiActor.healthHaver.OnPreDeath += (obj) =>
				{
					m_StartRoom.EndTerrifyingDarkRoom(1f, 0.1f, 1f, "Play_ENM_lighten_world_01");
				};
				base.healthHaver.healthHaver.OnDeath += (obj) =>
				{
					float itemsToSpawn = UnityEngine.Random.Range(2, 5);
					float spewItemDir = 360 / itemsToSpawn;
					for (int i = 0; i < itemsToSpawn; i++)
					{
						int id = BraveUtility.RandomElement<int>(Shellrax.Lootdrops);
						LootEngine.SpawnItem(PickupObjectDatabase.GetById(id).gameObject, base.aiActor.sprite.WorldCenter, new Vector2((spewItemDir * itemsToSpawn) * i, spewItemDir * itemsToSpawn), 2.2f, false, true, false);
					}

					Chest chest2 = GameManager.Instance.RewardManager.SpawnTotallyRandomChest(GameManager.Instance.PrimaryPlayer.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
					chest2.IsLocked = false;
					chest2.RegisterChestOnMinimap(chest2.GetAbsoluteParentRoom());
					AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.DEFEAT_ANNIHICHAMBER, true);


				}; ;
				this.aiActor.knockbackDoer.SetImmobile(true, "nope.");

			}
			private IEnumerator LoseIFRames()
            {
				yield return new WaitForSeconds(3.25f);
				base.aiActor.healthHaver.IsVulnerable = true;
				yield break;
            }
			private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameIdx)
			{
				if (clip.GetFrame(frameIdx).eventInfo == "lolwhat")
                {
					PlayerController player = GameManager.Instance.PrimaryPlayer;

					if (player.HasGun(ParasiticHeart.HeartID))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.CONFUSED_THE_CHAMBER, true);
						GameObject lightning = base.aiActor.PlayEffectOnActor(ConfusedPrefab, new Vector3(0f, -1f, 0f));
						lightning.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(base.aiActor.sprite.WorldTopCenter, tk2dBaseSprite.Anchor.LowerCenter);
						lightning.transform.position.WithZ(transform.position.z + 2);
						lightning.GetComponent<tk2dSpriteAnimator>().Play();
					}
				}
				if (clip.GetFrame(frameIdx).eventInfo == "tempgaintrail")
				{
					ImprovedAfterImage yes = base.aiActor.gameObject.AddComponent<ImprovedAfterImage>();
					yes.spawnShadows = true;
					yes.shadowLifetime = 0.5f;
					yes.shadowTimeDelay = 0.001f;
					yes.dashColor = Color.clear;
					yes.name = "Temp Trail";
					GameManager.Instance.StartCoroutine(WaitForTrail());
				}
				if (clip.GetFrame(frameIdx).eventInfo == "tempgaintrail2")
                {
					ImprovedAfterImage yes = base.aiActor.gameObject.AddComponent<ImprovedAfterImage>();
					yes.spawnShadows = true;
					yes.shadowLifetime = 0.5f;
					yes.shadowTimeDelay = 0f;
					yes.dashColor = Color.white;
					yes.name = "Temp Trail2";
					GameManager.Instance.StartCoroutine(WaitForTrail());
				}
				if (clip.GetFrame(frameIdx).eventInfo == "deathboom")
                {
					AkSoundEngine.PostEvent("Play_BigSlam", base.gameObject);
				}
				if (clip.GetFrame(frameIdx).eventInfo == "eat dicks")
                {
					AkSoundEngine.PostEvent("Play_BOSS_lichB_grab_01", gameObject);
					GameObject hand = UnityEngine.Object.Instantiate<GameObject>(PlanetsideModule.hellDrag.HellDragVFX);
					tk2dBaseSprite component1 = hand.GetComponent<tk2dBaseSprite>();
					component1.usesOverrideMaterial = true;
					component1.PlaceAtLocalPositionByAnchor(base.aiActor.specRigidbody.UnitCenter, tk2dBaseSprite.Anchor.LowerCenter);
					component1.renderer.material.shader = ShaderCache.Acquire("Brave/Effects/StencilMasked");
					base.aiActor.sprite.renderer.enabled = false;
				}
			}
			private IEnumerator WaitForTrail()
			{
				yield return new WaitForSeconds(2.5f);
				foreach (ImprovedAfterImage c in base.aiActor.gameObject.GetComponents(typeof(ImprovedAfterImage)))
				{
					if (c.name == "Temp Trail" && c != null)
					{
						Destroy(c);
					}
					if (c.name == "Temp Trail2" && c != null)
					{
						Destroy(c);
					}
				}
				yield break;
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
				if (GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() != null && GameManager.Instance.PrimaryPlayer.GetAbsoluteParentRoom() == m_StartRoom)
				{
					base.aiActor.HasBeenEngaged = true;
				}
				yield break;
			}
			public static bool Phase2AnnihiChamberCheck;
		}



		private static string[] spritePaths = new string[]
		{
			"Planetside/Resources/AnnihiChamber/annihichamber_idle_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_idle_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_idle_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_idle_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_idle_005.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_idle_006.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_idle_007.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_burp_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_burp_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_burp_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_burp_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_burp_005.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_burp_006.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_burp_007.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_burp_008.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_charge1_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_charge1_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_charge1_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_charge1_004.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_fire1_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_fire1_002.png",
			//=======================================================================================================

			"Planetside/Resources/AnnihiChamber/annihichamber_cloakidle_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloakidle_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloakidle_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloakidle_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloakidle_005.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloakidle_006.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloakidle_007.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_cloak_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloak_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloak_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloak_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloak_005.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloak_006.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_cloak_007.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_warpout_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpout_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpout_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpout_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpout_005.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpout_006.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_warpin_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpin_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpin_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpin_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_warpin_005.png",
			//=======================================================================================================

			"Planetside/Resources/AnnihiChamber/annihichamber_intro_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_005.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_006.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_007.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_008.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_009.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_010.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_011.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_012.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_013.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_014.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_015.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_016.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_017.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_018.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_019.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_020.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_021.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_intro_022.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_death_001.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_002.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_003.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_004.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_005.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_006.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_007.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_008.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_009.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_010.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_011.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_012.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_013.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_014.png",
			"Planetside/Resources/AnnihiChamber/annihichamber_death_015.png",

			"Planetside/Resources/AnnihiChamber/annihichamber_charge_001.png",

			

		};
		public class DashAttack : Script
		{
			protected override IEnumerator Top()
			{
				float Dir = UnityEngine.Random.Range(-180, 180);
				Vector2 targetPos = base.GetPredictedTargetPosition(0.72f, 10);
				bool shouldHome = true;
				for (int k = 0; k < 3; k++)
				{
					for (int u = 0; u < 6; u++)
					{
						base.Fire(new Direction((120 * k) + Dir, DirectionType.Aim, -1f), new Speed(7f, SpeedType.Absolute), new Spit(u * 3, targetPos, shouldHome));
					}
					Dir = UnityEngine.Random.Range(-180, 180);
					for (int u = 0; u < 3; u++)
					{
						base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(6f, SpeedType.Absolute), new Spit(u * 4, targetPos, shouldHome));
					}
					yield return this.Wait(1);
				}
				this.FireSpinningLine(new Vector2(-0.75f, 0f), new Vector2(0.75f, 0f), 3);
				this.FireSpinningLine(new Vector2(0f, -0.75f), new Vector2(0f, 0.75f), 3);
				yield break;
			}
			private void FireSpinningLine(Vector2 start, Vector2 end, int numBullets)
			{
				start *= 0.5f;
				end *= 0.5f;
				float direction = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
				for (int i = 0; i < numBullets; i++)
				{
					Vector2 b = Vector2.Lerp(start, end, (float)i / ((float)numBullets - 1f));
					base.Fire(new Direction(direction, DirectionType.Absolute, -1f), new DashAttack.SpinningBullet(base.Position, base.Position + b));
				}
			}
			public class SpinningBullet : Bullet
			{
				public SpinningBullet(Vector2 origin, Vector2 startPos) : base("gross", false, false, false)
				{
					this.m_origin = origin;
					this.m_startPos = startPos;
					base.SuppressVfx = true;
				}

				protected override IEnumerator Top()
				{
					base.ManualControl = true;
					Vector2 delta = (this.m_startPos - base.Position) / 45f;
					for (int i = 0; i < 45; i++)
					{
						base.Position += delta;
						yield return base.Wait(1);
					}
					this.Speed = 10f;
					float angle = 0f;
					Vector2 centerOfMass = this.m_origin;
					Vector2 centerOfMassOffset = this.m_origin - base.Position;
					for (int j = 0; j < 120; j++)
					{
						this.Direction = Mathf.MoveTowardsAngle(this.Direction, (this.BulletManager.PlayerPosition() - centerOfMass).ToAngle(), 1.5f);
						base.UpdateVelocity();
						centerOfMass += this.Velocity / 60f;
						angle += 2f;
						base.Position = centerOfMass + (Quaternion.Euler(0f, 0f, angle) * centerOfMassOffset).XY();
						yield return base.Wait(1);
					}
					base.Vanish(false);
					yield break;
				}
				private const float RotationSpeed = 6f;
				private Vector2 m_origin;
				private Vector2 m_startPos;
			}
		}

		public class Spit : Bullet
		{
			public Spit(int delay, Vector2 target, bool shouldHome) : base("gross", false, false, false)
			{
				this.m_delay = delay;
				this.m_target = target;
				this.m_shouldHome = shouldHome;
			}
			protected override IEnumerator Top()
			{
				base.ManualControl = true;
				yield return base.Wait(this.m_delay);
				Vector2 truePosition = base.Position;
				for (int i = 0; i < 360; i++)
				{
					float offsetMagnitude = Mathf.SmoothStep(-0.75f, 0.75f, Mathf.PingPong(0.5f + (float)i / 60f * 3f, 1f));
					if (this.m_shouldHome && i > 20 && i < 60)
					{
						float num = (this.m_target - truePosition).ToAngle();
						float value = BraveMathCollege.ClampAngle180(num - this.Direction);
						this.Direction += Mathf.Clamp(value, -6f, 6f);
					}
					truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
					base.Position = truePosition + BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude);
					yield return base.Wait(1);
				}
				base.Vanish(false);
				yield break;
			}
			private int m_delay;
			private Vector2 m_target;
			private bool m_shouldHome;
		}
		public class BigBall : Script
		{
			protected override IEnumerator Top()
			{
				float startAngle = base.RandomAngle();
				for (int i = 0; i < 8; i++)
				{
					float delta = 60f;
					for (int A = 0; A < 6; A++)
					{
						float num = startAngle + (float)A * delta;
						for (int E = 0; E < 6; E++)
						{
							this.Fire(new Direction((num) + 30*i, Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Relative), new Break(4 * E, base.Position, false, num, 0.15f));
						}
					}
					for (int A = 0; A < 6; A++)
					{
						float num = startAngle + (float)A * delta;
						for (int E = 0; E < 6; E++)
						{
							this.Fire(new Direction((num) + 30 * i, Brave.BulletScript.DirectionType.Absolute, -1f), new Speed(7f, SpeedType.Relative), new Break(4 * E, base.Position, true, num, 0.15f));
						}
					}

					yield return base.Wait(45);
				}
				yield break;
			}
			
			public class Break : Bullet
			{
				public Break(int delay, Vector2 centerPoint, bool speeen, float startAngle, float spinspeed) : base("gross", false, false, false)
				{
					this.centerPoint = centerPoint;
					this.yesToSpeenOneWay = speeen;
					this.startAngle = startAngle;
					this.SpinSpeed = spinspeed;
					this.m_delay = delay;
				}
				protected override IEnumerator Top()
				{
					base.ManualControl = true;
					yield return base.Wait(this.m_delay);
					float radius = Vector2.Distance(this.centerPoint, base.Position);
					float speed = this.Speed;
					float spinAngle = this.startAngle;
					float spinSpeed = 0f;
					for (int i = 0; i < 300; i++)
					{
						if (i >= 10)
						{
							radius += speed / 60f;
							if (yesToSpeenOneWay == true)
							{
								spinSpeed -= SpinSpeed;
							}
							else
							{
								spinSpeed += SpinSpeed;

							}
							spinAngle += spinSpeed / 60f;
						}
						base.Position = this.centerPoint + BraveMathCollege.DegreesToVector(spinAngle, radius);
						yield return base.Wait(1);
					}
					base.Vanish(false);
					yield break;
				}
				public Vector2 centerPoint;
				public bool yesToSpeenOneWay;
				public float startAngle;
				public float SpinSpeed;
				private int m_delay;
			}
		}
		public class ChargeAttack1Attack : Script
		{

			protected override IEnumerator Top()
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5").bulletBank.GetBullet("teeth_football"));
				this.FireCluster(this.Direction);
				for (int i = 0; i < 25; i++)
				{
					float num = -45f + (float)i * 3.75f;
					base.Fire(new Offset(0.5f, 0f, this.Direction + num, string.Empty, DirectionType.Aim), new Direction(num, DirectionType.Aim, -1f), new Speed(11f, SpeedType.Absolute), null);
				}
				this.FireCluster(this.Direction - 45f);
				this.FireCluster(this.Direction + 45f);
				this.FireCluster(this.Direction);
				//base.Fire(new Direction(-12f - (i * 1.33f), DirectionType.Aim, -1f), new Speed(12 - (i / 6f), SpeedType.Absolute), new ChargeAttack1Attack.CrossBullet(new Vector2(0.2f * i, 0f), 0, 5 * i));
				//base.Fire(new Direction(12f + (i * 1.33f), DirectionType.Aim, -1f), new Speed(12 - (i / 6f), SpeedType.Absolute), new ChargeAttack1Attack.CrossBullet(new Vector2(0.2f * i, 0f), 0, 5 * i));
				yield break;
			}
			private void FireCluster(float direction)
			{
				base.Fire(new Offset(-0.5f, 0f, direction, string.Empty, DirectionType.Aim), new Direction(direction, DirectionType.Aim, -1f), new Speed(13f, SpeedType.Absolute), new ChargeAttack1Attack.TootthBullet());
				base.Fire(new Offset(-0.275f, 0.25f, direction, string.Empty, DirectionType.Aim), new Direction(direction, DirectionType.Aim, -1f), new Speed(13f, SpeedType.Absolute), new ChargeAttack1Attack.TootthBullet());
				base.Fire(new Offset(-0.275f, -0.25f, direction, string.Empty, DirectionType.Aim), new Direction(direction, DirectionType.Aim, -1f), new Speed(13f, SpeedType.Absolute), new ChargeAttack1Attack.TootthBullet());
				base.Fire(new Offset(0f, 0.4f, direction, string.Empty, DirectionType.Aim), new Direction(direction, DirectionType.Aim, -1f), new Speed(13f, SpeedType.Absolute), new ChargeAttack1Attack.TootthBullet());
				base.Fire(new Offset(0f, -0.4f, direction, string.Empty, DirectionType.Aim), new Direction(direction, DirectionType.Aim, -1f), new Speed(13f, SpeedType.Absolute), new ChargeAttack1Attack.TootthBullet());
			}

			public class TootthBullet : Bullet
			{
				public TootthBullet(): base("teeth_football", false, false, false)
				{

				}
			}
			public class CrossBullet : Bullet
			{
				public CrossBullet(Vector2 offset, int setupDelay, int setupTime) : base("teeth_football", false, false, false)
				{
					this.m_offset = offset;
					this.m_setupDelay = setupDelay;
					this.m_setupTime = setupTime;
				}
				protected override IEnumerator Top()
				{
					base.ManualControl = true;
					this.m_offset = this.m_offset.Rotate(this.Direction);
					for (int i = 0; i < 360; i++)
					{
						if (i > this.m_setupDelay && i < this.m_setupDelay + this.m_setupTime)
						{
							base.Position += this.m_offset / (float)this.m_setupTime;
						}
						float speed = base.Speed;
						base.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
						yield return base.Wait(1);
					}
					base.Vanish(false);
					yield break;
				}

				private Vector2 m_offset;
				private int m_setupDelay;
				private int m_setupTime;
			}
		}
		public class VomitsGutsAndShit : Script
		{
			protected override IEnumerator Top()
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("796a7ed4ad804984859088fc91672c7f").bulletBank.bulletBank.GetBullet("default"));
				float aim = base.AimDirection;
				for (int i = 0; i < 150; i++)
				{
					float newAim = base.AimDirection;
					aim = Mathf.MoveTowardsAngle(aim, newAim, 1f);
					float t = Mathf.PingPong((float)i / 60f, 1f);
					Bullet bullet;
					bullet = new VomitsGutsAndShit.FirehoseBullet((float)((t >= 0.1f) ? 1 : -1));
					base.Fire(new Offset(UnityEngine.Random.insideUnitCircle * 0.5f, 0f, string.Empty, DirectionType.Absolute), new Direction(aim + Mathf.SmoothStep(-80f, 80f, t), DirectionType.Absolute, -1f), new Speed(10f, SpeedType.Absolute), bullet);
					if (i % 15 == 0)
					{
						float Dir = UnityEngine.Random.Range(-120, 120);
						float fard = UnityEngine.Random.Range(1, 6);
						int BoneLength = UnityEngine.Random.Range(2, 5);
						if (fard == 1)
                        {
							bool yeah = (UnityEngine.Random.value > 0.5f) ? false : true;
							base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(10, SpeedType.Absolute), new VomitsGutsAndShit.HelixBullet(yeah, 0, "ball"));
							for (int e = 0; e < BoneLength; e++)
                            {
								base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(10, SpeedType.Absolute), new VomitsGutsAndShit.HelixBullet(yeah, (6*e)+6, "link"));
							}
						}
						else
                        {
							int Sped = UnityEngine.Random.Range(7, 10);

							base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(Sped, SpeedType.Absolute), new VomitsGutsAndShit.CrossBullet(new Vector2((0.5f * BoneLength), .5f), 20, 20));
							base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(Sped, SpeedType.Absolute), new VomitsGutsAndShit.CrossBullet(new Vector2((-0.5f * BoneLength), .5f), 20, 20));
							base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(Sped, SpeedType.Absolute), new VomitsGutsAndShit.CrossBullet(new Vector2((0.5f * BoneLength), -0.5f), 20, 20));
							base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(Sped, SpeedType.Absolute), new VomitsGutsAndShit.CrossBullet(new Vector2((-0.5f * BoneLength), -0.5f), 20, 20));
							for (int e = 0; e < BoneLength; e++)
							{
								base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(Sped, SpeedType.Absolute), new VomitsGutsAndShit.CrossBullet(new Vector2(0.5f * e, 0f), 10, 10));
								base.Fire(new Direction(Dir, DirectionType.Aim, -1f), new Speed(Sped, SpeedType.Absolute), new VomitsGutsAndShit.CrossBullet(new Vector2(-0.5f * e, 0f), 10, 10));
							}
						}
					}
					
					yield return base.Wait(1);
				}
				float RNG = UnityEngine.Random.Range(1, 4);
				if (RNG == 1)
				{
					for (int e = 0; e < 4; e++)
					{
						this.QuadShot(base.RandomAngle(), UnityEngine.Random.Range(0f, 1.5f), UnityEngine.Random.Range(7f, 11f));
						yield return base.Wait(1);
					}
				}
				yield break;
			}

			public class HelixBullet : Bullet
			{
				public HelixBullet(bool reverse, int delay, string BulletType) : base(BulletType, false, false, false)
				{
					this.reverse = reverse;
					this.Delay = delay;
					this.bullettype = BulletType;
					base.SuppressVfx = true;
				}
				protected override IEnumerator Top()
				{
					this.ManualControl = true;
					yield return base.Wait(this.Delay);
					Vector2 truePosition = this.Position;
					float startVal = 1;
					for (int i = 0; i < 360; i++)
					{
						float offsetMagnitude = Mathf.SmoothStep(-.75f, .75f, Mathf.PingPong(startVal + (float)i / 90f * 3f, 1f));
						truePosition += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 90f);
						this.Position = truePosition + (this.reverse ? BraveMathCollege.DegreesToVector(this.Direction + 90f, offsetMagnitude) : BraveMathCollege.DegreesToVector(this.Direction - 90f, offsetMagnitude));
						yield return this.Wait(1);
					}
					this.Vanish(false);
					yield break;
				}
				private bool reverse;
				private int Delay;
				private string bullettype;
			}

			private void QuadShot(float direction, float offset, float speed)
			{
				for (int i = 0; i < 4; i++)
				{
					base.Fire(new Offset(offset, 0f, direction, string.Empty, DirectionType.Absolute), new Direction(direction, DirectionType.Absolute, -1f), new Speed(speed - (float)i * 1.5f, SpeedType.Absolute), new TheFart(speed, 120, -1));
				}
			}

			public class TheFart : Bullet
			{
				public TheFart(float newSpeed, int term, int destroyTimer = -1) : base("gross", false, false, false)
				{
					this.m_newSpeed = newSpeed;
					this.m_term = term;
					this.m_destroyTimer = destroyTimer;
				}

				public TheFart(string name, float newSpeed, int term, int destroyTimer = -1, bool suppressVfx = false) : base(name, suppressVfx, false, false)
				{
					this.m_newSpeed = newSpeed;
					this.m_term = term;
					this.m_destroyTimer = destroyTimer;
				}

				protected override IEnumerator Top()
				{
					base.ChangeSpeed(new Speed(this.m_newSpeed, SpeedType.Absolute), this.m_term);
					if (this.m_destroyTimer < 0)
					{
						yield break;
					}
					yield return base.Wait(this.m_term + this.m_destroyTimer);
					base.Vanish(false);
					yield break;
				}

				private float m_newSpeed;

				private int m_term;
				private int m_destroyTimer;
			}
			public class FirehoseBullet : Bullet
			{
				public FirehoseBullet(float direction) : base("firehose", false, false, false)
				{
					this.m_direction = direction;
				}

				protected override IEnumerator Top()
				{
					yield return base.Wait(UnityEngine.Random.Range(5, 30));
					this.Direction += this.m_direction * UnityEngine.Random.Range(10f, 25f);
					yield break;
				}
				private float m_direction;
			}

			public class CrossBullet : Bullet
			{
				public CrossBullet(Vector2 offset, int setupDelay, int setupTime) : base("default", false, false, false)
				{
					this.m_offset = offset;
					this.m_setupDelay = setupDelay;
					this.m_setupTime = setupTime;
				}
				protected override IEnumerator Top()
				{
					base.ManualControl = true;
					this.m_offset = this.m_offset.Rotate(this.Direction);
					for (int i = 0; i < 360; i++)
					{
						if (i > this.m_setupDelay && i < this.m_setupDelay + this.m_setupTime)
						{
							base.Position += this.m_offset / (float)this.m_setupTime;
						}
						float speed = base.Speed;
						base.Position += BraveMathCollege.DegreesToVector(this.Direction, this.Speed / 60f);
						yield return base.Wait(1);
					}
					base.Vanish(false);
					yield break;
				}

				private Vector2 m_offset;
				private int m_setupDelay;
				private int m_setupTime;
			}
		}
		public class FakeOut : Script
		{
			protected override IEnumerator Top()
			{
				base.PostWwiseEvent("Play_ENM_cannonball_eyes_01", null);
				//base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("5729c8b5ffa7415bb3d01205663a33ef").bulletBank.GetBullet("suck"));
				float newAim = base.AimDirection + UnityEngine.Random.Range(-30,30);
				base.Fire(new Offset(0, 1.5f), new Direction(newAim, DirectionType.Aim, -1f), new Speed(30, SpeedType.Absolute), new FakeOut.Faker(this));
				base.Fire(new Offset(0, -1.5f), new Direction(newAim, DirectionType.Aim, -1f), new Speed(30, SpeedType.Absolute), new FakeOut.Faker(this));
				base.Fire(new Offset(-1.4375f, 0.75f), new Direction(newAim, DirectionType.Aim, -1f), new Speed(30, SpeedType.Absolute), new FakeOut.Faker(this));
				base.Fire(new Offset(1.4375f, 0.75f), new Direction(newAim, DirectionType.Aim, -1f), new Speed(30, SpeedType.Absolute), new FakeOut.Faker(this));
				base.Fire(new Offset(-1.4375f, -0.75f), new Direction(newAim, DirectionType.Aim, -1f), new Speed(30, SpeedType.Absolute), new FakeOut.Faker(this));
				base.Fire(new Offset(1.4375f, -0.75f), new Direction(newAim, DirectionType.Aim, -1f), new Speed(30, SpeedType.Absolute), new FakeOut.Faker(this));


				yield return base.Wait(30);
				this.aimDirection = (this.BulletManager.PlayerPosition() - base.Position).ToAngle();
				yield break;
			}
			public float aimDirection;
			public class Faker : Bullet
			{
				public Faker(FakeOut parent) : base("spore2", false, false, false)
                {
					this.parent = parent;
				}
				protected override IEnumerator Top()
                {
					yield return base.Wait(10);
					base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 0);
					yield return base.Wait(UnityEngine.Random.Range(30, 75));
					base.PostWwiseEvent("Play_BOSS_doormimic_blast_01", null);
					float rand = UnityEngine.Random.Range(-180, 180);
					for (int i = 0; i < 6; i++)
                    {
						base.Fire(new Direction((60*i)+ rand, DirectionType.Aim, -1f), new Speed(0, SpeedType.Absolute), new FakeOut.LolOk());
					}
					base.Vanish(false);	
				}
				private FakeOut parent;
			}
			public class LolOk : Bullet
			{
				public LolOk() : base("spore2", false, false, false)
				{
				}
				protected override IEnumerator Top()
				{
					base.ChangeSpeed(new Speed(14f, SpeedType.Absolute), 60);
					yield break;
				}
			}
		}
		public class FakeOutCharge : Script
		{
			protected override IEnumerator Top()
			{
				float newAim = base.AimDirection + 180;
				int i = 0;
				for (; ; )
                {
					base.Fire(new Offset(0, 1.5f), new Direction(newAim, DirectionType.Absolute, -1f), new Speed(25, SpeedType.Absolute), new FakeOutCharge.Faker(this));
					base.Fire(new Offset(0, -1.5f), new Direction(newAim, DirectionType.Absolute, -1f), new Speed(25, SpeedType.Absolute), new FakeOutCharge.Faker(this));
					base.Fire(new Offset(-1.4375f, 0.75f), new Direction(newAim, DirectionType.Absolute, -1f), new Speed(25, SpeedType.Absolute), new FakeOutCharge.Faker(this));
					base.Fire(new Offset(1.4375f, 0.75f), new Direction(newAim, DirectionType.Absolute, -1f), new Speed(25, SpeedType.Absolute), new FakeOutCharge.Faker(this));
					base.Fire(new Offset(-1.4375f, -0.75f), new Direction(newAim, DirectionType.Absolute, -1f), new Speed(25, SpeedType.Absolute), new FakeOutCharge.Faker(this));
					base.Fire(new Offset(1.4375f, -0.75f), new Direction(newAim, DirectionType.Absolute, -1f), new Speed(25, SpeedType.Absolute), new FakeOutCharge.Faker(this));
				
					if (i % 20 == 1)
					{
						float fard = base.AimDirection;
						base.Fire(new Offset(0, 1.5f), new Direction(fard, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new FakeOutCharge.LolOk());
						base.Fire(new Offset(0, -1.5f), new Direction(fard, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new FakeOutCharge.LolOk());
						base.Fire(new Offset(-1.4375f, 0.75f), new Direction(fard, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new FakeOutCharge.LolOk());
						base.Fire(new Offset(1.4375f, 0.75f), new Direction(fard, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new FakeOutCharge.LolOk());
						base.Fire(new Offset(-1.4375f, -0.75f), new Direction(fard, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new FakeOutCharge.LolOk());
						base.Fire(new Offset(1.4375f, -0.75f), new Direction(fard, DirectionType.Absolute, -1f), new Speed(0, SpeedType.Absolute), new FakeOutCharge.LolOk());
					}
					yield return base.Wait(1);
					i++;
				}
			}
			public float aimDirection;
			public class Faker : Bullet
			{
				public Faker(FakeOutCharge parent) : base("spore2", false, false, false)
				{
					this.parent = parent;
				}
				protected override IEnumerator Top()
				{
					base.ChangeSpeed(new Speed(0f, SpeedType.Absolute), 60);
					yield return base.Wait(150);
					base.Vanish(false);
				}
				private FakeOutCharge parent;
			}
			public class LolOk : Bullet
			{
				public LolOk() : base("spore2", false, false, false)
				{
				}
				protected override IEnumerator Top()
				{
					base.ChangeSpeed(new Speed(12f, SpeedType.Absolute), 120);
					yield break;
				}
			}
		}
	}
}







