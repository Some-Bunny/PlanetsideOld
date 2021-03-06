using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NpcApi;
using UnityEngine;

namespace Planetside
{
    class CustomShopInitialiser
    {
        public static void InitialiseCustomShops()
        {
            InitialiseTimeTrader();
        }

        public static void InitialiseTimeTrader()
        {
            
            string baseFilepath = "Planetside/Resources/NPCs/TimeTrader/timetrader";
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_GENERIC", "Been a while since I seen ya!");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_STOPPER", "Ya' Don't got much time, ya' know that?");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_PURCHASE", "That'll save ya' some time!");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_FAILPURCHASE", "Maybe some other time, pal.");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_INTRO", "Just on time, pal!");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_ATTACKED", "Ya' bullets are in the wrong timeline pal!");
            ItsDaFuckinShopApi.SetUpShop(
                "timedshop"
                ,"psog"
                , new List<string> { baseFilepath + "_idle_001.png", baseFilepath + "_idle_002.png", baseFilepath + "_idle_003.png", baseFilepath + "_idle_004.png" }
                ,4
                , new List<string> { baseFilepath + "_talk_001.png", baseFilepath + "_talk_002.png", baseFilepath + "_talk_003.png", baseFilepath + "_talk_004.png" }
                ,6
                ,CustomLootTableInitialiser.TimeShopKeeperTable
                ,CustomShopItemController.ShopCurrencyType.COINS
                ,"#TIMETRADER_RUNBASEDMULTILINE_GENERIC"
                ,"#TIMETRADER_RUNBASEDMULTILINE_STOPPER"
                ,"#TIMETRADER_RUNBASEDMULTILINE_PURCHASE"
                ,"#TIMETRADER_RUNBASEDMULTILINE_FAILPURCHASE"
                ,"#TIMETRADER_RUNBASEDMULTILINE_INTRO"
                ,"#TIMETRADER_RUNBASEDMULTILINE_ATTACKED"
                ,new Vector3(1.375f, 2f)
                , new Vector3(0.9375f, 3f, 5.9375f)
                , new Vector3[] { new Vector3(1.125f, 1.5625f, 1), new Vector3(2.625f, 1f, 1), new Vector3(4.125f, 1.5625f, 1), new Vector3(2.625f, 2.125f, 1) }
               , 0.6f
               ,true
               , baseFilepath + "carpet.png");
                 
                 
                             
            /*
            string baseFilepath = "Planetside/Resources/NPCs/TimeTrader/timetrader";
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_GENERIC", "Been a while since I seen ya!");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_STOPPER", "Ya' Don't got much time, ya' know that?");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_PURCHASE", "That'll save ya' some time!");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_FAILPURCHASE", "Maybe some other time, pal.");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_INTRO", "Just on time, pal!");
            ETGMod.Databases.Strings.Core.Set("#TIMETRADER_RUNBASEDMULTILINE_ATTACKED", "Ya' bullets are in the wrong timeline pal!");
            ItsDaFuckinOldShopApi.SetUpShop(
                "timedshop",
                "psog",
                new List<string> { baseFilepath+"_idle_001.png", baseFilepath + "_idle_002.png", baseFilepath + "_idle_003.png", baseFilepath + "_idle_004.png" },
                4,
                new List<string> { baseFilepath + "_talk_001.png", baseFilepath + "_talk_002.png", baseFilepath + "_talk_003.png", baseFilepath + "_talk_004.png" },
                6,
                CustomLootTableInitialiser.TimeShopKeeperTable,
                BaseShopController.AdditionalShopType.TRUCK,
                "#TIMETRADER_RUNBASEDMULTILINE_GENERIC",
                "#TIMETRADER_RUNBASEDMULTILINE_STOPPER",
                "#TIMETRADER_RUNBASEDMULTILINE_PURCHASE",
                "#TIMETRADER_RUNBASEDMULTILINE_FAILPURCHASE",
                "#TIMETRADER_RUNBASEDMULTILINE_INTRO",
                "#TIMETRADER_RUNBASEDMULTILINE_ATTACKED",
                true,
                baseFilepath+"carpet.png",
                0.45f,
                -0.3125f,
                3.25f);
            */
        }
    }
}
