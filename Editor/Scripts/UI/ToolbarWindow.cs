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
using UnityEngine;

namespace UnityVCSInfo.Editor.UI
{
    internal class ToolbarWindow : PopupWindowContent
    {
        private readonly Vector2 _windowSize = new Vector2(250, 120);
        private readonly Rect _windowRect;
        private readonly Texture2D _windowBgTexture;

        private readonly GUIStyle _labelFieldHeaderStyle;
        private readonly GUIStyle _noVCSLabelStyle;

        internal ToolbarWindow()
        {
            _windowRect = new Rect(0, 0, _windowSize.x, _windowSize.y);

            _labelFieldHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                alignment = TextAnchor.MiddleLeft,
                clipping = TextClipping.Clip,
                padding = new RectOffset(10, 0, 0, 0)
            };
            
            _noVCSLabelStyle = new GUIStyle
            {
                wordWrap = true,
                padding = new RectOffset(10, 10, 10, 10),
            };

            if (EditorGUIUtility.isProSkin)
            {
                _noVCSLabelStyle.normal = new GUIStyleState()
                {
                    textColor = Color.white
                };
            }
            else
            {
                _noVCSLabelStyle.normal = new GUIStyleState()
                {
                    textColor = Color.black
                };
            }
            
            Color textureColor;
            if (EditorGUIUtility.isProSkin)
                textureColor = new Color(0.2f, 0.2f, 0.2f);
            else
                textureColor = new Color(0.65f, 0.65f, 0.65f);
            
            _windowBgTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            _windowBgTexture.SetPixel(0, 0, textureColor);
            _windowBgTexture.Apply();
        }

        public override Vector2 GetWindowSize()
        {
            return _windowSize;
        }

        public override void OnGUI(Rect rect)
        {
            // background
            GUI.DrawTexture(_windowRect, _windowBgTexture);

            if (RepositoryData.Instance.RepositoryType != ERepositoryType.Default)
                DrawDefaultState();
            else
                DrawNoVcsState();
        }

        private void DrawDefaultState()
        {
            EditorGUILayout.BeginVertical();
            DrawLabelField("VCS:", RepositoryData.Instance.VcsName);
            DrawLabelField("Repository:", RepositoryData.Instance.Name);
            DrawLabelField("Branch:", RepositoryData.Instance.Branch);
            EditorGUILayout.EndVertical();
            GUILayout.Space(1);
            
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(1);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("About ...", GUILayout.Width(90), GUILayout.Height(25)))
            {
                OnAboutButtonClick();
            }
            
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawNoVcsState()
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("No VCS were detected. To get the info you should install any VCS to the project",
                _noVCSLabelStyle);
            GUILayout.FlexibleSpace();
        }
        
        private void DrawLabelField(string label, string text)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(label, _labelFieldHeaderStyle, GUILayout.Width(80));
            GUILayout.Label(text);
            EditorGUILayout.EndHorizontal();
        }

        private void OnAboutButtonClick()
        {
            Application.OpenURL("https://github.com/CheeryLee/UnityVCSInfo");
        }
    }
}