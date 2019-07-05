using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RunAndJump.LevelCreator
{
    public class PaletteWindow : EditorWindow
    {
        public static PaletteWindow Instance;

        private List<PaletteItem.Category> _categories;
        private List<string> _categoryLabels;
        private PaletteItem.Category _categorySelected;


        private const float ButtonWidth = 80;
        private const float ButtonHeight = 90;

        private string _path = "Assets/Prefabs/LevelPieces";
        private List<PaletteItem> _items;
        private Dictionary<PaletteItem.Category, List<PaletteItem>> _categorizedItems;
        private Dictionary<PaletteItem, Texture2D> _previews;
        private Vector2 _scrollPosition;

        public static void ShowPalette()
        {
            Instance = GetWindow<PaletteWindow>();
            Instance.titleContent = new GUIContent("Palette");
        }

        private void OnEnable()
        {
            if (_categories == null)
            {
                _initCategories();
            }
            if (_categorizedItems == null)
            {
                _initContent();
            }
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable called...");
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy called...");
        }

        private void OnGUI()
        {
            _drawTabs();
        }

        private void Update()
        {
            if (_previews.Count != _items.Count)
            {
                _generatePreviews();
            }
        }

        private void _initCategories()
        {
            _categories = EditorUtils.GetListFromEnum<PaletteItem.Category>();
            _categoryLabels = new List<string>();
            foreach (PaletteItem.Category category in _categories)
            {
                _categoryLabels.Add(category.ToString());
            }
        }

        private void _initContent()
        {
            _items = EditorUtils.GetAssetsWithScript<PaletteItem>(_path);
            _categorizedItems = new Dictionary<PaletteItem.Category, List<PaletteItem>>();
            _previews = new Dictionary<PaletteItem, Texture2D>();
            foreach (PaletteItem.Category category in _categories)
            {
                _categorizedItems.Add(category, new List<PaletteItem>());
            }
            foreach (PaletteItem item in _items)
            {
                _categorizedItems[item.category].Add(item);
            }
        }

        private void _drawTabs()
        {
            int index = (int)_categorySelected;
            index = GUILayout.Toolbar(index, _categoryLabels.ToArray());
            _categorySelected = _categories[index];
        }

        private void _drawScroll()
        {
            if (_categorizedItems[_categorySelected].Count == 0)
            {
                EditorGUILayout.HelpBox("This category is empty.", MessageType.Info);
                return;
            }

            int rowCapacity = Mathf.FloorToInt(position.width / (ButtonWidth));
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
            int selectionGridIndex = -1;
            selectionGridIndex = GUILayout.SelectionGrid(
                selectionGridIndex,
                _getGUICotentFromItems(),
                rowCapacity,
                _getGUIStyle());
            GetSelectedItem(selectionGridIndex);
            GUILayout.EndScrollView();
        }

        private void _generatePreviews()
        {
            foreach (PaletteItem item in _items)
            {
                if (!_previews.ContainsKey(item))
                {
                    Texture2D preview = AssetPreview.GetAssetPreview(item.gameObject);
                    if (preview != null)
                    {
                        _previews.Add(item, preview);
                    }
                }
            }
        }

        private GUIContent[] _getGUICotentFromItems()
        {
            List<GUIContent> guiContents = new List<GUIContent>();
            if (_previews.Count == _items.Count)
            {
                int totalItems = _categorizedItems[_categorySelected].Count;
                for (int i = 0; i < totalItems; i++)
                {
                    GUIContent guiContent = new GUIContent();
                    guiContent.text = _categorizedItems[_categorySelected][i].itemName;
                    guiContent.image = _previews[_categorizedItems[_categorySelected][i]];
                    guiContents.Add(guiContent);
                }
            }
            return guiContents.ToArray();
        }

        private GUIStyle _getGUIStyle()
        {

        }

        private void _getSelectedItem(int index)
        {

        }
    }
}

