// MIT License
//
// Copyright (c) 2020 Alexander Pluzhnikov
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using UnityVCSInfo.Editor.Repository;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityVCSInfo.Editor.UI
{
    internal static class ToolbarStyles
    {
        public static readonly GUIStyle commandButtonStyle;
        private static readonly RectOffset _commandButtonPadding;
        
        static ToolbarStyles()
        {
            _commandButtonPadding = new RectOffset(10, 10, 0, 0);

            commandButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
            commandButtonStyle.imagePosition = ImagePosition.ImageLeft;
            commandButtonStyle.padding = _commandButtonPadding;
            commandButtonStyle.alignment = TextAnchor.MiddleRight;
            commandButtonStyle.clipping = TextClipping.Clip;
        }
    }
    
    [InitializeOnLoad]
    internal class ToolbarField
    {
        private static string _branch;
        private static Texture2D _vcsIcon;

        private static bool _editorFocused;

        private const int BRANCH_MAX_LENGTH = 15;

        static ToolbarField()
        {
            ToolbarExtender.RightToolbarGUI.Remove(OnFieldGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnFieldGUI);
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;

            _editorFocused = InternalEditorUtility.isApplicationActive;
            LoadData();
        }
        
        private static void OnFieldGUI()
        {
            if (GUILayout.Button(new GUIContent(_branch, _vcsIcon), ToolbarStyles.commandButtonStyle))
            {
                OnVcsButtonClick();
            }
        }

        private static void OnUpdate()
        {
            if (_editorFocused != InternalEditorUtility.isApplicationActive)
            {
                if (_editorFocused)
                    LoadData();
                _editorFocused = InternalEditorUtility.isApplicationActive;
            }
        }
        
        private static void OnVcsButtonClick()
        {
            PopupWindow.Show(new Rect(ToolbarExtender.RightToolbarWidth - 175, 25, 0, 0), new ToolbarWindow());
        }

        private static void LoadData()
        {
            if (RepositoryData.Instance.RepositoryType != ERepositoryType.Default)
                _branch = "   " + ShrinkString(RepositoryData.Instance.Branch, BRANCH_MAX_LENGTH);
            else
                _branch = "   " + "No VCS detected";
            
            _vcsIcon = RepositoryData.Instance.GetRepositoryIcon();
        }

        private static string ShrinkString(string str, int maxValue)
        {
            if (str.Length > maxValue)
            {
                if (maxValue < 0)
                    maxValue = 0;
                
                str = str.Substring(0, maxValue);
                str = str + "...";
            }

            return str;
        }
    }
}