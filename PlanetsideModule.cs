using ItemAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using MonoMod.Utils;
using Dungeonator;
using Brave.BulletScript;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using GungeonAPI;
using SaveAPI;
using Planetside;

namespace Planetside
{

    public class PlanetsideModule : ETGModule
    {
        public static readonly string MOD_NAME = "Planetside Of Gunymede";
        public static readonly string VERSION = "1.0.0";
        public static readonly string TEXT_COLOR = "#9006FF";

        public static string ZipFilePath;
        public static string FilePath;
        public static AdvancedStringDB Strings;

        public override void Start()
        {

            ZipFilePath = this.Metadata.Archive;
            FilePath = this.Metadata.Directory + "/rooms";
            StaticReferences.Init();
            DungeonHandler.Init(); 
            EnemyHooks.Init();
            ToolsEnemy.Init();
            DungeonHandler.Init();
            Hooks.Init();
            PlanetsideModule.Strings = new AdvancedStringDB();


            MultiActiveReloadManager.SetupHooks();
            Tools.Init();

            ETGModConsole.Commands.AddUnit("planetsideflow", (args) =>
            {
                DungeonHandler.debugFlow = !DungeonHandler.debugFlow;
                string status = DungeonHandler.debugFlow ? "enabled" : "disabled";
                string color = DungeonHandler.debugFlow ? "00FF00" : "FF0000";
                ETGModConsole.Log($"Planetside flow {status}", false);
            });

            ShrineFakePrefabHooks.Init();
            ShrineFactory.Init();
            OldShrineFactory.Init();
            EnemyBuilder.Init();
            FakePrefabHooks.Init();
            ItemBuilder.Init();
            BossBuilder.Init();


            GunOrbitShrine.Add();
            NullShrine.Add();
            HolyChamberShrine.Add();

            Unstabullets.Init();
            HullBreakerBullets.Init();
            //Unlocked By Killing Jammed Guard
            ShelltansBlessing.Init();

            EcholocationAmmolet.Init();
            GildedPots.Init();
            GunPrinter.Init();
            ElectrostaticGuonStone.Init();
            AoEBullets.Init();
            PsychicBlank.Init();
            LDCBullets.Init();
            BlastShower.Init();
            BrokenChamber.Init();
            //Unlocked by beating Lich with Broken Chamber
            DiamondChamber.Init();
            TableTechNullReferenceException.Init();

            TestActiveItem.Init();
            //Unlocked by killing Shellrax
            Shellheart.Init();

            SoulboundSkull.Init();
            DiasukesPolymorphine.Init();
            //Unlocked by beating Dragun at 15+ Curse
            FrailtyRounds.Init();
            FrailtyAmmolet.Init();

            OffWorldMedicine.Init();
            BabyGoodCandleKin.Init();

            ShellsnakeOil.Init();
            TableTechDevour.Init();
            UnstableTeslaCoil.Init();
            DeathWarrant.Init();
            OscilaltingBullets.Init();
            TinyPlanetBullets.Init();

            WitherLance.Add();
            SwanOff.Add();
            UglyDuckling.Add();
            HardlightNailgun.Add();
            BurningSun.Add();
            StatiBlast.Add();
            Polarity.Add();
            PolarityForme.Add();
            Revenant.Add();
            //Unlocked By Beating Bullet Banker
            SoulLantern.Add();
            VeteranShotgun.Add();
            VeteranerShotgun.Add();
            GTEE.Add();
            ShockChain.Add();
            Resault.Add();
            Immateria.Add();
            //Unlocked By Beating Loop 1
            ArmWarmer.Add();

            Oscillato.Add();
            OscillatoSynergyForme.Add();

            RebarPuncher.Add();
            LaserChainsaw.Add();
            ExecutionersCrossbow.Add();
            ExecutionersCrossbowSpecial.Init();

            ForgiveMePlease.Init();
            ForgiveMePlease.BuildPrefab();
            //ForgiveMePleaseAiActor.Init();

            PortablePylon.Init();
            LoaderPylonController.Init();

            LoaderPylonSynergyFormeController.Init();

            DeadKingsDesparation.Init();
            DeadKingsDesparation.BuildPrefab();

            //Debuff Icons
            BrokenArmorEffect.Init();
            FrailtyHealthEffect.Init();
            PossessedEffect.Init();

            LeSackPickup.Init();
            NullPickupInteractable.Init();
            
            DeTurretRight.Init();
            DeTurretLeft.Init();
            Barretina.Init();
            Glockulus.Init();
            Cursebulon.Init();
            ArchGunjurer.Init();
            RevolverSkull.Init();
            FodderEnemy.Init();
            JammedGuard.Init();

            An3sBullet.Init();
            HunterBullet.Init();
            NevernamedBullet.Init();
            SkilotarBullet.Init();
            BlazeyBullet.Init();
            SpapiBullet.Init();
            GlaurBullet.Init();
            ApacheBullet.Init();
            NeighborinoBullet.Init();
            BleakBullet.Init();
            KingBullet.Init();
            PandaBullet.Init();
            RetrashBullet.Init();
            KyleBullet.Init();
            BunnyBullet.Init();
            BotBullet.Init();
            WowBullet.Init();
            SpcreatBullet.Init();
            BulletBankMan.Init();

            Shellrax.Init();
            Wailer.Init();

            CelBullet.Init();

            InitialiseSynergies.DoInitialisation();
            SynergyFormInitialiser.AddSynergyForms();
            InitialiseGTEE.DoInitialisation();
            HoveringGunsAdder.AddHovers();

            BrokenChamberShrine.Add();

            Log($"{MOD_NAME} v{VERSION} started successfully.", TEXT_COLOR);
            List<string> RandomFunnys = new List<string>
            {
                "Now With 100% Less Nulls!",
                "You Lost The Game.",
                "UwU",
                "Bullet Banks are not to rob!",
                "Art By SirWow!",
                "Download Some Bunnys Content Pack",
                "weeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee",
                "If you see this, you owe me 10 bucks you nerd.",
                "https://www.youtube.com/watch?v=qn0YdT_pQF8",
                "https://www.youtube.com/watch?v=EGXPAoyP_cg",
                "Ashes To Ashes, To Ashes (To Ashes)",
                "Deadbolt Is Underrated!",
                "You're Gonna Need A Bigger Gun",
                "Chocolate And Mayonnaise Sandwich.",
                "o______________________________o",
                "Sai Sinut Nayttamaan",
                "oh no",
                "is that supposed to be like that???",
                "I'm so lonely.",
                "I removed an item from the item pool, which one is it? ;)",
                "Peter Griffin!",
                "Hey VSauce!",
                "Yo Mama!",
                "NullReferenceException: Object Reference not set to an instance of an object.",
                "I stole this one from Nevernamed.",
                "25.3°N 91.7°E",
                "poggers",
                "Nuh uh, yr'oue.",
                "eg",
                "._.",
                "mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm",
                "Warning! Level 5 Quarantine In Action! Do Not Land!",
                "Dusa",
                "Theres something in the walls...",
                "<3",
                "Did I ever fix that bug?",
                "I didn't get enough sleep last night.",
                "bonk",
                "This is not a GMod Server.",
                "oops",
                "Can I offer you a nice egg in this trying time?",
                "Up Up Down Down Left Right Left Right B A Start",
                "Another Night...",
                "skibidi bop mm dada",
                "I coded most of this after 1am.",
                "Planetside Supports Trans Rights",
                "bepis",
                "pootis",
                "Frogs are cool!",
                "It's... so... warm...",
                "Poor aim, and a poor Reaper."
            };
            Random r = new Random();
            int index = r.Next(RandomFunnys.Count);
            string randomString = RandomFunnys[index];
            Log(" - "+randomString, TEXT_COLOR);

            Log("Here's a To-Do list for unlocks, hope you have fun!", TEXT_COLOR);
            Log("If you ever need a reminder, the command 'psog to_do_list' to remind yourself.", TEXT_COLOR);
            string a = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.HIGHER_CURSE_DRAGUN_KILLED) ? " Done!\n" : " -Defeat The Dragun At A Higher Curse.\n";
            string b = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.JAMMED_GUARD_DEFEATED) ? " Done!\n" : " -Defeat The Guardian Of The Holy Chamber.\n";
            string c = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.SHELLRAX_DEFEATED) ? " Done!\n" : " -Defeat The Failed Demi-Lich\n";
            string d = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BULLETBANK_DEFEATED) ? " Done!\n" : " -Defeat The Banker Of Bullets.\n";
            string e = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED) ? " Done!\n" : " -Defeat The Lich With A Broken Remnant In Hand.\n";
            string f = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BEAT_LOOP_1) ? " Done!\n" : " -Beat The Game On Ouroborous Level 0.\n";
            string g = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BEAT_A_BOSS_UNDER_A_SECOND) ? " Done!\n" : " -Kill A Boss After Dealing 500 Damage Or More At Once.\n";
            string color1 = "9006FF";
            OtherTools.PrintNoID("Unlock List:\n" + a + b + c + d + e + f + g, color1);
            OtherTools.Init();
        }

        public static void Log(string text, string color="#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        public static void aNGERgODScOMPONENT(Action<PlayerController, float> orig, PlayerController self, float invisibleDelay)
        {
            orig(self, invisibleDelay);
            self.gameObject.AddComponent<TellThePlayerTofuckRightOff>();
        }


 
        /*
        public static void SpawnProjectilesLOTJ(Action<SuperReaperController> orig, SuperReaperController self)
        {
            GameObject obj = new GameObject();
            if (GameManager.Instance.PreventPausing || BossKillCam.BossDeathCamRunning)
            {
                return;
            }
            if (SuperReaperController.PreventShooting)
            {
                return;
            }
            CellData cellData = GameManager.Instance.Dungeon.data[self.ShootPoint.position.IntXY(VectorConversions.Floor)];
            if (cellData == null || cellData.type == CellType.WALL)
            {
                return;
            }
            if (!PlanetsideModule.m_bulletSource)
            {
                PlanetsideModule.m_bulletSource = self.ShootPoint.gameObject.GetOrAddComponent<BulletScriptSource>();
            }
            PlanetsideModule.m_bulletSource.BulletManager = self.bulletBank;
            PlanetsideModule.m_bulletSource.BulletScript = new CustomBulletScriptSelector(typeof(LJ));
            PlanetsideModule.m_bulletSource.Initialize();
        }
        */
        public static BulletScriptSource m_bulletSource;



        public class TellThePlayerTofuckRightOff : BraveBehaviour
        {
            // Token: 0x060004A9 RID: 1193 RVA: 0x0002A69C File Offset: 0x0002889C
            public void Start()
            {
                //Gun gun;
                //ETGModConsole.Log("AAAAAAAAAAAAAAAAAAAAAA");
                PlayerController player = GameManager.Instance.PrimaryPlayer;
                player.OnUsedBlank += this.HandleTriedAttack;
                GameManager.Instance.OnNewLevelFullyLoaded += this.OnNewFloor;
            }
            public void Update()
            {

            }
            private void OnNewFloor()
            {
                this.SummonedOnFloor = 0f;
            }

            private void HandleTriedAttack(PlayerController obj, int what)
            {
                //ETGModConsole.Log("ITS THIS TRIGGERING EVEN");
                if ((CheckforSpeciRoom.Contains(obj.CurrentRoom.GetRoomName())))
                {
                    bool TryForGuard = this.SummonedOnFloor == 0;
                    if (TryForGuard)
                    {
                        string header = "DEFILER!";
                        string text = "The Gods Have Been Angered.";
                        if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Diamond Chamber"].PickupObjectId)))
                        {
                            //this.SummonedOnFloor += 1f;
                            header = "YOU ARE FORGIVEN.";
                            text = "FOR NOW.";
                        }
                        else
                        {
                            AkSoundEngine.PostEvent("Play_BOSS_lichB_intro_01", base.gameObject);
                            GameObject gameObject = new GameObject();
                            gameObject.transform.position = obj.transform.position;
                            BulletScriptSource source = gameObject.GetOrAddComponent<BulletScriptSource>();
                            gameObject.AddComponent<BulletSourceKiller>();
                            var bulletScriptSelected = new CustomBulletScriptSelector(typeof(AngerGodsScript));
                            AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
                            AIBulletBank bulletBank = aIActor.GetComponent<AIBulletBank>();
                            bulletBank.CollidesWithEnemies = false;
                            source.BulletManager = bulletBank;
                            source.BulletScript = bulletScriptSelected;
                            source.Initialize();//to fire the script once
                        }
                        this.SummonedOnFloor += 1f;
                        TellThePlayerTofuckRightOff.Notify(header, text);
                    }
                }
            }
            private static void Notify(string header, string text)
            {
                tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
                int spriteIdByName = encounterIconCollection.GetSpriteIdByName("Planetside/Resources/shelltansblessing.png");
                GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, null, spriteIdByName, UINotificationController.NotificationColor.SILVER, false, true);
            }
            private float SummonedOnFloor = 0;
            public float NearRadius = 5f;
            public float FarRadius = 9f;
            public static List<string> CheckforSpeciRoom = new List<string>()
            {
            "HolyChamberRoom.room"
            };

        }

        public override void Exit() { }
        public override void Init() 
        {
            SaveAPIManager.Setup("psog");
        }
        public static void ReloadBreachShrinesPSOG(Action<Foyer> orig, Foyer self1)
        {
            orig(self1);
            //Bugun.ThisIsBasicallyCelsRNGUNButTakenToASillyLevel();
            bool flag = PlanetsideModule.hasInitialized;
            if (!flag)
            {
                OuroborousShrine.Add();
                {
                    ShrineFactory.PlaceBreachShrines();
                }
                PlanetsideModule.hasInitialized = true;
            }
            ShrineFactory.PlaceBreachShrines();
        }
        private static bool hasInitialized;
    }
}

/*
public class LJ : Script
{
    protected override IEnumerator Top()
    {

        base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("41ee1c8538e8474a82a74c4aff99c712").bulletBank.GetBullet("big"));
        base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("6868795625bd46f3ae3e4377adce288b").bulletBank.GetBullet("dagger"));
        this.Fire(new Direction(0f, DirectionType.Aim, -1f), new Speed(0f, SpeedType.Absolute), new LJ.Superball());
        /*
        float startingDir = UnityEngine.Random.Range(0f, 360f);
        for (int e = 0; e < 4; e++)
        {
            for (int i = -3; i < 2; i++)
            {
                base.Fire(new Direction((float)(i * 7) + 90 * e + startingDir, DirectionType.Aim, -1f), new Speed(2f - (float)Mathf.Abs(i) * 0.5f, SpeedType.Absolute), new WaveBullet());

            }
            for (int i = -3; i < 2; i++)
            {
                base.Fire(new Direction(((float)(i * 7) + 90 * e + startingDir)+45, DirectionType.Aim, -1f), new Speed(5f - (float)Mathf.Abs(i) * 0.5f, SpeedType.Absolute), new WaveBullet());

            }
        }
        
        yield break;
    }


    public class Superball : Bullet
    {
        public Superball() : base("big", false, false, false)
        {
        }
        protected override IEnumerator Top()
        {

            base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("ec6b674e0acd4553b47ee94493d66422").bulletBank.GetBullet("bigBullet"));
            yield return this.Wait(120);
            base.Vanish(false);
            yield break;
        }
        public override void OnBulletDestruction(Bullet.DestroyType destroyType, SpeculativeRigidbody hitRigidbody, bool preventSpawningProjectiles)
        {
            base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("ec6b674e0acd4553b47ee94493d66422").bulletBank.GetBullet("bigBullet"));

            if (!preventSpawningProjectiles)
            {
                float startAngle = base.RandomAngle();
                float delta = 90f;
                for (int A = 0; A < 4; A++)
                {
                    float num = startAngle + (float)A * delta;
                    this.Fire(new Direction(num, DirectionType.Absolute, -1f), new Speed(2.5f, SpeedType.Relative), new LJ.Break(base.Position,false ,num));
                }
                for (int A = 0; A < 4; A++)
                {
                    float num = startAngle + (float)A * delta;
                    this.Fire(new Direction(num + 9, DirectionType.Absolute, -1f), new Speed(2f, SpeedType.Relative), new LJ.Break(base.Position,true ,num));
                }
            }

        }
        
    }
    public class Break : Bullet
    {
        public Break(Vector2 centerPoint, bool speeen, float startAngle) : base("bigBullet", false, false, false)
        {
            this.centerPoint = centerPoint;
            this.yesToSpeenOneWay = speeen;
            this.startAngle = startAngle;
        }
        protected override IEnumerator Top()
        {
            for (int E = 0; E < 2; E++)
            {
                base.ManualControl = true;
                float radius = Vector2.Distance(this.centerPoint, base.Position);
                float speed = this.Speed;
                float spinAngle = this.startAngle;
                float spinSpeed = 0f;
                for (int i = 0; i < 180; i++)
                {
                    speed += 0.13333334f;
                    radius += speed / 60f;
                    if (yesToSpeenOneWay == true)
                    {
                        spinSpeed -= 0.666f;
                    }
                    else
                    {
                        spinSpeed += 0.666f;

                    }
                    spinAngle += spinSpeed / 60f;
                    base.Position = this.centerPoint + BraveMathCollege.DegreesToVector(spinAngle, radius);
                    yield return base.Wait(1);
                }
                base.Vanish(false);
                yield break;
            }
            yield break;
        }
        public Vector2 centerPoint;
        public bool yesToSpeenOneWay;
        public float startAngle;
    }
    public class WaveBullet : Bullet
    {
        public WaveBullet() : base("dagger", false, false, false)
        {
        }

        protected override IEnumerator Top()
        {
            yield return base.Wait(20);
            float speed = base.Speed;
            base.ChangeSpeed(new Speed(speed+10, SpeedType.Absolute), 60);

            yield break;
        }
    }

}
*/
public class AngerGodsScript : Script
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
        base.Fire(Offset.OverridePosition(vector2 + new Vector2(0f, 30f)), new Direction(-90f, DirectionType.Absolute, -1f), new Speed(30f, SpeedType.Absolute), new AngerGodsScript.BigBullet());

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
                base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
                var list = new List<string> {
				//"shellet",
				"jammed_guardian"
            };
                string guid = BraveUtility.RandomElement<string>(list);
                var Enemy = EnemyDatabase.GetOrLoadByGuid(guid);
                if ((GameManager.Instance.PrimaryPlayer.HasPickupID(DiamondChamber.ChamberID)))
                {
                    Enemy.IsHarmlessEnemy = true;
                    Enemy.CanTargetPlayers = false;
                    Enemy.CanTargetEnemies = true;
                }
                AIActor.Spawn(Enemy.aiActor, this.Projectile.sprite.WorldCenter, GameManager.Instance.PrimaryPlayer.CurrentRoom, true, AIActor.AwakenAnimationType.Default, true);
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

    }
}
