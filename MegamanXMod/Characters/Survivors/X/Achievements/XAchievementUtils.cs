using ExtraSkillSlots;
using RoR2;
using RoR2.Achievements;
using RoR2.Skills;
using System;

namespace MegamanXMod.Survivors.X.Achievements
{
    public static class XAchievementUtils
    {
        // true se o X está usando qualquer armadura (hyper mode). Sem nenhuma = forma normal.
        public static bool HasAnyArmor(CharacterBody body)
        {
            return body.HasBuff(XBuffs.LightArmorBuff)
                || body.HasBuff(XBuffs.SecondArmorBuff)
                || body.HasBuff(XBuffs.MaxArmorBuff)
                || body.HasBuff(XBuffs.FourthArmorBuff)
                || body.HasBuff(XBuffs.FalconArmorBuff)
                || body.HasBuff(XBuffs.GaeaArmorBuff)
                || body.HasBuff(XBuffs.ShadowArmorBuff)
                || body.HasBuff(XBuffs.UltimateArmorBuff)
                || body.HasBuff(XBuffs.RathalosArmorBuff)
                || body.HasBuff(XBuffs.XFireBuff)
                || body.HasBuff(XBuffs.BladeArmorBuff)
                || body.HasBuff(XBuffs.UltimateCMArmorBuff);
        }

        public static BodyIndex XBodyIndex => BodyCatalog.FindBodyIndex(XSurvivor.instance.bodyName);

        // Skin escolhida no loadout do jogador (vale para a run atual)
        public static bool LoadoutUsesCMSkin(UserProfile profile)
        {
            if (profile == null || profile.loadout == null || XSurvivor.CMSkinIndex < 0)
                return false;
            return profile.loadout.bodyLoadoutManager.GetSkinIndex(XBodyIndex) == (uint)XSurvivor.CMSkinIndex;
        }

        // Armadura escolhida no primeiro slot extra do loadout
        public static bool LoadoutUsesFirstExtraArmor(UserProfile profile, SkillDef armor)
        {
            if (profile == null || profile.loadout == null || !armor)
                return false;

            BodyIndex bodyIndex = XBodyIndex;
            ExtraSkillLocator extra = XSurvivor.instance.bodyPrefab.GetComponent<ExtraSkillLocator>();
            GenericSkill[] slots = BodyCatalog.GetBodyPrefabSkillSlots(bodyIndex);
            if (!extra || !extra.extraFirst || slots == null)
                return false;

            int slotIndex = Array.IndexOf(slots, extra.extraFirst);
            if (slotIndex < 0)
                return false;

            uint variant = profile.loadout.bodyLoadoutManager.GetSkillVariant(bodyIndex, slotIndex);
            SkillFamily family = extra.extraFirst.skillFamily;
            return family && variant < family.variants.Length && family.variants[variant].skillDef == armor;
        }
    }

    // Base para achievements "jogando de X" acompanhados pelo servidor (kills, fases, inventário...).
    public abstract class XServerTrackedAchievement : BaseAchievement
    {
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return XAchievementUtils.XBodyIndex;
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
    }
}
