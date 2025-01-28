using RoR2;
using System.Collections;
using UnityEngine;
using BepInEx;
using MegamanXMod.Modules;

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


            IsXWeak();
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