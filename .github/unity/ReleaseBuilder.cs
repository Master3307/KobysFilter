using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ReleaseBuilder
{
    public static void Build()
    {
        try
        {
            const string modPath = "Assets/KobysFilter";
            const string settingsPath = modPath + "/KobysFilter.asset";
            var settings = AssetDatabase.LoadAssetAtPath<ModBuilderSettings>(settingsPath);
            if (settings == null)
                throw new Exception("ModBuilderSettings not found at " + settingsPath);

            settings.modPath = modPath;
            settings.buildBundles = true;
            settings.buildLinux = true;
            settings.cacheBundles = false;
            settings.forceReimport = true;

            string output = Path.GetFullPath("ReleaseBuild");
            Directory.CreateDirectory(output);
            bool succeeded = false;
            PugMod.ModBuilder.BuildMod(settings, output, success => succeeded = success);
            if (!succeeded)
                throw new Exception("Core Keeper mod build failed.");

            string manifest = Path.Combine(output, settings.metadata.name, "ModManifest.json");
            if (!File.Exists(manifest))
                throw new Exception("Build did not produce ModManifest.json.");

            Debug.Log("Release build complete: " + manifest);
            EditorApplication.Exit(0);
        }
        catch (Exception exception)
        {
            Debug.LogException(exception);
            EditorApplication.Exit(1);
        }
    }
}
