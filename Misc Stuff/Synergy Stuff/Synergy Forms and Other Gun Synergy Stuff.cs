using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Planetside
{
	// Token: 0x020000AF RID: 175
	internal class SynergyFormInitialiser
	{
		public static void AddSynergyForms()
		{
			/*
			AdvancedTransformGunSynergyProcessor advancedTransformGunSynergyProcessor = (PickupObjectDatabase.GetById(ChaosRevolver.ChaosRevolverID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
			advancedTransformGunSynergyProcessor.NonSynergyGunId = ChaosRevolver.ChaosRevolverID;
			advancedTransformGunSynergyProcessor.SynergyGunId = ChaosRevolverSynergyForme.ChaosRevolverSynergyFormeID;
			advancedTransformGunSynergyProcessor.SynergyToCheck = "Reunion";
			AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor = (PickupObjectDatabase.GetById(Death.DeathID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			advancedDualWieldSynergyProcessor.PartnerGunID = Taxes.TaxesID;
			advancedDualWieldSynergyProcessor.SynergyNameToCheck = "Death & Taxes";
			AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor1 = (PickupObjectDatabase.GetById(Taxes.TaxesID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			advancedDualWieldSynergyProcessor1.PartnerGunID = Death.DeathID;
			advancedDualWieldSynergyProcessor1.SynergyNameToCheck = "Death & Taxes";
			*/
			AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor = (PickupObjectDatabase.GetById(HardlightNailgun.HardAsNailsID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			advancedDualWieldSynergyProcessor.PartnerGunID = 26;
			advancedDualWieldSynergyProcessor.SynergyNameToCheck = "Stop!";
			AdvancedDualWieldSynergyProcessor advancedDualWieldSynergyProcessor1 = (PickupObjectDatabase.GetById(26) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			advancedDualWieldSynergyProcessor1.PartnerGunID = HardlightNailgun.HardAsNailsID;
			advancedDualWieldSynergyProcessor1.SynergyNameToCheck = "Stop!";

			AdvancedDualWieldSynergyProcessor SPAGHEIIT = (PickupObjectDatabase.GetById(ShockChain.ElectricMusicID) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			SPAGHEIIT.PartnerGunID = 13;
			SPAGHEIIT.SynergyNameToCheck = "UNLIMITED POWER!!!";
			AdvancedDualWieldSynergyProcessor METABOLLS = (PickupObjectDatabase.GetById(13) as Gun).gameObject.AddComponent<AdvancedDualWieldSynergyProcessor>();
			METABOLLS.PartnerGunID = ShockChain.ElectricMusicID;
			METABOLLS.SynergyNameToCheck = "UNLIMITED POWER!!!";

			AdvancedTransformGunSynergyProcessor advancedTransformGunSynergyProcessor = (PickupObjectDatabase.GetById(Polarity.PolarityID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
			advancedTransformGunSynergyProcessor.NonSynergyGunId = Polarity.PolarityID;
			advancedTransformGunSynergyProcessor.SynergyGunId = PolarityForme.PolarityFormeID;
			advancedTransformGunSynergyProcessor.SynergyToCheck = "Refridgeration";

			AdvancedTransformGunSynergyProcessor advancedTransformGunSynergyProcessorveterna = (PickupObjectDatabase.GetById(VeteranShotgun.VeteranID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
			advancedTransformGunSynergyProcessorveterna.NonSynergyGunId = VeteranShotgun.VeteranID;
			advancedTransformGunSynergyProcessorveterna.SynergyGunId = VeteranerShotgun.VeteranerID;
			advancedTransformGunSynergyProcessorveterna.SynergyToCheck = "Old War";

			AdvancedTransformGunSynergyProcessor eae = (PickupObjectDatabase.GetById(Oscillato.AAID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
			eae.NonSynergyGunId = Oscillato.AAID;
			eae.SynergyGunId = OscillatoSynergyForme.AeID;
			eae.SynergyToCheck = "Reverberation";

			AdvancedTransformGunSynergyProcessor colos = (PickupObjectDatabase.GetById(Colossus.ColossusID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
			colos.NonSynergyGunId = Colossus.ColossusID;
			colos.SynergyGunId = PerfectedColossus.PerfectedColossusID;
			colos.SynergyToCheck = "Perfected";

			AdvancedTransformGunSynergyProcessor foosh = (PickupObjectDatabase.GetById(404) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();
			foosh.NonSynergyGunId = 404;
			foosh.SynergyGunId = SirenSynergyForme.smileid;
			foosh.SynergyToCheck = "Ultra";

			RevenantSynergyPlus plus = (PickupObjectDatabase.GetById(45)).gameObject.AddComponent<RevenantSynergyPlus>();
			plus.SynergyNameToCheck = "Boring Eternity";

			RevenantSynergyPlus a = (PickupObjectDatabase.GetById(29)).gameObject.AddComponent<RevenantSynergyPlus>();
			a.SynergyNameToCheck = "Boring Eternity";

			RevenantSynergyPlus fart = (PickupObjectDatabase.GetById(Revenant.RevenantID) as Gun).gameObject.AddComponent<RevenantSynergyPlus>();
			fart.SynergyNameToCheck = "Boring Eternity";

		}
	}
}

