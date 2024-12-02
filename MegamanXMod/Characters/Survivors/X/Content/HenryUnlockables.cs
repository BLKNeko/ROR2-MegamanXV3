using MegamanXMod.Survivors.X.Achievements;
using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X
{
    public static class HenryUnlockables
    {
        public static UnlockableDef characterUnlockableDef = null;
        public static UnlockableDef masterySkinUnlockableDef = null;

        public static void Init()
        {
            masterySkinUnlockableDef = Modules.Content.CreateAndAddUnlockbleDef(
                HenryMasteryAchievement.unlockableIdentifier,
                Modules.Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier),
                XSurvivor.instance.assetBundle.LoadAsset<Sprite>("texMasteryAchievement"));
        }
    }
}
