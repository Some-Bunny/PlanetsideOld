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
using Pathfinding;


namespace Planetside
{
	public class MarkWithColorComponent : BraveBehaviour
	{
		public void Start()
		{
			//base.healthHaver.minimumHealth = this.Mark;
		}
		public void Update()
		{
			PlayerController player = (GameManager.Instance.PrimaryPlayer);
			float Scale = Mark * player.stats.GetStatValue(PlayerStats.StatType.Damage);

			if (base.healthHaver.GetCurrentHealth() <= Scale)
			{
				Material outlineMaterial1 = SpriteOutlineManager.GetOutlineMaterial(base.aiActor.sprite);
				outlineMaterial1.SetColor("_OverrideColor", new Color(10f, 10f, 42f));
			}
		}
		protected override void OnDestroy()
		{
			Material outlineMaterial1 = SpriteOutlineManager.GetOutlineMaterial(base.aiActor.sprite);
			outlineMaterial1.SetColor("_OverrideColor", new Color(0f, 0f, 0f));
			base.OnDestroy();
		}
		public float minimumHealth;
		public float Mark = 15.001f;
	}
}