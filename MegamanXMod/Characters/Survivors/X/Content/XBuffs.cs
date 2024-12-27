using RoR2;
using UnityEngine;

namespace MegamanXMod.Survivors.X
{
    public static class XBuffs
    {
        // armor buff gained during roll
        public static BuffDef armorBuff;

        public static BuffDef LightArmorBuff;
        public static BuffDef SecondArmorBuff;
        public static BuffDef MaxArmorBuff;
        public static BuffDef FourthArmorBuff;
        public static BuffDef FalconArmorBuff;
        public static BuffDef GaeaArmorBuff;
        public static BuffDef ShadowArmorBuff;
        public static BuffDef UltimateArmorBuff;
        public static BuffDef RathalosArmorBuff;

        public static BuffDef GigaBusterChargeBuff;

        public static void Init(AssetBundle assetBundle)
        {
            armorBuff = Modules.Content.CreateAndAddBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite,
                Color.white,
                false,
                false);

            LightArmorBuff = Modules.Content.CreateAndAddBuff("LightArmorBuff",
                XAssets.IconLight,
                Color.white,
                false,
                false);

            SecondArmorBuff = Modules.Content.CreateAndAddBuff("GigaArmorBuff",
                XAssets.IconSecond,
                Color.white,
                false,
                false);

            MaxArmorBuff = Modules.Content.CreateAndAddBuff("MaxArmorBuff",
                XAssets.IconMax,
                Color.white,
                false,
                false);

            FourthArmorBuff = Modules.Content.CreateAndAddBuff("ForceArmorBuff",
                XAssets.IconFourth,
                Color.white,
                false,
                false);

            FalconArmorBuff = Modules.Content.CreateAndAddBuff("FalconArmorBuff",
                XAssets.IconFalcon,
                Color.white,
                false,
                false);

            GaeaArmorBuff = Modules.Content.CreateAndAddBuff("GaeaArmorBuff",
                XAssets.IconGaea,
                Color.white,
                false,
                false);

            ShadowArmorBuff = Modules.Content.CreateAndAddBuff("ShadowArmorBuff",
                XAssets.IconXShadow,
                Color.white,
                false,
                false);

            UltimateArmorBuff = Modules.Content.CreateAndAddBuff("UltimateArmorBuff",
                XAssets.IconUltimate,
                Color.white,
                false,
                false);

            RathalosArmorBuff = Modules.Content.CreateAndAddBuff("RathalosArmorBuff",
                XAssets.IconXRathalos,
                Color.white,
                false,
                false);

            GigaBusterChargeBuff = Modules.Content.CreateAndAddBuff("GigaBusterChargeBuff",
                XAssets.IconXRathalos,
                Color.white,
                false,
                false);

        }
    }
}
