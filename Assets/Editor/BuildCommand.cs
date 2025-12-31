
using UnityEditor;

public class BuildCommand
{
    public static void PerformBuild()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        // Các scene muốn build (thêm đường dẫn scene của bạn vào đây)
        // Ví dụ: buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainScene.unity" };
        // Nếu để trống nó sẽ lấy các scene đang active trong Build Settings
        buildPlayerOptions.scenes = EditorBuildSettingsScene.GetActiveSceneList(EditorBuildSettings.scenes);
        
        buildPlayerOptions.locationPathName = "build/WebGL"; // Output folder
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.None;

        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
