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
	public class BrokenChamberComponent : MonoBehaviour
	{
		public BrokenChamberComponent()
		{
			this.TimeBetweenRockFalls = 7.5f;
		}

		private void Awake()
		{
			this.m_player = base.GetComponent<PlayerController>();
			this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
		}

		private void Update()
		{
			bool flag2 = this.speculativeRigidBoy == null;
			if (flag2)
			{
				this.speculativeRigidBoy = base.GetComponent<SpeculativeRigidbody>();
			}
			bool eiie = this.m_player == null;
			if (eiie)
			{
				this.m_player = base.GetComponent<PlayerController>();
			}
			this.elapsed += BraveTime.DeltaTime;
			bool flag3 = this.elapsed > TimeBetweenRockFalls;
			if (flag3)
			{
				if (this.m_player != null)
                {
					AkSoundEngine.PostEvent("Play_BOSS_wall_slam_01", base.gameObject);
					GameObject dragunBoulder = EnemyDatabase.GetOrLoadByGuid("05b8afe0b6cc4fffa9dc6036fa24c8ec").GetComponent<DraGunController>().skyBoulder;
					GameObject dragunRocket = EnemyDatabase.GetOrLoadByGuid("05b8afe0b6cc4fffa9dc6036fa24c8ec").GetComponent<DraGunController>().skyRocket;
					IntVector2? vector = (m_player as PlayerController).CurrentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), CellTypes.FLOOR | CellTypes.PIT, false, null);
					Vector2 vector2 = vector.Value.ToVector2();
					this.FireRocket(dragunBoulder, m_player.sprite.WorldCenter);
					this.FireRocket(dragunBoulder, vector2);
				}
				this.elapsed = 0f;
			}
		}
		private void FireRocket(GameObject skyRocket, Vector2 target)
		{
			PlayerController player = m_player;
			SkyRocket component = SpawnManager.SpawnProjectile(skyRocket, player.sprite.WorldCenter, Quaternion.identity, true).GetComponent<SkyRocket>();
			component.TargetVector2 = target;
			tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
			component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
			component.ExplosionData.ignoreList.Add(player.specRigidbody);
		}
		private SpeculativeRigidbody speculativeRigidBoy;
		private PlayerController m_player;

		public float TimeBetweenRockFalls;
		private float elapsed;
	}
}