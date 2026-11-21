using RoR2;
using RoR2.Achievements;
using System.Collections.Generic;

namespace MegamanXMod.Survivors.X.Achievements
{
    // "Shadow Hunter": mate 100 inimigos com o sabre da Shadow Armor em uma única run.
    // Libera: Blade Armor.
    //
    // A kill conta quando o X está com a Shadow Armor (ShadowArmorBuff) e o golpe veio da skill
    // secundária (o sabre substitui a secundária nessa armadura). A contagem fica guardada por jogador
    // e zera no começo de cada run, então trocar de fase não perde o progresso.
    [RegisterAchievement(identifier, unlockableIdentifier, null, 5, typeof(ServerAchievement))]
    public class XShadowSaberKillsAchievement : BaseAchievement
    {
        public const string identifier = XSurvivor.MEGAMAN_x_PREFIX + "ShadowSaberKillsAchievement";
        public const string unlockableIdentifier = XSurvivor.MEGAMAN_x_PREFIX + "BladeArmorUnlockable";

        public const int requiredKills = 100;

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
            // Contagem por jogador, durante a run atual
            private static readonly Dictionary<NetworkUser, int> killsThisRun = new Dictionary<NetworkUser, int>();
            private static bool resetHooked;

            public override void OnInstall()
            {
                base.OnInstall();
                GlobalEventManager.onCharacterDeathGlobal += OnCharacterDeath;

                if (!resetHooked)
                {
                    resetHooked = true;
                    Run.onRunStartGlobal += _ => killsThisRun.Clear();
                }
            }

            public override void OnUninstall()
            {
                GlobalEventManager.onCharacterDeathGlobal -= OnCharacterDeath;
                base.OnUninstall();
            }

            private void OnCharacterDeath(DamageReport report)
            {
                if (report == null || !report.attackerBody || report.damageInfo == null)
                    return;

                if (report.attackerBody != GetCurrentBody())
                    return;

                if (!report.attackerBody.HasBuff(XBuffs.ShadowArmorBuff))
                    return;

                // Golpe da skill secundária = sabre da Shadow Armor
                if ((report.damageInfo.damageType.damageSource & DamageSource.Secondary) == 0)
                    return;

                NetworkUser user = serverAchievementTracker.networkUser;
                killsThisRun.TryGetValue(user, out int kills);
                kills++;
                killsThisRun[user] = kills;

                if (kills >= requiredKills)
                    Grant();
            }
        }
    }
}
