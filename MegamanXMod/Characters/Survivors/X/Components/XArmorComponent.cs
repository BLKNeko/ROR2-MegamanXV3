using RoR2;
using System.Collections;
using UnityEngine;
using ExtraSkillSlots;
using RoR2.Skills;
using UnityEngine.Networking;
using System.Xml.Linq;

namespace MegamanXMod.Survivors.X.Components
{
    internal class XArmorComponent : MonoBehaviour
    {
        private Transform XmodelTransform;

        private Animator XAnim;

        private HealthComponent XHealth;

        private CharacterBody XBody;

        private CharacterModel XModel;

        private bool isWeak;

        private bool secondArmorUnlock = false;
        private bool thirdArmorUnlock = false;
        private bool fourthArmorUnlock = false;

        private float minHpWeak, initialStoreTime;
        private float timeBetweenBlink = 2f;

        private GameObject blinkObject, blinkObject2;

        private ChildLocator childLocator;

        private ExtraSkillLocator extraskillLocator;

        private SkillDef ArmorSkill1, ArmorSkill2, ArmorSkill3, ArmorSkill4, BaseSkill2, BaseSkill3, BaseSkill4;

        TemporaryOverlayInstance temporaryOverlayInstance;

        private SkinnedMeshRenderer XmeshRenderer;




        private void Start()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
            if(XBody == null)
            {
                XBody = GetComponent<CharacterBody>();
            }

            extraskillLocator = base.GetComponent<ExtraSkillLocator>();

            XHealth = XBody.GetComponent<HealthComponent>();

            XmodelTransform = XBody.transform;

            XAnim = XBody.characterDirection.modelAnimator;

            minHpWeak = 0.3f;

            childLocator = GetComponentInChildren<ChildLocator>();

            temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(XBody.modelLocator.gameObject);

            if (ArmorSkill1 == null)
                ArmorSkill1 = extraskillLocator.extraFirst.skillDef;

            if (ArmorSkill2 == null)
                ArmorSkill2 = extraskillLocator.extraSecond.skillDef;


            if (ArmorSkill3 == null)
                ArmorSkill3 = extraskillLocator.extraThird.skillDef;


            if (ArmorSkill4 == null)
                ArmorSkill4 = extraskillLocator.extraFourth.skillDef;

            if (BaseSkill2 == null)
                BaseSkill2 = XBody.skillLocator.secondary.skillDef;

            if (BaseSkill3 == null)
                BaseSkill3 = XBody.skillLocator.utility.skillDef;

            if (BaseSkill4 == null)
                BaseSkill4 = XBody.skillLocator.special.skillDef;


        }

        private void Awake()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
            //XAnim = this.GetComponent<Animator>();
            //XHealth = this.GetComponent<HealthComponent>();

        }

        void FixedUpdate()
        {
            //Debug.Log("isWeak: " + isWeak);
            //Debug.Log("XAnim: " + XAnim);
            //Debug.Log("XHealth: " + XHealth);
            //Debug.Log("XBody: " + XBody);


            FalconDashReset();
            ShouldRemoveHyperChipBuff();
            //ShouldRemoveGoldTexture();
            //SholdApplyGoldenArmorTexture();

            if (!secondArmorUnlock)
                SecondArmorLockCheck();

            if (!thirdArmorUnlock)
                ThirdArmorLockCheck();

            if (!fourthArmorUnlock)
                FourthArmorLockCheck();
        }

        private void SecondArmorLockCheck()
        {
            if (XBody.level < XConfig.secondArmorSlotLvl.Value)
            {
                extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, XSurvivor.LockArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            else
            {
                UnsetAllExtraSecondSkills();
                extraskillLocator.extraSecond.SetSkillOverride(extraskillLocator.extraSecond, ArmorSkill2, GenericSkill.SkillOverridePriority.Contextual);
                secondArmorUnlock = true;
            }
        }

        private void ThirdArmorLockCheck()
        {
            if (XBody.level < XConfig.thirdArmorSlotLvl.Value)
            {
                extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, XSurvivor.LockArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            else
            {
                UnsetAllExtraThirdSkills();
                extraskillLocator.extraThird.SetSkillOverride(extraskillLocator.extraThird, ArmorSkill3, GenericSkill.SkillOverridePriority.Contextual);
                thirdArmorUnlock = true;
            }
        }

        private void FourthArmorLockCheck()
        {
            if (XBody.level < XConfig.fourthArmorSlotLvl.Value)
            {
                extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, XSurvivor.LockArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            }
            else
            {
                UnsetAllExtraFourthSkills();
                extraskillLocator.extraFourth.SetSkillOverride(extraskillLocator.extraFourth, ArmorSkill4, GenericSkill.SkillOverridePriority.Contextual);
                fourthArmorUnlock = true;
            }
        }

        private void FalconDashReset()
        {
            if (XBody.characterMotor.isGrounded && XBody.skillLocator.utility.skillDef == XSurvivor.XFalconDashSkillDef && !XBody.inputBank.skill3.down)
            {
                XBody.skillLocator.utility.stock = XBody.skillLocator.utility.maxStock;
            }
        }

        private void ShouldRemoveHyperChipBuff()
        {
            if(!XBody.HasBuff(XBuffs.MaxArmorBuff) && XBody.HasBuff(XBuffs.HyperChipBuff))
            {
                if (NetworkServer.active)
                {
                    //XBody.RemoveBuff(XBuffs.HyperChipBuff);
                    XBody.RemoveOldestTimedBuff(XBuffs.HyperChipBuff);
                    //Debug.Log("REMOVE GOLD BUFF");
                }
            }
        }

        private void ShouldRemoveGoldTexture()
        {

            if (NetworkServer.active)
            {

                if (XBody.HasBuff(XBuffs.MaxArmorBuff) && !XBody.HasBuff(XBuffs.HyperChipBuff))
                {
                    if (XModel && XmeshRenderer)
                    {
                        if (XModel.baseRendererInfos[0].defaultMaterial == XAssets.MatMaxGold)
                        {
                            XmeshRenderer.sharedMaterial = XAssets.MatMax;
                            XModel.baseRendererInfos[0].defaultMaterial = XAssets.MatMax;
                        }
                    }
                }
                RPCShouldRemoveGoldTexture();

            }

            
        }

        [ClientRpc]
        private void RPCShouldRemoveGoldTexture()
        {

            if (NetworkServer.active)
            {

                if (XBody.HasBuff(XBuffs.MaxArmorBuff) && !XBody.HasBuff(XBuffs.HyperChipBuff))
                {
                    if (XModel && XmeshRenderer)
                    {
                        if (XModel.baseRendererInfos[0].defaultMaterial == XAssets.MatMaxGold)
                        {
                            XmeshRenderer.sharedMaterial = XAssets.MatMax;
                            XModel.baseRendererInfos[0].defaultMaterial = XAssets.MatMax;
                            
                        }
                    }
                }

            }


        }

        public void SetXModel(CharacterModel model)
        {
            XModel = model;
        }

        public void SetXMeshRender(SkinnedMeshRenderer render)
        {
            XmeshRenderer = render;
        }



        //private void SholdApplyGoldenArmorTexture()
        //{
        //    if (XBody)
        //    {
        //        if (XBody.HasBuff(XBuffs.MaxArmorBuff) && XBody.HasBuff(XBuffs.HyperChipBuff))
        //        {

        //            if (XBody.modelLocator)
        //            {
        //                temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(XBody.modelLocator.gameObject);
        //                temporaryOverlayInstance.duration = 20f;
        //                temporaryOverlayInstance.animateShaderAlpha = true;
        //                temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        //                temporaryOverlayInstance.destroyComponentOnEnd = true;
        //                //temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
        //                temporaryOverlayInstance.originalMaterial = XAssets.MatMaxGold;
        //                temporaryOverlayInstance.inspectorCharacterModel = XBody.modelLocator.GetComponent<CharacterModel>();
        //                temporaryOverlayInstance.AddToCharacterModel(XBody.modelLocator.GetComponent<CharacterModel>());

        //                //XmodelTransform.GetComponent<CharacterModel>().AddTempOverlay(temporaryOverlayInstance);
        //                //XmodelTransform.GetComponent<CharacterModel>().UpdateOverlays();
        //                //XmodelTransform.GetComponent<CharacterModel>().UpdateOverlayStates();

        //                Debug.Log("ADD OVERLAY");
        //            }



        //        }
        //        else
        //        {
        //            //temporaryOverlayInstance.CleanupEffect();
        //            if (XBody.modelLocator)
        //            {
        //                //XmodelTransform.GetComponent<CharacterModel>().temporaryOverlays.Clear();
        //                Debug.Log("Cleanup");
        //            }

        //        }
        //    }

        //}

        //public void ApplyGoldTexture(CharacterModel characterModel)
        //{

        //    XModel = characterModel;

        //    temporaryOverlayInstance = TemporaryOverlayManager.AddOverlay(characterModel.gameObject);
        //    temporaryOverlayInstance.duration = 20f;
        //    temporaryOverlayInstance.animateShaderAlpha = true;
        //    temporaryOverlayInstance.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
        //    temporaryOverlayInstance.destroyComponentOnEnd = true;
        //    //temporaryOverlayInstance.originalMaterial = LegacyResourcesAPI.Load<Material>("Materials/matHuntressFlashBright");
        //    temporaryOverlayInstance.originalMaterial = XAssets.MatMaxGold;
        //    //temporaryOverlayInstance.originalMaterial = XSurvivor.instance.assetBundle.LoadAsset<Material>("matMaxG");
        //    temporaryOverlayInstance.inspectorCharacterModel = characterModel;
        //    temporaryOverlayInstance.AddToCharacterModel(characterModel);

        //    Debug.Log("Overlay Mat: " + temporaryOverlayInstance.originalMaterial);
        //    Debug.Log("Overlay: " + temporaryOverlayInstance);
        //    Debug.Log("Overlay assing charmodel: " + temporaryOverlayInstance.assignedCharacterModel);
        //    Debug.Log("Overlay initialized: " + temporaryOverlayInstance.initialized);
        //    Debug.Log("Overlay is assigned: " + temporaryOverlayInstance.isAssigned);
        //    Debug.Log("Overlay transmit: " + temporaryOverlayInstance.transmit);
        //    Debug.Log("Overlay GO: " + temporaryOverlayInstance.gameObject);

        //}


        public void RemoveArmorBuffs()
        {

            if (XBody.HasBuff(XBuffs.LightArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.LightArmorBuff);
                }   
            }

            if (XBody.HasBuff(XBuffs.SecondArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.SecondArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.MaxArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.MaxArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.MaxArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.MaxArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.FourthArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.FourthArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.FalconArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.FalconArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.GaeaArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.GaeaArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.ShadowArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.ShadowArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.UltimateArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.UltimateArmorBuff);
                }
            }

            if (XBody.HasBuff(XBuffs.RathalosArmorBuff))
            {
                if (NetworkServer.active)
                {
                    XBody.RemoveBuff(XBuffs.RathalosArmorBuff);
                }
            }




        }

        public void UnsetAllExtraFirstSkills()
        {
            //UNSET ALL FIRST SKILL
            extraskillLocator.extraFirst.UnsetSkillOverride(extraskillLocator.extraFirst, XSurvivor.CoolDownXArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFirst.UnsetSkillOverride(extraskillLocator.extraFirst, XSurvivor.HyperModeLightArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFirst.UnsetSkillOverride(extraskillLocator.extraFirst, XSurvivor.HyperModeSecondArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);

            
        }

        public void UnsetAllExtraSecondSkills()
        {
            //UNSET ALL SECOND SKILL
            extraskillLocator.extraSecond.UnsetSkillOverride(extraskillLocator.extraSecond, XSurvivor.LockArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraSecond.UnsetSkillOverride(extraskillLocator.extraSecond, XSurvivor.CoolDownXArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraSecond.UnsetSkillOverride(extraskillLocator.extraSecond, XSurvivor.HyperModeMaxArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraSecond.UnsetSkillOverride(extraskillLocator.extraSecond, XSurvivor.HyperModeFourthArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }


        public void UnsetAllExtraThirdSkills()
        {
            //UNSET ALL THIRD SKILL
            extraskillLocator.extraThird.UnsetSkillOverride(extraskillLocator.extraThird, XSurvivor.LockArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraThird.UnsetSkillOverride(extraskillLocator.extraThird, XSurvivor.CoolDownXArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraThird.UnsetSkillOverride(extraskillLocator.extraThird, XSurvivor.HyperModeFalconArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraThird.UnsetSkillOverride(extraskillLocator.extraThird, XSurvivor.HyperModeGaeaArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public void UnsetAllExtraFourthSkills()
        {
            //UNSET ALL FOURTH SKILL
            extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, XSurvivor.LockArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, XSurvivor.CoolDownXArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, XSurvivor.HyperModeShadowArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, XSurvivor.HyperModeUltimateArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            extraskillLocator.extraFourth.UnsetSkillOverride(extraskillLocator.extraFourth, XSurvivor.HyperModeRathalosArmorSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public void UnsetAllPrimarySkills()
        {
            //UNSET ALL PRIMARY SKILL
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XLightBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XGigaBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XMaxBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XForceBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XFalconBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XGaeaBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XShadowBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XUltimateBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.primary.UnsetSkillOverride(XBody.skillLocator.primary, XSurvivor.XRathalosBusterSkillDef, GenericSkill.SkillOverridePriority.Contextual);
        }

        public void UnsetAllSecondarySkills()
        {
            //UNSET ALL SECONDARY SKILL
            XBody.skillLocator.secondary.UnsetSkillOverride(XBody.skillLocator.secondary, XSurvivor.XShadowSaberSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.secondary.UnsetSkillOverride(XBody.skillLocator.secondary, XSurvivor.XRathalosSaberSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.secondary.UnsetSkillOverride(XBody.skillLocator.secondary, XSurvivor.XShotgunIceSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.secondary.UnsetSkillOverride(XBody.skillLocator.secondary, XSurvivor.XSqueezeBombSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.secondary.UnsetSkillOverride(XBody.skillLocator.secondary, XSurvivor.XFireWaveSkillDef, GenericSkill.SkillOverridePriority.Contextual);

        }

        public void UnsetAllUtilitySkills()
        {
            //UNSET ALL UTILITY SKILL
            XBody.skillLocator.utility.UnsetSkillOverride(XBody.skillLocator.utility, XSurvivor.XDashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.utility.UnsetSkillOverride(XBody.skillLocator.utility, XSurvivor.XFalconDashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.utility.UnsetSkillOverride(XBody.skillLocator.utility, XSurvivor.XNovaDashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.utility.UnsetSkillOverride(XBody.skillLocator.utility, XSurvivor.XNovaStrikeSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            
        }

        public void UnsetAllSpecialSkills()
        {
            //UNSET ALL SPECIAL SKILL
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XHeadScannerSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XHyperChipSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XGaeaGigaAttackSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XRathalosSlashSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XRisingFireSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XAcidBurstSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XChameleonStingSkillDef, GenericSkill.SkillOverridePriority.Contextual);
            XBody.skillLocator.special.UnsetSkillOverride(XBody.skillLocator.special, XSurvivor.XMeltCreeperSkillDef, GenericSkill.SkillOverridePriority.Contextual);

        }

        public SkillDef GetPrimaryArmorSkillDef()
        {
            return ArmorSkill1;
        }

        public SkillDef GetSecondaryArmorSkillDef()
        {
            return ArmorSkill2;
        }

        public SkillDef GetThirdArmorSkillDef()
        {
            return ArmorSkill3;
        }

        public SkillDef GetFourthArmorSkillDef()
        {
            return ArmorSkill4;
        }

        public SkillDef GetSecondaryBaseSkillDef()
        {
            return BaseSkill2;
        }

        public SkillDef GetUtilityBaseSkillDef()
        {
            return BaseSkill3;
        }

        public SkillDef GetSpecialBaseSkillDef()
        {
            return BaseSkill4;
        }

    }
}