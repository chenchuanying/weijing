﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UITaskAComponent : Entity, IAwake, IDestroy
	{
		public GameObject Text_expValue;
		public GameObject Button_Giveup;
		public GameObject Button_Going;
		public GameObject Text_jinbiValue;
		public GameObject Text_taskTarget;
		public GameObject Text_jiangliText;
		public GameObject Button_CancelZhuizong ;
		public GameObject Button_Zhuizong;
		public GameObject RewardListNode;
		public GameObject TypeListNode;
		public GameObject ImageButton;
		public GameObject Text_comTaskNpc;



		public int TaskId;
		public TaskPro TaskPro;
		public TaskConfig TaskConfig;

		public List<UITaskTypeComponent> TaskTypeUIList = new List<UITaskTypeComponent>();
		public List<UIItemComponent> RewardUIList = new List<UIItemComponent>();
		public TaskComponent TaskComponent;
	}


	public class UIRoleTaskAComponentAwakeSystem : AwakeSystem<UITaskAComponent>
	{
		public override void Awake(UITaskAComponent self)
		{
			self.RewardUIList.Clear();
			self.TaskTypeUIList.Clear();
			ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

			self.Text_expValue = rc.Get<GameObject>("Text_expValue");

			self.Button_Giveup = rc.Get<GameObject>("Button_Giveup");
			self.Button_Giveup.GetComponent<Button>().onClick.AddListener(() => { self.OnGiveUpTask(); });

			self.Button_Going = rc.Get<GameObject>("Button_Going");
			self.Button_Going.GetComponent<Button>().onClick.AddListener(() => { self.OnExcuteTask(); });

			self.Text_jinbiValue = rc.Get<GameObject>("Text_jinbiValue");
			self.Text_taskTarget = rc.Get<GameObject>("Text_taskTarget");
			self.Text_jiangliText = rc.Get<GameObject>("Text_jiangliText");

			self.Button_CancelZhuizong = rc.Get<GameObject>("Button_CancelZhuizong");
			self.Button_CancelZhuizong.GetComponent<Button>().onClick.AddListener(() => { self.OnTrackTask(false); });
			self.Button_Zhuizong = rc.Get<GameObject>("Button_Zhuizong");
			self.Button_Zhuizong.GetComponent<Button>().onClick.AddListener(() => { self.OnTrackTask(true); });

			self.RewardListNode = rc.Get<GameObject>("RewardListNode");
			self.TypeListNode = rc.Get<GameObject>("TypeListNode");

			self.ImageButton = rc.Get<GameObject>("ImageButton");
			self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.OnCloseTask(); });

			self.Text_comTaskNpc = rc.Get<GameObject>("Text_comTaskNpc");

            self.TaskComponent = self.ZoneScene().GetComponent<TaskComponent>();

            DataUpdateComponent.Instance.AddListener(DataType.TaskGet, self);
			DataUpdateComponent.Instance.AddListener(DataType.TaskUpdate, self);
			DataUpdateComponent.Instance.AddListener(DataType.TaskGiveUp, self);
			DataUpdateComponent.Instance.AddListener(DataType.TaskComplete, self);

			self.InitTaskTypeList();
		}
	}


	public class UIRoleTaskAComponentDestroySystem : DestroySystem<UITaskAComponent>
	{
		public override void Destroy(UITaskAComponent self)
		{
			DataUpdateComponent.Instance.RemoveListener(DataType.TaskGet, self);
			DataUpdateComponent.Instance.RemoveListener(DataType.TaskUpdate, self);
			DataUpdateComponent.Instance.RemoveListener(DataType.TaskGiveUp, self);
			DataUpdateComponent.Instance.RemoveListener(DataType.TaskComplete, self);
		}
	}

	public static class UIRoleTaskAComponentSystem
	{

		public static void OnTaskGiveUp(this UITaskAComponent self)
		{
			int index = 0;
			for (int i = 0; i < self.TaskTypeUIList.Count; i++)
			{
				if (self.TaskTypeUIList[i].bSelected == true)
				{
					index = i;
					break;
				}
			}
			self.TaskTypeUIList[index].SetExpand();
		}

		public static  void InitTaskTypeList(this UITaskAComponent self)
		{
			string path = ABPathHelper.GetUGUIPath("Main/Task/UITaskType");
			GameObject bundleObj =ResourcesComponent.Instance.LoadAsset<GameObject>(path);

			List<int> ids = new List<int>() { (int)TaskTypeEnum.Main, (int)TaskTypeEnum.Branch, (int)TaskTypeEnum.Daily };
			for (int i = 0; i < ids.Count; i++)
			{
				GameObject taskTypeItem = GameObject.Instantiate(bundleObj);
				UICommonHelper.SetParent(taskTypeItem, self.TypeListNode);

				UITaskTypeComponent uiTaskTypeComponent = self.AddChild<UITaskTypeComponent, GameObject>(taskTypeItem);
				uiTaskTypeComponent.OnUpdateData(ids[i]);
				uiTaskTypeComponent.SetClickTypeHandler((int typeid) => { self.OnClickTaskType(typeid); });
				uiTaskTypeComponent.SetClickTypeItemHandler((int typeid, int chapterId) => { self.OnClickTaskTypeItem(typeid, chapterId); });
				self.TaskTypeUIList.Add(uiTaskTypeComponent);
			}

			TaskPro taskPro = self.TaskComponent.GetTaskById(self.TaskId);
			if (taskPro == null)
			{
				self.OnClickTaskType(ids[0]);
			}
			else
			{
				TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskPro.taskID);
				self.OnClickTaskType(taskConfig.TaskType, taskPro.taskID);
			}

		}

		public static void OnClickTaskType(this UITaskAComponent self, int type, int taskId = 0)
		{
			for (int i = 0; i < self.TaskTypeUIList.Count; i++)
			{
				self.TaskTypeUIList[i].SetSelected(type, taskId);
			}
		}

		public static void OnClickTaskTypeItem(this UITaskAComponent self, int type, int taskId)
		{
			self.TaskPro = self.ZoneScene().GetComponent<TaskComponent>().GetTaskById(taskId);
			self.TaskId = taskId;

			self.UpdateTaskInfo(self.TaskPro);
		}

		public static void OnCloseTask(this UITaskAComponent self)
		{
			UIHelper.Remove(self.DomainScene(), UIType.UITask);
		}

		public static void  UpdateTaskInfo(this UITaskAComponent self, TaskPro taskPro)
		{
			self.TaskPro = taskPro;
			self.GetParent<UI>().GameObject.transform.Find("Right").gameObject.SetActive(taskPro != null);
			if (taskPro == null)
			{
				return;
			}

			self.TaskConfig = TaskConfigCategory.Instance.Get(taskPro.taskID);

			self.Text_expValue.GetComponent<Text>().text = self.TaskConfig.TaskExp.ToString();
			self.Text_jinbiValue.GetComponent<Text>().text = self.TaskConfig.TaskCoin.ToString();

			self.Text_taskTarget.GetComponent<Text>().text = TaskViewHelp.Instance.GetTaskProgessDesc(taskPro);
			self.Text_jiangliText.GetComponent<Text>().text = self.TaskConfig.TaskDes;

			self.Button_Zhuizong.SetActive(taskPro.TrackStatus == 0);
			self.Button_CancelZhuizong.SetActive(taskPro.TrackStatus == 1);


			//显示提交任务
			if(self.TaskConfig.CompleteNpcID > 0)
			{
                string npcName = NpcConfigCategory.Instance.Get(self.TaskConfig.CompleteNpcID).Name;
                self.Text_comTaskNpc.GetComponent<Text>().text = $"完成任务请找:<color=#5C7B32>{npcName}</color>";
            }
			

			string path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
			GameObject bundleObj =ResourcesComponent.Instance.LoadAsset<GameObject>(path);

			string rewardStr = self.TaskConfig.ItemID;
			string rewardNum = self.TaskConfig.ItemNum;

            float coffiexp = 1f;
            float cofficoin = 1f;
            if (self.TaskConfig.Development == 1)
            {
				UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
                coffiexp = ComHelp.GetTaskExpRewardCof(userInfoComponent.UserInfo.Lv);
                cofficoin = ComHelp.GetTaskCoinRewardCof(userInfoComponent.UserInfo.Lv);
            }

            int TaskExp = (int)(self.TaskConfig.TaskExp * coffiexp);
            int TaskCoin = (int)(self.TaskConfig.TaskCoin * cofficoin);

            if (ComHelp.IfNull(rewardStr))
			{

				rewardStr = "1;2";
				rewardNum = TaskCoin + ";" + TaskExp;
			}
			else 
			{
				rewardStr = "1;2;" + rewardStr;
				rewardNum = TaskCoin + ";" + TaskExp + ";" + rewardNum;
			}

			string[] rewarditems = rewardStr.Split(';');
			string[] rewardItemNums = rewardNum.Split(';');

			int number = 0;

			for (int i = self.RewardUIList.Count-1; i >= 0; i--) {
				self.RewardUIList[i].Dispose();
			}
			self.RewardUIList.Clear();

			//FunctionUI.GetInstance().DestoryTargetObj(self.RewardListNode);

			// 跑环任务显示对应的环数奖励
			Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
			int taskType = TaskConfigCategory.Instance.Get(taskPro.taskID).TaskType;
			int nowNum = 0;
			if (taskType == 5)
			{
				nowNum = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.WeeklyTaskNumber) + 1;

                if (nowNum != 0)
                {
                    if (ConfigHelper.WeekTaskDrop.Keys.Contains(nowNum))
                    {
                        List<RewardItem> droplist = new List<RewardItem>();
                        DropHelper.DropIDToDropItem_2(ConfigHelper.WeekTaskDrop[nowNum], droplist);

                        if (droplist.Count > 0)
                        {
                            for (int i = 0; i < droplist.Count; i++)
                            {
                                Array.Resize(ref rewarditems, rewarditems.Length + 1);
                                rewarditems[rewarditems.Length - 1] = droplist[i].ItemID.ToString();

                                Array.Resize(ref rewardItemNums, rewardItemNums.Length + 1);
                                rewardItemNums[rewardItemNums.Length - 1] = droplist[i].ItemNum.ToString();
                            }
                        }
                    }
                }
            }
			if (taskType == 10)
			{
				nowNum = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RingTaskNumber) + 1;

                if (nowNum != 0)
                {
                    if (ConfigHelper.RingTaskDrop.Keys.Contains(nowNum))
                    {
                        List<RewardItem> droplist = new List<RewardItem>();
                        DropHelper.DropIDToDropItem_2(ConfigHelper.RingTaskDrop[nowNum], droplist);

                        if (droplist.Count > 0)
                        {
                            for (int i = 0; i < droplist.Count; i++)
                            {
                                Array.Resize(ref rewarditems, rewarditems.Length + 1);
                                rewarditems[rewarditems.Length - 1] = droplist[i].ItemID.ToString();

                                Array.Resize(ref rewardItemNums, rewardItemNums.Length + 1);
                                rewardItemNums[rewardItemNums.Length - 1] = droplist[i].ItemNum.ToString();
                            }
                        }
                    }
                }
            }
			
			
			for (int i = 0; i < rewarditems.Length; i++)
			{
				UIItemComponent ui_1;
				
				if (rewarditems[i] == "0" || rewardItemNums[i] == "0")
				{
					number++;
					continue;
				}
				

				if (number < self.RewardUIList.Count)
				{
					ui_1 = self.RewardUIList[number];
					ui_1.GameObject.SetActive(true);
				}
				else
				{
					GameObject skillItem = GameObject.Instantiate(bundleObj);
					UICommonHelper.SetParent(skillItem, self.RewardListNode);
					ui_1 = self.AddChild<UIItemComponent, GameObject>(skillItem);
					self.RewardUIList.Add(ui_1);
				}
				
				ui_1.UpdateItem(new BagInfo() { ItemID = int.Parse(rewarditems[number]), ItemNum = int.Parse(rewardItemNums[number]) }, ItemOperateEnum.TaskItem);
				ui_1.Label_ItemNum.SetActive(true);
				number++;
			}

			//显示UI
			for (int i = number; i < self.RewardUIList.Count; i++)
			{
				self.RewardUIList[i].GameObject.SetActive(false);
			}

			self.Button_Going.transform.GetComponentInChildren<Text>().text = "前往任务";
			if (self.TaskConfig.TargetType == TaskTargetType.GiveItem_10)
			{
				self.Button_Going.transform.GetComponentInChildren<Text>().text = "上交装备";
			}
			else if(self.TaskConfig.TargetType == TaskTargetType.GivePet_25)
			{
				self.Button_Going.transform.GetComponentInChildren<Text>().text = "上交宠物";
			}

			if ((self.TaskConfig.TaskType == TaskTypeEnum.Ring || self.TaskConfig.TaskType == TaskTypeEnum.Union ||
				    self.TaskConfig.TaskType == TaskTypeEnum.Daily || self.TaskConfig.TaskType == TaskTypeEnum.Treasure) &&
			    self.TaskPro.taskStatus == (int)TaskStatuEnum.Completed)
			{
				self.Button_Going.transform.GetComponentInChildren<Text>().text = "完成任务";
			}
		}
		
		public static void OnRecvTaskUpdate(this UITaskAComponent self)
		{
			self.UpdateTaskInfo(self.TaskComponent.GetTaskById(self.TaskId));
		}

		public static void OnTrackTask(this UITaskAComponent self, bool track)
		{
			if (self.TaskPro == null)
			{
				return;
			}

			TaskConfig taskConfig = TaskConfigCategory.Instance.Get(self.TaskPro.taskID);
			//if (!track && taskConfig.TaskType == (int)TaskTypeEnum.Main)
			//{
   //             FloatTipManager.Instance.ShowFloatTip("主线任务不能取消追踪!");
   //             return;
   //         }
			if (self.ZoneScene().GetComponent<TaskComponent>().GetAllTrackList().Count >= 3 && track)
			{
                FloatTipManager.Instance.ShowFloatTip("追踪数量不能超过三个!");
                return;
            }

			self.ZoneScene().GetComponent<TaskComponent>().SendTaskTrack(self.TaskPro.taskID, self.TaskPro.TrackStatus == 0 ? 1 : 0).Coroutine();

			self.Button_Zhuizong.SetActive(!self.Button_Zhuizong.activeSelf);
			self.Button_CancelZhuizong .SetActive(!self.Button_CancelZhuizong .activeSelf);

			//提示
			if (self.Button_Zhuizong.activeSelf == false)
			{
				FloatTipManager.Instance.ShowFloatTip("任务开启追踪!");
			}
			else 
			{
				FloatTipManager.Instance.ShowFloatTip("任务取消追踪!");
			}

		}

		public static void OnGiveUpTask(this UITaskAComponent self)
		{
			if (self.TaskPro == null)
				return;

			TaskConfig taskConfig = TaskConfigCategory.Instance.Get(self.TaskPro.taskID);
			if (taskConfig.TaskType == (int)TaskTypeEnum.Main)
			{
				FloatTipManager.Instance.ShowFloatTip("主线任务不能放弃");
				return;
			}
			if (taskConfig.TaskType == TaskTypeEnum.Ring)
			{
				FloatTipManager.Instance.ShowFloatTip("跑环任务不能放弃");
				return;
			}
			self.ZoneScene().GetComponent<TaskComponent>().SendGiveUpTask(self.TaskPro.taskID).Coroutine();
		}

		public static void  OnExcuteTask(this UITaskAComponent self)
		{
			bool value = TaskViewHelp.Instance.ExcuteTask(self.ZoneScene(), self.TaskPro);
			TaskConfig taskConfig = TaskConfigCategory.Instance.Get(self.TaskPro.taskID);
			if (value && taskConfig.TaskType != TaskTypeEnum.Ring && taskConfig.TaskType != TaskTypeEnum.Union &&
			    taskConfig.TaskType != TaskTypeEnum.Daily && taskConfig.TaskType != TaskTypeEnum.Treasure)
			{
				self.OnCloseTask();
			}
		}
	}

}
