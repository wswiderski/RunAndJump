using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace RunAndJump.LevelCreator
{
    public static class EditorUtils
    {
        public static void NewLevel()
        {
            _newScene();
            GameObject levelGo = new GameObject("Level");
            levelGo.transform.position = Vector3.zero;
            levelGo.AddComponent<Level>();
        }

        private static void _newScene()
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
    }
}

