using System;
using System.Collections;
using System.Collections.Generic;
using Brave.BulletScript;
using Dungeonator;
using FullInspector;
using UnityEngine;

/*
namespace Planetside
{


	public class TestOverrideBehavior : OverrideBehavior
	{
		public override string OverrideAIActorGUID => "c4cf0620f71c4678bb8d77929fd4feff"; // Replace the GUID with whatever enemy you want to modify. This GUID is for the bullet kin.
																						  // You can find a full list of GUIDs at https://github.com/ModTheGungeon/ETGMod/blob/master/Assembly-CSharp.Base.mm/Content/gungeon_id_map/enemies.txt
		public override void DoOverride()
		{
			// In this method, you can do whatever you want with the enemy using the fields "actor", "healthHaver", "behaviorSpec", and "bulletBank".

			//actor.MovementSpeed *= 2; // Doubles the enemy movement speed

			//healthHaver.SetHealthMaximum(healthHaver.GetMaxHealth() * 0.5f); // Halves the enemy health

			//The BehaviorSpeculator is responsible for almost everything an enemy does, from shooting a gun to teleporting.
			// Tip: To debug an enemy's BehaviorSpeculator, you can uncomment the line below. This will print all the behavior information to the console.
			ToolsEnemy.DebugInformation(behaviorSpec);

			// For this first change, we're just going to increase the lead amount of the bullet kin's ShootGunBehavior so its shots fire like veteran kin.
			//ShootBehavior shootGunBehavior2 = behaviorSpec.AttackBehaviorGroup.AttackBehaviors[0].Behavior as ShootBehavior;
			//ShootBehavior shootGunBehavior1 = behaviorSpec.AttackBehaviorGroup.AttackBehaviors[1].Behavior as ShootBehavior;
		   //ShootBehavior shootGunBehavior = behaviorSpec.AttackBehaviorGroup.AttackBehaviors[2].Behavior as ShootBehavior;
			//ShootBehavior shootGunBehavior1 = behaviorSpec.AttackBehaviorGroup.AttackBehaviors[1].Behavior as ShootBehavior;
			//ShootBehavior shootGunBehavior = behaviorSpec.AttackBehaviors[0] as ShootBehavior;
			ShootGunBehavior shootGunBehavior1 = behaviorSpec.AttackBehaviors[0] as ShootGunBehavior;
			//base.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("41ee1c8538e8474a82a74c4aff99c712").bulletBank.GetBullet("big"));
			//base.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("ec6b674e0acd4553b47ee94493d66422").bulletBank.GetBullet("bigBullet"));
			//yah.chargeSpeed = 30;
			//yah.chargeRange = 30;
			//yah.AttackCooldown = 1f;
			//yah.delayWallRecoil = false;
			//yah.wallRecoilForce = 160000;
			shootGunBehavior1.WeaponType = WeaponType.BulletScript;
			//shootGunBehavior.BulletScript = new CustomBulletScriptSelector(typeof(Speeeeeeen));
			shootGunBehavior1.BulletScript = new CustomBulletScriptSelector(typeof(TheBiggerUkulele));

			//shootGunBehavior1.BulletScript = new CustomBulletScriptSelector(typeof(Gear));
			//shootGunBehavior.LeadAmount = 0.9f;


			// Next, we're going to change another few things on the ShootGunBehavior so that it has a custom BulletScript.
			//shootGunBehavior.WeaponType = WeaponType.BulletScript; // Makes it so the bullet kin will shoot our bullet script instead of his own gun shot.
			//shootGunBehavior.BulletScript = new CustomBulletScriptSelector(typeof(Death)); // Sets the bullet kin's bullet script to our custom bullet script.
		}

		public class TheBiggerUkulele : Script
		{
			protected override IEnumerator Top()
			{
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("fa76c8cfdf1c4a88b55173666b4bc7fb").bulletBank.GetBullet("fastBullet"));
				base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("fa76c8cfdf1c4a88b55173666b4bc7fb").bulletBank.GetBullet("homingBullet"));
				for (int j = 0; j < 2; j++)
				{
					base.Fire(new Direction(0, DirectionType.Aim, -1f), new Speed(20f, SpeedType.Absolute), new TheBiggerUkulele.BigCannon());
					for (int e = -1; e < 1; e++)
					{
						base.Fire(new Direction(30f*e, DirectionType.Aim, -1f), new Speed(7f, SpeedType.Absolute), new TheBiggerUkulele.Missiles());
					}
					yield return base.Wait(45);
				}				
				yield break;
			}


			private const int NumBullets = 24;

			private class BigCannon : Bullet
			{
				public BigCannon() : base("fastBullet", false, false, false)
				{

				}
			}
			public class Missiles : Bullet
			{
				public Missiles() : base("homingBullet", false, false, false)
				{

					this.SuppressVfx = true;
				}
				protected override IEnumerator Top()
				{
					float waittime = UnityEngine.Random.Range(90, 30);
					for (int i = 0; i < 150-waittime; i++)
					{
						base.ChangeDirection(new Direction(0f, DirectionType.Aim, 1f), 1);
						if (i == 120-waittime)
						{
							this.Projectile.spriteAnimator.Play("enemy_projectile_rocket_impact");
						}
						yield return base.Wait(1);
					}
					base.Vanish(false);
					yield break;
				}

				public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
				{
					if (preventSpawningProjectiles)
					{
						return;
					}
					float num = base.RandomAngle();
					float num2 = 90f;
					for (int e = 0; e < 2; e++)
                    {
						for (int i = 0; i < 4; i++)
						{
							base.Fire(new Direction(num + num2 * (float)i +(30*e), DirectionType.Absolute, -1f), new Speed(5+(e+5), SpeedType.Absolute), new Base());
						}
						AkSoundEngine.PostEvent("Play_WPN_golddoublebarrelshotgun_shot_01", this.Projectile.gameObject);
					}
				}
			}
			public class Base : Bullet
			{
				public Base() : base(null, false, false, false)
				{

					this.SuppressVfx = true;
				}
				
			}
		}
	}
}
*/