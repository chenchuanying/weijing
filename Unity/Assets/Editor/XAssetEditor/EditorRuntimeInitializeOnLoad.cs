﻿//
// EditorRuntimeInitializeOnLoad.cs
//
// Author:
//       fjy <jiyuan.feng@live.com>
//
// Copyright (c) 2020 fjy
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using ET;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
//using JEngine.Core;
//using JEngine.Editor;
using UnityEditor;
using UnityEngine;


namespace libx
{
    public static class EditorRuntimeInitializeOnLoad
    {

        public static int GetVersion()
        {
            int version = -1;

            UnityEngine.SceneManagement.Scene curScene = UnityEditor.SceneManagement.EditorSceneManager.GetSceneByPath("Assets/Scenes/Init.unity");
            GameObject[] gos = curScene.GetRootGameObjects();
            foreach (var go in gos)
            {
                if (go.name != "Global")
                {
                    continue;
                }

                Init ai_1 = go.GetComponent<Init>();
                FieldInfo[] allFieldInfo = (ai_1.GetType()).GetFields(BindingFlags.NonPublic | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);

                for (int f = 0; f < allFieldInfo.Length; f++)
                {
                    if (allFieldInfo[f].Name == "VersionMode")
                    {
                        version = Convert.ToInt32(allFieldInfo[f].GetValue(ai_1));
                        break;
                    }
                }
            }

            if (version == -1)
            {
                UnityEngine.Debug.LogError("version == -1");
                return version;
            }
            switch ((VersionMode)version)
            {
                case VersionMode.Alpha:
                    BuildScript.outputPath = "../Release/DLCAlpha/" + BuildScript.GetPlatformName();
                    break;
                case VersionMode.BanHao:
                case VersionMode.Beta:
                    BuildScript.outputPath = "../Release/DLCBeta/" + BuildScript.GetPlatformName();
                    break;
            }
            Assets.basePath = BuildScript.outputPath + Path.DirectorySeparatorChar;
            Assets.loadDelegate = AssetDatabase.LoadAssetAtPath;
            return version;
        }

        [RuntimeInitializeOnLoadMethod()]
        public static void OnInitialize()
        {
            Assets.basePath = BuildScript.outputPath + Path.DirectorySeparatorChar;
            Assets.loadDelegate = AssetDatabase.LoadAssetAtPath;
            var assets = new List<string>();
            var rules = BuildScript.GetBuildRules();
            foreach (var asset in rules.scenesInBuild)
            {
                var path = AssetDatabase.GetAssetPath(asset);
                if (string.IsNullOrEmpty(path))
                {
                    continue;
                }
                assets.Add(path); 
            } 
            foreach (var rule in rules.rules)
            {
                if (rule.searchPattern.Contains("*.unity"))
                {
                    assets.AddRange(rule.GetAssets());
                }
            }
            
            //List<EditorBuildSettingsScene> _scenes =new List<EditorBuildSettingsScene>(0);
            //foreach (var scene in EditorBuildSettings.scenes)
            //{
            //    if (scene.path.Equals(JEngineSetting.StartUpScenePath) || assets.Contains(scene.path))
            //    {
            //        continue;
            //    }
            //    _scenes.Add(scene);
            //}

            //List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(0);
            
            //scenes.Add(new EditorBuildSettingsScene(JEngineSetting.StartUpScenePath, true));//添加启动场景
            //for (var index = 0; index < _scenes.Count; index++)//添加其他场景（用户自己加的）
            //{
            //    scenes.Add(new EditorBuildSettingsScene(_scenes[index].path, true));
            //}
            //for (var index = 0; index < assets.Count; index++)//添加热更场景（用于编译测试）
            //{
            //    var asset = assets[index];
            //    if (asset == JEngineSetting.StartUpScenePath) continue;
            //    scenes.Add(new EditorBuildSettingsScene(asset, true));
            //}

            //EditorBuildSettings.scenes = scenes.ToArray();
        }

        [InitializeOnLoadMethod]
        private static void OnEditorInitialize()
        {
            EditorUtility.ClearProgressBar();
            // BuildScript.GetManifest();
            // BuildScript.GetBuildRules();
        }
    }
}