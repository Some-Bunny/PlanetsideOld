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
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using Brave.BulletScript;
using GungeonAPI;
using SpriteBuilder = ItemAPI.SpriteBuilder;
using DirectionType = DirectionalAnimation.DirectionType;
using static DirectionalAnimation;

namespace Planetside
{
	public class PickupGuonComponent : MonoBehaviour
	{
		public PickupGuonComponent()
		{
			this.HitsBeforeDeath = 10;
			this.player = GameManager.Instance.PrimaryPlayer;
			this.Hits = 0;
			this.IsAmmo = false;
			this.IsHalfAmmo = false;
			this.IsHeart = false;
			this.IsHalfHeart = false;
			this.IsBlank = false;
			this.IsKey = false;
			this.IsArmor = false;

		}

		public void Awake()
		{
			this.actor = base.GetComponent<PlayerOrbital>();
			this.player = base.GetComponent<PlayerController>();

		}
		public static Hook guonHook;

		public void Start()
		{
			LootEngine.DoDefaultItemPoof(actor.transform.position, false, false);
			PickupGuonComponent.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(GunknownGuonComponent).GetMethod("GuonInit"));
			PlayerController player = GameManager.Instance.PrimaryPlayer;
			if (this.actor == null)
            {
				this.actor = base.GetComponent<PlayerOrbital>();
			}
			if (this.player == null)
			{
				this.player = base.GetComponent<PlayerController>();
			}
			PlayerOrbital playerOrbital2 = actor;
			SpeculativeRigidbody specRigidbody = playerOrbital2.specRigidbody;
			specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));

		}
		public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
		{
			orig(self, player);
		}
		private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
		{
			Hits++;
			if (Hits == HitsBeforeDeath)
            {
				LootEngine.DoDefaultItemPoof(actor.sprite.WorldCenter, false, true);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}


		public void Update()
		{
			if (this.actor == null)
			{
				this.actor = base.GetComponent<PlayerOrbital>();
			}
			if (this.player == null)
			{
				this.player = base.GetComponent<PlayerController>();
			}
		}

		public bool IsHeart;
		public bool IsHalfHeart;
		public bool IsAmmo;
		public bool IsHalfAmmo;
		public bool IsKey;
		public bool IsArmor;
		public bool IsBlank;


		public PlayerController sourcePlayer;
		public int HitsBeforeDeath = 10;
		private PlayerOrbital actor;
		public PlayerController player;
		public int Hits;
	}
}
