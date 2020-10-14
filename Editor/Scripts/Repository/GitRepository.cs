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

using System.IO;
using System.Text;
using UnityEngine;

namespace UnityVCSInfo.Editor.Repository
{
    internal class GitRepository : IRepository
    {
        public ERepositoryType RepositoryType => ERepositoryType.Git;
        public string VcsName => "Git";
        public string Name => GetName();
        public string Branch => GetBranch();

        public static bool Exists()
        {
            var path = GetRepositoryPath();
            return !string.IsNullOrEmpty(path) && Directory.Exists(path);
        }
        
        private string GetName()
        {
            var gitFolderPath = GetRepositoryPath();
            if (string.IsNullOrEmpty(gitFolderPath))
                return string.Empty;
            
            var configFilePath = gitFolderPath + "/config";
            
            if (!Directory.Exists(gitFolderPath) || !File.Exists(configFilePath))
                return string.Empty;

            var configData = File.ReadAllText(configFilePath, Encoding.UTF8);
            var remoteLocation = configData.IndexOf("[remote");
            var urlLocation = configData.IndexOf("url", remoteLocation);
            var endStringLocation = configData.IndexOf(".git", urlLocation);
            var urlDataSplit = configData
                .Substring(urlLocation, endStringLocation - urlLocation)
                .Split('/');

            return urlDataSplit?.Length > 0 ? urlDataSplit[urlDataSplit.Length - 1] : string.Empty;
        }
        
        private string GetBranch()
        {
            var gitFolderPath = GetRepositoryPath();
            if (string.IsNullOrEmpty(gitFolderPath))
                return string.Empty;
            
            var headFilePath = gitFolderPath + "/HEAD";
            
            if (!Directory.Exists(gitFolderPath) || !File.Exists(headFilePath))
                return string.Empty;

            var headData = File.ReadAllText(headFilePath, Encoding.UTF8).Split('/');
            if (headData?.Length > 0)
            {
                var branch = headData[headData.Length - 1];
                branch = branch.Trim('\n');
                return branch;
            }

            return string.Empty;
        }

        private static string GetRepositoryPath()
        {
            var assetsFolderPath = Application.dataPath.Split('/');
            var gitFolderPath = "";

            for (var i = 0; i < assetsFolderPath.Length - 1; i++)
                gitFolderPath += assetsFolderPath[i] + "/"; 
            gitFolderPath += ".git";

            return gitFolderPath;
        }
    }
}