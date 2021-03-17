﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SaveAPI
{
    public enum CustomDungeonFlags
    {
        //Add your custom flags here
        //You can remove any flags here (except NONE, don't remove it)
        NONE, 
        EXAMPLE_FLAG,
        EXAMPLE_BLUEPRINTMETA_1,
        EXAMPLE_BLUEPRINTMETA_2,
        EXAMPLE_BLUEPRINTMETA_3,
        EXAMPLE_BLUEPRINTBEETLEE,
        EXAMPLE_BLUEPRINTGOOP,
        EXAMPLE_BLUEPRINTTRUCK,
        EXAMPLE_HUNT,
        EXAMPLE_HUNT_REWARD,
        EXAMPLE_ENEMY_DEATH_FLAG,
        EXAMPLE_ENEMY_ACTIVATION_FLAG,
        
        JAMMED_GUARD_DEFEATED,
        BROKEN_CHAMBER_RUN_COMPLETED,
        HIGHER_CURSE_DRAGUN_KILLED,
        SHELLRAX_DEFEATED,
        BULLETBANK_DEFEATED,
        BEAT_LOOP_1,
        BEAT_A_BOSS_UNDER_A_SECOND
    }
}
