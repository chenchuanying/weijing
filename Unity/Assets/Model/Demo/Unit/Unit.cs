﻿using System;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace ET
{
    public class Unit: Entity, IAwake<int>
    {
        public int Type;

        public int ConfigId; //配置表id

        public bool MainHero;

        public bool WaitLoad;

        public long MasterId;

        public float UpdateUITime;

        private WrapVector3 position = new WrapVector3(); //坐标

        public Vector3 Position
        {
            get => this.position.Value;
            set
            {
                EventType.ChangePosition.Instance.OldPos.Value = this.position.Value;
                this.position.Value = value;

                EventType.ChangePosition.Instance.Unit = this;
                Game.EventSystem.PublishClass(EventType.ChangePosition.Instance);
            }
        }

        [BsonIgnore]
        public Vector3 Forward
        {
            get => this.Rotation * Vector3.forward;
            set => this.Rotation = Quaternion.LookRotation(value, Vector3.up);
        }

        private WrapQuaternion rotation = new WrapQuaternion();
        public Quaternion Rotation
        {
            get => this.rotation.Value;
            set
            {
                this.rotation.Value = value;
                EventType.ChangeRotation.Instance.Unit = this;
                Game.EventSystem.PublishClass(EventType.ChangeRotation.Instance);
            }
        }
    }
}