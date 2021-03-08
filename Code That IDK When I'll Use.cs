using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 * 
 *Reverse Distortion Wave 
 *
private IEnumerator DoDistortionWaveLocal(Vector2 center, float distortionIntensity, float distortionRadius, float maxRadius, float duration)
        {
            Material distMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionWave"));
            Vector4 distortionSettings = this.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
            distMaterial.SetVector("_WaveCenter", distortionSettings);
            Pixelator.Instance.RegisterAdditionalRenderPass(distMaterial);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                    elapsed += BraveTime.DeltaTime;
                float t = elapsed / duration;
                t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
                distortionSettings = this.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
                distortionSettings.w = Mathf.Lerp(distortionSettings.w, 0f, t);
                distMaterial.SetVector("_WaveCenter", distortionSettings);
                float currentRadius = Mathf.Lerp(maxRadius, 0f, t);
                distMaterial.SetFloat("_DistortProgress", (currentRadius / (maxRadius * 5)));
                yield return null;
            }
            Pixelator.Instance.DeregisterAdditionalRenderPass(distMaterial);
            UnityEngine.Object.Destroy(distMaterial);

*
*
* Adding A pickup to the pickup shop pool & room clear pool
item.CustomCost = 30;
            item.UsesCustomCost = true;

            WeightedGameObject weightedObject = new WeightedGameObject();
            weightedObject.SetGameObject(gameObject);
            weightedObject.weight = 1;
            weightedObject.rawGameObject = gameObject;
            weightedObject.pickupId = item.PickupObjectId;
            weightedObject.forceDuplicatesPossible = true;
            weightedObject.additionalPrerequisites = new DungeonPrerequisite[0];

            foreach (FloorRewardData rewardData in GameManager.Instance.RewardManager.FloorRewardData)
            {
                ETGModConsole.Log($"{rewardData.AssociatedTilesets}:");
                if (rewardData.SingleItemRewardTable.defaultItemDrops.elements != null && rewardData.SingleItemRewardTable.defaultItemDrops.elements.Count != 0 && !rewardData.SingleItemRewardTable.defaultItemDrops.elements.Contains(weightedObject));
                {
                    rewardData.SingleItemRewardTable.defaultItemDrops.Add(weightedObject);
                    foreach (WeightedGameObject shrug in rewardData.SingleItemRewardTable.defaultItemDrops.elements)
                    {
                        if (shrug.gameObject != null)
                        {
                            ETGModConsole.Log($"{shrug.gameObject.name} : {shrug.weight}");
                        }
                    }
                }
                ETGModConsole.Log("------");
            }

            GenericLootTable thanksbotluvya = ItemBuilder.LoadShopTable("Shop_Gungeon_Cheap_Items_01");
            thanksbotluvya.defaultItemDrops.elements.Add(weightedObject);


		public static int SackPickupID;

		public override void Pickup(PlayerController player)
		{
			this.random = UnityEngine.Random.Range(0.0f, 1.0f);
			if (random < 0.7)
			{
				int id = BraveUtility.RandomElement<int>(SackPickup.GenericPool);
				float itemsToSpawn = UnityEngine.Random.Range(2, 4);
				for (int i = 0; i < itemsToSpawn; i++)
				{
					LootEngine.SpawnItem(PickupObjectDatabase.GetById(id).gameObject, base.aiActor.sprite.WorldCenter, new Vector2(0, 0), 2.2f, false, true, false);
				}
			}
			else if (random > 0.7 && random < 0.95)
			{
				int id = BraveUtility.RandomElement<int>(SackPickup.HPPool);
				float itemsToSpawn = UnityEngine.Random.Range(2, 4);
				for (int i = 0; i < itemsToSpawn; i++)
				{
					LootEngine.SpawnItem(PickupObjectDatabase.GetById(id).gameObject, base.aiActor.sprite.WorldCenter, new Vector2(0, 0), 2.2f, false, true, false);
				}
			}
			if (random > 0.95)
			{
				int id = BraveUtility.RandomElement<int>(SackPickup.Lootdrops);
				float itemsToSpawn = UnityEngine.Random.Range(3, 5);
				for (int i = 0; i < itemsToSpawn; i++)
				{
					LootEngine.SpawnItem(PickupObjectDatabase.GetById(id).gameObject, base.aiActor.sprite.WorldCenter, new Vector2(0, 0), 2.2f, false, true, false);
				}
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
		private float random;

		protected void Start()
		{
			try
			{
				this.storedBody = base.gameObject.GetComponent<SpeculativeRigidbody>();
				SpeculativeRigidbody speculativeRigidbody = this.storedBody;
				SpeculativeRigidbody speculativeRigidbody2 = speculativeRigidbody;
				speculativeRigidbody2.OnTriggerCollision = (SpeculativeRigidbody.OnTriggerDelegate)Delegate.Combine(speculativeRigidbody2.OnTriggerCollision, new SpeculativeRigidbody.OnTriggerDelegate(this.OnPreCollision));
			}
			catch (Exception ex)
			{
				ETGModConsole.Log(ex.Message, false);
			}
		}

		private void OnPreCollision(SpeculativeRigidbody otherRigidbody, SpeculativeRigidbody source, CollisionData collisionData)
		{
			bool hasBeenPickedUp = this.m_hasBeenPickedUp;
			if (!hasBeenPickedUp)
			{
				PlayerController component = otherRigidbody.GetComponent<PlayerController>();
				bool flag = component != null;
				if (flag)
				{
					this.m_hasBeenPickedUp = true;
					this.Pickup(component);
				}
			}
		}
		public SpeculativeRigidbody storedBody;
		private bool m_hasBeenPickedUp;



		public static List<int> GenericPool = new List<int>
		{
			67,//key
			224,//blank
			600,//partial-ammo
			78,//ammo
			565//glass guon stone
		};
		public static List<int> HPPool = new List<int>
		{
			73,//half-heart
			85,//full-heart
			120//armor

		};
		public static List<int> MoneyPool = new List<int>
		{
			68, //1 casing

		};

		public static List<int> Lootdrops = new List<int>
		{
			73,//half-heart
			85,//full-heart
			120,//armor
			67,//key
			224,//blank
			600,//partial-ammo
			78,//ammo
			565//glass guon stone

		};
*/
