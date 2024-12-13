using RoR2;
using System.Collections;
using UnityEngine;
using ExtraSkillSlots;
using RoR2.Skills;

namespace MegamanXMod.Survivors.X.Components
{
    internal class XArmorComponent : MonoBehaviour
    {
        private Transform XmodelTransform;

        private Animator XAnim;

        private HealthComponent XHealth;

        private CharacterBody XBody;

        private bool isWeak;

        private float minHpWeak, initialStoreTime;
        private float timeBetweenBlink = 2f;

        private GameObject blinkObject, blinkObject2;

        private ChildLocator childLocator;

        private ExtraSkillLocator extraskillLocator;

        private SkillDef ArmorSkill1, ArmorSkill2, ArmorSkill3, ArmorSkill4;




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

            if (ArmorSkill1 == null)
                ArmorSkill1 = extraskillLocator.extraFirst.skillDef;

            if (ArmorSkill2 == null)
                ArmorSkill2 = extraskillLocator.extraSecond.skillDef;


            if (ArmorSkill3 == null)
                ArmorSkill3 = extraskillLocator.extraThird.skillDef;


            if (ArmorSkill4 == null)
                ArmorSkill4 = extraskillLocator.extraFourth.skillDef;


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
        }

        private void FalconDashReset()
        {
            if (XBody.characterMotor.isGrounded && XBody.skillLocator.utility.skillDef == XSurvivor.XFalconDashSkillDef && !XBody.inputBank.skill3.down)
            {
                XBody.skillLocator.utility.stock = XBody.skillLocator.utility.maxStock;
            }
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


    }
}