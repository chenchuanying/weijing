﻿using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class UIHuntTaskComponent: Entity, IAwake
    {
        public GameObject TaskListNode;
        public GameObject UIHuntTaskItem;
        public List<UIHuntTaskItemComponent> TaskList = new List<UIHuntTaskItemComponent>();
    }

    public class UIHuntTaskComponentAwakeSystem: AwakeSystem<UIHuntTaskComponent>
    {
        public override void Awake(UIHuntTaskComponent self)
        {
            self.Awake();
        }
    }

    public static class UIHuntTaskComponentSystem
    {
        public static void Awake(this UIHuntTaskComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.TaskListNode = rc.Get<GameObject>("TaskListNode");
            self.UIHuntTaskItem = rc.Get<GameObject>("UIHuntTaskItem");
            
            self.UIHuntTaskItem.SetActive(false);
            self.TaskList.Clear();

            self.GetParent<UI>().OnUpdateUI = () => { self.OnUpdateUI(); };
        }

        public static void OnUpdateUI(this UIHuntTaskComponent self)
        {
            self.UpdateTaskCountrys();
        }

        public static void UpdateTaskCountrys(this UIHuntTaskComponent self)
        {
            List<TaskPro> taskPros = self.ZoneScene().GetComponent<TaskComponent>().TaskCountryList;
            taskPros.Sort(delegate(TaskPro a, TaskPro b)
            {
                int commita = a.taskStatus == (int)TaskStatuEnum.Commited? 1 : 0;
                int commitb = b.taskStatus == (int)TaskStatuEnum.Commited? 1 : 0;
                int completea = a.taskStatus == (int)TaskStatuEnum.Completed? 1 : 0;
                int completeb = b.taskStatus == (int)TaskStatuEnum.Completed? 1 : 0;

                if (commita == commitb)
                    return completeb - completea; //可以领取的在前
                else
                    return commitb - commita; //已经完成的在前
            });

            int number = 0;
            for (int i = 0; i < taskPros.Count; i++)
            {
                TaskCountryConfig taskConfig = TaskCountryConfigCategory.Instance.Get(taskPros[i].taskID);
                if (taskConfig.TaskType != TaskCountryType.ShowLie)
                {
                    continue;
                }

                UIHuntTaskItemComponent ui_1 = null;
                if (number < self.TaskList.Count)
                {
                    ui_1 = self.TaskList[number];
                    ui_1.GameObject.SetActive(true);
                }
                else
                {
                    GameObject taskTypeItem = GameObject.Instantiate(self.UIHuntTaskItem);
                    taskTypeItem.SetActive(true);
                    UICommonHelper.SetParent(taskTypeItem, self.TaskListNode);
                    ui_1 = self.AddChild<UIHuntTaskItemComponent, GameObject>(taskTypeItem);
                    self.TaskList.Add(ui_1);
                }

                ui_1.OnUpdateData(taskPros[i]);
                number++;
            }

            for (int k = number; k < self.TaskList.Count; k++)
            {
                self.TaskList[k].GameObject.SetActive(false);
            }
        }
    }
}