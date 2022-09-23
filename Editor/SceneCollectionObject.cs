using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace Editor {
    [CreateAssetMenu(menuName = "Scene Toolkit/Scene Collection", fileName = "SceCol_Collection")]
    public class SceneCollectionObject : ScriptableObject {
        public string title;
        public SceneAsset[] scenes;

        public void OpenCollection() {
            for (var i = 0; i < scenes.Length; i++){
                var scene = scenes[i];
                var mode = (i == 0) ? OpenSceneMode.Single : OpenSceneMode.Additive;
                EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(scene), mode);
            }
        }
    }
}
