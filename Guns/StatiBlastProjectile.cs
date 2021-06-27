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
using System.Collections.ObjectModel;

using UnityEngine.Serialization;

namespace Planetside
{
	internal class StatiBlastProjectile : MonoBehaviour
	{
		public StatiBlastProjectile()
		{
		}
        public void Start()
        {
            this.projectile = base.GetComponent<Projectile>();
            Projectile projectile = this.projectile;
            PlayerController playerController = projectile.Owner as PlayerController;
            Projectile component = base.gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            bool flag2 = flag;
            if (flag2)
            {
                projectile.OnDestruction += this.Zzap;

            }
        }
		private void Zzap(Projectile projectile)
		{
			PlayerController player = projectile.Owner as PlayerController;
			bool isInCombat = player.IsInCombat;
			if (isInCombat)
			{
				float num2 = 10f;
				List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
				bool flag5 = activeEnemies == null | activeEnemies.Count <= 0;
				{
					AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, projectile.sprite.WorldCenter, out num2, null);
					bool flag8 = nearestEnemy && nearestEnemy != null;
					if (flag8)
					{
						float dmg = (player.stats.GetStatValue(PlayerStats.StatType.Damage));
						Vector2 worldCenter3 = projectile.sprite.WorldCenter;
						Vector2 unitCenter3 = nearestEnemy.specRigidbody.HitboxPixelCollider.UnitCenter;
						float z3 = BraveMathCollege.Atan2Degrees((unitCenter3 - worldCenter3).normalized);
						Projectile projectile3 = ((Gun)ETGMod.Databases.Items[153]).DefaultModule.projectiles[0];
						GameObject gameObject3 = SpawnManager.SpawnProjectile(projectile3.gameObject, projectile.sprite.WorldCenter, Quaternion.Euler(0f, 0f, z3), true);
						Projectile component3 = gameObject3.GetComponent<Projectile>();
						bool flag15 = component3 != null;
						bool flag16 = flag15;
						if (flag16)
						{
							component3.Owner = projectile.PossibleSourceGun.CurrentOwner as PlayerController;
							component3.baseData.range *= 5f;
							component3.pierceMinorBreakables = true;
							component3.collidesWithPlayer = false;
							PierceProjModifier spook = component3.gameObject.AddComponent<PierceProjModifier>();
							spook.penetration = 10;
							component3.AdditionalScaleMultiplier = 0.5f;
							component3.baseData.damage = 3f * dmg;
						}
					}
				}
			}
		}
		public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
		{
			AIActor aiactor = null;
			nearestDistance = float.MaxValue;
			bool flag = activeEnemies == null;
			bool flag2 = flag;
			bool flag3 = flag2;
			bool flag4 = flag3;
			bool flag5 = flag4;
			AIActor result;
			if (flag5)
			{
				result = null;
			}
			else
			{
				for (int i = 0; i < activeEnemies.Count; i++)
				{
					AIActor aiactor2 = activeEnemies[i];
					bool flag6 = !aiactor2.healthHaver.IsDead && aiactor2.healthHaver.IsVulnerable;
					bool flag7 = flag6;
					if (flag7)
					{
						bool flag8 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
						bool flag9 = flag8;
						if (flag9)
						{
							float num = Vector2.Distance(position, aiactor2.CenterPosition);
							bool flag10 = num < nearestDistance;
							bool flag11 = flag10;
							if (flag11)
							{
								nearestDistance = num;
								aiactor = aiactor2;
							}
						}
					}
				}
				result = aiactor;
			}
			return result;
		}
		private Projectile projectile;
	}
}

