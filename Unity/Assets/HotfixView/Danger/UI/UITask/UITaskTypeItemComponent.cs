﻿
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UITaskTypeItemComponent : Entity, IAwake<GameObject>
    {
        public GameObject Ima_DiButton;
        public GameObject Lab_TaskName;
        public GameObject Ima_Ongoing;
        public GameObject Ima_CompleteTask;
        public GameObject Ima_SelectStatus;

        public TaskPro TaskPro;
        public Action<int> ClickTaskHandler;

        public GameObject GameObject;
    }


    public class UITaskTypeItemComponentAwakeSystem : AwakeSystem<UITaskTypeItemComponent, GameObject>
    {
        public override void Awake(UITaskTypeItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Ima_DiButton = rc.Get<GameObject>("Ima_DiButton");
            self.Lab_TaskName = rc.Get<GameObject>("Lab_TaskName");
            self.Ima_Ongoing = rc.Get<GameObject>("Ima_Ongoing");
            self.Ima_CompleteTask = rc.Get<GameObject>("Ima_CompleteTask");
            self.Ima_SelectStatus = rc.Get<GameObject>("Ima_SelectStatus");
            self.Ima_SelectStatus.SetActive(false);

            self.Ima_DiButton.GetComponent<Button>().onClick.AddListener(() => { self.OnClickTaskTypeItem(); });
        }
    }

    public static class UITaskTypeItemComponentSystem
    {

        public static void SetSelected(this UITaskTypeItemComponent self, int taskid)
        {
            self.Ima_SelectStatus.SetActive(self.TaskPro.taskID == taskid);
        }

        public static void SetClickTypeHandler(this UITaskTypeItemComponent self, Action<int> action)
        {
            self.ClickTaskHandler = action;
        }

        public static void OnClickTaskTypeItem(this UITaskTypeItemComponent self)
        {
            self.ClickTaskHandler(self.TaskPro.taskID);
        }

        public static void OnUpdateData(this UITaskTypeItemComponent self, TaskPro taskPro)
        {
            self.TaskPro = taskPro;
            string name_1 = TaskConfigCategory.Instance.Get(taskPro.taskID).TaskName;
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());

            self.Lab_TaskName.GetComponent<Text>().text = name_1;
            int taskType = TaskConfigCategory.Instance.Get(taskPro.taskID).TaskType;
            if (taskType == 3)
            {
                int nowNum = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.DailyTaskNumber) + 1;
                self.Lab_TaskName.GetComponent<Text>().text = name_1 + "(第" + nowNum + "/" + GlobalValueConfigCategory.Instance.Get(58).Value + "环)";
            }
            if (taskType == 5)
            {
                int nowNum = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.WeeklyTaskNumber) + 1;
                self.Lab_TaskName.GetComponent<Text>().text = name_1 + "(第" + nowNum + "/10环)";
            }
            if (taskType == 7)
            {
                int nowNum = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.UnionTaskNumber) + 1;
                self.Lab_TaskName.GetComponent<Text>().text = name_1 + "(第" + nowNum + "/" + GlobalValueConfigCategory.Instance.Get(108).Value + "环)";
            }
            if (taskType == 10)
            {
                int nowNum = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RingTaskNumber) + 1;
                self.Lab_TaskName.GetComponent<Text>().text = name_1 + "(第" + nowNum + "/100环)";
            }
          
            self.Ima_Ongoing.SetActive(taskPro.taskStatus != (int)TaskStatuEnum.Completed);
            self.Ima_CompleteTask.SetActive(taskPro.taskStatus == (int)TaskStatuEnum.Completed);
        }

    }

}
