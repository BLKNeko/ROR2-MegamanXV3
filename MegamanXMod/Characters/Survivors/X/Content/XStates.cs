using MegamanXMod.Modules.BaseContent.BaseStates;
using MegamanXMod.Survivors.X.SkillStates;

namespace MegamanXMod.Survivors.X
{
    public static class XStates
    {
        public static void Init()
        {

            Modules.Content.AddEntityState(typeof(CooldownXArmor));
            Modules.Content.AddEntityState(typeof(HyperModeLightArmor));
            Modules.Content.AddEntityState(typeof(HyperModeSecondArmor));
            Modules.Content.AddEntityState(typeof(HyperModeMaxArmor));
            Modules.Content.AddEntityState(typeof(HyperModeFourthArmor));
            Modules.Content.AddEntityState(typeof(HyperModeFalconArmor));
            Modules.Content.AddEntityState(typeof(HyperModeGaeaArmor));
            Modules.Content.AddEntityState(typeof(HyperModeShadowArmor));
            Modules.Content.AddEntityState(typeof(HyperModeUltimateArmor));
            Modules.Content.AddEntityState(typeof(HyperModeRathalosArmor));
            Modules.Content.AddEntityState(typeof(XBuster));
            Modules.Content.AddEntityState(typeof(XLightBuster));
            Modules.Content.AddEntityState(typeof(XGigaBuster));
            Modules.Content.AddEntityState(typeof(XMaxBuster));
            Modules.Content.AddEntityState(typeof(XForceBuster));
            Modules.Content.AddEntityState(typeof(XFalconBuster));
            Modules.Content.AddEntityState(typeof(XGaeaBuster));
            Modules.Content.AddEntityState(typeof(ShadowBuster));
            Modules.Content.AddEntityState(typeof(XUltimateBuster));
            Modules.Content.AddEntityState(typeof(XRathalosBuster));
            Modules.Content.AddEntityState(typeof(XSSlashCombo));
            Modules.Content.AddEntityState(typeof(XRSlashCombo));
            Modules.Content.AddEntityState(typeof(XShotgunIce));
            Modules.Content.AddEntityState(typeof(SqueezeBomb));
            Modules.Content.AddEntityState(typeof(XFireWave));
            Modules.Content.AddEntityState(typeof(XFireWave2));
            Modules.Content.AddEntityState(typeof(XFireWave3));
            Modules.Content.AddEntityState(typeof(XDash2));
            Modules.Content.AddEntityState(typeof(FalconDash));
            Modules.Content.AddEntityState(typeof(NovaDash));
            Modules.Content.AddEntityState(typeof(NovaStrike));
            Modules.Content.AddEntityState(typeof(HeadScanner));
            Modules.Content.AddEntityState(typeof(HyperChip));
            Modules.Content.AddEntityState(typeof(GigaAttackGaea));
            Modules.Content.AddEntityState(typeof(XRathalosSlashCombo1));
            Modules.Content.AddEntityState(typeof(XRathalosSlashCombo2));
            Modules.Content.AddEntityState(typeof(RisingFire));
            Modules.Content.AddEntityState(typeof(AcidBurst));
            Modules.Content.AddEntityState(typeof(ChameleonSting));
            Modules.Content.AddEntityState(typeof(LockArmor));
            Modules.Content.AddEntityState(typeof(MeltCreeper));

            Modules.Content.AddEntityState(typeof(XHeart));
            Modules.Content.AddEntityState(typeof(SpawnState));
            Modules.Content.AddEntityState(typeof(DeathState));

        }
    }
}
