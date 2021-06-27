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
using SaveAPI;

namespace Planetside
{
    public static class RandomPiecesOfStuffToInitialise
    {

        public static void BuildPrefab()
        {

            BuildSoulGuon();

			BuildHeartGuon();
			BuildHalfHeartGuon();
			BuildAmmoGuon();
			BuildHalfAmmoGuon();
			BuildArmorGuon();
			BuildKeyGuon();
			BuildBlankguon();

			BuildMedal();
		}
		public static void BuildSoulGuon()
        {
			GameObject gameObject = SpriteBuilder.SpriteFromResource("Planetside/Resources/Guons/SoulGuon/guoner.png");
			gameObject.name = $"Soul Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 4f;
			orbitalPrefab.orbitDegreesPerSecond = 30;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			SoulSynergyGuon = gameObject;

			ImprovedAfterImage yeah = gameObject.AddComponent<ImprovedAfterImage>();
			yeah.dashColor = new Color(0, 0.7f, 1);
			yeah.spawnShadows = true;
			yeah.shadowTimeDelay = 0.01f;
			yeah.shadowLifetime = 0.5f;

			PlayerOrbital si = gameObject.GetComponent<PlayerOrbital>();

			si.sprite.usesOverrideMaterial = true;
			si.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
			Material mat = si.sprite.GetCurrentSpriteDef().material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
			mat.mainTexture = si.sprite.renderer.material.mainTexture;
			mat.SetColor("_EmissiveColor", new Color32(255, 255, 255, 255));
			mat.SetFloat("_EmissiveColorPower", 1.55f);
			mat.SetFloat("_EmissivePower", 100);
			si.sprite.renderer.material = mat;

		}
		public static GameObject SoulSynergyGuon;
		public static string vfxNamefrailty = "FrailtyVFX";
		public static GameObject MedalObject;
		public static void BuildMedal()
		{
			MedalObject = SpriteBuilder.SpriteFromResource("Planetside/Resources/planetsidemedal", new GameObject("Medal"));
			MedalObject.SetActive(false);
			tk2dBaseSprite vfxSprite = MedalObject.GetComponent<tk2dBaseSprite>();
			vfxSprite.GetCurrentSpriteDef().ConstructOffsetsFromAnchor(tk2dBaseSprite.Anchor.LowerCenter, vfxSprite.GetCurrentSpriteDef().position3);
			FakePrefab.MarkAsFakePrefab(MedalObject);
			UnityEngine.Object.DontDestroyOnLoad(MedalObject);
		}



		public static void BuildHeartGuon()
		{
			GameObject deathmark = ItemBuilder.AddSpriteToObject("heart_guon", "Planetside/Resources/Guons/PickupGuons/HeartGuon/heartguon_001", null);
			FakePrefab.MarkAsFakePrefab(deathmark);
			UnityEngine.Object.DontDestroyOnLoad(deathmark);
			tk2dSpriteAnimator animator = deathmark.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 4;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			animationClip.name = "start";

			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/Guons/PickupGuons/HeartGuon/heartguon_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 5; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/Guons/PickupGuons/HeartGuon/heartguon_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;

			GameObject gameObject = animator.gameObject;
			gameObject.name = $"Heart Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 2f;
			orbitalPrefab.orbitDegreesPerSecond = 60;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			HeartGuon = gameObject;

		}
		public static void BuildHalfHeartGuon()
		{
			GameObject deathmark = ItemBuilder.AddSpriteToObject("half_heart_guon", "Planetside/Resources/Guons/PickupGuons/HalfheartGuon/halfheartguon_001", null);
			FakePrefab.MarkAsFakePrefab(deathmark);
			UnityEngine.Object.DontDestroyOnLoad(deathmark);
			tk2dSpriteAnimator animator = deathmark.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 4;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			animationClip.name = "start";

			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/Guons/PickupGuons/HalfheartGuon/halfheartguon_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 5; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/Guons/PickupGuons/HalfheartGuon/halfheartguon_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;

			GameObject gameObject = animator.gameObject;
			gameObject.name = $"Half Heart Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 3f;
			orbitalPrefab.orbitDegreesPerSecond = 60;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			HalfheartGuon = gameObject;

		}
		public static void BuildAmmoGuon()
		{
			GameObject deathmark = ItemBuilder.AddSpriteToObject("ammo_guon", "Planetside/Resources/Guons/PickupGuons/AmmoGuon/ammoguon_001", null);
			FakePrefab.MarkAsFakePrefab(deathmark);
			UnityEngine.Object.DontDestroyOnLoad(deathmark);
			tk2dSpriteAnimator animator = deathmark.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 4;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			animationClip.name = "start";

			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/Guons/PickupGuons/AmmoGuon/ammoguon_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 5; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/Guons/PickupGuons/AmmoGuon/ammoguon_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;

			GameObject gameObject = animator.gameObject;
			gameObject.name = $"Ammo Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 2.5f;
			orbitalPrefab.orbitDegreesPerSecond = 40;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			AmmoGuon = gameObject;

		}
		public static void BuildHalfAmmoGuon()
		{
			GameObject deathmark = ItemBuilder.AddSpriteToObject("half_ammo_guon", "Planetside/Resources/Guons/PickupGuons/HalfAmmoguon/halfammoguon_001", null);
			FakePrefab.MarkAsFakePrefab(deathmark);
			UnityEngine.Object.DontDestroyOnLoad(deathmark);
			tk2dSpriteAnimator animator = deathmark.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 4;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			animationClip.name = "start";

			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/Guons/PickupGuons/HalfAmmoguon/halfammoguon_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 5; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/Guons/PickupGuons/HalfAmmoguon/halfammoguon_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;

			GameObject gameObject = animator.gameObject;
			gameObject.name = $"Half Ammo Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 4.5f;
			orbitalPrefab.orbitDegreesPerSecond = 40;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			HalfAmmoGuon = gameObject;

		}
		public static void BuildArmorGuon()
		{
			GameObject deathmark = ItemBuilder.AddSpriteToObject("armor_guon", "Planetside/Resources/Guons/PickupGuons/Armor/armorguon_001", null);
			FakePrefab.MarkAsFakePrefab(deathmark);
			UnityEngine.Object.DontDestroyOnLoad(deathmark);
			tk2dSpriteAnimator animator = deathmark.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 4;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			animationClip.name = "start";

			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/Guons/PickupGuons/Armor/armorguon_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 5; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/Guons/PickupGuons/Armor/armorguon_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;

			GameObject gameObject = animator.gameObject;
			gameObject.name = $"Armor Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 1.5f;
			orbitalPrefab.orbitDegreesPerSecond = 40;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			ArmorGuon = gameObject;

		}
		public static void BuildKeyGuon()
		{
			GameObject deathmark = ItemBuilder.AddSpriteToObject("key_guon", "Planetside/Resources/Guons/PickupGuons/keyguon/keyguon_001", null);
			FakePrefab.MarkAsFakePrefab(deathmark);
			UnityEngine.Object.DontDestroyOnLoad(deathmark);
			tk2dSpriteAnimator animator = deathmark.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 4;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			animationClip.name = "start";

			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/Guons/PickupGuons/keyguon/keyguon_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 5; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/Guons/PickupGuons/keyguon/keyguon_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;

			GameObject gameObject = animator.gameObject;
			gameObject.name = $"Key Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 1f;
			orbitalPrefab.orbitDegreesPerSecond = 30;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			KeyGuon = gameObject;

		}
		public static void BuildBlankguon()
		{
			GameObject deathmark = ItemBuilder.AddSpriteToObject("blank_guon", "Planetside/Resources/Guons/PickupGuons/BlankGuon/blankguon_001", null);
			FakePrefab.MarkAsFakePrefab(deathmark);
			UnityEngine.Object.DontDestroyOnLoad(deathmark);
			tk2dSpriteAnimator animator = deathmark.AddComponent<tk2dSpriteAnimator>();
			tk2dSpriteAnimationClip animationClip = new tk2dSpriteAnimationClip();
			animationClip.fps = 4;
			animationClip.wrapMode = tk2dSpriteAnimationClip.WrapMode.Loop;
			animationClip.name = "start";

			GameObject spriteObject = new GameObject("spriteObject");
			ItemBuilder.AddSpriteToObject("spriteObject", $"Planetside/Resources/Guons/PickupGuons/BlankGuon/blankguon_001", spriteObject);
			tk2dSpriteAnimationFrame starterFrame = new tk2dSpriteAnimationFrame();
			starterFrame.spriteId = spriteObject.GetComponent<tk2dSprite>().spriteId;
			starterFrame.spriteCollection = spriteObject.GetComponent<tk2dSprite>().Collection;
			tk2dSpriteAnimationFrame[] frameArray = new tk2dSpriteAnimationFrame[]
			{
				starterFrame
			};
			animationClip.frames = frameArray;
			for (int i = 2; i < 5; i++)
			{
				GameObject spriteForObject = new GameObject("spriteForObject");
				ItemBuilder.AddSpriteToObject("spriteForObject", $"Planetside/Resources/Guons/PickupGuons/BlankGuon/blankguon_00{i}", spriteForObject);
				tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
				frame.spriteId = spriteForObject.GetComponent<tk2dBaseSprite>().spriteId;
				frame.spriteCollection = spriteForObject.GetComponent<tk2dBaseSprite>().Collection;
				animationClip.frames = animationClip.frames.Concat(new tk2dSpriteAnimationFrame[] { frame }).ToArray();
			}
			animator.Library = animator.gameObject.AddComponent<tk2dSpriteAnimation>();
			animator.Library.clips = new tk2dSpriteAnimationClip[] { animationClip };
			animator.DefaultClipId = animator.GetClipIdByName("start");
			animator.playAutomatically = true;

			GameObject gameObject = animator.gameObject;
			gameObject.name = $"Blank Guon";
			SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
			PlayerOrbital orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
			speculativeRigidbody.CollideWithTileMap = false;
			speculativeRigidbody.CollideWithOthers = true;
			speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
			orbitalPrefab.shouldRotate = false;
			orbitalPrefab.orbitRadius = 5f;
			orbitalPrefab.orbitDegreesPerSecond = 120;
			//orbitalPrefab.perfectOrbitalFactor = 1000f;
			orbitalPrefab.SetOrbitalTier(0);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			FakePrefab.MarkAsFakePrefab(gameObject);
			gameObject.SetActive(false);
			BlankGuon = gameObject;

		}


		public static GameObject KeyGuon;
		public static GameObject HalfheartGuon;
		public static GameObject HeartGuon;
		public static GameObject AmmoGuon;
		public static GameObject ArmorGuon;
		public static GameObject HalfAmmoGuon;
		public static GameObject BlankGuon;


	}
}
