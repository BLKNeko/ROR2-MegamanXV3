using EntityStates;
using MegamanXMod.Modules;
using MegamanXMod.Survivors.X;
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class HomingTorpedo3 : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.HomingTorpedoDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 400f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerSmokeChase");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;
        private string muzzleString2;

        private HuntressTracker huntressTracker;
        private float stopwatch;
        private Transform modelTransform;
        private bool hasTriedToThrowDagger;
        private HurtBox initialOrbTarget;
        private ChildLocator childLocator;

        private const float Level1ChargeTime = 0.5f; // Tempo para ativar o primeiro nível de carregamento
        private const float Level2ChargeTime = 1.8f; // Tempo para ativar o segundo nível de carregamento
        private const float ChargeInterval = 0.68f;  // Intervalo entre efeitos de carregamento completo

        private float chargeTime = 0f;
        private float lastChargeTime = 0f;
        private bool chargeFullSFX = false;
        private bool hasTime = false;
        private int chargeLevel = 0;
        private bool chargingSFX = false;
        private bool playedVSFX = false;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "BusterMuzzPos";

            this.modelTransform = base.GetModelTransform();
            this.animator = base.GetModelAnimator();
            this.huntressTracker = base.GetComponent<HuntressTracker>();


            //if (this.huntressTracker && base.isAuthority)
            //{
            //    this.initialOrbTarget = this.huntressTracker.GetTrackingTarget();
            //}

            if (this.modelTransform)
            {
                this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
            }

            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.duration);
            }

            //Debug.Log("Tracker:" + huntressTracker);
            //Debug.Log("OrbTarget:" + initialOrbTarget);
            //Debug.Log("HT3 OrbTarget:" + initialOrbTarget);

            //Util.PlaySound(Modules.Sounds.vileFragDrop, base.gameObject);


            //Util.PlaySound(Sounds.Play_M2UseSFX, base.gameObject);

        }

        public void SetTarget(HurtBox target)
        {
            initialOrbTarget = target;
        }

        public override void OnExit()
        {
            playedVSFX = false;
            chargeTime = 0f;
            chargeLevel = 0;

            base.OnExit();
        }

        protected virtual GenericDamageOrb CreateArrowOrb()
        {
            return new HomingTorpedoOrb();
        }

        private void FireHT()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                if (NetworkServer.active)
                {

                    

                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    //Debug.Log("Create arrow:" + CreateArrowOrb());

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_VSFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_SFX, this.gameObject);

                    GenericDamageOrb genericDamageOrb = this.CreateArrowOrb();
                    genericDamageOrb.damageValue = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    genericDamageOrb.isCrit = RollCrit();
                    genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                    genericDamageOrb.attacker = base.gameObject;
                    genericDamageOrb.procCoefficient = procCoefficient;
                    genericDamageOrb.damageType = DamageType.Generic;
                    genericDamageOrb.damageColorIndex = DamageColorIndex.Default;

                    //genericDamageOrb.damageType = DamageType.ApplyMercExpose;

                    //Debug.Log("GenereciDamageOrb:" + genericDamageOrb);

                    HurtBox hurtBox = this.initialOrbTarget;
                    if (hurtBox)
                    {
                        Transform transform = this.childLocator.FindChild(this.muzzleString);
                        EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString, true);
                        genericDamageOrb.origin = transform.position;
                        genericDamageOrb.target = hurtBox;
                        OrbManager.instance.AddOrb(genericDamageOrb);
                        OrbManager.instance.AddOrb(genericDamageOrb);
                        OrbManager.instance.AddOrb(genericDamageOrb);
                        OrbManager.instance.AddOrb(genericDamageOrb);
                        OrbManager.instance.AddOrb(genericDamageOrb);
                    }

                    //Debug.Log("HurbBox3:" + hurtBox);



                    base.characterBody.AddSpreadBloom(0.15f);
                    Ray aimRay = base.GetAimRay();
                    EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString, false);
                }

                    
                
                
            }
        }

       

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (fixedAge >= fireTime)
            {
                FireHT();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                hasTime = false;
                this.outer.SetNextStateToMain();
                return;
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Frozen;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(this.initialOrbTarget));
        }

        // Token: 0x06000E6F RID: 3695 RVA: 0x0003E6A4 File Offset: 0x0003C8A4
        public override void OnDeserialize(NetworkReader reader)
        {
            this.initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }
    }
}