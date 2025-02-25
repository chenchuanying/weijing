﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	/// <summary>
	/// 管理所有UI GameObject
	/// </summary>
	public class UIEventComponent: Entity, IAwake, IDestroy
	{
		public static UIEventComponent Instance;
		
		public Dictionary<string, AUIEvent> UIEvents = new Dictionary<string, AUIEvent>();
		
		public Dictionary<int, Transform> UILayers = new Dictionary<int, Transform>();

		public GameObject BloodPlayer;
        public GameObject BloodMonster;
        public GameObject BloodText;
    }
}