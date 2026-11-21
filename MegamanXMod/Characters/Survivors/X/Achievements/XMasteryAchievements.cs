using MegamanXMod.Modules.Achievements;
using RoR2;

namespace MegamanXMod.Survivors.X.Achievements
{
    // "Ultimate Mastery": como X, vença o jogo (ou obliterate) no Monsoon ou mais difícil. Libera: Ultimate Armor.
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class XUltimateMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "UltimateMasteryAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "UltimateArmorUnlockable";

        public override string RequiredCharacterBody => XSurvivor.instance.bodyName;
        public override float RequiredDifficultyCoefficient => 3; // Monsoon
    }

    // "Final Mission": como X, vença no Monsoon com a skin Command Mission e a X-Fire no loadout. Libera: Ultimate Armor CM.
    // Corrente: só aparece depois de "Electric Heat" (que libera a X-Fire).
    [RegisterAchievement(identifier, unlockableIdentifier, XElectricHeatAchievement.identifier, 10, null)]
    public class XFinalMissionAchievement : BaseMasteryAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "FinalMissionAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "UltimateArmorCMUnlockable";

        public override string RequiredCharacterBody => XSurvivor.instance.bodyName;
        public override float RequiredDifficultyCoefficient => 3; // Monsoon

        protected override bool MeetsExtraRequirements()
        {
            return XAchievementUtils.LoadoutUsesCMSkin(userProfile)
                && XAchievementUtils.LoadoutUsesFirstExtraArmor(userProfile, XSurvivor.HyperModeXFireSkillDef);
        }
    }
}
