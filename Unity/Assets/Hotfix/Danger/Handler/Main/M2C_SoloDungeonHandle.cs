﻿namespace ET
{
    public class M2C_SoloDungeonHandle : AMHandler<M2C_SoloDungeon>
    {
        protected override void Run(Session session, M2C_SoloDungeon message)
        {

            Log.Debug("恭喜你！竞技场获胜...." + message.SoloResult + "并且获得奖励:" + message.RewardItem[0].ItemID + ";" + message.RewardItem[0].ItemNum);

            EventType.UISoloReward.Instance.ZoneScene = session.ZoneScene();
            EventType.UISoloReward.Instance.m2C_SoloDungeon = message;
            EventSystem.Instance.PublishClass(EventType.UISoloReward.Instance);

        }
    }
}
