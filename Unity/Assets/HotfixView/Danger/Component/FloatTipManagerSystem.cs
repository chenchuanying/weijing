﻿using System;
using UnityEngine;
using System.Collections.Generic;

namespace ET
{

    [Timer(TimerType.FloatTipTimer)]
	public class FloatTipTimer : ATimer<FloatTipManager>
	{
		public override void Run(FloatTipManager self)
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

	public class FloatTipManagerAwakeSystem : AwakeSystem<FloatTipManager>
	{
		public override void Awake(FloatTipManager self)
		{
			FloatTipManager.Instance = self;
			self.Awake(); 
		}
	}

	public class FloatTipManagerDestroySystem : DestroySystem<FloatTipManager>
	{
		public override void Destroy(FloatTipManager self)
		{
			self.FloatTipList = null;
			self.WaitFloatTip = null;
            FloatTipManager.Instance = null;
			TimerComponent.Instance?.Remove(ref self.Timer);
		}
	}

	public static class FloatTipManagerSystem
	{
		public static void Awake(this FloatTipManager self)
		{
			self.FloatTipList = new List<FloatTipComponent>();
			self.WaitFloatTip = new List<FloatTipType>();
		}

		public static void OnUpdate(this FloatTipManager self)
		{
			if (self.WaitFloatTip.Count > 0)
			{
				self.PassTime += Time.deltaTime;
				if (self.PassTime >= self.IntervalTime)
				{
					FloatTipType floatTipType = self.WaitFloatTip[0];
					self.CreateFloatTip(floatTipType);
					self.WaitFloatTip.RemoveAt(0);
					self.PassTime = 0;
				}
			}

			for (int i = self.FloatTipList.Count - 1; i >= 0; i--)
			{
				bool value = self.FloatTipList[i].OnUpdate();
				if (value)
				{
					self.FloatTipList[i].Dispose();
					self.FloatTipList.RemoveAt(i);
				}
			}
			if (self.FloatTipList.Count == 0 &&  self.WaitFloatTip.Count == 0)
			{
				TimerComponent.Instance?.Remove(ref self.Timer);
			}
		}

		public static  void CreateFloatTip(this FloatTipManager self, FloatTipType tip)
		{
			FloatTipComponent uiitem = self.AddChild<FloatTipComponent>();
			uiitem.OnInitData(tip);
			self.FloatTipList.Add(uiitem);
		}
		
		//不带底图的Tips
		public static void ShowFloatTip(this FloatTipManager self, string tip)
		{
			if (self.WaitFloatTip.Count >= 20)
			{
				return;
			}
			self.CheckTheSameTime(tip);
			self.WaitFloatTip.Add(new FloatTipType() {  type = 0, tip = tip });
			if (self.Timer == 0)
			{
				self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.FloatTipTimer, self);
			}
		}

		public static void CheckTheSameTime(this FloatTipManager self, string tip)
		{
			for (int i = self.WaitFloatTip.Count - 1; i >= 0; i--)
			{
				if (self.WaitFloatTip[i].tip.Equals(tip))
				{
					self.WaitFloatTip.RemoveAt(i);	
				}
			}
		}

		//带底图的Tips
		public static void ShowFloatTipDi(this FloatTipManager self, string tip)
		{
			if (self.WaitFloatTip.Count >= 20)
			{
				return;
			}
			self.CheckTheSameTime(tip);
			self.WaitFloatTip.Add(new FloatTipType() { type = 1, tip = tip });
			if (self.Timer == 0 && TimerComponent.Instance!=null)
			{
				self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.FloatTipTimer, self);
			}
		}

	}
}
