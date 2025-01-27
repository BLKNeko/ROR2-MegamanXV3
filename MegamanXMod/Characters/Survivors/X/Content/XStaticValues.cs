using MegamanXMod.Survivors.X.SkillStates;
using System;

namespace MegamanXMod.Survivors.X
{
    public static class XStaticValues
    {
        public const float swordDamageCoefficient = 2.8f;

        public const float gunDamageCoefficient = 4.2f;

        public const float bombDamageCoefficient = 16f;

        public static readonly float XMidChargeDamageCoefficient = XConfig.midChargeMultiplierFloat.Value;

        public static readonly float XFullChargeDamageCoefficient = XConfig.fullChargeMultiplierFloat.Value;

        public const float XBusterDamageCoefficient = 1f;
        
        public const float XLightBusterDamageCoefficient = 1.25f;
        
        public const float XGigaBusterDamageCoefficient = 1.3f;
        
        public const float XMaxBusterDamageCoefficient = 2f;
        
        public const float XForceBusterDamageCoefficient = 2.5f;
        
        public const float XFalconBusterDamageCoefficient = 1.2f;
        
        public const float XGaeaBusterDamageCoefficient = 2f;
        
        public const float ShadowBusterDamageCoefficient = 2.5f;
        
        public const float XUltimateBusterDamageCoefficient = 4f;
        
        public const float XRathalosBusterDamageCoefficient = 3.5f;
        
        public const float XShadowSlashComboDamageCoefficient = 3f;
        
        public const float XSSlashComboDamageCoefficient = 3f;
        
        public const float XRSlashComboDamageCoefficient = 4f;
        
        public const float XShotgunIceDamageCoefficient = 4f;
                
        public const float XFireWaveDamageCoefficient = 0.1f;
                        
        public const float NovaDashDamageCoefficient = 5f;
        
        public const float NovaStrikeDamageCoefficient = 8f;
        
        public const float GigaAttackGaeaDamageCoefficient = 1.5f;
        
        public const float XRathalosSlashCombo1DamageCoefficient = 5f;
        
        public const float XRathalosSlashCombo2DamageCoefficient = 5f;
        
        public const float RisingFireDamageCoefficient = 4f;
        
        public const float AcidBurstDamageCoefficient = 1.5f;
        
        public const float ChameleonStingDamageCoefficient = 2f;
        
        public const float MeltCreeperDamageCoefficient = 1f;

        public const float HomingTorpedoDamageCoefficient = 1.2f;

        public const float XShockSphereDamageCoefficient = 0.25f;


        //SOUND STRINGS

        public static readonly string X_die_VSFX = "X_Die";
	    public static readonly string X_shotgunIce_VSFX = "X_ShotgunIce";
	    public static readonly string X_MaxBusterChargeShot_SFX = "Play_X_MaxBusterChargeShot_SFX";
	    public static readonly string X_Rathalos_Swing_SFX = "Play_X_Rathalos_Swing_SFX";
	    public static readonly string X_Dash_SFX = "X_Dash";
	    public static readonly string X_Charge_Shot = "X_ChargeShot";
	    public static readonly string fullCharge = "X_FullCharged";
	    public static readonly string X_HomingTorpedo_VSFX = "X_HomingTorpedo";
	    public static readonly string X_FireWave_SFX = "X_FireWaveSFX";
	    public static readonly string X_Ready = "Play_XReady";
	    public static readonly string X_Simple_Bullet = "Play_X_Simple_Bullet";
	    public static readonly string X_Passive_VSFX = "X_XPassive";
	    public static readonly string X_RathalosBusterSimple_SFX = "Play_X_RathalosBuster_S";
	    public static readonly string X_RathalosBusterCharge_SFX = "Play_X_RathalosBuster_C";
	    public static readonly string X_NovaStrike_SFX = "Play_X_NovaStrike_SFX";
	    public static readonly string X_Mid_Bullet = "X_MidBullet";
	    public static readonly string X_squeezeBomb_VSFX = "X_SqueezeBomb";
	    public static readonly string X_FireWave_VSFX = "X_FireWave";
	    public static readonly string X_Attack_VSFX = "X_XAttack";
	    public static readonly string X_Squeezebomb_SFX = "Play_X_Squeezebomb_SFX";
	    public static readonly string X_GoldenArmor_SFX = "Play_X_GoldenArmor_SFX";
	    public static readonly string charging = "X_Charging";
	    public static readonly string X_meltCreeper_VSFX = "Play_X_MeltCreeper_VSFX";
        public static readonly string X_ChameleonSting_VSFX = "Play_ChameleonSting";
        public static readonly string X_Slash2_SFX   = "Play_X_Slash2_SFX";
        public static readonly string X_Slash3_SFX = "Play_X_Slash3_SFX";
        public static readonly string X_LightBuster_C = "Play_X_LightBuster_C";
        public static readonly string X_HyperMode_SFX = "Play_X_Hypermode2_SFX";
        public static readonly string X_HMCooldown_SFX = "Play_X_HMCooldown2_SFX";
        public static readonly string X_Head_SFX = "Play_X_Head_SFX";
        public static readonly string X_GAGaea = "Play_X_GAGaea";
        public static readonly string X_Falcon_Dash = "Play_X_Falcon_Dash";
        public static readonly string X_RisingFire_SFX = "Play_X_RisingFire_SFX";
        public static readonly string X_RisingFireCharged_SFX = "Play_X_RisingFireCharged_SFX";
        public static readonly string X_AcidBurst_SFX = "Play_X_AcidBurst_SFX";
        public static readonly string X_HomingTorpedo_SFX = "Play_X_HomingTorpedo_SFX";
        public static readonly string X_ChameleonSting_SFX = "Play_X_ChameleonSting_SFX";
        public static readonly string X_Error_SFX = "Play_X_Error_SFX";

        public static readonly string xFootstep = "Play_X_Footstep_SFX";
        
        
        public static readonly string xFullPower = "CallXFullPower";
        
        public static readonly string xHurt = "CallXHurt";
        
        
        public static readonly string XPassive = "CallXPassive";


        

    }
}