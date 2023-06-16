﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{

    [ActorMessageHandler]
    public class G2Robot_MessageHandler : AMActorHandler<Scene, G2Robot_MessageRequest>
    {
        protected override async ETTask Run(Scene scene, G2Robot_MessageRequest message)
        {
            RobotManagerComponent robotManagerComponent = scene.GetComponent<RobotManagerComponent>();
            switch (message.MessageType)
            {
                case NoticeType.CreateRobot:
                    int robotId = 1001;
                    for (int i = 0; i < 100; ++i)
                    {
                        int robotZone = robotManagerComponent.ZoneIndex++;
                        Log.Console($"create robot22 {robotZone}");
                        Scene robot = await robotManagerComponent.NewRobot(message.Zone, robotZone, robotId);
                        if (robot == null)
                        {
                            continue;
                        }
                        BehaviourComponent behaviourComponent = robot.AddComponent<BehaviourComponent, int>(robotId);
                        if (behaviourComponent == null)
                        {
                            continue;
                        }
                        behaviourComponent.CreateTime = TimeHelper.ClientNow();
                        await TimerComponent.Instance.WaitAsync(1000);
                    }
                    break;
                case NoticeType.TeamDungeon:
                    List<Entity> ts = robotManagerComponent.Children.Values.ToList();
                    if (ts.Count >= 1000)
                    {
                        return;
                    }
                    int robotnumber = 0;
                    while(robotnumber < 1)
                    {
                        int robotZone = robotManagerComponent.ZoneIndex++;
                        string[] teamInfo = message.Message.Split('_');
                        int fubenId = int.Parse(teamInfo[0]);
                        long teamId = long.Parse(teamInfo[1]);
                        robotId = BattleHelper.GetTeamRobotId(fubenId);
                        Scene robotScene = await robotManagerComponent.NewRobot(message.Zone, robotZone, robotId);
                        if (robotScene == null)
                        {
                            continue;
                        }
                        BehaviourComponent behaviourComponent =  robotScene?.AddComponent<BehaviourComponent, int>(robotId);
                        if (behaviourComponent == null)
                        {
                            continue;
                        }
                        behaviourComponent.MessageValue = message.Message;
                        behaviourComponent.CreateTime = TimeHelper.ClientNow();
                        robotnumber++;
                        await TimerComponent.Instance.WaitAsync(200);
                    }
                    break;
                case NoticeType.YeWaiBoss:
                    //sceneid@x;y;z
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.NewRobot, 1))
                    {
                        string[] messageInfo = message.Message.Split('@');
                        string[] positionInfo = messageInfo[1].Split(";");
                        Vector3 targetPosition = new Vector3(float.Parse(positionInfo[0]), float.Parse(positionInfo[1]), float.Parse(positionInfo[2]));
                        for (int i = 0; i < 10; i++)
                        {
                            int robotZone = robotManagerComponent.ZoneIndex++;
                            robotId = BattleHelper.GetBattleRobotId(4, int.Parse(messageInfo[2]));
                            if (robotId == 0)
                            {
                                continue;
                            }
                            Scene robotScene = await robotManagerComponent.NewRobot(message.Zone, robotZone, robotId);
                            if (robotScene == null)
                            {
                                continue;
                            }
                            BehaviourComponent behaviourComponent = robotScene?.AddComponent<BehaviourComponent, int>(robotId);
                            behaviourComponent.TargetPosition = targetPosition;
                            behaviourComponent.MessageValue = message.Message;
                            await TimerComponent.Instance.WaitAsync(1000);
                        }
                    } 
                    break;
                case NoticeType.SoloBegin:
                    Log.Debug($"机器人数量[SoloBegin]");
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.NewRobot, 1))
                    {
                        for (int i = 0; i < 0; i++)
                        {
                            int robotZone = robotManagerComponent.ZoneIndex++;
                            robotId = BattleHelper.GetBattleRobotId(6, 0);
                            if (robotId == 0)
                            {
                                continue;
                            }
                            Scene robotScene = await robotManagerComponent.NewRobot(message.Zone, robotZone, robotId);
                            robotScene?.AddComponent<BehaviourComponent, int>(robotId);
                            await TimerComponent.Instance.WaitAsync(1000);
                        }
                    }
                    break;
                case NoticeType.ArenaOpen:
                    Log.Debug($"机器人数量[ArenaOpen]");
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.NewRobot, 1))
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            int robotZone = robotManagerComponent.ZoneIndex++;
                            robotId = BattleHelper.GetBattleRobotId(5, 0);
                            if (robotId == 0)
                            {
                                continue;
                            }
                            Scene robotScene = await robotManagerComponent.NewRobot(message.Zone, robotZone, robotId);
                            robotScene?.AddComponent<BehaviourComponent, int>(robotId);
                            await TimerComponent.Instance.WaitAsync(1000);
                        }
                    }
                    break;
                case NoticeType.BattleOpen:
                    Log.Debug($"战场机器人[BattleOpen]: {message.Zone}");
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.NewRobot, 1))
                    {
                        int robotNumber = 0;
                        while (robotNumber < 12)
                        {
                            int robotZone = robotManagerComponent.ZoneIndex++;
                            robotId = BattleHelper.GetBattleRobotId(3, 0);

                            Scene robotScene = await robotManagerComponent.NewRobot(message.Zone, robotZone, robotId);
                            if (robotScene == null)
                            {
                                continue;
                            }
                            robotScene.AddComponent<BehaviourComponent, int>(robotId);
                            await TimerComponent.Instance.WaitAsync(1000);
                            robotNumber++;
                        }
                    }
                    break;
                case NoticeType.SoloOver:
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.RemoveRobot, 1))
                    {
                        ts = robotManagerComponent.Children.Values.ToList();
                        for (int i = 0; i < ts.Count; i++)
                        {
                            Scene robotScene = ts[i] as Scene;
                            if (robotScene.GetComponent<BehaviourComponent>() == null)
                            {
                                continue;
                            }
                            if (robotScene.GetComponent<BehaviourComponent>().GetBehaviour() != 6)
                            {
                                continue;
                            }
                            if (message.Zone != robotScene.GetComponent<AccountInfoComponent>().ServerId)
                            {
                                continue;
                            }
                            robotScene.GetComponent<AttackComponent>().RemoveTimer();
                            robotManagerComponent.RemoveRobot(robotScene, "Solo结束").Coroutine();
                            await TimerComponent.Instance.WaitAsync(1000);
                        }
                    }
                    break;
                case NoticeType.BattleOver:
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.RemoveRobot, 1))
                    {
                        ts = robotManagerComponent.Children.Values.ToList();
                        for (int i = 0; i < ts.Count; i++)
                        {
                            Scene robotScene = ts[i] as Scene;
                            if (robotScene.GetComponent<BehaviourComponent>() == null)
                            {
                                continue;
                            }
                            if (robotScene.GetComponent<BehaviourComponent>().GetBehaviour() != 3)
                            {
                                continue;
                            }
                            if (message.Zone != robotScene.GetComponent<AccountInfoComponent>().ServerId)
                            {
                                continue;
                            }
                            robotScene.GetComponent<AttackComponent>().RemoveTimer();
                            robotManagerComponent.RemoveRobot(robotScene, "战场结束").Coroutine();
                            await TimerComponent.Instance.WaitAsync(1000);
                        }
                    }
                    break;

            }
            await ETTask.CompletedTask;
        }
    }
}
