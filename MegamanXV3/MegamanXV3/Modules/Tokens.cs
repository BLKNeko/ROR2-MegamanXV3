using R2API;
using System;

namespace MegamanXV3.Modules
{
    internal static class Tokens
    {
        internal static void AddTokens()
        {
            #region Megaman
            string prefix = MegamanXV3Plugin.DEVELOPER_PREFIX + "_MEGAMANXV3_BODY_";


            string desc = "Megaman X, the B class maverick hunter<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X can transform either of his arms into a powerful buster to shoot bullets of compressed solar energy, and has an energy amplifier that allows it to be charged up and release a more powerful shot." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X-Buster is powerful but slow and his charged shot have a limited range, FK-Buster is weaker but faster and have no range limit</color>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X fires a shard of ice that can freeze and damage a target." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Emergency Acceleration System(DASH) is a move that temporarily speeds up the character." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X's Limitless Potencial make it easy to turn on the table, but be wary of it's high cooldown." + Environment.NewLine + Environment.NewLine;

            string outro = "Back to the Hunter Base.";
            string outroFailure = "Sorry...I...Failed....";

            LanguageAPI.Add(prefix + "NAME", "Megaman X");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Megaman X, B class Hunter");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            LanguageAPI.Add(prefix + "PASSIVE_NAME", "Limitless Potential");
            LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "<style=cIsUtility>X's true potential still unachieved, but his adaptation and improvement grow's super fast.</style> <style=cIsHealing>When X HP gets Low, he uses his true powers, getting temporary stronger and generating a shield</style>, <style=cIsDamage> but after this he need to recharge before use this again.</style>");
            #endregion

            #region Primary

            LanguageAPI.Add(prefix + "CHARGE_SHOT_NAME", "X-Buster");
            LanguageAPI.Add(prefix + "CHARGE_SHOT_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>170 % damage</style>.");


            LanguageAPI.Add(prefix + "FK_BUSTER_NAME", "FK-Buster");
            LanguageAPI.Add(prefix + "FK_BUSTER_DESCRIPTION", "Shoot with FK-Buster, dealing <style=cIsDamage>125% damage</style>. his charged attack bypass some enemies armor");

            #endregion

            #region Secondary

            LanguageAPI.Add(prefix + "SHOTGUNICE_NAME", "ShotgunIce");
            LanguageAPI.Add(prefix + "SHOTGUNICE_DESCRIPTION", "Shoot an IceMissle that pierce enemies, dealing <style=cIsDamage> 200 % damage </style>.");

            LanguageAPI.Add(prefix + "SQUEEZEBOMB_NAME", "Squeeze Bomb");
            LanguageAPI.Add(prefix + "SQUEEZEBOMB_DESCRIPTION", "A gravity-based weapon. Creates localized black holes that hold up enemies.");

            LanguageAPI.Add(prefix + "FIREWAVE_NAME", "Fire Wave");
            LanguageAPI.Add(prefix + "FIREWAVE_DESCRIPTION", "X releases a constant stream of flames from his buster");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "DASH_NAME", "Dash");
            LanguageAPI.Add(prefix + "DASH_DESCRIPTION", "<style=cIsDamage>Perform a Dash</style>.</style>");

            LanguageAPI.Add(prefix + "NOVADASH_NAME", "Nova Strike");
            LanguageAPI.Add(prefix + "NOVADASH_DESCRIPTION", "<style=cIsDamage>X first surrounds his body with immense energy, then performs an invincible flying tackle</style>.</style>");
            #endregion

            #region Special

            LanguageAPI.Add(prefix + "GREENNEEDLE_NAME", "Homing Torpedo");
            LanguageAPI.Add(prefix + "GREENNEEDLE_DESCRIPTION", "fires a small missile that seeks out enemies, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "HOMINGTORPEDO_NAME", "GreenNeedle");
            LanguageAPI.Add(prefix + "HOMINGTORPEDO_DESCRIPTION", "Shoot a small missle tha follow some targets, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "RISINGFIRER_NAME", "Rising Fire R");
            LanguageAPI.Add(prefix + "RISINGFIRER_DESCRIPTION", "When equipped with this weapon, X raises his arm in the air and shoots firebombs upwards dealing <style=cIsDamage> 200 % base damage </style>.");

            LanguageAPI.Add(prefix + "ACIDBURST_NAME", "Acid Burst");
            LanguageAPI.Add(prefix + "ACIDBURST_DESCRIPTION", "When fired, it creates a glob of acid which, upon contact with any surface, will create acid crystals, dealing <style=cIsDamage> 125 % base damage </style> and poisoning enemies, When charged, X will fire two balls of acid dealing a little more damage");

            LanguageAPI.Add(prefix + "CHAMELEONSTING_NAME", "Chameleon Sting");
            LanguageAPI.Add(prefix + "CHAMELEONSTING_DESCRIPTION", "X fires tree beams in a wide angle, dealing <style=cIsDamage> 160 % base damage </style>. When charged, the Chameleon Sting make X temporarily invulnerable");


            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion
            #endregion
        }
    }
}