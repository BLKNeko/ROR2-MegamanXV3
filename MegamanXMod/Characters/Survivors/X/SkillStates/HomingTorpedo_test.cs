using EntityStates;
using MegamanXMod.Modules.BaseStates;
using MegamanXMod.Survivors.X;
using MegamanXMod.Survivors.X.Components;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HomingTorpedo_test : BaseChargeSpecial
    {

        private HuntressTracker huntressTracker;
        private Transform modelTransform;
        private HurtBox initialOrbTarget;
        private ChildLocator childLocator;

        public override void OnEnter()
        {
            base.OnEnter();

            damageCoefficient = XStaticValues.HomingTorpedoDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 0.4f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 3f;
            range = 256f;

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "BusterMuzzPos";
            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/HitsparkCommandoFMJ");
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashBanditShotgun");

            modelTransform = base.GetModelTransform();
            huntressTracker = base.GetComponent<HuntressTracker>();


            if (this.huntressTracker && base.isAuthority)
            {
                this.initialOrbTarget = this.huntressTracker.GetTrackingTarget();
            }

            if (this.modelTransform)
            {
                this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
            }


        }

        public override void OnExit()
        {

            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        protected override void FireSimpleBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {

                    HomingTorpedo1 HT1 = new HomingTorpedo1();

                    HT1.SetTarget(this.initialOrbTarget);

                    //Debug.Log("HT OrbTarget:" + initialOrbTarget);

                    SetNextEntityState(HT1);

                }

            }
        }

        protected override void FireMediumBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {

                    HomingTorpedo2 HT2 = new HomingTorpedo2();

                    HT2.SetTarget(this.initialOrbTarget);

                    //Debug.Log("2HT OrbTarget:" + initialOrbTarget);

                    SetNextEntityState(HT2);

                }

            }
        }

        protected override void FireChargedBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                if (isAuthority)
                {

                    HomingTorpedo3 HT3 = new HomingTorpedo3();

                    HT3.SetTarget(this.initialOrbTarget);

                    //Debug.Log("3HT OrbTarget:" + initialOrbTarget);

                    SetNextEntityState(HT3);

                }

            }
        }

    }
}