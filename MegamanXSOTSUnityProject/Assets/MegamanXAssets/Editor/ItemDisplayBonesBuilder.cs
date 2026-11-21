using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// Adiciona os ossos do X ao ChildLocator do mdlX4 com os nomes padrão do RoR2 (Head, Chest, HandR...).
// Menu: MegamanX > Model > Add Item Display Bones
//
// Sem essas entradas o RoR2 não tem onde prender os item displays, e o ItemDisplayPlacementHelper
// não mostra nenhum osso para o X. Pode rodar quantas vezes quiser: nomes que já existem no
// ChildLocator são mantidos como estão (não duplica nem sobrescreve).
public static class ItemDisplayBonesBuilder
{
    private const string ModelPrefabPath = "Assets/MegamanXAssets/MegamanXBundle/mdlX4.prefab";

    // Nome no ChildLocator -> nome do osso no esqueleto do X
    private static readonly (string childName, string boneName)[] bones =
    {
        ("Head", "Bip Head"),
        ("Neck", "Bip Neck"),
        ("Chest", "Bip Spine1"),
        ("Stomach", "Bip Spine"),
        ("Pelvis", "Bip Pelvis"),
        ("ClavicleL", "Bip L Clavicle"),
        ("ClavicleR", "Bip R Clavicle"),
        ("UpperArmL", "Bip L UpperArm"),
        ("UpperArmR", "Bip R UpperArm"),
        ("LowerArmL", "Bip L Forearm"),
        ("LowerArmR", "Bip R Forearm"),
        ("HandL", "Bip L Hand"),
        ("HandR", "Bip R Hand"),
        ("ThighL", "Bip L Thigh"),
        ("ThighR", "Bip R Thigh"),
        ("CalfL", "Bip L Calf"),
        ("CalfR", "Bip R Calf"),
        ("FootL", "Bip L Foot"),
        ("FootR", "Bip R Foot"),
    };

    [MenuItem("MegamanX/Model/Add Item Display Bones")]
    [MenuItem("Tools/MegamanX/Add Item Display Bones")]
    [MenuItem("Assets/MegamanX/Add Item Display Bones")]
    public static void Build()
    {
        GameObject root = PrefabUtility.LoadPrefabContents(ModelPrefabPath);
        if (!root)
        {
            EditorUtility.DisplayDialog("Item Display Bones", "Prefab não encontrado: " + ModelPrefabPath, "OK");
            return;
        }

        try
        {
            ChildLocator locator = root.GetComponentInChildren<ChildLocator>(true);
            if (!locator)
            {
                EditorUtility.DisplayDialog("Item Display Bones", "O mdlX4 não tem ChildLocator.", "OK");
                return;
            }

            var pairs = new List<ChildLocator.NameTransformPair>(locator.transformPairs ?? new ChildLocator.NameTransformPair[0]);
            var existing = new HashSet<string>(pairs.Select(p => p.name));
            Transform[] all = root.GetComponentsInChildren<Transform>(true);

            var added = new List<string>();
            var skipped = new List<string>();
            var missing = new List<string>();

            foreach (var (childName, boneName) in bones)
            {
                if (existing.Contains(childName))
                {
                    skipped.Add(childName);
                    continue;
                }

                Transform bone = all.FirstOrDefault(t => t.name == boneName);
                if (!bone)
                {
                    missing.Add($"{childName} ({boneName})");
                    continue;
                }

                pairs.Add(new ChildLocator.NameTransformPair { name = childName, transform = bone });
                existing.Add(childName);
                added.Add(childName);
            }

            locator.transformPairs = pairs.ToArray();
            PrefabUtility.SaveAsPrefabAsset(root, ModelPrefabPath);

            string report =
                $"Adicionados ({added.Count}): {(added.Count > 0 ? string.Join(", ", added) : "-")}\n\n" +
                $"Já existiam ({skipped.Count}): {(skipped.Count > 0 ? string.Join(", ", skipped) : "-")}\n\n" +
                $"Ossos não encontrados ({missing.Count}): {(missing.Count > 0 ? string.Join(", ", missing) : "-")}";

            Debug.Log("[ItemDisplayBones] " + report.Replace("\n\n", " | "));
            EditorUtility.DisplayDialog("Item Display Bones", report, "OK");
        }
        finally
        {
            PrefabUtility.UnloadPrefabContents(root);
        }
    }
}
