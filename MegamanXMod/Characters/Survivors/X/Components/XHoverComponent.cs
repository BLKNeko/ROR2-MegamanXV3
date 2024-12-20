using RoR2;
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

            childLocator = GetComponentInChildren<ChildLocator>();


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
            }
        }


    }
}