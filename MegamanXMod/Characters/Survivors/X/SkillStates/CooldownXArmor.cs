using EntityStates;
using ExtraSkillSlots;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class CooldownXArmor : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;


        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private ExtraSkillLocator extraskillLocator;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        private bool setSkills = false;

        private XArmorComponent armorComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            armorComponent = GetComponent<XArmorComponent>();
            extraskillLocator = base.GetComponent<ExtraSkillLocator>();

            PlayAnimation("FullBody, Override", "HyperMode", "HyperMode.playbackRate", duration);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f * duration);
            }

            EffectManager.SimpleMuzzleFlash(XAssets.HyperModeEffect, base.gameObject, "CorePosition", true);

            armorComponent.RemoveArmorBuffs();

            //TRANSFORM INTO X ARMOR
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                if(this.characterModel != null)
                {
                    childLocator = this.characterModel.GetComponent<ChildLocator>();

                    meshRenderer = childLocator.FindChildGameObject("XBodyMesh").GetComponent<SkinnedMeshRenderer>();
                    meshRenderer.sharedMesh = XAssets.XBodyMesh;
                    meshRenderer.sharedMaterial = XAssets.MatX;
                    characterModel.baseRendererInfos[0].defaultMaterial = XAssets.MatX;
                    childLocator.FindChildGameObject("XShadowSaber").SetActive(false);
                    childLocator.FindChildGameObject("XRathalosSaber").SetActive(false);

                }
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!setSkills && isAuthority)
                SetSkills();


            if (fixedAge >= duration && isAuthority && setSkills)
            {
                setSkills = false;
                outer.SetNextStateToMain();
                return;
            }
        }

        private void SetSkills()
        {
            armorComponent.UnsetAllExtraFirstSkills();
            armorComponent.UnsetAllExtraSecondSkills();
            armorComponent.UnsetAllExtraThirdSkills();
            armorComponent.UnsetAllExtraFourthSkills();
            armorComponent.UnsetAllPrimarySkills();
            armorComponent.UnsetAllSecondarySkills();
            armorComponent.UnsetAllUtilitySkills();
            armorComponent.UnsetAllSpecialSkills();

            //RESET ALL SKILLS TO BASE
            characterBody.skillLocator.primary.SetSkillOverride(characterBody.skillLocator.primary, XSurvivor.XBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.secondary.SetSkillOverride(characterBody.skillLocator.secondary, armorComponent.GetSecondaryBaseSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.utility.SetSkillOverride(characterBody.skillLocator.utility, armorComponent.GetUtilityBaseSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.special.SetSkillOverride(characterBody.skillLocator.special, armorComponent.GetSpecialBaseSkillDef(), GenericSkill.SkillOverridePriority.Contextual);

            extraskillLocator.extraFirst.SetSkillOverride(extraskillLocator.extraFirst, armorComponent.GetPrimaryArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, armorComponent.GetSecondaryArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, armorComponent.GetThirdArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, armorComponent.GetFourthArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);

            setSkills = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}