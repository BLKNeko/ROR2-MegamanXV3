using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace MegamanXMod.Survivors.X
{
    // Item displays do X (config "Item Displays > Show Item Displays", desligada por padrão).
    // As posições NÃO foram configuradas para o X: usamos as do Commando.
    //
    // Copia todas as regras de item display de um sobrevivente vanilla para o X, trocando os ossos
    // pelos nomes do ChildLocator do X (Head, Chest, HandR...). Assim, no ItemDisplayPlacementHelper,
    // cada item já aparece com o modelo certo e preso no osso certo; falta só ajustar posição, ângulo e escala.
    //
    // As posições vêm do outro personagem e muitas ficam fora do lugar no X (esqueleto com outra escala e outros eixos).
    // Se um dia quiser posicionar: ligue a opção, ajuste no ItemDisplayPlacementHelper e exporte o .idrsjson.
    public static class XItemDisplayTemplate
    {
        public const string templateBody = "CommandoBody";

        // Nomes do ChildLocator do Commando -> nomes do ChildLocator do X
        private static readonly Dictionary<string, string> childMap = new Dictionary<string, string>
        {
            { "Head", "Head" },
            { "Neck", "Neck" },
            { "Chest", "Chest" },
            { "Stomach", "Stomach" },
            { "Pelvis", "Pelvis" },
            { "UpperArmL", "UpperArmL" },
            { "UpperArmR", "UpperArmR" },
            { "LowerArmL", "LowerArmL" },
            { "LowerArmR", "LowerArmR" },
            { "HandL", "HandL" },
            { "HandR", "HandR" },
            { "ThighL", "ThighL" },
            { "ThighR", "ThighR" },
            { "CalfL", "CalfL" },
            { "CalfR", "CalfR" },
            { "FootL", "FootL" },
            { "FootR", "FootR" },
            // Pontos que o X não tem: manda para o osso mais próximo
            { "GunL", "HandL" },
            { "GunR", "HandR" },
            { "MuzzleLeft", "HandL" },
            { "MuzzleRight", "HandR" },
            { "Base", "Pelvis" },
            { "Root", "Pelvis" },
        };
        private const string fallbackChild = "Chest";

        public static void Apply(ItemDisplayRuleSet target)
        {
            if (!target)
                return;

            GameObject templatePrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/CharacterBodies/" + templateBody);
            ModelLocator modelLocator = templatePrefab ? templatePrefab.GetComponent<ModelLocator>() : null;
            CharacterModel templateModel = modelLocator && modelLocator.modelTransform ? modelLocator.modelTransform.GetComponent<CharacterModel>() : null;
            ItemDisplayRuleSet source = templateModel ? templateModel.itemDisplayRuleSet : null;

            if (!source || source.keyAssetRuleGroups == null)
            {
                Log.Error($"[XItemDisplayTemplate] Não achei o ItemDisplayRuleSet do {templateBody}.");
                return;
            }

            var groups = new List<ItemDisplayRuleSet.KeyAssetRuleGroup>();
            int ruleCount = 0;

            foreach (ItemDisplayRuleSet.KeyAssetRuleGroup sourceGroup in source.keyAssetRuleGroups)
            {
                ItemDisplayRule[] sourceRules = sourceGroup.displayRuleGroup.rules;
                if (sourceRules == null || sourceRules.Length == 0)
                    continue;

                var rules = new List<ItemDisplayRule>();
                foreach (ItemDisplayRule sourceRule in sourceRules)
                {
                    // Regras de "LimbMask" escondem partes do corpo do Commando; não fazem sentido no X
                    if (sourceRule.ruleType != ItemDisplayRuleType.ParentedPrefab)
                        continue;

                    ItemDisplayRule rule = sourceRule; // struct: cópia
                    rule.childName = childMap.TryGetValue(sourceRule.childName ?? "", out string xChild) ? xChild : fallbackChild;
                    rules.Add(rule);
                }

                if (rules.Count == 0)
                    continue;

                ItemDisplayRuleSet.KeyAssetRuleGroup group = sourceGroup; // struct: cópia
                group.displayRuleGroup = new DisplayRuleGroup { rules = rules.ToArray() };
                groups.Add(group);
                ruleCount += rules.Count;
            }

            target.keyAssetRuleGroups = groups.ToArray();
            Log.Info($"[XItemDisplayTemplate] {groups.Count} itens / {ruleCount} regras copiadas do {templateBody} para o X (modo de edição).");
        }
    }
}
