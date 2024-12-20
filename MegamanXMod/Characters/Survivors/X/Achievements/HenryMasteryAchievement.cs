using RoR2;
using MegamanXMod.Modules.Achievements;

namespace MegamanXMod.Survivors.X.Achievements
{
    //automatically creates language tokens "ACHIEVMENT_{identifier.ToUpper()}_NAME" and "ACHIEVMENT_{identifier.ToUpper()}_DESCRIPTION" 
    [RegisterAchievement(identifier, unlockableIdentifier, null, 10, null)]
    public class HenryMasteryAchievement : BaseMasteryAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "masteryAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "masteryUnlockable";

        public override string RequiredCharacterBody => XSurvivor.instance.bodyName;

        //difficulty coeff 3 is monsoon. 3.5 is typhoon for grandmastery skins
        public override float RequiredDifficultyCoefficient => 3;
    }
}