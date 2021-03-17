using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Planetside
{
    class HoveringGunsAdder
    {
        public static void AddHovers()
        {
            AdvancedHoveringGunProcessor UglyDuckling1 = (PickupObjectDatabase.GetById(SwanOff.SwanOffID) as Gun).gameObject.AddComponent<AdvancedHoveringGunProcessor>();
            UglyDuckling1.Activate = true;
            UglyDuckling1.ConsumesTargetGunAmmo = false;
            UglyDuckling1.AimType = HoveringGunController.AimType.PLAYER_AIM;
            UglyDuckling1.PositionType = HoveringGunController.HoverPosition.CIRCULATE;
            UglyDuckling1.FireType = HoveringGunController.FireType.ON_FIRED_GUN;
            UglyDuckling1.UsesMultipleGuns = false;
            UglyDuckling1.TargetGunID = UglyDuckling.DuckyID;
            UglyDuckling1.RequiredSynergy = "Ugly Duckling";
            UglyDuckling1.FireCooldown = .33f;
            UglyDuckling1.FireDuration = 0.1f;
        }
    }
}