using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class SkillConfigCategory : ProtoObject, IMerge
    {
        public static SkillConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, SkillConfig> dict = new Dictionary<int, SkillConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<SkillConfig> list = new List<SkillConfig>();
		
        public SkillConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            SkillConfigCategory s = o as SkillConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            foreach (SkillConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public SkillConfig Get(int id)
        {
            this.dict.TryGetValue(id, out SkillConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (SkillConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, SkillConfig> GetAll()
        {
            return this.dict;
        }

        public SkillConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class SkillConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>技能名称</summary>
		[ProtoMember(2)]
		public string SkillName { get; set; }
		/// <summary>技能等级</summary>
		[ProtoMember(3)]
		public int SkillLv { get; set; }
		/// <summary>技能Icon</summary>
		[ProtoMember(4)]
		public string SkillIcon { get; set; }
		/// <summary>下一级技能</summary>
		[ProtoMember(5)]
		public int NextSkillID { get; set; }
		/// <summary>使用武器触发</summary>
		[ProtoMember(6)]
		public int WeaponType { get; set; }
		/// <summary>学习技能等级</summary>
		[ProtoMember(7)]
		public int LearnRoseLv { get; set; }
		/// <summary>升级消耗SP值</summary>
		[ProtoMember(8)]
		public int CostSPValue { get; set; }
		/// <summary>升级消耗金币</summary>
		[ProtoMember(9)]
		public int CostGoldValue { get; set; }
		/// <summary>被控制是否可以放技能</summary>
		[ProtoMember(10)]
		public int OpenType { get; set; }
		/// <summary>装备升级前ID</summary>
		[ProtoMember(11)]
		public string EquipSkill { get; set; }
		/// <summary>技能类型</summary>
		[ProtoMember(12)]
		public int SkillType { get; set; }
		/// <summary>被动技能触发类型</summary>
		[ProtoMember(13)]
		public int[] PassiveSkillType { get; set; }
		/// <summary>被动技能触发参数</summary>
		[ProtoMember(14)]
		public double[] PassiveSkillPro { get; set; }
		/// <summary>被动技能触发一次</summary>
		[ProtoMember(15)]
		public int PassiveSkillTriggerOnce { get; set; }
		/// <summary>施法目标</summary>
		[ProtoMember(16)]
		public int SkillTargetTypeNum { get; set; }
		/// <summary>连招技能ID</summary>
		[ProtoMember(17)]
		public int ComboSkillID { get; set; }
		/// <summary>技能攻击类型</summary>
		[ProtoMember(18)]
		public int SkillActType { get; set; }
		/// <summary>伤害类型</summary>
		[ProtoMember(19)]
		public int DamgeType { get; set; }
		/// <summary>伤害元素攻击</summary>
		[ProtoMember(20)]
		public int DamgeElementType { get; set; }
		/// <summary>攻击系数</summary>
		[ProtoMember(21)]
		public double ActDamge { get; set; }
		/// <summary>怪物攻击系数</summary>
		[ProtoMember(22)]
		public double MonsterActDamge { get; set; }
		/// <summary>固定伤害值</summary>
		[ProtoMember(23)]
		public int DamgeValue { get; set; }
		/// <summary>是否必中</summary>
		[ProtoMember(24)]
		public int IfMustAct { get; set; }
		/// <summary>消耗魔法</summary>
		[ProtoMember(25)]
		public int SkillUseMP { get; set; }
		/// <summary>增加魔法</summary>
		[ProtoMember(26)]
		public int SkillAddMP { get; set; }
		/// <summary>是否触发公共CD</summary>
		[ProtoMember(27)]
		public int IfPublicSkillCD { get; set; }
		/// <summary>是否触发冷却技能CD(废弃)</summary>
		[ProtoMember(28)]
		public int IfSkillCD { get; set; }
		/// <summary>冷却CD</summary>
		[ProtoMember(29)]
		public double SkillCD { get; set; }
		/// <summary>伤害范围类型</summary>
		[ProtoMember(30)]
		public int DamgeRangeType { get; set; }
		/// <summary>伤害范围</summary>
		[ProtoMember(31)]
		public double[] DamgeRange { get; set; }
		/// <summary>技能目标类型</summary>
		[ProtoMember(32)]
		public int SkillTargetType { get; set; }
		/// <summary>释放区域类型</summary>
		[ProtoMember(33)]
		public int SkillZhishiType { get; set; }
		/// <summary>释放区域目标点类型</summary>
		[ProtoMember(34)]
		public int SkillZhishiTargetType { get; set; }
		/// <summary>释放区域大小</summary>
		[ProtoMember(35)]
		public double SkillRangeSize { get; set; }
		/// <summary>技能指示器增加范围</summary>
		[ProtoMember(36)]
		public int SkillRangeZhiShiSize { get; set; }
		/// <summary>施法前吟唱时间</summary>
		[ProtoMember(37)]
		public double SkillFrontSingTime { get; set; }
		/// <summary>施法中吟唱时间</summary>
		[ProtoMember(38)]
		public double SkillSingTime { get; set; }
		/// <summary>技能僵直</summary>
		[ProtoMember(39)]
		public double SkillRigidity { get; set; }
		/// <summary>技能存在时间[毫秒]</summary>
		[ProtoMember(40)]
		public int SkillLiveTime { get; set; }
		/// <summary>技能效果延迟时间</summary>
		[ProtoMember(41)]
		public double SkillDelayTime { get; set; }
		/// <summary>技能移动速度</summary>
		[ProtoMember(42)]
		public double SkillMoveSpeed { get; set; }
		/// <summary>初始化BUFFID</summary>
		[ProtoMember(43)]
		public int[] InitBuffID { get; set; }
		/// <summary>释放BUFFID</summary>
		[ProtoMember(44)]
		public int[] BuffID { get; set; }
		/// <summary>只释放一次buff</summary>
		[ProtoMember(45)]
		public int[] OnlyOnceBuffID { get; set; }
		/// <summary>施法动作名称</summary>
		[ProtoMember(46)]
		public string SkillAnimation { get; set; }
		/// <summary>技能音效</summary>
		[ProtoMember(47)]
		public string SkillMusic { get; set; }
		/// <summary>技能特效ID</summary>
		[ProtoMember(48)]
		public int SkillHitEffectID { get; set; }
		/// <summary>技能特效ID</summary>
		[ProtoMember(49)]
		public int[] SkillEffectID { get; set; }
		/// <summary>脚本名称</summary>
		[ProtoMember(50)]
		public string GameObjectName { get; set; }
		/// <summary>每个脚本对应参数</summary>
		[ProtoMember(51)]
		public string GameObjectParameter { get; set; }
		/// <summary>所有脚本通用参数</summary>
		[ProtoMember(52)]
		public string ComObjParameter { get; set; }
		/// <summary>是否显示</summary>
		[ProtoMember(53)]
		public int IsShow { get; set; }
		/// <summary>技能描述</summary>
		[ProtoMember(54)]
		public string SkillDescribe { get; set; }
		/// <summary>施法时面对目标时间</summary>
		[ProtoMember(55)]
		public double IfLookAtTatgetTime { get; set; }
		/// <summary>触发技能时附带技能</summary>
		[ProtoMember(56)]
		public int[] AddSkillID { get; set; }
		/// <summary>技能触发时间</summary>
		[ProtoMember(57)]
		public double PassiveSkillTriggerTime { get; set; }
		/// <summary>施法时是否面对目标</summary>
		[ProtoMember(58)]
		public int IfLookAtTarget { get; set; }
		/// <summary>怪物技能延迟</summary>
		[ProtoMember(59)]
		public double MonsterDelayTime { get; set; }
		/// <summary>宠物互斥ID</summary>
		[ProtoMember(60)]
		public int HuChiID { get; set; }
		/// <summary>触发自身拥有技能</summary>
		[ProtoMember(61)]
		public int[] TriggerSelfSkillID { get; set; }
		/// <summary>释放技能是否打断移动</summary>
		[ProtoMember(62)]
		public int IfStopMove { get; set; }
		/// <summary>技能持续伤害是否触发Buff</summary>
		[ProtoMember(63)]
		public int DamgeChiXuTrigerBuff { get; set; }
		/// <summary>技能持续伤害间隔时间</summary>
		[ProtoMember(64)]
		public int DamgeChiXuInterval { get; set; }
		/// <summary>技能持续伤害百分比</summary>
		[ProtoMember(65)]
		public double DamgeChiXuPro { get; set; }
		/// <summary>技能持续伤害固定值</summary>
		[ProtoMember(66)]
		public int DamgeChiXuValue { get; set; }
		/// <summary>是否显示技能指示器字段</summary>
		[ProtoMember(67)]
		public int IfShowSkillZhiShi { get; set; }
		/// <summary>结束时技能</summary>
		[ProtoMember(68)]
		public int EndSkillId { get; set; }
		/// <summary>Buff触发技能</summary>
		[ProtoMember(69)]
		public string BuffToSkill { get; set; }
		/// <summary>技能伤害增加</summary>
		[ProtoMember(70)]
		public string SkillDamgeAddValue { get; set; }
		/// <summary>技能最多攻击人数</summary>
		[ProtoMember(71)]
		public int MaxAttackNumber { get; set; }

	}
}
