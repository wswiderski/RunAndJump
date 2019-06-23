using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RunAndJump.LevelCreator
{
    [CustomEditor(typeof(Level))]
    public class LevelInspector : Editor
    {
        private SerializedObject _mySerializedObject;
        private SerializedProperty _serializedTotalTime;
        private Level _myTarget;
        private int _newTotalColumns;
        private int _newTotalRows;

        private void OnEnable()
        {
            Debug.Log("OnEnable was called...");
            _myTarget = (Level)target;
            _initLevel();
            _resetResizeValues();
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable was called...");
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy was called...");
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            _drawLevelDataGUI();
            _drawLevelSizeGUI();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_myTarget);
            }
            //EditorGUILayout.LabelField("The GUI of this inspector was modified.");
            //EditorGUILayout.LabelField("The current level time is: " + _myTarget.TotalTime);
        }

        private void _drawLevelDataGUI()
        {
            EditorGUILayout.LabelField("Data", EditorStyles.boldLabel);
            _myTarget.TotalTime = EditorGUILayout.IntField("Total Time", Mathf.Max(0, _myTarget.TotalTime));
            EditorGUILayout.PropertyField(_serializedTotalTime);
            _myTarget.Gravity = EditorGUILayout.FloatField("Gravity", _myTarget.Gravity);
            _myTarget.Bgm = (AudioClip)EditorGUILayout.ObjectField("Bgm", _myTarget.Bgm, typeof(AudioClip), false);
            _myTarget.Background = (Sprite)EditorGUILayout.ObjectField("Background", _myTarget.Background, typeof(Sprite), false);
        }

        private void _initLevel()
        {
            _mySerializedObject = new SerializedObject(_myTarget);
            _serializedTotalTime = _mySerializedObject.FindProperty("_totalTime");

            if (_myTarget.Pieces == null || _myTarget.Pieces.Length == 0)
            {
                Debug.Log("Initializing the Pieces array...");
                _myTarget.Pieces = new LevelPiece[_myTarget.TotalColumns * _myTarget.TotalRows];
            }
        }

        private void _resetResizeValues()
        {
            _newTotalColumns = _myTarget.TotalColumns;
            _newTotalRows = _myTarget.TotalRows;
        }

        private void _resizeLevel()
        {
            LevelPiece[] newPieces = new LevelPiece[_newTotalColumns * _newTotalRows];
            for (int col = 0; col < _myTarget.TotalColumns; col++)
            {
                for (int row = 0; row < _myTarget.TotalRows; row++)
                {
                    if (col < _newTotalColumns && row < _newTotalRows)
                    {
                        newPieces[col + row * _newTotalColumns] = _myTarget.Pieces[col + row * _myTarget.TotalColumns];
                    }
                    else
                    {
                        LevelPiece piece = _myTarget.Pieces[col + row * _myTarget.TotalColumns];
                        if (piece != null)
                        {
                            Object.DestroyImmediate(piece.gameObject);
                        }
                    }
                }
            }
            _myTarget.Pieces = newPieces;
            _myTarget.TotalColumns = _newTotalColumns;
            _myTarget.TotalRows = _newTotalRows;
        }

        private void _drawLevelSizeGUI()
        {
            EditorGUILayout.LabelField("Size", EditorStyles.boldLabel);
            _newTotalColumns = EditorGUILayout.IntField("Columns", Mathf.Max(1, _newTotalColumns));
            _newTotalRows = EditorGUILayout.IntField("Rows", Mathf.Max(1, _newTotalRows));

            bool oldEnabled = GUI.enabled;
            GUI.enabled = _newTotalColumns != _myTarget.TotalColumns || _newTotalRows != _myTarget.TotalRows;
            bool buttonResize = GUILayout.Button("Resize", GUILayout.Height(2 * EditorGUIUtility.singleLineHeight));
            if (buttonResize)
            {
                if (EditorUtility.DisplayDialog(
                    "Level Creator",
                    "Are you sure you want to resize the level?\nThis action cannot be undone.",
                    "Yes",
                    "No"))
                {
                    _resizeLevel();
                }
            }
            bool buttonReset = GUILayout.Button("Reset");
            if (buttonReset)
            {
                _resetResizeValues();
            }
            GUI.enabled = oldEnabled;
        }
    }
}

