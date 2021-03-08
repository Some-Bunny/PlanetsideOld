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
	// Token: 0x02000033 RID: 51
	public class UnchangeableRangeControllerThanksCel : BraveBehaviour
	{
		public void Awake()
		{
			bool flag = base.projectile != null;
			if (flag)
			{
				this.m_origRange = base.projectile.baseData.range;
			}
		}

		public void Update()
		{
			bool flag = base.projectile != null && base.projectile.baseData.range != this.m_origRange;
			if (flag)
			{
				base.projectile.baseData.range = this.m_origRange;
			}
		}

		private float m_origRange;
	}
}