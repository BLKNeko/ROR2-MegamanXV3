using RoR2;
using RoR2.Achievements;

namespace MegamanXMod.Survivors.X.Achievements
{
    // "Wyvern Slayer": como X, derrote um Magma Worm (a versão elétrica, Overloading Worm, também conta).
    // Vale a morte do Worm por qualquer um do seu time, enquanto você joga de X. Libera: Rathalos Armor.
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5, typeof(ServerAchievement))]
    public class XWyvernSlayerAchievement : XServerTrackedAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "WyvernSlayerAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "RathalosArmorUnlockable";

        private class ServerAchievement : BaseServerAchievement
        {
            private BodyIndex magmaWorm;
            private BodyIndex electricWorm;

            public override void OnInstall()
            {
                base.OnInstall();
                magmaWorm = BodyCatalog.FindBodyIndex("MagmaWormBody");
                electricWorm = BodyCatalog.FindBodyIndex("ElectricWormBody");
                GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
            }

            public override void OnUninstall()
            {
                GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
                base.OnUninstall();
            }

            private void OnCharacterDeath(DamageReport report)
            {
                if (report == null || !report.victimBody)
                    return;

                BodyIndex victim = report.victimBody.bodyIndex;
                if (victim == BodyIndex.None || (victim != magmaWorm && victim != electricWorm))
                    return;

                CharacterBody body = GetCurrentBody();
                if (body && report.victimTeamIndex != body.teamComponent.teamIndex)
                    Grant();
            }
        }
    }
}
