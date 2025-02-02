using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;
using MegamanXMod.Modules;
using EmotesAPI;
using static Rewired.Utils.Classes.Utility.ObjectInstanceTracker;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.Components
{
    internal class XBaseComponent : MonoBehaviour
    {
        private Transform XmodelTransform;

        private Animator XAnim;

        private HealthComponent XHealth;

        private CharacterBody XBody;

        private bool isWeak;

        private bool giveExtraLife { get; set; }
        private bool HasUsedExtraLife { get; set; }

        private float minHpWeak, initialStoreTime;
        private float timeBetweenBlink = 2f;

        private GameObject blinkObject, blinkObject2;

        private ChildLocator childLocator;

        private FootstepHandler footstepHandler;

        private bool shoudlAplyCSBuffs = false;



        private void Start()
        {
            //any funny custom behavior you want here
            //for example, enforcer uses a component like this to change his guns depending on selected skill
            if(XBody == null)
            {
                XBody = GetComponent<CharacterBody>();
            }
            
            XHealth = XBody.GetComponent<HealthComponent>();

            XmodelTransform = XBody.transform;

            XAnim = XBody.characterDirection.modelAnimator;

            minHpWeak = 0.45f;

            childLocator = GetComponentInChildren<ChildLocator>();

            giveExtraLife = false;


            footstepHandler = XBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().GetComponent<FootstepHandler>();

            //Debug.Log("footstepHandler: " + footstepHandler);

            switch (XConfig.enableXFootstep.Value)
            {
                case 0:
                    footstepHandler.baseFootstepString = "";
                    footstepHandler.sprintFootstepOverrideString = "";
                    break;
                case 1:
                    footstepHandler.baseFootstepString = "Play_X_Footstep_SFX";
                    footstepHandler.sprintFootstepOverrideString = "Play_X_Footstep_SFX";
                    break;
                case 2:
                    footstepHandler.baseFootstepString = "Play_X_Footstep_X8_SFX";
                    footstepHandler.sprintFootstepOverrideString = "Play_X_Footstep_X8_SFX";
                    break;
                default:
                    footstepHandler.baseFootstepString = "";
                    footstepHandler.sprintFootstepOverrideString = "";
                    break;
            }


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

            //Debug.Log("Xbody: " + XBody.transform.localPosition);
            //Debug.Log("Xhurtbox: " + XBody.mainHurtBox.transform.localPosition);
            //Debug.Log("Xmodel: " + XBody.GetComponent<ModelLocator>().modelTransform.gameObject.GetComponent<CharacterModel>().transform.localPosition);

            if (shoudlAplyCSBuffs)
            {
                CSBuffs();
            }

            IsXWeak();
        }

        private void CSBuffs()
        {
            if (NetworkServer.active)
            {
                XBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 8f);
                XBody.AddTimedBuff(RoR2Content.Buffs.Intangible, 8f);
                XBody.AddTimedBuff(RoR2Content.Buffs.Cloak, 8f);
                XBody.AddTimedBuff(RoR2Content.Buffs.CloakSpeed, 8f);
            }

            shoudlAplyCSBuffs = false;
        }

        public void SetCSBuff(bool b)
        {
            shoudlAplyCSBuffs = b;
        }

        public CharacterBody GetXBody()
        {
            return XBody;
        }

        private void IsXWeak()
        {
            isWeak = XHealth.combinedHealthFraction < minHpWeak;

            XAnim.SetBool("isWeak", isWeak);
        }

        public void SetExtraLife(bool b)
        {
            giveExtraLife = b;
        }

        public bool GetExtraLife()
        {
            return giveExtraLife;
        }


    }
}