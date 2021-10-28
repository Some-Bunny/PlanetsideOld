﻿using System;
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
	internal class RevenantProjectile : MonoBehaviour
	{
		public RevenantProjectile()
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
                try
                {
                    projectile.specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(projectile.specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.HandlePreCollision));
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log(ex.Message, false);
                }
            }
        }
        private void HandlePreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myPixelCollider, SpeculativeRigidbody otherRigidbody, PixelCollider otherPixelCollider)
        {
            bool flag = otherRigidbody.gameObject.name != null;
            if (flag)
            {
                bool flag2 = otherRigidbody.gameObject.name == "Table_Vertical" || otherRigidbody.gameObject.name == "Table_Horizontal";
                if (flag2)
                {
                    PhysicsEngine.SkipCollision = true;
                }
            }
        }

        private Projectile projectile;
	}
}

