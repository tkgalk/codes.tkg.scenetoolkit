using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace Editor {
    internal static class ToolbarStyles {
        public static readonly GUIStyle ToolbarButtonStyle;

        static ToolbarStyles() {
            ToolbarButtonStyle = new GUIStyle("ToolbarButton") {
                fontSize = 12,
                alignment = TextAnchor.MiddleCenter,
                imagePosition = ImagePosition.ImageAbove,
            };
        }
    }

    [InitializeOnLoad]
    public class SceneToolkit {
        private static readonly GUIContent SceneToolkitGUIContent = new("Scene Toolkit", "Open Scene Toolkit Window");
        private static readonly GUIContent PreviewPlayGUIContent = new("Preview Play", "Preview Play from the Boot Scene");
        private static SceneCollectionObject[] _sceneCollectionObjects;
        private static GenericMenu _sceneCollectionsMenu;

        static SceneToolkit() {
            ToolbarExtender.RightToolbarGUI.Add(SceneToolkitButton);
            ToolbarExtender.LeftToolbarGUI.Add(PreviewPlayButton);
        }

        private static void SceneToolkitButton() {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(SceneToolkitGUIContent, ToolbarStyles.ToolbarButtonStyle, GUILayout.MaxHeight(20))) {
                CreateSceneCollectionsMenu();
                _sceneCollectionsMenu.ShowAsContext();
            }
        }

        private static void PreviewPlayButton() {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(PreviewPlayGUIContent, ToolbarStyles.ToolbarButtonStyle, GUILayout.MaxHeight(20))) {
                EditorSceneLoader.PlayFromFirstScene = true;
                EditorApplication.isPlaying = true;
            }
        }

        private static void CreateSceneCollectionsMenu() {
            _sceneCollectionObjects = Resources.LoadAll<SceneCollectionObject>("");
            _sceneCollectionsMenu = new GenericMenu();
            foreach (var obj in _sceneCollectionObjects) {
                _sceneCollectionsMenu.AddItem(new GUIContent(obj.title), false, () => {
                    obj.OpenCollection();
                });
            }
        }

        public class EditorSceneLoader : UnityEditor.Editor {
            private const string PlayFromFirstMenu = "PlayPreview";
            public static bool PlayFromFirstScene { get => EditorPrefs.HasKey(PlayFromFirstMenu) && EditorPrefs.GetBool(PlayFromFirstMenu); set => EditorPrefs.SetBool(PlayFromFirstMenu, value); }

            // This method is called before any Awake. It's the perfect callback for this feature
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            private static void LoadFirstSceneAtGameBegins() {
                if (!PlayFromFirstScene) {
                    return;
                }

                PlayFromFirstScene = false;
                if (EditorBuildSettings.scenes.Length == 0) {
                    Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
                    return;
                }

                foreach (var go in FindObjectsOfType<GameObject>())
                    go.SetActive(false);

                SceneManager.LoadScene(0);
            }
        }
    }
}
