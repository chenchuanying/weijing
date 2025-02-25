﻿using UnityEngine;

namespace ET
{
	[MessageHandler]
	public class M2C_PathfindingRequestHandler : AMHandler<M2C_PathfindingRequest>
	{
		protected override void Run(Session session, M2C_PathfindingRequest message)
		{
			Unit unit = session.ZoneScene().CurrentScene().GetComponent<UnitComponent>().Get(message.Id);
			if (unit == null)
			{
				return;
			}

			//MapHelper.LogMoveInfo($"移动寻路返回 {TimeHelper.ServerNow()}");
           
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Now_Speed);
            using (ListComponent<Vector3> list = ListComponent<Vector3>.Create())
			{
				for (int i = 0; i < message.Xs.Count; ++i)
				{
					list.Add(new Vector3(message.Xs[i], message.Ys[i], message.Zs[i]));
				}

				unit.GetComponent<MoveComponent>().MoveToAsync(list, speed).Coroutine();
			}
		}
	}
}
