using RoR2;
using System;
using System.Collections;
using UnityEngine;

namespace MegamanXMod.Survivors.X.Components
{
    internal class XHoverComponent : MonoBehaviour
    {
        private Transform XmodelTransform;

        private Animator XAnim;

        private HealthComponent XHealth;

        private CharacterBody XBody;

        private float initialStoreTime, hoverTimer;
        private float timeBetweenBlink = 2f;
        private float hoverTimeLimit = 5f;

        private ChildLocator childLocator;

        public static float hoverVelocity = -2f;
        public static float hoverAcceleration = 0.5f;

        private bool shouldHover { get; set; }

        private string LDashPos = "LDashPos";
        private string RDashPos = "RDashPos";
        private string FWingR1 = "FWingR1";
        private string FWingR2 = "FWingR2";
        private string FWingR3 = "FWingR3";
        private string FWingL1 = "FWingL1";
        private string FWingL2 = "FWingL2";
        private string FWingL3 = "FWingL3";

        private GameObject EffectPrefab;

        private float timeLimit = 0.4f;
        private float timer;

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

            //EffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/Muzzleflash1");
            EffectPrefab = XAssets.FJetVFX;
            

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

            Hover();
            CheckGround();
        }

        public void SetHover(bool b)
        {
            shouldHover = b;
            hoverTimer = 0f;
        }

        public void SetChildLocator(ChildLocator cLocator)
        {
            childLocator = cLocator;
        }

        public bool GetHover()
        {
            return shouldHover;
        }

        private void CheckGround()
        {
            if (XBody.characterMotor.isGrounded)
            {
                shouldHover = false;
                hoverTimer = 0f;
            }
                
        }

        private void Hover()
        {
            if (shouldHover && hoverTimer < hoverTimeLimit)
            {
                float num = XBody.characterMotor.velocity.y;
                num = Mathf.MoveTowards(num, hoverVelocity, hoverAcceleration);
                XBody.characterMotor.velocity = new Vector3(XBody.characterMotor.velocity.x, num, XBody.characterMotor.velocity.z);
                hoverTimer += Time.fixedDeltaTime;

                timer += Time.fixedDeltaTime;

                if(timer > timeLimit)
                {
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, LDashPos, true);
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, RDashPos, true);
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, FWingR1, true);
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, FWingR2, true);
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, FWingR3, true);
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, FWingL1, true);
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, FWingL2, true);
                    EffectManager.SimpleMuzzleFlash(EffectPrefab, gameObject, FWingL3, true);

                    //EffectManager.SpawnEffect(EffectPrefab, new EffectData
                    //{
                    //    origin = childLocator.FindChild(FWingL3).position,
                    //    rotation = Util.QuaternionSafeLookRotation(Vector3.down),
                    //    scale = 8f,

                    //}, true);


                    timer = 0;
                }

                

            }
        }


    }
}