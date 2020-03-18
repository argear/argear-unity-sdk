using UnityEngine;
using System.Collections;
using UnityEditor;
public static class ExportPackage {
    [MenuItem("Export/Export with tags and layers, Input settings")]
public static void export()
    {
        string[] projectContent = new string[] {
                // "Assets", 
                "Assets/ARGear",
                "Assets/Plugins",
                "ProjectSettings/TagManager.asset",
                "ProjectSettings/InputManager.asset",
                "ProjectSettings/ProjectSettings.asset"
                };
        AssetDatabase.ExportPackage(projectContent, "argear.unitypackage",
                        ExportPackageOptions.Interactive | ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies);
        Debug.Log("Project Exported");
    }
}