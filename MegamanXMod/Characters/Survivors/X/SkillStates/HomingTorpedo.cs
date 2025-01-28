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
    public class HomingTorpedo : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.HomingTorpedoDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
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


            if (this.huntressTracker && base.isAuthority)
            {
                this.initialOrbTarget = this.huntressTracker.GetTrackingTarget();
            }

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

            //Util.PlaySound(Modules.Sounds.vileFragDrop, base.gameObject);


            //Util.PlaySound(Sounds.Play_M2UseSFX, base.gameObject);

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

        private void FireSimpleBullet()
        {
            if (!this.hasFired)
            {
                
                
                    this.hasFired = true;
                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    //Debug.Log("Create arrow:" + CreateArrowOrb());

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_VSFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_SFX, this.gameObject);

                    if (base.isAuthority)
                    {
                        if (NetworkServer.active)
                        {
                            GenericDamageOrb genericDamageOrb = this.CreateArrowOrb();
                            genericDamageOrb.damageValue = damageCoefficient * damageStat;
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
                            }

                            //Debug.Log("HurbBox:" + hurtBox);



                            base.characterBody.AddSpreadBloom(0.15f);
                            Ray aimRay = base.GetAimRay();
                            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString, false);
                        }
                    }

                    
                
                
            }
        }

        private void FireMediumBullet()
        {
            if (!this.hasFired)
            {
                
                    this.hasFired = true;
                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_VSFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_SFX, this.gameObject);

                    if (base.isAuthority)
                    {
                        if (NetworkServer.active)
                        {
                        GenericDamageOrb genericDamageOrb = this.CreateArrowOrb();
                        genericDamageOrb.damageValue = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                        genericDamageOrb.isCrit = RollCrit();
                        genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                        genericDamageOrb.attacker = base.gameObject;
                        genericDamageOrb.procCoefficient = procCoefficient;
                        genericDamageOrb.damageType = DamageType.Generic;
                        genericDamageOrb.damageColorIndex = DamageColorIndex.Default;
                        //genericDamageOrb.damageType = DamageType.ApplyMercExpose;

                        HurtBox hurtBox = this.initialOrbTarget;
                        if (hurtBox)
                        {
                            Transform transform = this.childLocator.FindChild(this.muzzleString);
                            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString, true);
                            genericDamageOrb.origin = transform.position;
                            genericDamageOrb.target = hurtBox;
                            OrbManager.instance.AddOrb(genericDamageOrb);
                            OrbManager.instance.AddOrb(genericDamageOrb);
                        }



                        base.characterBody.AddSpreadBloom(0.15f);
                        Ray aimRay = base.GetAimRay();
                        EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString, false);
                        }
                    }

                    
                }
                
            
        }

        private void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                
                    this.hasFired = true;
                    PlayAnimation("Gesture, Override", "XBusterChargeAttack", "attackSpeed", this.duration);

                    if (XConfig.enableVoiceBool.Value)
                    {
                        AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_VSFX, this.gameObject);
                    }
                    AkSoundEngine.PostEvent(XStaticValues.X_HomingTorpedo_SFX, this.gameObject);

                    if (base.isAuthority)
                    {
                        if (NetworkServer.active)
                        {
                            GenericDamageOrb genericDamageOrb = this.CreateArrowOrb();
                            genericDamageOrb.damageValue = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                            genericDamageOrb.isCrit = RollCrit();
                            genericDamageOrb.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                            genericDamageOrb.attacker = base.gameObject;
                            genericDamageOrb.procCoefficient = procCoefficient;
                            genericDamageOrb.damageType = DamageType.Generic;
                            genericDamageOrb.damageColorIndex = DamageColorIndex.Default;
                            //genericDamageOrb.damageType = DamageType.ApplyMercExpose;

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



                            base.characterBody.AddSpreadBloom(0.15f);
                            Ray aimRay = base.GetAimRay();
                            EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireRocket.effectPrefab, base.gameObject, this.muzzleString, false);
                        }
                    }

                    
                
                
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Carregar o ataque
            if (base.inputBank.skill4.down)
            {
                ChargeShot();
            }
            else // Soltar o ataque
            {
                ReleaseChargeShot();
            }



            if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill4.down) && chargeLevel == 3 && hasTime == true)
            {
                FireChargedBullet();
            }
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill4.down) && chargeLevel == 2 && hasTime == true)
            {
                FireMediumBullet();
            }
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill4.down) && chargeLevel == 1 && hasTime == true)
            {
                FireSimpleBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && hasTime == true)
            {
                hasTime = false;

                this.outer.SetNextStateToMain();
            }

        }

        //CHARGE LOGIC

        private void ChargeShot()
        {
            chargeTime += Time.fixedDeltaTime;
            base.characterBody.SetAimTimer(2f);

            if (chargeTime > Level1ChargeTime && chargeTime <= Level2ChargeTime && !chargingSFX)
            {
                PlayChargingEffects(1);
                chargingSFX = true;
            }

            if (chargeTime >= Level2ChargeTime && !chargeFullSFX)
            {
                PlayChargingEffects(2);
                chargeFullSFX = true;
                lastChargeTime = chargeTime;
            }

            if ((chargeTime - lastChargeTime) >= ChargeInterval && chargeFullSFX)
            {
                PlayChargingEffects(2);
                lastChargeTime = chargeTime;
            }
        }

        private void ReleaseChargeShot()
        {
            // Determina o nível de carregamento com base no tempo
            if (chargeTime >= Level2ChargeTime)
            {
                chargeLevel = 3; // Nível máximo de carregamento
            }
            else if (chargeTime >= Level1ChargeTime)
            {
                chargeLevel = 2; // Nível intermediário de carregamento
            }
            else
            {
                chargeLevel = 1; // Nível mínimo de carregamento
            }

            AkSoundEngine.PostEvent(21663534, base.gameObject);

            if (XConfig.enableVoiceBool.Value && !playedVSFX && chargeLevel == 3)
            {
                AkSoundEngine.PostEvent(XStaticValues.X_Attack_VSFX, this.gameObject);
                playedVSFX = true;
            }

            chargingSFX = false;
            chargeFullSFX = false;
            hasTime = true;
            //chargeTime = 0; // Reseta o tempo de carga - movido para o onexit

        }

        private void PlayChargingEffects(int level)
        {
            switch (level)
            {
                case 1:
                    //Util.PlaySound(XStaticValues.charging, base.gameObject);
                    AkSoundEngine.PostEvent(3358936867, this.gameObject);
                    EffectManager.SimpleMuzzleFlash(XAssets.Charge1VFX, base.gameObject, "CorePosition", true);
                    break;

                case 2:
                    //Util.PlaySound(XStaticValues.fullCharge, base.gameObject);
                    AkSoundEngine.PostEvent(992292707, this.gameObject);
                    EffectManager.SimpleMuzzleFlash(XAssets.Charge2VFX, base.gameObject, "CorePosition", true);
                    break;

                default:
                    break;
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