using EntityStates;
using MegamanXMod.Modules.BaseStates;
using MegamanXMod.Survivors.X;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XFireWave : BaseChargeSecondary
    {

        public override void OnEnter()
        {
            base.OnEnter();
            damageCoefficient = XStaticValues.XFireWaveDamageCoefficient;
            procCoefficient = 1f;
            baseDuration = 0.1f;
            firePercentTime = 0.0f;
            force = 400f;
            recoil = 3f;
            range = 256f;
            muzzleString = "BusterMuzzPos";

            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(1f);
            hitEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/ImpactEffects/HitsparkCommandoFMJ");
            muzzleEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/MuzzleFlashes/MuzzleflashFMJ");



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
            XFireWave2 xFireWave2 = new XFireWave2();
            SetNextEntityState(xFireWave2);
        }

        protected override void FireMediumBullet()
        {
            XFireWave2 xFireWave2 = new XFireWave2();
            SetNextEntityState(xFireWave2);
        }

        protected override void FireChargedBullet()
        {
            XFireWave3 xFireWave3 = new XFireWave3();
            SetNextEntityState(xFireWave3);
        }


    }
}