using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RunAndJump.LevelCreator
{
    public static class MenuItems
    {
        [MenuItem("Tools/Level Creator/Create New Scene")]
        public static void CreateScene()
        {
            EditorUtils.NewLevel();
        }
    }
}
