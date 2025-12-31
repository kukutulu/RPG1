using UnityEditor;
using System.IO;

public class BuildWebGL
{
    public static void Build()
    {
        string buildPath = "Build/WebGL";

        if (!Directory.Exists(buildPath))
            Directory.CreateDirectory(buildPath);

        BuildPipeline.BuildPlayer(
            EditorBuildSettings.scenes,
            buildPath,
            BuildTarget.WebGL,
            BuildOptions.None
        );
    }
}
