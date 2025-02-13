using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    [Timer(TimerType.JoystickTimer)]
    public class JoystickTimer : ATimer<UIJoystickMoveComponent>
    {
        public override void Run(UIJoystickMoveComponent self)
        {
            try
            {
                self.OnUpdate();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class UIJoystickMoveComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject Thumb;
        public GameObject CenterShow;

        public Image ThumbImage;
        public Image CenterShowImage;

        public GameObject YaoGanDiMove;
        public GameObject YaoGanDiFix;

        public Vector2 OldPoint;
        public Vector2 NewPoint;
        public float Distance = 110;
        public long lastSendTime;
        public long checkTime;
        public long noCheckTime;

        public int direction;
        public int lastDirection;

        public Camera UICamera;
        public Camera MainCamera;
        public float LastShowTip;

        public Unit MainUnit;
        public NumericComponent NumericComponent;
        public AttackComponent AttackComponent;
       
        public GameObject GameObject;

        public int ObstructLayer;
        public int BuildingLayer;
        public int MapLayer;
        public long Timer;

        public int OperateMode;
        public int SceneTypeEnum;
    }


    public class UIJoystickMoveComponentDestroy : DestroySystem<UIJoystickMoveComponent>
    {
        public override void Destroy(UIJoystickMoveComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
        }
    }


    public class UIJoystickMoveComponentAwake : AwakeSystem<UIJoystickMoveComponent, GameObject>
    {
        public override void Awake(UIJoystickMoveComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.direction = 0;
            self.lastDirection = 0;
            self.CenterShow = rc.Get<GameObject>("CenterShow");

            self.YaoGanDiMove = rc.Get<GameObject>("YaoGanDiMove");
            self.YaoGanDiFix = rc.Get<GameObject>("YaoGanDiFix");

            self.Thumb = rc.Get<GameObject>("Thumb");

            self.ThumbImage = rc.Get<GameObject>("Thumb").GetComponent<Image>();

            self.CenterShowImage = rc.Get<GameObject>("Thumb").GetComponent<Image>();

            ButtonHelp.AddEventTriggers(self.YaoGanDiMove, (PointerEventData pdata) => { self.PointerDown_Move(pdata); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(self.YaoGanDiMove, (PointerEventData pdata) => { self.BeginDrag(pdata); }, EventTriggerType.BeginDrag);
            ButtonHelp.AddEventTriggers(self.YaoGanDiMove, (PointerEventData pdata) => { self.Draging(pdata); }, EventTriggerType.Drag);
            ButtonHelp.AddEventTriggers(self.YaoGanDiMove, (PointerEventData pdata) => { self.EndDrag(pdata); }, EventTriggerType.EndDrag);
            ButtonHelp.AddEventTriggers(self.YaoGanDiMove, (PointerEventData pdata) => { self.EndDrag(pdata); }, EventTriggerType.PointerUp);

            ButtonHelp.AddEventTriggers(self.YaoGanDiFix, (PointerEventData pdata) => { self.PointerDown_Fix(pdata); }, EventTriggerType.PointerDown);
            //ButtonHelp.AddEventTriggers(self.YaoGanDiFix, (PointerEventData pdata) => { self.BeginDrag(pdata); }, EventTriggerType.BeginDrag);
            ButtonHelp.AddEventTriggers(self.YaoGanDiFix, (PointerEventData pdata) => { self.Draging(pdata); }, EventTriggerType.Drag);
            ButtonHelp.AddEventTriggers(self.YaoGanDiFix, (PointerEventData pdata) => { self.EndDrag(pdata); }, EventTriggerType.EndDrag);
            ButtonHelp.AddEventTriggers(self.YaoGanDiFix, (PointerEventData pdata) => { self.EndDrag(pdata); }, EventTriggerType.PointerUp);


            self.UICamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            self.MainCamera = self.DomainScene().GetComponent<UIComponent>().MainCamera;
            self.AttackComponent = self.ZoneScene().GetComponent<AttackComponent>();

            self.ObstructLayer = (1 << LayerMask.NameToLayer(LayerEnum.Obstruct.ToString()));
            self.BuildingLayer = (1 << LayerMask.NameToLayer(LayerEnum.Building.ToString()));
            self.MapLayer = (1 << LayerMask.NameToLayer(LayerEnum.Map.ToString()));

            self.ResetUI();
            self.AfterEnterScene();
        }
    }

    public static class UIJoystickMoveComponentSystem
    {

        public static void UpdateOperateMode(this UIJoystickMoveComponent self, int operateMode)
        {
            self.OperateMode = operateMode;

            // 0固定 1移动
            self.YaoGanDiFix.SetActive( operateMode == 0 );
            self.YaoGanDiMove.SetActive(operateMode == 1 );

            //self.YaoGanDiFix.transform.localPosition = new Vector3 (434, 376, 0 );
         
            self.CenterShow.transform.SetParent(operateMode == 0 ? self.YaoGanDiFix.transform : self.YaoGanDiMove.transform);
            self.Thumb.transform.transform.SetParent(operateMode == 0 ? self.YaoGanDiFix.transform : self.YaoGanDiMove.transform);

            self.CenterShow.SetActive(self.OperateMode == 0);
            self.Thumb.SetActive(self.OperateMode == 0);

            self.SetAlpha(0.3f);

            self.CenterShow.transform.localPosition = Vector3.zero;
            self.Thumb.transform.localPosition = Vector3.zero;
        }

        public static void SetAlpha(this UIJoystickMoveComponent self, float value)
        {
            Color color_1 = new Color(1f, 1f, 1f);
            color_1.a = value;
            self.CenterShowImage.color = color_1;
            self.ThumbImage.color = color_1;
        }

        public static void PointerDown_Move(this UIJoystickMoveComponent self, PointerEventData pdata)
        {
            RectTransform canvas = self.GetYaoGanDi().GetComponent<RectTransform>();
            Camera uiCamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out self.OldPoint);
            self.SetAlpha(1f);
            if (self.OperateMode == 0)
            {
                self.YaoGanDiFix.SetActive(true);
                self.CenterShow.transform.localPosition = Vector3.zero;
                self.Thumb.transform.localPosition = Vector3.zero;
                self.OldPoint = Vector2.zero;
            }
            else
            {
                self.YaoGanDiFix.SetActive(true);
                self.CenterShow.SetActive(true);
                self.Thumb.SetActive(true);
                self.CenterShow.transform.localPosition = new Vector3(self.OldPoint.x, self.OldPoint.y, 0f);
                self.Thumb.transform.localPosition = new Vector3(self.OldPoint.x, self.OldPoint.y, 0f);
            }

            //MapHelper.LogMoveInfo($"移动摇杆按下: {TimeHelper.ServerNow()}");
        }

        /// <summary>
        /// 按下就移动
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pdata"></param>
        public static void PointerDown_Fix(this UIJoystickMoveComponent self, PointerEventData pdata)
        {
            RectTransform canvas = self.GetYaoGanDi().GetComponent<RectTransform>();
            Camera uiCamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out self.OldPoint);
            self.SetAlpha(1f);
            if (self.OperateMode == 0)
            {
                self.YaoGanDiFix.SetActive(true);
                self.CenterShow.transform.localPosition = Vector3.zero;
                self.Thumb.transform.localPosition = Vector3.zero;
                self.OldPoint = Vector2.zero;
            }
            else
            {
                self.YaoGanDiFix.SetActive(true);
                self.CenterShow.SetActive(true);
                self.Thumb.SetActive(true);
                self.CenterShow.transform.localPosition = new Vector3(self.OldPoint.x, self.OldPoint.y, 0f);
                self.Thumb.transform.localPosition = new Vector3(self.OldPoint.x, self.OldPoint.y, 0f);
            }
            //MapHelper.LogMoveInfo($"移动摇杆按下: {TimeHelper.ServerNow()}");
            TimerComponent.Instance.Remove(ref self.Timer);
            self.BeginDrag(pdata);
        }

        public static void BeginDrag(this UIJoystickMoveComponent self, PointerEventData pdata)
        {
            Unit unit = self.MainUnit;
            if (unit == null || unit.IsDisposed)
            {
                return;
            }
            
            //MapHelper.LogMoveInfo($"移动摇杆拖动: {TimeHelper.ServerNow()}");
            self.lastSendTime = 0;
            self.direction = self.GetDirection(pdata);
            self.SendMove(self.direction);
            TimerComponent.Instance?.Remove(ref self.Timer);
            self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.JoystickTimer, self);

            if (SettingHelper.ClintFindPath)
            {
                EventType.MoveStart.Instance.Unit = unit;
                Game.EventSystem.PublishClass(EventType.MoveStart.Instance);
                unit.Rotation = Quaternion.Euler(0, self.direction, 0);
            }
        }

        public static GameObject GetYaoGanDi(this UIJoystickMoveComponent self)
        {
            return self.OperateMode == 0 ? self.YaoGanDiFix : self.YaoGanDiMove;
        }

        public static int GetDirection(this UIJoystickMoveComponent self, PointerEventData pdata)
        {
            RectTransform canvas = self.GetYaoGanDi().GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, self.UICamera, out self.NewPoint);

            Vector3 vector3 = new Vector3(self.NewPoint.x, self.NewPoint.y, 0f);
            float maxDistance = Vector2.Distance(self.OldPoint, self.NewPoint);
            if (maxDistance < self.Distance)
            {
                self.Thumb.transform.localPosition = vector3;
            }
            else
            {
                self.NewPoint = self.OldPoint + (self.NewPoint - self.OldPoint).normalized * self.Distance;
                vector3.x = self.NewPoint.x;
                vector3.y = self.NewPoint.y;
                self.Thumb.transform.localPosition = vector3;
            }

            Vector2 indicator = self.NewPoint - self.OldPoint;
            int angle = 90 - (int)(Mathf.Atan2(indicator.y, indicator.x) * Mathf.Rad2Deg) + (int)self.MainCamera.transform.eulerAngles.y;
            angle = (angle - angle % 15);
            return angle;
        }

        public static void Draging(this UIJoystickMoveComponent self, PointerEventData pdata)
        {
            self.direction = self.GetDirection(pdata);
        }

        public static void OnUpdate(this UIJoystickMoveComponent self)
        {
            self.SendMove(self.direction);
        }

        public static void OnMainHeroMove(this UIJoystickMoveComponent self)
        {
            Unit unit = self.MainUnit;
            Vector3 newv3 = unit.Position + unit.Rotation * Vector3.forward * 3f;
            int obstruct = self.CheckObstruct(unit, newv3);
            if (obstruct == 0)
            {
                return;
            }
            if (unit.GetComponent<MoveComponent>().IsArrived())
            {
                return;
            }
            self.ZoneScene().GetComponent<SessionComponent>().Session.Send(new C2M_Stop());
        }

        public static void SendMove(this UIJoystickMoveComponent self, int direction)
        {
            long clientNow = TimeHelper.ClientNow();
           
            if ( clientNow - self.lastSendTime < 30)
            {
                return;
            }

            Unit unit = self.MainUnit;
            Quaternion rotation = Quaternion.Euler(0, direction, 0);
           
            if (clientNow - self.AttackComponent.MoveAttackTime < 200)
            {
                return;
            }
            if (self.lastDirection == direction && clientNow - self.lastSendTime < self.checkTime)
            {
                return;
            }

            int errorCode = MoveHelper.IfCanMove(unit);
            if (errorCode == ErrorCode.ERR_CanNotMove_Rigidity)
            {
                SkillManagerComponent skillManager = unit.GetComponent<SkillManagerComponent>();
                if (skillManager.HaveSkillType(SkillHelp.Skill_XuanZhuan_Attack_2))
                {
                    self.checkTime = 100;
                    self.lastSendTime = clientNow;
                    self.lastDirection = direction;
                    NetHelper.RequestSkillXuanZhuan( self.ZoneScene(), direction ).Coroutine();
                    return;
                }
            }

            if (errorCode != ErrorCode.ERR_Success)
            {
                HintHelp.GetInstance().ShowHintError( errorCode, self.ZoneScene());
                return;
            }

            if (self.SceneTypeEnum == SceneTypeEnum.TeamDungeon)
            {
                //检测光墙
                int obstruct = self.CheckObstruct(unit, unit.Position + rotation * Vector3.forward * 2f);
                if (obstruct != 0)
                {
                    self.ShowObstructTip(obstruct);
                    if (SettingHelper.ClintFindPath)
                    {
                        EventType.MoveStart.Instance.Unit = unit;
                        Game.EventSystem.PublishClass(EventType.MoveStart.Instance);
                        unit.Rotation = Quaternion.Euler(0, self.direction, 0);
                        unit.StopResult();
                    }
                    else
                    {
                        unit.Stop();
                    }
                    return;
                }
            }

            Vector3 newv3;
            float distance;
            float speed = self.NumericComponent.GetAsFloat(NumericType.Now_Speed);
            speed = Mathf.Max(speed, 4f);
            if (SettingHelper.ClintFindPath )
            {
                List<Vector3> pathfind = new List<Vector3>();
                newv3 = self.CanMovePosition(unit, rotation, pathfind);
                if (pathfind.Count < 2)
                {
                    EventType.MoveStart.Instance.Unit = unit;
                    Game.EventSystem.PublishClass(EventType.MoveStart.Instance);
                    unit.Rotation = Quaternion.Euler(0, self.direction, 0);
                    return;
                }

                bool oldfunction = false;
                List<Vector3> pathfind_2 = new List<Vector3>();
                if (oldfunction)
                {
                    Vector3 initpos = pathfind[0];
                    pathfind_2.Add(initpos);

                    for (int i = 1; i < pathfind.Count; i++)
                    {
                        //if (!pathfind[i].y.Equals(pathfind[i-1].y))
                        if (Math.Abs(pathfind[i].y - pathfind[i - 1].y) > 0.05f)
                        {
                            pathfind_2.Add(pathfind[i]);
                        }
                    }

                    if (pathfind_2.Count > 2)
                    {
                        int distance_init = 0;
                        for (int i = 1; i < pathfind_2.Count;)
                        {
                            float distance_cur = Vector3.Distance(pathfind_2[i], pathfind_2[distance_init]);
                            if (distance_cur < 0.5f)
                            {
                                pathfind_2.RemoveAt(i);
                            }
                            else
                            {
                                distance_init = i;
                                i++;
                            }
                        }
                    }
                    /////////--------------------------------

                    if (pathfind_2.Count < 2)
                    {
                        pathfind_2.Add(pathfind[pathfind.Count - 1]);
                    }               
                }
                else
                {
                    pathfind_2 = pathfind;      
                }

                newv3 = pathfind_2[pathfind_2.Count - 1];
                distance = Vector3.Distance(newv3, unit.Position);
                float needTime = distance / speed;
                self.checkTime = (long)(1000 * needTime) - 200;

                unit.MoveResultToAsync(pathfind_2, null).Coroutine();
                unit.GetComponent<MoveComponent>().MoveToAsync(pathfind_2, speed).Coroutine();
            }
            else
            {
                distance = self.CanMoveDistance(unit, rotation);
                distance = Mathf.Max(distance, 2f);

                if (self.noCheckTime < clientNow)
                {
                    float needtime = distance / speed;
                    self.checkTime = (int)(1000 * needtime) - 200;
                }
                else
                {
                    self.checkTime = 100;
                }
                newv3 = unit.Position + rotation * Vector3.forward * distance;
                unit.MoveByYaoGan(newv3, direction, distance, null).Coroutine();
            }

            EventType.DataUpdate.Instance.DataType = DataType.BeforeMove;
            EventType.DataUpdate.Instance.DataParamString = string.Empty;
            Game.EventSystem.PublishClass(EventType.DataUpdate.Instance);
          
            self.lastSendTime = clientNow;
            self.lastDirection = direction;
        }

        public static void ShowObstructTip(this UIJoystickMoveComponent self, int monsterId)
        {
            if (Time.time - self.LastShowTip < 1f)
            {
                return;
            }
            self.LastShowTip = Time.time;
            string monsterName = MonsterConfigCategory.Instance.Get(monsterId).MonsterName;
            FloatTipManager.Instance.ShowFloatTip($"请先消灭{monsterName}");
        }

        public  static Vector3 CanMovePosition(this UIJoystickMoveComponent self, Unit unit, Quaternion rotation, List<Vector3> pathfind)
        {
            unit.GetComponent<PathfindingComponent>().Find(unit.Position, unit.Position + rotation * Vector3.forward * 2f, pathfind);  
            return unit.Position;
            //Vector3 targetPosi = unit.Position;
            //for (int i = 0; i < 30; i++)
            //{
            //    targetPosi = targetPosi + rotation * Vector3.forward * 0.2f;
            //    RaycastHit hit;

            //    Physics.Raycast(targetPosi + new Vector3(0f, 2f, 0f), Vector3.down, out hit, 20, self.BuildingLayer);
            //    if (hit.collider != null)
            //    {
            //        break;
            //    }

            //    Physics.Raycast(targetPosi + new Vector3(0f, 2f, 0f), Vector3.down, out hit, 20, self.MapLayer);
            //    if (hit.collider == null)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        if (Mathf.Abs(hit.point.y - targetPosi.y) > 0.4f)
            //        {
            //            break;
            //        }
            //        else
            //        {
            //            targetPosi = hit.point;
            //            pathfind.Add(targetPosi);
            //        }
            //    }
            //}

            //return targetPosi;
        }


        /// <summary>
        /// </summary>
        /// <param name="self"></param>
        /// <param name="unit"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static float CanMoveDistance(this UIJoystickMoveComponent self, Unit unit, Quaternion rotation)
        {
            float intveral = 1f;   //每次寻的长度
            int distance = 2;
            int maxnumber = 5;     //最多寻多少次
            for (int i = distance; i <= maxnumber; i++)
            {
                Vector3 target = unit.Position + rotation * Vector3.forward * i * intveral;
                RaycastHit hit;
                //Physics.Raycast(target + new Vector3(0f, 10f, 0f), Vector3.down, out hit, 100, self.ObstructLayer);
                //if (mapComponent.SceneTypeEnum == SceneTypeEnum.TeamDungeon && i <= 3 && hit.collider != null)
                //{
                //    return -1;
                //}
                distance = i;
                Physics.Raycast(target + new Vector3(0f, 10f, 0f), Vector3.down, out hit, 100, self.BuildingLayer);
                if (hit.collider != null)
                {
                    Log.Debug($" hit.collider != null: i : {i}   x: {target.x}  z:{target.z} ");
                    break;
                }
            }

            return distance * intveral;
        }

        public static int CheckObstruct(this UIJoystickMoveComponent self, Unit unit, Vector3 target)
        {
            RaycastHit hit;
            Physics.Raycast(target + new Vector3(0f, 10f, 0f), Vector3.down, out hit, 100, self.ObstructLayer);
            if (hit.collider == null)
            {
                return 0;
            }
            int monsterid = int.Parse(hit.collider.gameObject.name);
            List<Unit> units = UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Monster);
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].ConfigId == monsterid)
                {
                    return monsterid;
                }
            }
            return 0;
        }

        public static Vector3 GetCanReachPath(this UIJoystickMoveComponent self, Vector3 start, Vector3 target)
        {
            Vector3 dir = (target - start).normalized;

            while (true)
            {
                RaycastHit hit;
                int mapMask = (1 << LayerMask.NameToLayer(LayerEnum.Map.ToString()));
                Physics.Raycast(start + new Vector3(0f,10f,0f), Vector3.down, out hit, 100, mapMask);

                if (hit.collider == null)
                {
                    break;
                }

                if (Vector3.Distance(start, target) < 0.2f)
                {
                    break;
                }
                start = start + (0.2f * dir);
            }
            return start;
        }

        public static void ResetUI(this UIJoystickMoveComponent self)
        {
            self.SetAlpha(0.3f);
            if (self.OperateMode == 0)
            {
                self.CenterShow.transform.localPosition = Vector3.zero; 
                self.Thumb.transform.localPosition = Vector3.zero;
            }
            else
            {
                self.CenterShow.SetActive(false);
                self.Thumb.SetActive(false);
                self.YaoGanDiFix.SetActive(false);
            }
            TimerComponent.Instance?.Remove(ref self.Timer);
        }

        public static void ShowUI(this UIJoystickMoveComponent self)
        { 
            
        }

        public static void AfterEnterScene(this UIJoystickMoveComponent self)
        {
            self.MainUnit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            self.NumericComponent = self.MainUnit.GetComponent<NumericComponent>();
            self.SceneTypeEnum = self.ZoneScene().GetComponent<MapComponent>().SceneTypeEnum;
        }

        public static void EndDrag(this UIJoystickMoveComponent self, PointerEventData pdata)
        {
            Unit unit = self.MainUnit;
            if (unit == null || unit.IsDisposed)
            {
                return;
            }

            if (SettingHelper.ClintFindPath)
            {
                EventType.MoveStop.Instance.Unit = unit;
                Game.EventSystem.PublishClass(EventType.MoveStop.Instance);
            }
             

            long lastTimer = self.Timer;
            self.ResetUI();
            if (lastTimer == 0)
            {
                return;
            }
            
            if (ErrorCode.ERR_Success != unit.GetComponent<StateComponent>().CanMove())
            {
                return;
            }

            //MapHelper.LogMoveInfo($"移动摇杆停止: {TimeHelper.ServerNow()}");

            if (SettingHelper.ClintFindPath )
            {
                unit.StopResult();
            }
            else
            {
                unit.Stop();
            }
        }
    }
}
