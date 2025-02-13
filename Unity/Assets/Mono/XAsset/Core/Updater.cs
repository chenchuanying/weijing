//
// Updater.cs
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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HotVersion
{
    public int Version;
    public int VersionIOS;
    public string IOS_URL;
    public string Apk_URL;
    public int ClearData;
}

namespace libx
{
    public interface IUpdater
    {
        void OnStart();

        void OnMessage(string msg);

        void OnProgress(float progress);

        void OnVersion(string ver);

        void OnClear();
    }

    [RequireComponent(typeof(Downloader))]
    [RequireComponent(typeof(NetworkMonitor))]
    public class Updater : MonoBehaviour, IUpdater, INetworkMonitorListener
    {
        enum Step
        {
            Wait,
            Copy,
            Coping,
            Versions,
            Prepared,
            Download,
        }

        [SerializeField] private Step _step;
        //[SerializeField] private string gameScene = "Game.unity";

        private string baseURL = "";
        private bool development;
        private bool enableVFS = true;

        public IUpdater listener { get; set; }

        private Downloader _downloader;
        private NetworkMonitor _monitor;
        private string _platform;
        private string _savePath;
        private List<VFile> _versions = new List<VFile>();
        private Dictionary<string, VFile> _localAssets = new Dictionary<string, VFile>();

        public void OnMessage(string msg)
        {
            if (listener != null)
            {
                listener.OnMessage(msg);
            }
        }

        public void OnProgress(float progress)
        {
            if (listener != null)
            {
                listener.OnProgress(progress);
            }
        }

        public void OnVersion(string ver)
        {
            if (listener != null)
            {
                listener.OnVersion(ver);
            }
        }

        private void Start()
        {
            VersionMode versionMode = GameObject.Find("Global").GetComponent<Init>().VersionMode;
            string dlcPath = "";
            switch (versionMode)
            {
                case VersionMode.Alpha:
                    dlcPath = "DLCAlpha";
                    break;
                case VersionMode.Beta:
                case VersionMode.BanHao:
                    dlcPath = "DLCBeta";
                    break;
            }
            baseURL = "http://weijinghot.weijinggame.com/weijing1/" + dlcPath + "/";
            baseURL = baseURL.EndsWith("/") ? baseURL : baseURL + "/";

            Init init = GameObject.Find("Global").GetComponent<Init>();
#if UNITY_IPHONE
            transform.Find("Text_BigVersion").GetComponent<Text>().text = init.BigVersionIOS.ToString();
#else
            transform.Find("Text_BigVersion").GetComponent<Text>().text = init.BigVersion.ToString();
#endif

            UnityEngine.Debug.Log("baseURL  " +  baseURL);

            _downloader = gameObject.GetComponent<Downloader>();
            _downloader.onUpdate = OnUpdate;
            _downloader.onFinished = OnComplete;
            _downloader.onDownload = OnDownLoad;

            _monitor = gameObject.GetComponent<NetworkMonitor>();
            _monitor.listener = this;

            _savePath = string.Format("{0}/DLC/", Application.persistentDataPath);

            _platform = GetPlatformForAssetBundles(Application.platform);

            _step = Step.Wait;

            development = GameObject.Find("Global").GetComponent<Init>().Development;

            Assets.updatePath = _savePath;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (_reachabilityChanged || _step == Step.Wait)
            {
                return;
            }

            if (hasFocus)
            {
                MessageBox.CloseAll();
                if (_step == Step.Download)
                {
                    _downloader.Restart();
                }
                else
                {
                    StartUpdate();
                }
            }
            else
            {
                if (_step == Step.Download)
                {
                    _downloader.Stop();
                }
            }
        }

        private bool _reachabilityChanged;

        public void OnReachablityChanged(NetworkReachability reachability)
        {
            if (_step == Step.Wait)
            {
                return;
            }

            _reachabilityChanged = true;
            if (_step == Step.Download)
            {
                _downloader.Stop();
            }

            if (reachability == NetworkReachability.NotReachable)
            {
                MessageBox.Show("提示！", "找不到网络，请确保手机已经联网", "确定", "退出").onComplete += delegate(MessageBox.EventId id)
                {
                    if (id == MessageBox.EventId.Ok)
                    {
                        if (_step == Step.Download)
                        {
                            _downloader.Restart();
                        }
                        else
                        {
                            StartUpdate();
                        }

                        _reachabilityChanged = false;
                    }
                    else
                    {
                        Quit();
                    }
                };
            }
            else
            {
                if (_step == Step.Download)
                {
                    _downloader.Restart();
                }
                else
                {
                    StartUpdate();
                }

                _reachabilityChanged = false;
                MessageBox.CloseAll();
            }
        }

        private void OnUpdate(long progress, long size, float speed)
        {
            OnMessage(string.Format("下载中...{0}/{1}, 速度：{2}",
                Downloader.GetDisplaySize(progress),
                Downloader.GetDisplaySize(size),
                Downloader.GetDisplaySpeed(speed)));

            OnProgress(progress * 1f / size);
        }

        public void Clear()
        {
            MessageBox.Show("提示", "清除数据后所有数据需要重新下载，请确认！", "清除").onComplete += id =>
            {
                if (id != MessageBox.EventId.Ok)
                    return;
                OnClear();
            };
        }

        public void OnClear()
        {
            OnMessage("数据清除完毕");
            OnProgress(0);
            _versions.Clear();
            _downloader.Clear();
            _step = Step.Wait;
            _reachabilityChanged = false;

            Assets.Clear();

            if (listener != null)
            {
                listener.OnClear();
            }

            if (Directory.Exists(_savePath))
            {
                Directory.Delete(_savePath, true);
            }
        }

        public void OnStart()
        {
            if (listener != null)
            {
                listener.OnStart();
            }
        }

        private IEnumerator _checking;
        private bool _startUpdate =  false;

        public void StartUpdate()
        {
            _startUpdate = true;
            Debug.Log("StartUpdate.Development:" + development);
#if UNITY_EDITOR
            if (development)
            {
                Debug.Log("StartUpdate.Development1:");
                Assets.runtimeMode = false;
                StartCoroutine(LoadGameScene());
                return;
            }
#endif
            OnStart();
            Debug.Log("StartUpdate.Development1:OnStart");
            if (_checking != null)
            {
                StopCoroutine(_checking);
            }

            _checking = Checking();
            CheckLocalAsset();
            StartCoroutine(_checking);
        }

        private void CheckLocalAsset()
        {
            if (!Directory.Exists(_savePath))
            {
                Directory.CreateDirectory(_savePath);
            }
            if (_localAssets.Count == 0)
            {
                string filePath = _savePath + "md5.txt";
                if (!File.Exists(filePath))
                {
                    FileStream fs = new FileStream(filePath, FileMode.CreateNew);
                    StreamWriter sw_2 = new StreamWriter(fs);
                    sw_2.Flush();
                    fs.Dispose();
                }
                else
                {
                    List<string> assetList = new List<string>();
                    StreamReader sr = new StreamReader(filePath, Encoding.Default);
                    string content;
                    while ((content = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(content.ToString());
                        assetList.Add(content);
                    }
                    sr.Close();
                    InitLocalAssets(assetList);
                }
            }
        }

        private void AddDownload(VFile item)
        {
            _downloader.AddDownload(GetDownloadURL(item.name), item.name, _savePath + item.name, item.hash, item.len);
        }

        private void PrepareDownloads()
        {
            if (enableVFS)
            {
                var path = string.Format("{0}{1}", _savePath, Versions.Dataname);
                if (!File.Exists(path))
                {
                    AddDownload(_versions[0]);
                    return;
                }

                Versions.LoadDisk(path);
            }

            for (var i = 1; i < _versions.Count; i++)
            {
                var item = _versions[i];

                if (CheckAsset(item))
                {
                    continue;
                }

                if (Versions.IsNew(string.Format("{0}{1}", _savePath, item.name), item.len, item.hash))
                {
                    AddDownload(item);
                }
            }
        }

        private static string GetPlatformForAssetBundles(RuntimePlatform target)
        {
#if UNITY_EDITOR
            var t = EditorUserBuildSettings.activeBuildTarget;
            switch (t)
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
            return null;
#endif

            switch (target)
            {
                case RuntimePlatform.Android:
                    return "Android";
                case RuntimePlatform.IPhonePlayer:
                    return "iOS";
                case RuntimePlatform.WebGLPlayer:
                    return "WebGL";
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    return "Windows";
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                    return "OSX"; // OSX
                default:
                    return null;
            }
            return null;
        }

        private string GetDownloadURL(string filename)
        {
            return string.Format("{0}{1}/{2}", baseURL, _platform, filename);
        }

        private float passTime = 0;
        private string _localAssetList = String.Empty;
        void Update()
        {
            if (!_startUpdate)
            {
                return;
            }
            passTime += Time.deltaTime;
            if (passTime < 0.5f)
            {
                return;
            }

            passTime = 0;
            string filePath = _savePath + "md5.txt";
            StreamWriter sw = File.AppendText(filePath);

            sw.Write(_localAssetList);                  //在文本末尾写入文本 
            sw.Flush();                                 //清空 
            sw.Close();
            _localAssetList = String.Empty;
        }

        private void OnDownLoad(Download download)
        {
            string md5 = $"\r\n{download.id}_{download.hash}_{download.len}";
            _localAssetList += md5;
        }

        private bool CheckAsset(VFile vFile)
        {
            VFile localFile = null;
            _localAssets.TryGetValue(vFile.name, out localFile);
            if (localFile == null)
                return false;

            return localFile.len == vFile.len && localFile.hash.Equals(vFile.hash, StringComparison.OrdinalIgnoreCase);
        }

        private void UpdateLocalAsset()
        {
            string filePath = _savePath + "md5.txt";

            string md5 = "";
            for (int i = 0; i < _versions.Count; i++)
            {
                VFile download = _versions[i];
                md5 += (System.Environment.NewLine + download.id +  "_" + download.hash + "_" + download.len);
            }

            FileStream fs = new FileStream(filePath, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(md5);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }

        private void InitLocalAssets(List<string> assetList)
        {
            _localAssets.Clear();

            //string[] assetList =  text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i< assetList.Count; i++)
            {
                string[] assetInfo = assetList[i].Split('_');
                if (assetInfo.Length < 3)
                {
                    continue;
                }
                VFile vFile = null;
                string name = assetInfo[1];
                _localAssets.TryGetValue(name, out vFile);
                if (vFile ==null )
                {
                    vFile = new VFile();
                    _localAssets.Add(name, vFile);
                }

                vFile.id = int.Parse(assetInfo[0]);
                vFile.name = name;
                vFile.hash = assetInfo[1];
                vFile.len = int.Parse(assetInfo[2]);

            }
            UnityEngine.Debug.Log("_localAssets.Count: " + _localAssets.Count.ToString());
        }

        private IEnumerator Checking()
        {
            if (_step == Step.Wait)
            {
                enableVFS = false;
#if UNITY_IPHONE
                enableVFS = false;
#endif
                _step = Step.Copy;
            }

            if (_step == Step.Copy)
            {
                yield return RequestCopy();
            }

            if (_step == Step.Coping)   //本地流目录版本高于或等于网络目录版本
            {
                var path = _savePath + Versions.Filename + ".tmp";
                //获取ver的数据
                var versions = Versions.LoadVersions(path);
                var basePath = GetBasePath();
                yield return UpdateCopy(versions, basePath);
                _step = Step.Versions;
            }

            if (_step == Step.Versions)   //本地流版本低于网络版本
            {
                var path = _savePath + Versions.Filename + ".tmp";
                var versions = Versions.LoadVersions(path);
                yield return RequestVersions();
            }

            if (_step == Step.Prepared)
            {
                OnMessage("正在检查版本信息...");
                var totalSize = _downloader.size;
                if (totalSize > 0)
                {
                    var tips = string.Format("发现内容更新，总计需要下载 {0} 内容", Downloader.GetDisplaySize(totalSize));
                    var mb = MessageBox.Show("提示", tips, "下载", "退出");
                    yield return mb;
                    if (mb.isOk)
                    {
                        _downloader.StartDownload();
                        _step = Step.Download;
                    }
                    else
                    {
                        Quit();
                    }
                }
                else
                {
                    OnComplete();
                }
            }
        }

        private IEnumerator RequestVersions()
        {
            OnMessage("正在获取版本信息...");
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                var mb = MessageBox.Show("提示", "请检查网络连接状态", "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                }

                yield break;
            }

            //1
            string downuri = GetDownloadURL(Versions.Filename);
            var request = UnityWebRequest.Get(downuri);
            request.downloadHandler = new DownloadHandlerFile(_savePath + Versions.Filename);
            yield return request.SendWebRequest();
            var error = request.error;
            request.Dispose();

            //2
            //var error = "";
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(GetDownloadURL(Versions.Filename));
            //request.Method = "GET";
            //HttpWebResponse response = null;
            //try
            //{
            //    response = (HttpWebResponse)request.GetResponse();

            //    //获取文件名
            //    string fileName = response.Headers["Content-Disposition"];//attachment;filename=FileName.txt
            //    string contentType = response.Headers["Content-Type"];//attachment;

            //    if (string.IsNullOrEmpty(fileName))
            //        fileName = response.ResponseUri.Segments[response.ResponseUri.Segments.Length - 1];
            //    else
            //        fileName = fileName.Remove(0, fileName.IndexOf("filename=") + 9);

            //    //直到request.GetResponse()程序才开始向目标网页发送Post请求
            //    using (Stream responseStream = response.GetResponseStream())
            //    {
            //        long totalLength = response.ContentLength;

            //        ///文件byte形式
            //        using (Stream stream = new FileStream(Path.Combine(_savePath, fileName), FileMode.Create))
            //        {
            //            byte[] bArr = new byte[1024];
            //            int size;
            //            while ((size = responseStream.Read(bArr, 0, bArr.Length)) > 0)
            //            {
            //                stream.Write(bArr, 0, size);
            //            }
            //        }
            //    }
            //}
            //catch (WebException ex)
            //{
            //    error = ex.Message;
            //    response = (HttpWebResponse)ex.Response;
            //}

            if (!string.IsNullOrEmpty(error))
            {
                var mb = MessageBox.Show("提示", string.Format("获取服务器版本失败：{0}", error), "重试", "退出");
                yield return mb;
                if (mb.isOk)
                {
                    StartUpdate();
                }
                else
                {
                    Quit();
                }

                yield break;
            }

            try
            {
                var v1 = Versions.LoadVersion(_savePath + Versions.Filename);           //网络版本文件
                var v2 = Versions.LoadVersion(_savePath + Versions.Filename + ".tmp");  //本地临时文件

                Log.ILog.Debug($"网络版本：{v1}， 本地版本{v2}");

                if (v2 > v1)
                {
                    //如果本地版本高于网络版本，就别更新了
                    OnComplete();
                    yield break;
                }
                //网络版本高于或者等于本地版本，则检查更新
                _versions = Versions.LoadVersions(_savePath + Versions.Filename, true);
               
                if (_versions.Count > 0)
                {
                    PrepareDownloads();
                    _step = Step.Prepared;
                }
                else
                {
                    OnComplete();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                MessageBox.Show("提示", "版本文件加载失败", "重试", "退出").onComplete +=
                    delegate(MessageBox.EventId id)
                    {
                        if (id == MessageBox.EventId.Ok)
                        {
                            StartUpdate();
                        }
                        else
                        {
                            Quit();
                        }
                    };
            }
        }

        private static string GetBasePath()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath + "/";
            }

            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                return "file:///" + Application.streamingAssetsPath + "/";
            }

            return "file://" + Application.streamingAssetsPath + "/";
        }

        private IEnumerator RequestCopy()
        {
            //先把APK的ver文件下载到缓存目录ver.tmp
            // _savePath    C:/Users/Admin/AppData/LocalLow/yinghuochong/危境/DLC/" 
            // basePath "file:///H:/GitWeiJing/Unity/Assets/StreamingAssets/"   
            var v1 = Versions.LoadVersion(_savePath + Versions.Filename);
            var basePath = GetBasePath();
            var request = UnityWebRequest.Get(Path.Combine(basePath, Versions.Filename));
            var path = _savePath + Versions.Filename + ".tmp";
            request.downloadHandler = new DownloadHandlerFile(path);
            yield return request.SendWebRequest();
            var v2 = -1;
            var hasFile = string.IsNullOrEmpty(request.error);
            if (hasFile) { v2 = Versions.LoadVersion(path); }
            var steamFileThenSave = v2 >= v1;
            if (steamFileThenSave) { Debug.LogWarning("本地流目录版本高于或等于网络目录版本"); }
            _step = hasFile && steamFileThenSave ? Step.Coping : Step.Versions;
            request.Dispose();
        }

        private IEnumerator UpdateCopy(IList<VFile> versions, string basePath)
        {
            var version = versions[0];
            if (version.name.Equals(Versions.Dataname))
            {
                var request = UnityWebRequest.Get(Path.Combine(basePath, version.name));
                request.downloadHandler = new DownloadHandlerFile(_savePath + version.name);
                var req = request.SendWebRequest();
                while (!req.isDone)
                {
                    OnMessage("正在复制文件");
                    OnProgress(req.progress);
                    yield return null;
                }

                request.Dispose();
            }
            else
            {
                for (var index = 0; index < versions.Count; index++)
                {
                    var item = versions[index];
                    var request = UnityWebRequest.Get(Path.Combine(basePath, item.name));
                    request.downloadHandler = new DownloadHandlerFile(_savePath + item.name);
                    yield return request.SendWebRequest();
                    request.Dispose();
                    OnMessage(string.Format("正在复制文件：{0}/{1}", index, versions.Count));
                    OnProgress(index * 1f / versions.Count);
                }
            }
        }

        private void OnComplete()
        {
            if (enableVFS)
            {
                var dataPath = _savePath + Versions.Dataname;
                var downloads = _downloader.downloads;
                if (downloads.Count > 0 && File.Exists(dataPath))
                {
                    OnMessage("更新本地版本信息");
                    var files = new List<VFile>(downloads.Count);
                    foreach (var download in downloads)
                    {
                        files.Add(new VFile
                        {
                            name = download.name,
                            hash = download.hash,
                            len = download.len,
                        });
                    }

                    var file = files[0];
                    if (!file.name.Equals(Versions.Dataname))
                    {
                        Versions.UpdateDisk(dataPath, files);
                    }
                }

                Versions.LoadDisk(dataPath);
            }

            OnProgress(1);
            OnMessage("更新完成");
            var version = Versions.LoadVersion(_savePath + Versions.Filename);
            if (version > 0)
            {
                OnVersion("资源版本号: " + version.ToString());
            }

            passTime = -10000000f;
            UpdateLocalAsset();
            StartCoroutine(LoadGameScene());
        }

        private IEnumerator LoadGameScene()
        {
            OnMessage("正在初始化游戏");
#if UNITY_EDITOR
            Assets.runtimeMode = !development;
#endif
            var init = Assets.Initialize();
            yield return init;
            if (string.IsNullOrEmpty(init.error))
            {
                Assets.AddSearchPath("Assets/Bundles/Jpg");
                Assets.AddSearchPath("Assets/Bundles/Material");
                Assets.AddSearchPath("Assets/Bundles/Other");
                Assets.AddSearchPath("Assets/Bundles/Prefab");
                Assets.AddSearchPath("Assets/Bundles/Text");
                Assets.AddSearchPath("Assets/Bundles/Config");
                Assets.AddSearchPath("Assets/Bundles/UI");
                Assets.AddSearchPath("Assets/Bundles/Effect");
                Assets.AddSearchPath("Assets/Bundles/Independent");
                Assets.AddSearchPath("Assets/Bundles/Recast");
                Assets.AddSearchPath("Assets/Bundles/Unit");
                Assets.AddSearchPath("Assets/Bundles/Icon");
                Assets.AddSearchPath("Assets/Res/Scenes");
                init.Release();

                Init init_cs = GameObject.Find("Global").GetComponent<Init>();
                int apkversion = init_cs.BigVersion;

                HotVersion hotVersion1 = GetHotVersion();
                int hotVersion = hotVersion1.Version;
                string downloadUrl = "http://weijinghot.weijinggame.com/weijing1/apk/beta/weijing.apk";
#if UNITY_IPHONE
                downloadUrl = hotVersion1.IOS_URL;
                 apkversion = init_cs.BigVersionIOS;
                  hotVersion = hotVersion1.VersionIOS;
#else
                //downloadUrl = hotVersion1.Apk_URL + init_cs.Apk_Extension + ".apk";
                downloadUrl = hotVersion1.Apk_URL;
                apkversion = init_cs.BigVersion;
                hotVersion = hotVersion1.Version;
#endif
                if (apkversion < hotVersion)
                {
                    var mb = MessageBox.Show("提示", string.Format("应用版本过低，请重新下载：{0}, {1}", apkversion, hotVersion), "确定", "退出");
                    yield return mb;
                    if (mb.isOk)
                    {
                        Application.OpenURL(downloadUrl);
                    }
                    else
                    {
                        Quit();
                    }
                }
                else
                {
                    yield return LoadHelper.PreLoad();
                    //yield return MessageBox.Show("提示", "是否进入游戏");
                    //HotfixHelper.StartHotfix();
                    //GameObject.Find("Global").GetComponent<Init>().enabled = true;
                    GameObject.Find("Global").GetComponent<Init>().OnHotUpdateComplete();
                    //this.gameObject.SetActive(false);
                    //Destroy(this.gameObject);

                    //OnProgress(0);
                    //OnMessage("加载游戏场景");
                    //var scene = Assets.LoadSceneAsync(gameScene, false);
                    //scene.completed += (AssetRequest request) =>
                    //{
                    //    HotfixHelper.StartHotfix();
                    //};
                    //while (!scene.isDone)
                    //{
                    //    OnProgress(scene.progress);
                    //    yield return null;
                    //}
                }
            }
            else
            {
                init.Release();
                var mb = MessageBox.Show("提示", "初始化异常错误：" + init.error + "请联系技术支持");
                yield return mb;
                Quit();
            }
        }

       

        public HotVersion GetHotVersion()
        {
            var path_1 = ABPathHelper.GetTextPath("Version");
            var request = libx.Assets.LoadAsset(path_1, typeof(TextAsset));
            TextAsset textAsset3 = request.asset as TextAsset;
            //return int.Parse(textAsset3.text);
            return JsonHelper.FromJson<HotVersion>(textAsset3.text);
        }

        private void OnDestroy()
        {
            MessageBox.Dispose();
        }

        private void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}