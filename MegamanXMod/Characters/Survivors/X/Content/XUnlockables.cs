using MegamanXMod.Survivors.X.Achievements;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X
{
    public static class XUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static UnlockableDef cmSkinUnlockableDef = null;          // XBossNormalFormAchievement
        public static UnlockableDef bladeArmorUnlockableDef = null;      // XShadowSaberKillsAchievement
        public static UnlockableDef secondArmorUnlockableDef = null;     // XCapsuleHunterAchievement
        public static UnlockableDef xFireUnlockableDef = null;           // XElectricHeatAchievement
        public static UnlockableDef gaeaArmorUnlockableDef = null;       // XHeavyLiftingAchievement
        public static UnlockableDef ultimateArmorUnlockableDef = null;   // XUltimateMasteryAchievement
        public static UnlockableDef rathalosArmorUnlockableDef = null;   // XWyvernSlayerAchievement
        public static UnlockableDef ultimateArmorCMUnlockableDef = null; // XFinalMissionAchievement

        // O que vai de fato na skin/armadura: null (já liberado) se a opção "Unlocks" correspondente estiver marcada.
        // As opções são lidas ao carregar o mod, então mudar exige reiniciar o jogo.
        private static UnlockableDef Locked(UnlockableDef def, BepInEx.Configuration.ConfigEntry<bool> unlockOption)
            => unlockOption != null && unlockOption.Value ? null : def;

        public static UnlockableDef CMSkin => Locked(cmSkinUnlockableDef, XConfig.unlockCMSkin);
        public static UnlockableDef BladeArmor => Locked(bladeArmorUnlockableDef, XConfig.unlockBladeArmor);
        public static UnlockableDef SecondArmor => Locked(secondArmorUnlockableDef, XConfig.unlockSecondArmor);
        public static UnlockableDef XFire => Locked(xFireUnlockableDef, XConfig.unlockXFire);
        public static UnlockableDef GaeaArmor => Locked(gaeaArmorUnlockableDef, XConfig.unlockGaeaArmor);
        public static UnlockableDef UltimateArmor => Locked(ultimateArmorUnlockableDef, XConfig.unlockUltimateArmor);
        public static UnlockableDef RathalosArmor => Locked(rathalosArmorUnlockableDef, XConfig.unlockRathalosArmor);
        public static UnlockableDef UltimateArmorCM => Locked(ultimateArmorCMUnlockableDef, XConfig.unlockUltimateArmorCM);

        private static UnlockableDef Create(string unlockableIdentifier, string achievementIdentifier, string iconName)
        {
            // O identificador do UnlockableDef tem que ser o mesmo "unlockableIdentifier" do [RegisterAchievement]
            return Modules.Content.CreateAndAddUnlockbleDef(
                unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(achievementIdentifier),
                XSurvivor.instance.assetBundle.LoadAsset<Sprite>(iconName));
        }

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                HenryMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier),
                XSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));

            cmSkinUnlockableDef = Create(XBossNormalFormAchievement.unlockableIdentifier, XBossNormalFormAchievement.identifier, "XCMIcon");
            bladeArmorUnlockableDef = Create(XShadowSaberKillsAchievement.unlockableIdentifier, XShadowSaberKillsAchievement.identifier, "XBladeIcon");
            secondArmorUnlockableDef = Create(XCapsuleHunterAchievement.unlockableIdentifier, XCapsuleHunterAchievement.identifier, "XSecondIcon");
            xFireUnlockableDef = Create(XElectricHeatAchievement.unlockableIdentifier, XElectricHeatAchievement.identifier, "XFireIcon");
            gaeaArmorUnlockableDef = Create(XHeavyLiftingAchievement.unlockableIdentifier, XHeavyLiftingAchievement.identifier, "XGaeaIcon");
            ultimateArmorUnlockableDef = Create(XUltimateMasteryAchievement.unlockableIdentifier, XUltimateMasteryAchievement.identifier, "XUltimateIcon");
            rathalosArmorUnlockableDef = Create(XWyvernSlayerAchievement.unlockableIdentifier, XWyvernSlayerAchievement.identifier, "XRathalosIcon");
            ultimateArmorCMUnlockableDef = Create(XFinalMissionAchievement.unlockableIdentifier, XFinalMissionAchievement.identifier, "XUltimateCMIcon");
        }
    }
}
