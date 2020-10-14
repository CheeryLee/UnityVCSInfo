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

namespace UnityVCSInfo.Editor
{
    internal class RepositoryData
    {
        public static RepositoryData Instance
        {
            get
            {
                if (_instance == null)
                    new RepositoryData();
                return _instance;
            }
        }

        public ERepositoryType RepositoryType => _repository?.RepositoryType ?? ERepositoryType.Default;
        public string VcsName => _repository != null ? _repository.VcsName : string.Empty;
        public string Name => _repository != null ? _repository.Name : string.Empty;
        public string Branch => _repository != null ? _repository.Branch : string.Empty;
        
        private IRepository _repository;
        private SettingsModel _settingsModel;
        
        private static RepositoryData _instance;

        private RepositoryData()
        {
            _instance = this;
            
            SetupSettingsModel();
            SetupRepository();
        }

        public Texture2D GetRepositoryIcon()
        {
            if (!_settingsModel)
                return null;

            if (_repository?.RepositoryType == ERepositoryType.Git)
            {
                if (EditorGUIUtility.isProSkin)
                    return _settingsModel.gitIcon.white;
                return _settingsModel.gitIcon.black;
            }
            else
            {
                if (EditorGUIUtility.isProSkin)
                    return _settingsModel.defaultIcon.white;
                return _settingsModel.defaultIcon.black;
            }
        }
        
        private void SetupRepository()
        {
            if (GitRepository.Exists())
                _repository = new GitRepository();
        }

        private void SetupSettingsModel()
        {
            var path = "Packages/com.cheerylee.unity-vcs-info/Editor/Settings/VCSSettings.asset";
            _settingsModel = AssetDatabase.LoadAssetAtPath(path, typeof(SettingsModel)) as SettingsModel;
        }
    }
}