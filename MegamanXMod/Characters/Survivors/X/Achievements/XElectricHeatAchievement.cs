using RoR2;
using RoR2.Achievements;

namespace MegamanXMod.Survivors.X.Achievements
{
    // "Electric Heat": como X, complete Abyssal Depths usando a skin Command Mission. Libera: X-Fire.
    // Corrente: só aparece depois de "Commanding the Mission" (que libera a skin CM).
    [RegisterAchievement(identifier, unlockableIdentifier, XBossNormalFormAchievement.identifier, 5, typeof(ServerAchievement))]
    public class XElectricHeatAchievement : XServerTrackedAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "ElectricHeatAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "XFireUnlockable";

        public const string requiredScene = "dampcavesimple"; // Abyssal Depths

        private class ServerAchievement : BaseServerAchievement
        {
            public override void OnInstall()
            {
                base.OnInstall();
                Stage.onServerStageComplete += OnStageComplete;
            }

            public override void OnUninstall()
            {
                Stage.onServerStageComplete -= OnStageComplete;
                base.OnUninstall();
            }

            private void OnStageComplete(Stage stage)
            {
                if (!stage || !stage.sceneDef || stage.sceneDef.baseSceneName != requiredScene)
                    return;

                CharacterBody body = GetCurrentBody();
                if (body && XSurvivor.CMSkinIndex >= 0 && body.skinIndex == (uint)XSurvivor.CMSkinIndex)
                    Grant();
            }
        }
    }
}
