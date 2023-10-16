﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public class UIMapBigComponent : Entity, IAwake, IDestroy
    {
        public int SceneId = 1;
        public float ScaleRateX = 1f;
        public float ScaleRateY = 1f;
        public Vector2 localPoint;
        public GameObject MapName;
        public GameObject bossIcon;
        public GameObject chuansong;
        public GameObject teamerPostion;
        public GameObject ChuanSongName;
        public GameObject jiayuanPet;
        public GameObject jiayuanRubsh;
        public GameObject TextStall;
        public GameObject RawImage;
        public GameObject Btn_Close;
        public GameObject MainPostion;
        public GameObject NpcPostion;
        public GameObject pathPoint;
        public GameObject NpcNodeList;
        public GameObject ImageSelect;
        public GameObject MapCamera;

        public MoveComponent MoveComponent;
        public List<GameObject> PathPointList = new List<GameObject>();
        public Dictionary<int, GameObject> NpcGameObject = new Dictionary<int, GameObject>();
        public Dictionary<int, Vector3> BossList = new Dictionary<int, Vector3>();
        public List<GameObject> TeamerPointList = new List<GameObject>();

        public Vector3 InvisiblePosition = new Vector3( -3000f, -3000f, 0f );
    }


    public class UIMapBigComponentDestroySystem : DestroySystem<UIMapBigComponent>
    {
        public override void Destroy(UIMapBigComponent self)
        {
            DataUpdateComponent.Instance.RemoveListener(DataType.MainHeroMove, self);
        }
    }

    public class UIMapBigComponentAwakeSystem : AwakeSystem<UIMapBigComponent>
    {

        public override void Awake(UIMapBigComponent self)
        {
            self.PathPointList.Clear();
            self.NpcGameObject.Clear();

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.TextStall = rc.Get<GameObject>("TextStall");
            self.TextStall.SetActive(false);
            self.chuansong = rc.Get<GameObject>("chuansong");
            self.ChuanSongName = rc.Get<GameObject>("ChuanSongName");
            self.chuansong.SetActive(false);
            self.MapName = rc.Get<GameObject>("MapName");
            self.bossIcon = rc.Get<GameObject>("bossIcon");
            self.bossIcon.SetActive(false);

            self.jiayuanPet = rc.Get<GameObject>("jiayuanPet");
            self.jiayuanPet.SetActive(false);

            self.jiayuanRubsh = rc.Get<GameObject>("jiayuanRubsh");
            self.jiayuanRubsh.SetActive(false);

            self.NpcNodeList = rc.Get<GameObject>("NpcNodeList");
            self.RawImage = rc.Get<GameObject>("RawImage");
            self.ImageSelect = rc.Get<GameObject>("ImageSelect");
            self.ImageSelect.SetActive(false);

            self.teamerPostion = rc.Get<GameObject>("teamerPostion");
            self.teamerPostion.SetActive(false);
            self.MainPostion = rc.Get<GameObject>("mainPostion");
            self.NpcPostion = rc.Get<GameObject>("npcPostion");
            self.pathPoint = rc.Get<GameObject>("pathPoint");
            self.Btn_Close = rc.Get<GameObject>("Btn_Close");

            self.Btn_Close.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { self.OnCloseMiniMap(); });

            ButtonHelp.AddEventTriggers(self.RawImage, (PointerEventData pdata) => { self.PointerDown(pdata); }, EventTriggerType.PointerDown);

            self.MoveComponent = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene()).GetComponent<MoveComponent>();

            self.OnAwake().Coroutine();
            self.InitNpcList().Coroutine();

            DataUpdateComponent.Instance.AddListener(DataType.MainHeroMove, self);
        }
    }

    public static class UIMapBigComponentSystem
    {
        public static async ETTask OnAwake(this UIMapBigComponent self)
        {
            GameObject mapCamera = GameObject.Find("MapCamera");
            if (mapCamera == null)
            {
                var path = ABPathHelper.GetUnitPath("Component/MapCamera");
                GameObject prefab = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
                mapCamera = GameObject.Instantiate(prefab);
                mapCamera.name = "MapCamera";
            }
            Camera camera = mapCamera.GetComponent<Camera>();
            camera.enabled = true;

            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
            {
                DungeonConfig dungeonConfig = DungeonConfigCategory.Instance.Get(mapComponent.SceneId);
                mapCamera.transform.position = new Vector3((float)dungeonConfig.CameraPos[0], (float)dungeonConfig.CameraPos[1], (float)dungeonConfig.CameraPos[2]);
                mapCamera.transform.eulerAngles = new Vector3(90, 0, (float)dungeonConfig.CameraPos[3]);
                camera.orthographicSize = (float)dungeonConfig.CameraPos[4];
            }
            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.MainCityScene
                || mapComponent.SceneTypeEnum == (int)SceneTypeEnum.TeamDungeon)
            {
                SceneConfig dungeonConfig = SceneConfigCategory.Instance.Get(mapComponent.SceneId);
                mapCamera.transform.position = new Vector3((float)dungeonConfig.CameraPos[0], (float)dungeonConfig.CameraPos[1], (float)dungeonConfig.CameraPos[2]);
                mapCamera.transform.eulerAngles = new Vector3(90, 0, (float)dungeonConfig.CameraPos[3]);
                camera.orthographicSize = (float)dungeonConfig.CameraPos[4];
            }
            self.MapCamera = mapCamera;

            self.SceneId = self.ZoneScene().GetComponent<MapComponent>().SceneId;
            self.ScaleRateX = self.RawImage.GetComponent<RectTransform>().rect.height / (camera.orthographicSize * 2);
            self.ScaleRateY = self.RawImage.GetComponent<RectTransform>().rect.height / (camera.orthographicSize * 2);

            self.OnMainHeroMove();
            self.MainPostion.transform.Find("Text").GetComponent<Text>().text = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Name;

            await TimerComponent.Instance.WaitAsync(200);
            if (self.IsDisposed)
            {
                return;
            }

            camera.enabled = false;
            self.ZoneScene().GetComponent<GuideComponent>().OnTrigger(GuideTriggerType.OpenUI, UIType.UIMapBig);
        }

        public static void ShowStallArea(this UIMapBigComponent self)
        {
            int[] stallArea = SceneConfigCategory.Instance.Get(self.SceneId).StallArea;
            if (stallArea != null && stallArea.Length == 4 && self.NpcPostion != null)
            {
                /*
                Vector3 stallPosition = new Vector3(stallArea[0] * 0.01f * self.ScaleRateX, stallArea[2] * 0.01f * self.ScaleRateY, 0);
                self.TextStall.SetActive(true);
                self.TextStall.transform.SetParent(self.NpcPostion.transform.parent);
                self.TextStall.transform.localPosition = stallPosition;
                */
            }
        }

        public static void ShowChuansong(this UIMapBigComponent self)
        {
            int[] transfers = DungeonConfigCategory.Instance.Get(self.SceneId).TransmitPos;
            if (transfers == null || transfers.Length == 0)
            {
                return;
            }
            for (int i = 0; i < transfers.Length; i++)
            {
                if (transfers[i] == 0)
                {
                    continue;
                }
                DungeonTransferConfig npcConfig = DungeonTransferConfigCategory.Instance.Get(transfers[i]);
                Vector3 npcPos = self.GetWordToUIPositon(new Vector3(npcConfig.Position[0] * 0.01f, npcConfig.Position[2] * 0.01f, 0));

                GameObject gameObject = GameObject.Instantiate(self.chuansong);
                gameObject.SetActive(true);
                gameObject.transform.SetParent(self.chuansong.transform.parent);
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = npcPos;
                gameObject.transform.Find("Text").GetComponent<Text>().text = npcConfig.Name;
            }
        }

        public static void ShowLocalBossList(this UIMapBigComponent self)
        {
            //DungeonConfig chapterSonConfig = DungeonConfigCategory.Instance.Get(self.SceneId);
            self.CreateMonsterList(SceneConfigHelper.GetLocalDungeonMonsters_2(self.SceneId));
        }

        public static async ETTask RequestJiaYuanInfo(this UIMapBigComponent self)
        {
            JiaYuanComponent jiaYuanComponent = self.ZoneScene().GetComponent<JiaYuanComponent>();
            C2M_JiaYuanPetPositionRequest  request  = new C2M_JiaYuanPetPositionRequest() { MasterId = jiaYuanComponent.MasterId };
            M2C_JiaYuanPetPositionResponse response = (M2C_JiaYuanPetPositionResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (self.IsDisposed)
            {
                return;
            }

            for (int i = 0; i < response.PetList.Count; i++)
            {
                UnitInfo unitInfo = response.PetList[i];
                Vector3 vector3 = new Vector3(unitInfo.X, unitInfo.Z, 0);
                Vector3 npcPos = self.GetWordToUIPositon(vector3);

                GameObject gameObject = GameObject.Instantiate(unitInfo.UnitType == UnitType.Pet ? self.jiayuanPet : self.jiayuanRubsh);
                gameObject.SetActive(true);
                gameObject.transform.SetParent(self.jiayuanPet.transform.parent);
                gameObject.transform.localScale = Vector3.one;
                gameObject.transform.localPosition = npcPos;

                if (unitInfo.UnitType == UnitType.Pet)
                {
                    PetConfig petConfig = PetConfigCategory.Instance.Get(unitInfo.ConfigId);
                    gameObject.transform.Find("Text").GetComponent<Text>().text = petConfig.PetName;
                }
                if (unitInfo.UnitType == UnitType.Monster)
                {
                    MonsterConfig petConfig = MonsterConfigCategory.Instance.Get(unitInfo.ConfigId);
                    gameObject.transform.Find("Text").GetComponent<Text>().text = petConfig.MonsterName;
                }
            }    
        }

        public static async ETTask RequestTeamerPosition(this UIMapBigComponent self)
        {
            long instanceid = self.InstanceId;
            while(true)
            {
                C2M_TeamerPositionRequest request = new C2M_TeamerPositionRequest();
                M2C_TeamerPositionResponse response = (M2C_TeamerPositionResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
                if (instanceid != self.InstanceId)
                {
                    break;
                }
                self.OnUpdateTeamerList(response.UnitList);

                await TimerComponent.Instance.WaitAsync( TimeHelper.Second * 5 );
                if (instanceid != self.InstanceId)
                {
                    break;
                }
            }
        }

        public static void OnUpdateTeamerList(this UIMapBigComponent self, List<UnitInfo> unitInfos)
        {
            for (int i = 0; i < unitInfos.Count; i++)
            {
                UnitInfo unitInfo = unitInfos[i];   
                Vector3  vector31 = self.GetWordToUIPositon(new Vector3() { x = unitInfo.X, y  = unitInfo.Z, z = 0 });
                GameObject go = null;
                if (i < self.TeamerPointList.Count)
                {
                    go = self.TeamerPointList[i];
                }
                else
                {
                    go = GameObject.Instantiate(self.teamerPostion);
                    go.SetActive(true);
                    go.transform.SetParent(self.teamerPostion.transform.parent);
                    go.transform.localScale = Vector3.one;
                    self.TeamerPointList.Add(go);
                }

                go.transform.localPosition = vector31;
                go.transform.localScale = Vector3.one * 2f;
                go.transform.Find("Text").GetComponent<Text>().text = unitInfo.UnitName;
            }
            for (int i = unitInfos.Count; i < self.TeamerPointList.Count; i++)
            {
                self.TeamerPointList[i].transform.localPosition = self.InvisiblePosition;
            }
        }

        public static void ShowTeamBossList(this UIMapBigComponent self)
        {
            SceneConfig chapterSonConfig = SceneConfigCategory.Instance.Get(self.SceneId);
            self.CreateMonsterList(chapterSonConfig.CreateMonster);

            if (chapterSonConfig.CreateMonsterPosi != null)
            {
                for (int i = 0; i < chapterSonConfig.CreateMonsterPosi.Length; i++)
                {
                    int monsterId = chapterSonConfig.CreateMonsterPosi[i];
                    while (monsterId != 0)
                    {
                        monsterId = self.CreateMonsterByPos(monsterId);
                    }
                }
            }
        }

        public static int CreateMonsterByPos(this UIMapBigComponent self, int monsterId)
        {
            MonsterPositionConfig monsterPosition = MonsterPositionConfigCategory.Instance.Get(monsterId);
            int monsterid = monsterPosition.MonsterID;
            string[] position = monsterPosition.Position.Split(',');
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterid);
            if (monsterConfig.MonsterType != (int)MonsterTypeEnum.Boss)
            {
                return monsterPosition.NextID; ;
            }
            Vector3 vector3 = new Vector3(float.Parse(position[0]), float.Parse(position[2]), 0);
            Vector3 npcPos = self.GetWordToUIPositon(vector3);

            GameObject gameObject = GameObject.Instantiate(self.bossIcon);
            gameObject.SetActive(true);
            gameObject.transform.SetParent(self.bossIcon.transform.parent);
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = npcPos;
            gameObject.transform.Find("Text").GetComponent<Text>().text = monsterConfig.MonsterName;

            self.BossList.Add(monsterConfig.Id, new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2])));

            return monsterPosition.NextID;
        }

        public static void CreateMonsterList(this UIMapBigComponent self, string createMonster)
        {
            string[] monsters = createMonster.Split('@');
            for (int i = 0; i < monsters.Length; i++)
            {
                if (ComHelp.IfNull(monsters[i]))
                {
                    continue;
                }
                try
                {
                    //1;37.65,0,3.2;70005005;1
                    string[] mondels = monsters[i].Split(';');
                    string[] mtype = mondels[0].Split(',');
                    string[] position = mondels[1].Split(',');
                    string[] monsterid = mondels[2].Split(',');

                    if (mtype[0] != "1" && mtype[0] != "2")
                    {
                        continue;
                    }
                    MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(int.Parse(monsterid[0]));
                    if (monsterConfig.MonsterType != (int)MonsterTypeEnum.Boss)
                    {
                        continue;
                    }
                    Vector3 vector3 = new Vector3(float.Parse(position[0]), float.Parse(position[2]), 0);
                    Vector3 npcPos = self.GetWordToUIPositon(vector3);

                    GameObject gameObject = GameObject.Instantiate(self.bossIcon);
                    gameObject.SetActive(true);
                    gameObject.transform.SetParent(self.bossIcon.transform.parent);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.localPosition = npcPos;
                    gameObject.transform.Find("Text").GetComponent<Text>().text = monsterConfig.MonsterName;

                    self.BossList.Add(monsterConfig.Id, new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2])));
                }
                catch (Exception ex)
                {
                    Log.Debug(monsters[i] + "  " + ex.ToString());
                }
            }
        }

        public static async ETTask InitNpcList(this UIMapBigComponent self)
        {
            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            int[] npcList = null;

            if (SceneConfigHelper.UseSceneConfig(mapComponent.SceneTypeEnum))
            {
                npcList = SceneConfigCategory.Instance.Get(self.SceneId).NpcList;
                self.MapName.GetComponent<Text>().text = SceneConfigCategory.Instance.Get(self.SceneId).Name;
                self.ShowStallArea();
            }
            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
            {
                npcList = DungeonConfigCategory.Instance.Get(self.SceneId).NpcList;
                self.MapName.GetComponent<Text>().text = DungeonConfigCategory.Instance.Get(self.SceneId).ChapterName;
                self.ShowChuansong();
                self.ShowLocalBossList();
            }
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.JiaYuan)
            {
                self.RequestJiaYuanInfo().Coroutine();
            }
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.TeamDungeon)
            {
                self.RequestTeamerPosition().Coroutine();
                self.ShowTeamBossList();
            }
 
            var path = ABPathHelper.GetUGUIPath("Main/MiniMap/UIMapBigNpcItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            GameObject mapCamera = self.MapCamera;
            if (npcList != null)
            {
                for (int i = 0; i < npcList.Length; i++)
                {
                    if (!NpcConfigCategory.Instance.Contain(npcList[i]))
                    {
                        continue;
                    }
                    if (npcList[i] == 20000040)
                    {
                        PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
                        if (!PetHelper.IsShenShouFull(petComponent.RolePetInfos))
                        {
                            continue;
                        }
                    }

                    NpcConfig npcConfig = NpcConfigCategory.Instance.Get(npcList[i]);
                    Vector3 npcPos = Vector3.zero;

                    npcPos = self.GetWordToUIPositon(new Vector3(npcConfig.Position[0] * 0.01f, npcConfig.Position[2] * 0.01f, 0));

                    GameObject gameObject = GameObject.Instantiate(self.NpcPostion);
                    gameObject.SetActive(true);
                    gameObject.transform.SetParent(self.NpcPostion.transform.parent);
                    gameObject.transform.localScale = Vector3.one;
                    gameObject.transform.localPosition = npcPos;
                    gameObject.transform.Find("Text").GetComponent<Text>().text = npcConfig.Name;

                    GameObject npcGo = GameObject.Instantiate(bundleGameObject);
                    UICommonHelper.SetParent(npcGo, self.NpcNodeList);
                    UI uI = self.AddChild<UI, string, GameObject>("IMapBigNpcItem", npcGo);
                    UIMapBigNpcItemComponent uIItemComponent = uI.AddComponent<UIMapBigNpcItemComponent>();
                    uIItemComponent.SetClickHandler(UnitType.Npc, npcList[i], (int unitype, int npcid) => { self.OnClickNpcItem(unitype, npcid); });
                    self.NpcGameObject.Add(npcList[i], npcGo);
                }
            }

            foreach (var item in self.BossList)
            {
                GameObject npcGo = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(npcGo, self.NpcNodeList);
                UI uI = self.AddChild<UI, string, GameObject>("IMapBigNpcItem", npcGo);
                UIMapBigNpcItemComponent uIItemComponent = uI.AddComponent<UIMapBigNpcItemComponent>();
                uIItemComponent.SetClickHandler(UnitType.Monster, item.Key, (int unitype, int npcid) => { self.OnClickNpcItem(unitype, npcid); });
                self.NpcGameObject.Add(item.Key, npcGo);
            }
        }

        public static void OnClickNpcItem(this UIMapBigComponent self,  int unitype, int configid)
        {
            self.ImageSelect.SetActive(true);
            UICommonHelper.SetParent(self.ImageSelect, self.NpcGameObject[configid]);
            self.ImageSelect.transform.SetSiblingIndex(0);

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            if (ErrorCode.ERR_Success != unit.GetComponent<StateComponent>().CanMove())
                return;

            if (unitype == UnitType.Npc)
            {
                self.ZoneScene().CurrentScene().GetComponent<OperaComponent>().OnClickNpc(configid, "1").Coroutine();
            }
            else
            {
                unit.MoveToAsync2(self.BossList[configid], false).Coroutine();
            }
        }

        public static void PointerDown(this UIMapBigComponent self, PointerEventData pdata)
        { 
            Scene curScene = self.ZoneScene().CurrentScene();
            if (curScene == null)
            {
                return;
            }
            Unit unit = UnitHelper.GetMyUnitFromCurrentScene(curScene);
            if (unit == null)
            {
                return;
            }
            GameObject mapCamera = self.MapCamera;
            RectTransform canvas = self.RawImage.GetComponent<RectTransform>();
            Camera uiCamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out self.localPoint);

            Quaternion rotation = Quaternion.Euler(0,  mapCamera.transform.eulerAngles.y, 0);
          
            Vector3 wordpos = new Vector3(self.localPoint.x / self.ScaleRateX, 100f, self.localPoint.y / self.ScaleRateY);
            wordpos = rotation * wordpos;

            wordpos.x += (mapCamera.transform.position.x);
            wordpos.z += (mapCamera.transform.position.z );

            RaycastHit hit;
            int mapMask = (1 << LayerMask.NameToLayer(LayerEnum.Map.ToString()));
            Physics.Raycast(wordpos, Vector3.down, out hit, 1000, mapMask);

            if (hit.collider != null)
            {
                EventType.DataUpdate.Instance.DataType = DataType.BeforeMove;
                EventType.DataUpdate.Instance.DataParamString = "1";
                Game.EventSystem.PublishClass(EventType.DataUpdate.Instance);
                unit.MoveToAsync2(hit.point).Coroutine();
            }
        }

        public static Vector3 GetWordToUIPositon(this UIMapBigComponent self, Vector3 vector3)
        {
            GameObject mapCamera = self.MapCamera;
            vector3.x -= mapCamera.transform.position.x;
            vector3.y -= mapCamera.transform.position.z;

            Quaternion rotation = Quaternion.Euler(0, 0, 1 * mapCamera.transform.eulerAngles.y);
            vector3 = rotation * vector3;

            vector3.x *= self.ScaleRateX;
            vector3.y *= self.ScaleRateY;
            return vector3;
        }

        public static void OnCloseMiniMap(this UIMapBigComponent self)
        {
            UI ui =  UIHelper.GetUI(self.ZoneScene(), UIType.UIGuide);
            if (ui != null)
            {
                UIGuideComponent uIGuideComponent = ui.GetComponent<UIGuideComponent>();
                self.ZoneScene().GetComponent<GuideComponent>().OnNext(uIGuideComponent.guidCof.GroupId);
            }
            UIHelper.Remove(self.DomainScene(), UIType.UIMapBig);
        }

        public static void OnMainHeroMove(this UIMapBigComponent self)
        {
            Vector3 vector3 = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene()).Position;
            Vector3 vector31 = new Vector3(vector3.x, vector3.z, 0f);
            self.MainPostion.transform.localPosition = self.GetWordToUIPositon(vector31);

            self.UpdatePathPoint();
        }

        public static GameObject GetPathPointObj(this UIMapBigComponent self, int index)
        {
            if (self.PathPointList.Count > index)
            {
                return self.PathPointList[index];
            }
            GameObject go = GameObject.Instantiate(self.pathPoint);
            go.transform.SetParent(self.pathPoint.transform.parent);
            go.transform.localScale = Vector3.one;
            self.PathPointList.Add(go);
            return go;
        }

        public static void UpdatePathPoint(this UIMapBigComponent self)
        {
            int N = self.MoveComponent.N;
            List<Vector3> target = self.MoveComponent.Targets;
            Vector3 lastVector = new Vector3(-1000,-1000,0);
            int showNumber = 0;
            for (int i = target.Count - 1; i >= N; i--)
            {
                Vector3 temp = target[i];
                Vector3 vector31 = new Vector3(temp.x, temp.z, 0f);
                vector31 = self.GetWordToUIPositon(vector31);

                if (Vector3.Distance(vector31, lastVector) > 20f)
                {
                    GameObject go = self.GetPathPointObj(showNumber);
                    showNumber++;
                    go.transform.localPosition = vector31;
                    go.transform.localScale = Vector3.one * 2f;
                    lastVector = vector31;
                }
            }
            for (int i = showNumber; i < self.PathPointList.Count; i++)
            {
                self.PathPointList[i].transform.localPosition = self.InvisiblePosition;
            }
        }

    }
}