using RoR2;
using RoR2.Achievements;

namespace MegamanXMod.Survivors.X.Achievements
{
    // "Heavy Lifting": tenha a Loader liberada. Libera: Gaea Armor.
    // Não depende de jogar de X: quem já tem a Loader ganha logo que o perfil carrega.
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5, null)]
    public class XHeavyLiftingAchievement : BaseAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "HeavyLiftingAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "GaeaArmorUnlockable";

        public const string loaderUnlockable = "Characters.Loader";

        public override void OnInstall()
        {
            base.OnInstall();
            UserProfile.onUnlockableGranted += OnUnlockableGranted;
            Check();
        }

        public override void OnUninstall()
        {
            UserProfile.onUnlockableGranted -= OnUnlockableGranted;
            base.OnUninstall();
        }

        private void OnUnlockableGranted(UserProfile profile, UnlockableDef unlockableDef)
        {
            if (profile == userProfile)
                Check();
        }

        private void Check()
        {
            UnlockableDef loader = UnlockableCatalog.GetUnlockableDef(loaderUnlockable);
            if (loader && userProfile.HasUnlockable(loader))
                Grant();
        }
    }
}
