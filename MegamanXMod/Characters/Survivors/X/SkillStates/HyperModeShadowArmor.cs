using EntityStates;
using ExtraSkillSlots;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HyperModeShadowArmor : BaseSkillState
    {
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;


        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private bool setSkills = false;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        private ExtraSkillLocator extraskillLocator;

        private XArmorComponent armorComponent;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "Muzzle";

            PlayAnimation("FullBody, Override", "HyperMode", "HyperMode.playbackRate", duration);

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 1.5f * duration);
            }

            EffectManager.SimpleMuzzleFlash(XAssets.HyperModeEffect, base.gameObject, "CorePosition", true);

            armorComponent = GetComponent<XArmorComponent>();
            extraskillLocator = base.GetComponent<ExtraSkillLocator>();

            armorComponent.RemoveArmorBuffs();

            AkSoundEngine.PostEvent(XStaticValues.X_HyperMode_SFX, this.gameObject);

            //TRANSFORM INTO SHADOW ARMOR
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                if(this.characterModel != null)
                {
                    childLocator = this.characterModel.GetComponent<ChildLocator>();

                    meshRenderer = childLocator.FindChildGameObject("XBodyMesh").GetComponent<SkinnedMeshRenderer>();
                    meshRenderer.sharedMesh = XAssets.ShadowBodyMesh;
                    meshRenderer.sharedMaterial = XAssets.MatShadow;
                    characterModel.baseRendererInfos[0].defaultMaterial = XAssets.MatShadow;
                    childLocator.FindChildGameObject("XShadowSaber").SetActive(true);
                    childLocator.FindChildGameObject("XRathalosSaber").SetActive(false);

                }
            }

        }

        public override void OnExit()
        {
            base.OnExit();

            if (this.modelTransform)
            {
                TemporaryOverlayInstance temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(this.modelTransform.gameObject);
                temporaryOverlayInstance.duration = 0.4f;
                temporaryOverlayInstance.animateShaderAlpha = true;
                temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlayInstance.destroyComponentOnEnd = true;
                temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
                temporaryOverlayInstance.AddToCharacterModel(this.modelTransform.GetComponent<CharacterModel>());
                TemporaryOverlayInstance temporaryOverlayInstance2 = TemporaryOverlayManager.AddOverlay(this.modelTransform.gameObject);
                temporaryOverlayInstance2.duration = 0.5f;
                temporaryOverlayInstance2.animateShaderAlpha = true;
                temporaryOverlayInstance2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                temporaryOverlayInstance2.destroyComponentOnEnd = true;
                temporaryOverlayInstance2.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashExpanded");
                temporaryOverlayInstance2.AddToCharacterModel(this.modelTransform.GetComponent<CharacterModel>());
            }

            if (NetworkServer.active)
            {
                characterBody.AddBuff(XBuffs.ShadowArmorBuff);
            }

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


            //RESET ALL EXTRA SKILLS AND SET FOURTH EXTRA TO COOLDOWN X
            extraskillLocator.extraFirst.SetSkillOverride(extraskillLocator.extraFirst, armorComponent.GetPrimaryArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, armorComponent.GetSecondaryArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, armorComponent.GetThirdArmorSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, XSurvivor.CoolDownXArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            //RESET ALL NORMAL SKILLS AND SET THE PRIMARY AND SECONDARY FOR SHADOW
            characterBody.skillLocator.primary.SetSkillOverride(characterBody.skillLocator.primary, XSurvivor.XShadowBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.secondary.SetSkillOverride(characterBody.skillLocator.secondary, XSurvivor.XShadowSaberSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.utility.SetSkillOverride(characterBody.skillLocator.utility, armorComponent.GetUtilityBaseSkillDef(), GenericSkill.SkillOverridePriority.Contextual);
            characterBody.skillLocator.special.SetSkillOverride(characterBody.skillLocator.special, armorComponent.GetSpecialBaseSkillDef(), GenericSkill.SkillOverridePriority.Contextual);

            setSkills = true;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }
    }
}