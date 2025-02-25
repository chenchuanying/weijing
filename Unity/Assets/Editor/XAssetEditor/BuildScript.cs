//
// BuildScript.cs
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
using UnityEditor;
using UnityEngine;

namespace libx
{

    public struct ResourceInfo
    {
        public long Size;
        public string Path;
        public string Hash;
    }

    public static class BuildScript
    {

        public static string outputPath = string.Empty;

        public static BuildTarget Current_BuildTarget  => EditorUserBuildSettings.activeBuildTarget;

        public static void SetBuildTarget(BuildTarget buildTarget)
        {
            //Current_BuildTarget = EditorUserBuildSettings.activeBuildTarget;
        }

        public static void RestoreBuildTarget(BuildTarget buildTarget)
        {
           // Current_BuildTarget = EditorUserBuildSettings.activeBuildTarget;
        }

        public static void ClearAssetBundles()
        {
            var allAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (var i = 0; i < allAssetBundleNames.Length; i++)
            {
                var text = allAssetBundleNames[i];
                if (EditorUtility.DisplayCancelableProgressBar(
                                    string.Format("Clear AssetBundles {0}/{1}", i, allAssetBundleNames.Length), text,
                                    i * 1f / allAssetBundleNames.Length))
                    break;

                AssetDatabase.RemoveAssetBundleName(text, true);
            }
            EditorUtility.ClearProgressBar();
        }

        internal static void ApplyBuildRules(bool hash = true)
        {
            int version = EditorRuntimeInitializeOnLoad.GetVersion();
            switch ((VersionMode)version)
            {
                case VersionMode.Alpha:
                    outputPath = "../Release/DLCAlpha/" + GetPlatformName();
                    break;
                case VersionMode.Beta:
                case VersionMode.BanHao:
                    outputPath = "../Release/DLCBeta/" + GetPlatformName();
                    break;
            }

            BuildRules rules = GetBuildRules();
            BuildRules.nameByHash = hash;
            rules.Apply();
        }

        internal static BuildRules GetBuildRules()
        {
            var rule = GetAsset<BuildRules>("Assets/Res/XAsset/Rules.asset");
            string hotUpdatePath = "Assets/Bundles/";   
            if (!Directory.Exists(hotUpdatePath))
            {
                Directory.CreateDirectory(hotUpdatePath);
            }


            foreach (var _rule in rule.rules)
            {
                if (!Directory.Exists(_rule.searchPath))
                {
                    Directory.CreateDirectory(_rule.searchPath);
                }
            }

            AssetDatabase.SaveAssets();

            return rule;
        }

        public static void CopyAssetBundlesTo(string path, bool vfs = true)
        {
#if UNITY_IPHONE
                vfs = false;
#endif

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var versions = new List<VFile>();

            if (!vfs)
            {
                versions.AddRange(Versions.LoadVersions(outputPath + "/" + Versions.Filename));
                versions.Add(new VFile() { name = Versions.Filename });
                versions.RemoveAt(versions.FindIndex(file => file.name.Equals(Versions.Dataname)));
            }
            else
            {
                versions.Add(new VFile() { name = Versions.Filename });
                versions.Add(new VFile() { name = Versions.Dataname });
            }

            foreach (var item in versions)
            {
                var src = outputPath + "/" + item.name;
                var dest = path + "/" + item.name;
                if (File.Exists(src))
                {
                    File.Copy(src, dest, true);
                }
            }

            AssetDatabase.Refresh();
        }

        public static string GetPlatformName()
        {
            return GetPlatformForAssetBundles(Current_BuildTarget);
        }

        private static string GetPlatformForAssetBundles(BuildTarget target)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "Windows";
#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return "OSX";
#else
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return "OSX";
#endif
                default:
                    return null;
            }
        }

        private static string[] GetLevelsFromBuildSettings()
        {
            List<string> scenes = new List<string>();
            foreach (var item in GetBuildRules().scenesInBuild)
            {
                var path = AssetDatabase.GetAssetPath(item);
                if (!string.IsNullOrEmpty(path))
                {
                    scenes.Add(path);
                }
            }

            return scenes.ToArray();
        }

        private static string GetAssetBundleManifestFilePath()
        {
            var relativeAssetBundlesOutputPathForPlatform = Path.Combine("Asset", GetPlatformName());
            return Path.Combine(relativeAssetBundlesOutputPathForPlatform, GetPlatformName()) + ".manifest";
        }

        public static void BuildStandalonePlayer()
        {
            var outputPath =
                Path.Combine(Environment.CurrentDirectory,
                    "Build/" + GetPlatformName()
                        .ToLower()); //EditorUtility.SaveFolderPanel("Choose Location of the Built Game", "", "");
            if (outputPath.Length == 0)
                return;

            var levels = GetLevelsFromBuildSettings();
            if (levels.Length == 0)
            {
                Log.Debug("Nothing to build.");
                return;
            }

            var targetName = GetBuildTargetName(Current_BuildTarget);
            if (targetName == null)
                return;
#if UNITY_5_4 || UNITY_5_3 || UNITY_5_2 || UNITY_5_1 || UNITY_5_0
			BuildOptions option = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None;
			BuildPipeline.BuildPlayer(levels, outputPath + targetName, EditorUserBuildSettings.activeBuildTarget, option);
#else
            var buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = levels,
                locationPathName = outputPath + targetName,
                assetBundleManifestPath = GetAssetBundleManifestFilePath(),
                target = Current_BuildTarget,
                options = EditorUserBuildSettings.development ? BuildOptions.Development : BuildOptions.None
            };
            BuildPipeline.BuildPlayer(buildPlayerOptions);
#endif
        }

        public static string CreateAssetBundleDirectory()
        {
            // Choose the output path according to the build target.
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            return outputPath;
        }

        public static void SaveResourceList_1(List<ResourceInfo> ResourceInfos, string file)
        {
            string asssetName = "";
            string dataPath = Application.dataPath;
            dataPath = dataPath.Substring(0, dataPath.Length - 6);
            for (int i = 0; i < ResourceInfos.Count; i++)
            {
                asssetName += ResourceInfos[i].Size + "    ";
                asssetName += ResourceInfos[i].Path + "    ";
                asssetName += "       "+ ResourceInfos[i].Hash;
                asssetName += "\r\n";
            }
            //D:/weijingHot/trunk_2021_0808/Unity/
            dataPath = dataPath.Substring(0, dataPath.Length - 6);
            string filePath = dataPath + file;//这里是你的已知文件
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
            }

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            fs.SetLength(0);//首先把文件清空了。
            sw.Write(asssetName);//写你的字符串。
            sw.Close();
        }

        public static string GetSizeNum(long fileSize)
        {
            string strNum;
            int mb = 1024 * 1024;
            int kb = 1024;
            if (fileSize > kb)
            {
                float xx = fileSize * 1f / mb;
                decimal num1 = (decimal)xx;
                strNum = String.Format("{0:N3}", xx) + "mb    ";
            }
            else
            {
                float xx = fileSize * 1f / kb;
                strNum = String.Format("{0:N3}", xx) + "kb    ";
            }
            return strNum;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sort">1大小  2路径</param>
        public static void BundleResourceList()
        {
            var rules = GetBuildRules();
            var builds = rules.GetBuilds();

            string dataPath = Application.dataPath;
            dataPath = dataPath.Substring(  0, dataPath.Length - 6 );

            List<ResourceInfo> ResourceInfos = new List<ResourceInfo>();

            for (int i = 0; i < builds.Length; i++)
            {
                //D:/weijingHot/trunk_2021_0808/Unity/Assets
                string path = dataPath + builds[i].assetNames[0];
                long fileSize = new FileInfo(path).Length;

                string strNum = GetSizeNum(fileSize);
               
                ResourceInfos.Add(  new ResourceInfo() {   Path = (strNum + builds[i].assetNames[0]) , Size = (int)fileSize } );
            }
            List<ResourceInfo> resourceInfos_1 = new List<ResourceInfo>();
            resourceInfos_1.AddRange(ResourceInfos);
            resourceInfos_1.Sort(delegate (ResourceInfo a, ResourceInfo b)
            {
                //Assets/Bundles/Audio/Skill/baolie_3.mp3
                string path_1 = a.Path;
                string path_2 = b.Path;
                string[] path1_list = path_1.Split('/');
                string[] path2_list = path_2.Split('/');
                path_1 = path_1.Substring(0, path_1.Length - path1_list[path1_list.Length - 1].Length);
                path_2 = path_2.Substring(0, path_2.Length - path2_list[path2_list.Length - 1].Length);

                if (path_1 == path_2)
                {
                    return 0;
                }
                else
                {
                    if (path_1.Length == path_2.Length)
                    {
                        //文件夹首字母
                        char folder_1 = path1_list[path1_list.Length - 1].ToCharArray()[0];
                        char folder_2 = path2_list[path2_list.Length - 1].ToCharArray()[0];
                        if (folder_1 == folder_2)
                        {
                            return path1_list[path1_list.Length - 1].Length - path2_list[path2_list.Length - 1].Length;
                        }
                        else
                        {
                            return folder_1 - folder_2;
                        }
                    }
                    else
                    {
                        return path_1.Length - path_2.Length;
                    }
                }
            });
            SaveResourceList_1(resourceInfos_1, "/Release/HotRes_1.txt");

            List<ResourceInfo> resourceInfos_2 = new List<ResourceInfo>();
            resourceInfos_2.AddRange(ResourceInfos);
            resourceInfos_2.Sort(delegate (ResourceInfo a, ResourceInfo b)
            {
                return (int)b.Size - (int)a.Size;
            });
            SaveResourceList_1(resourceInfos_2, "/Release/HotRes_2.txt");

            SaveAssetBundleList();
        }

        public static void SaveAssetBundleList()
        {
            List<string> fileList = new List<string>();
            int version = EditorRuntimeInitializeOnLoad.GetVersion();
            string outputPath = string.Empty;
            switch ((VersionMode)version)
            {
                case VersionMode.Alpha:
                    outputPath = "DLCAlpha";
                    break;
                case VersionMode.Beta:
                case VersionMode.BanHao:
                    outputPath = "DLCBeta";
                    break;
            }
            string dataPath = Application.dataPath;
            dataPath = dataPath.Substring(0, dataPath.Length - 6);
            dataPath = dataPath.Substring(0, dataPath.Length - 6);
            fileList = CheckReferences.GetFile(dataPath + $"/Release/{outputPath}/Android/assets", fileList);

            List<ResourceInfo> ResourceInfos = new List<ResourceInfo>();
            for (var index = 0; index < fileList.Count; index++)
            {
                string path = fileList[index];

                using (var stream = File.OpenRead(path))
                {
                    long fileSize = stream.Length;
                    string strNum = GetSizeNum(fileSize);
                    ResourceInfos.Add(new ResourceInfo() { Path = (strNum +"    " +path), Size = (int)fileSize });
                }
            }
            ResourceInfos.Sort(delegate (ResourceInfo a, ResourceInfo b)
            {
                return (int)b.Size - (int)a.Size;
            });
            SaveResourceList_1(ResourceInfos, "/Release/HotRes_3.txt");
        }

        public static void BuildAssetBundles()
        {
            //set aes key
            //    BuildPipeline.SetAssetBundleEncryptKey(ET.BundleHelper.AES_Pass);

            // Choose the output path according to the build target.
            var outputPath = CreateAssetBundleDirectory();
            const BuildAssetBundleOptions options = BuildAssetBundleOptions.ChunkBasedCompression;
            var targetPlatform = Current_BuildTarget;
            var rules = GetBuildRules();
            var builds = rules.GetBuilds();
            var assetBundleManifest = BuildPipeline.BuildAssetBundles(outputPath, builds, options, targetPlatform);
            if (assetBundleManifest == null)
            {
                return;
            }

            Dictionary<string, AssetBundleBuild> assetBundleBuilds = new Dictionary<string, AssetBundleBuild>();
            for (int i = 0; i < builds.Length; i++)
            {
                assetBundleBuilds.Add(builds[i].assetBundleName, builds[i]);
            }
            //assetBundleName 加密 assetNames 资源名字  addresname 和  var一般为空
            //assetBundleBuilds[0].assetBundleName + assetBundleBuilds[0].assetNames[0];

            var manifest = GetManifest();
            var dirs = new List<string>();
            var assets = new List<AssetRef>();
            var bundles = assetBundleManifest.GetAllAssetBundles();
            var bundle2Ids = new Dictionary<string, int>();

            for (var index = 0; index < bundles.Length; index++)
            {
                var bundle = bundles[index];
                bundle2Ids[bundle] = index;

                AssetBundleBuild assetBundleBuild = assetBundleBuilds[bundle];
            }

            var bundleRefs = new List<BundleRef>();
            for (var index = 0; index < bundles.Length; index++)
            {
                var bundle = bundles[index];
                var deps = assetBundleManifest.GetAllDependencies(bundle);
                var path = string.Format("{0}/{1}", outputPath, bundle);
                if (File.Exists(path))
                {
                    using (var stream = File.OpenRead(path))
                    {
                        bundleRefs.Add(new BundleRef
                        {
                            name = bundle,
                            id = index,
                            deps = Array.ConvertAll(deps, input => bundle2Ids[input]),
                            len = stream.Length,
                            hash = assetBundleManifest.GetAssetBundleHash(bundle).ToString(),
                        });
                    }
                }
                else
                {
                    Debug.LogError(path + " file not exsit.");
                }
            }

            for (var i = 0; i < rules.ruleAssets.Length; i++)
            {
                var item = rules.ruleAssets[i];
                var path = item.path;
                var dir = Path.GetDirectoryName(path).Replace("\\", "/");
                var index = dirs.FindIndex(o => o.Equals(dir));
                if (index == -1)
                {
                    index = dirs.Count;
                    dirs.Add(dir);
                }

                if (!bundle2Ids.ContainsKey(item.bundle))
                {
                    UnityEngine.Debug.Log("!bundle2Ids.ContainsKey");
                    Log.Debug(item.bundle);
                    Log.Debug(path);
                }
                try
                {
                    var asset = new AssetRef { bundle = bundle2Ids[item.bundle], dir = index, name = Path.GetFileName(path) };
                    assets.Add(asset);
                }
                catch
                {
                    Log.Debug(path);
                }

            }

            manifest.dirs = dirs.ToArray();
            manifest.assets = assets.ToArray();
            manifest.bundles = bundleRefs.ToArray();

            EditorUtility.SetDirty(manifest);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            var manifestBundleName = "manifest.unity3d";
            builds = new[] {
                new AssetBundleBuild {
                    assetNames = new[] { AssetDatabase.GetAssetPath (manifest), },
                    assetBundleName = manifestBundleName
                }
            };

            BuildPipeline.BuildAssetBundles(outputPath, builds, options, targetPlatform);
            ArrayUtility.Add(ref bundles, manifestBundleName);

            Versions.BuildVersions(outputPath, bundles, GetBuildRules().AddVersion());

            List<ResourceInfo> resourceInfos_2 = new List<ResourceInfo>();
            foreach (var file in bundles)
            {
                using (var fs = File.OpenRead(outputPath + "/" + file))
                {
                    if (assetBundleBuilds.ContainsKey(file))
                    {
                        string assetname = assetBundleBuilds[file].assetNames[0];
                        resourceInfos_2.Add(new ResourceInfo() { Size = fs.Length, Path = assetname, Hash = file });
                    }
                }
            }
            resourceInfos_2.Sort(delegate (ResourceInfo a, ResourceInfo b)
            {
                return (int)b.Size - (int)a.Size;
            });
            SaveResourceList_1(resourceInfos_2, "/Release/HotRes_4.txt");
        }


        private static string GetBuildTargetName(BuildTarget target)
        {
            var time = DateTime.Now.ToString("yyyyMMdd-HHmmss");
            var name = PlayerSettings.productName + "-v" + PlayerSettings.bundleVersion + ".";
            switch (target)
            {
                case BuildTarget.Android:
                    return string.Format("/{0}{1}-{2}.apk", name, GetBuildRules().version, time);

                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return string.Format("/{0}{1}-{2}.exe", name, GetBuildRules().version, time);

#if UNITY_2017_3_OR_NEWER
                case BuildTarget.StandaloneOSX:
                    return "/" + name + ".app";

#else
                case BuildTarget.StandaloneOSXIntel:
                case BuildTarget.StandaloneOSXIntel64:
                case BuildTarget.StandaloneOSXUniversal:
                    return "/" + path + ".app";

#endif

                case BuildTarget.WebGL:
                case BuildTarget.iOS:
                    return "";
                // Add more build targets for your own.
                default:
                    Log.Debug("Target not implemented.");
                    return null;
            }
        }

        private static T GetAsset<T>(string path) where T : ScriptableObject
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }

            return asset;
        }

        public static Manifest GetManifest()
        {
            return GetAsset<Manifest>(libx.Assets.ManifestAsset);
        }
    }
}
