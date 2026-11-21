using RoR2;
using RoR2.Achievements;

namespace MegamanXMod.Survivors.X.Achievements
{
    // "Hunter Rank": derrote um chefe jogando de X, na forma normal (sem nenhuma armadura ativa).
    // Libera: skin Command Mission.
    //
    // Como funciona:
    //  - A parte do cliente (esta classe) só fica "ligada" enquanto o jogador está jogando de X
    //    (LookUpRequiredBodyIndex). Quando liga, pede ao servidor para acompanhar (SetServerTracked).
    //  - A parte do servidor (ServerAchievement) escuta as mortes. Quem conta a kill é o servidor,
    //    porque é ele quem sabe quem matou quem; ao cumprir, Grant() avisa o cliente e o achievement é salvo no perfil.
    //
    // Tokens criados em XTokens: ACHIEVEMENT_{identifier em maiúsculas}_NAME / _DESCRIPTION
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5, typeof(ServerAchievement))]
    public class XBossNormalFormAchievement : BaseAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "BossNormalFormAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "CMSkinUnlockable";

        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex(XSurvivor.instance.bodyName);
        }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            SetServerTracked(true);
        }

        public override void OnBodyRequirementBroken()
        {
            SetServerTracked(false);
            base.OnBodyRequirementBroken();
        }

        private class ServerAchievement : BaseServerAchievement
        {
            public override void OnInstall()
            {
                base.OnInstall();
                GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;
            }

            public override void OnUninstall()
            {
                GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
                base.OnUninstall();
            }

            private void OnCharacterDeath(DamageReport report)
            {
                if (report == null || !report.attackerBody || !report.victimBody)
                    return;

                // Tem que ser o X deste jogador
                if (report.attackerBody != GetCurrentBody())
                    return;

                // Chefe: boss do teleporter (e afins) ou qualquer "campeão" (Titã, Besouro Rainha, Mithrix...)
                bool isBoss = report.victimIsBoss || report.victimBody.isChampion;
                if (!isBoss)
                    return;

                if (XAchievementUtils.HasAnyArmor(report.attackerBody))
                    return;

                Grant();
            }
        }
    }
}
