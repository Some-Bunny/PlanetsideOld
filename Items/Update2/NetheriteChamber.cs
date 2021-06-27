using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;
using SaveAPI;
using Brave.BulletScript;

namespace Planetside
{
	public class NetheriteChamber : PassiveItem
	{
		public static void Init()
		{
			string name = "Netherite Chamber";
			string resourcePath = "Planetside/Resources/brokenchamberfixedtier2.png";
			GameObject gameObject = new GameObject(name);
			NetheriteChamber chamber = gameObject.AddComponent<NetheriteChamber>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Forged From Bullet Hells Metals";
			string longDesc = "A diamond-encrusted chamber plated with a metal unique to Bullet Hell. It's reinforced power calls upon the Guardians of Kaliber to your aid.";
			ItemBuilder.SetupItem(chamber, shortDesc, longDesc, "psog");
			chamber.quality = PickupObject.ItemQuality.EXCLUDED;
			NetheriteChamber.ChaamberID = chamber.PickupObjectId;

		}
		public static int ChaamberID;

		public override void Pickup(PlayerController player)
		{
			player.OnEnteredCombat = (Action)Delegate.Combine(player.OnEnteredCombat, new Action(this.SpawnGuard));

			base.Pickup(player);
		}

		public override DebrisObject Drop(PlayerController player)
		{
			player.OnEnteredCombat = (Action)Delegate.Remove(player.OnEnteredCombat, new Action(this.SpawnGuard));
			DebrisObject result = base.Drop(player);
			return result;
		}
		private void SpawnGuard()
		{
			List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
			bool flag = activeEnemies != null;
			if (flag)
			{
				RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
				AIActor randomActiveEnemy;
				randomActiveEnemy = base.Owner.CurrentRoom.GetRandomActiveEnemy(true);
				int num = 5;
				do
				{
					randomActiveEnemy = base.Owner.CurrentRoom.GetRandomActiveEnemy(true);
					num--;
				}
				while (num > 0 && (!randomActiveEnemy.healthHaver.IsBoss));
				bool ee = num == 0;
				if (ee)
				{

				}
				else
				{
					GameObject gameObject = new GameObject();
					gameObject.transform.position = base.Owner.transform.position;
					BulletScriptSource source = gameObject.GetOrAddComponent<BulletScriptSource>();
					gameObject.AddComponent<BulletSourceKiller>();
					var bulletScriptSelected = new CustomBulletScriptSelector(typeof(TheEpicGuard));
					AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
					AIBulletBank bulletBank = aIActor.GetComponent<AIBulletBank>();
					bulletBank.CollidesWithEnemies = false;

					source.BulletManager = bulletBank;
					source.BulletScript = bulletScriptSelected;
					source.Initialize();//to fire the script once
				}
			}
		}		
			
		
		protected override void Update()
		{
			base.Update();
			bool flag = base.Owner != null;
			if (flag)
			{
				this.CalculateStats(base.Owner);

			}
		}
		private void CalculateStats(PlayerController player)
		{
			this.currentItems = player.passiveItems.Count;
			bool flag = this.currentItems != this.lastItems;
			if (flag)	
			{
				this.RemoveStat(PlayerStats.StatType.Health);
				this.RemoveStat(PlayerStats.StatType.Damage);
				this.RemoveStat(PlayerStats.StatType.RateOfFire);
				foreach (PassiveItem passiveItem in player.passiveItems)
				{
					bool flag2 = passiveItem is BasicStatPickup && (passiveItem as BasicStatPickup).IsMasteryToken;
					if (flag2)
					{
						this.AddStat(PlayerStats.StatType.Health, 1f, StatModifier.ModifyMethod.ADDITIVE);
						this.AddStat(PlayerStats.StatType.RateOfFire, 1.15f, StatModifier.ModifyMethod.MULTIPLICATIVE);

					}
					string encounterNameOrDisplayName = passiveItem.EncounterNameOrDisplayName;
					bool fucker = encounterNameOrDisplayName.Contains("Chamber") || encounterNameOrDisplayName.Contains("chamber");
					if (fucker)
					{
						this.AddStat(PlayerStats.StatType.Damage, .25f, StatModifier.ModifyMethod.ADDITIVE);
					}
				}

					this.lastItems = this.currentItems;
				player.stats.RecalculateStats(player, true, false);
			}
		}
		private void AddStat(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod method = StatModifier.ModifyMethod.ADDITIVE)
		{
			StatModifier statModifier = new StatModifier
			{
				amount = amount,
				statToBoost = statType,
				modifyType = method
			};
			bool flag = this.passiveStatModifiers == null;
			if (flag)
			{
				this.passiveStatModifiers = new StatModifier[]
				{
					statModifier
				};
			}
			else
			{
				this.passiveStatModifiers = this.passiveStatModifiers.Concat(new StatModifier[]
				{
					statModifier
				}).ToArray<StatModifier>();
			}
		}
		private void RemoveStat(PlayerStats.StatType statType)
		{
			List<StatModifier> list = new List<StatModifier>();
			for (int i = 0; i < this.passiveStatModifiers.Length; i++)
			{
				bool flag = this.passiveStatModifiers[i].statToBoost != statType;
				if (flag)
				{
					list.Add(this.passiveStatModifiers[i]);
				}
			}
			this.passiveStatModifiers = list.ToArray();
		}
		private int currentItems;
		private int lastItems;
	}
}
public class TheEpicGuard : Script
{
	protected override IEnumerator Top()
	{
		PlayerController player = (GameManager.Instance.PrimaryPlayer);
		RoomHandler currentRoom = player.CurrentRoom;
		AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_002");
		this.Mines_Cave_In = assetBundle.LoadAsset<GameObject>("Mines_Cave_In");
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(Mines_Cave_In, player.sprite.WorldCenter, Quaternion.identity);
		HangingObjectController RockSlideController = gameObject.GetComponent<HangingObjectController>();
		RockSlideController.triggerObjectPrefab = null;
		GameObject[] additionalDestroyObjects = new GameObject[]
		{
				RockSlideController.additionalDestroyObjects[1]
		};
		RockSlideController.additionalDestroyObjects = additionalDestroyObjects;
		UnityEngine.Object.Destroy(gameObject.transform.Find("Sign").gameObject);
		RockSlideController.ConfigureOnPlacement(currentRoom);
		yield return Wait(60);
		IntVector2? vector = player.CurrentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), CellTypes.FLOOR | CellTypes.PIT, false, null);
		Vector2 vector2 = player.sprite.WorldCenter + new Vector2(UnityEngine.Random.Range(-2, 2), UnityEngine.Random.Range(-2, 2));
		base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("4d164ba3f62648809a4a82c90fc22cae").bulletBank.GetBullet("big_one"));
		base.Fire(Offset.OverridePosition(vector2 + new Vector2(0f, 30f)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new TheEpicGuard.BigBullet());

		yield break;
	}
	private GameObject Mines_Cave_In;
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
				PlayerController player = (GameManager.Instance.PrimaryPlayer);

				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
				var list = new List<string> {
				//"shellet",
				"jammed_guardian"
			    };
				string guid = BraveUtility.RandomElement<string>(list);
				var Enemy = EnemyDatabase.GetOrLoadByGuid(guid);
				AIActor mhm = AIActor.Spawn(Enemy.aiActor, this.Projectile.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
				CompanionController yup = mhm.gameObject.AddComponent<CompanionController>();

				yup.companionID = CompanionController.CompanionIdentifier.NONE;
				yup.Initialize(player);
				Planetside.OtherTools.CompanionisedEnemyBulletModifiers yeehaw = mhm.gameObject.AddComponent<Planetside.OtherTools.CompanionisedEnemyBulletModifiers>();
				yeehaw.jammedDamageMultiplier *= 3;
				yeehaw.baseBulletDamage = 12f;
				yeehaw.TintBullets = true;
				yeehaw.TintColor = new Color(0.85f, 0.7f, 0.2f);
				for (int j = 0; j < mhm.behaviorSpeculator.AttackBehaviors.Count; j++)
				{
					if (mhm.behaviorSpeculator.AttackBehaviors[j] is AttackBehaviorGroup)
					{
						this.ProcessAttackGroup(mhm.behaviorSpeculator.AttackBehaviors[j] as AttackBehaviorGroup);
					}
				}

				mhm.CanTargetEnemies = true;
				mhm.CanTargetPlayers = false;
				float num = base.RandomAngle();
				float Amount = 12;
				float Angle = 360 / Amount;
				for (int i = 0; i < Amount; i++)
				{
					base.Fire(new Direction(num + Angle * (float)i + 10, DirectionType.Absolute, -1f), new Speed(5f, SpeedType.Absolute), new BurstBullet());
				}
				base.PostWwiseEvent("Play_ENM_bulletking_slam_01", null);
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
		private void ProcessAttackGroup(AttackBehaviorGroup attackGroup)
		{
			for (int i = 0; i < attackGroup.AttackBehaviors.Count; i++)
			{
				AttackBehaviorGroup.AttackGroupItem attackGroupItem = attackGroup.AttackBehaviors[i];
				if (attackGroupItem.Behavior is ShootBehavior)
				{
					attackGroupItem.Probability = 0f;
				}
			}
		}

	}
}
