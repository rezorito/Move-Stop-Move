using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayModeStartScene {
    static PlayModeStartScene() {
        EditorApplication.playModeStateChanged += LoadStartScene;
    }

    static void LoadStartScene(PlayModeStateChange state) {
        if (state == PlayModeStateChange.EnteredPlayMode) {
            // Khi vừa bấm Play → load scene bằng runtime API
            SceneManager.LoadScene("IntroScreen");
        }
        else if (state == PlayModeStateChange.ExitingEditMode) {
            // Trước khi vào Play Mode → đảm bảo đang mở scene hợp lệ
            string scenePath = "Assets/Scenes/IntroScreen.unity";
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(scenePath);
        }
    }
}
