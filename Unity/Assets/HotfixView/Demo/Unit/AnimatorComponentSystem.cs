﻿using System;
using UnityEngine;

namespace ET
{

	public class AnimatorComponentAwakeSystem : AwakeSystem<AnimatorComponent>
	{
		public override void Awake(AnimatorComponent self)
		{
			self.Parameter.Clear();
			self.MissParameter.Clear();
			self.animationClips.Clear();
            self.animatorControllers.Clear();
            GameObject gameObject = self.Parent.GetComponent<GameObjectComponent>().GameObject;
            self.InitController(gameObject);
            self.UpdateAnimator(gameObject);
			self.UpdateController();
        }
	}

	public class AnimatorComponentDestroySystem : DestroySystem<AnimatorComponent>
	{
		public override void Destroy(AnimatorComponent self)
		{
			self.Animator = null;
		}
	}

	public static class AnimatorComponentSystem
	{
		public static int Speed = 1;
		//public static SkillActionBase skill;

		public static void InitController(this AnimatorComponent self, GameObject gameObject)
		{
            Unit unit = self.GetParent<Unit>();
			self.UnitType = unit.Type;
            if (unit.Type != UnitType.Player || unit.ConfigId != 3)
            {
                return;
            }
			ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
			if (rc == null)
			{
				self.animatorControllers.Add(gameObject.GetComponent<Animator>().runtimeAnimatorController);

                return;
			}
            GameObject AnimatorList = rc.Get<GameObject>("AnimatorList");
			if (AnimatorList == null)
			{
                self.animatorControllers.Add(gameObject.GetComponent<Animator>().runtimeAnimatorController);

                return;
			}
            for(int i = 0; i < AnimatorList.transform.childCount; i++)
			{
				GameObject child = AnimatorList.transform.GetChild(i).gameObject;
				self.animatorControllers.Add( child.GetComponent<Animator>().runtimeAnimatorController );
            }
        }

        public static void UpdateController(this AnimatorComponent self)
		{
			Unit unit = self.GetParent<Unit>();
			if (unit.Type != UnitType.Player || unit.ConfigId != 3)
			{
				return;
			}
			int equipIndex = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.EquipIndex);
			if (self.animatorControllers.Count < equipIndex)
			{
                equipIndex = 0;
			}

            GameObject gameObject = unit.GetComponent<GameObjectComponent>().GameObject;
            Animator animator = gameObject.GetComponentInChildren<Animator>();
			animator.runtimeAnimatorController = self.animatorControllers[equipIndex];

            self.Animator = animator;
            self.animationClips.Clear();	
			self.Parameter.Clear();	
            foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
            {
                self.animationClips[animationClip.name] = animationClip;
            }
            foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
            {
                self.Parameter.Add(animatorControllerParameter.name);
            }
        }

		public static void UpdateAnimator(this AnimatorComponent self, GameObject gameObject)
		{
			Animator animator = gameObject.GetComponentInChildren<Animator>();
			if (animator == null)
			{
				return;
			}

			if (animator.runtimeAnimatorController == null)
			{
				return;
			}

			if (animator.runtimeAnimatorController.animationClips == null)
			{
				return;
			}
			self.Animator = animator;
			self.animationClips.Clear();
			self.Parameter.Clear();
            foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
			{
				self.animationClips[animationClip.name] = animationClip;
			}
			foreach (AnimatorControllerParameter animatorControllerParameter in animator.parameters)
			{
				self.Parameter.Add(animatorControllerParameter.name);
			}
		}

		public static void Update(this AnimatorComponent self)
		{
			if (self.isStop)
			{
				return;
			}

			if (self.MotionType == MotionType.None)
			{
				return;
			}

			try
			{
				self.Animator.SetFloat("MotionSpeed", self.MontionSpeed);

				self.Animator.SetTrigger(self.MotionType.ToString());

				self.MontionSpeed = 1;
				self.MotionType = MotionType.None;
			}
			catch (Exception ex)
			{
				throw new Exception($"动作播放失败: {self.MotionType}", ex);
			}
		}

		public static bool HasParameter(this AnimatorComponent self, string parameter)
		{
			return self.Parameter.Contains(parameter);
		}

		public static string CurrentAnimation(this AnimatorComponent self)
		{
			AnimatorClipInfo animatorClipInfo = self.Animator.GetCurrentAnimatorClipInfo(0)[0];
			Log.Debug($"animatorClipInfo.clip.name： {animatorClipInfo.clip.name}");
			return animatorClipInfo.clip.name;
		}

		public static float CurrentSateTime(this AnimatorComponent self)
		{
			AnimatorStateInfo animatorStateInfo = self.Animator.GetCurrentAnimatorStateInfo(0); 
			return animatorStateInfo.normalizedTime;
		}

		public static void Play(this AnimatorComponent self, string motionType, int skillid = 0, float motionSpeed = 1f, bool repteat = false)
		{
            self.MotionType = motionType;
			self.MontionSpeed = motionSpeed;
			if (null == self.Animator || !SettingHelper.ShowAnimation)
			{
				return;
			}
			if (self.MissParameter.Contains(motionType))
			{
				if (self.UnitType == UnitType.Player)
				{
					Log.ILog.Debug($"缺失动作: {motionType}  技能ID:{skillid}  强制改为动作:Skill_1");
					motionType = "Skill_1";
					self.MotionType = motionType;
				}
				else
				{
                    return;
                }
			}

            if (self.HasParameter(motionType.ToString()))
            {
				if (repteat)
				{
                    self.Animator.Play(motionType, 0, 0);
                }
				else
				{
                    self.Animator.Play(motionType);
                }
				return;
            }

            bool hasAction = self.Animator.HasState(0,Animator.StringToHash(motionType));
			if (hasAction)
			{
				self.Parameter.Add(motionType);
				self.Animator.Play(motionType);
			}
			else
			{
				self.MissParameter.Add(motionType);
			}
		}

		public static void PauseAnimator(this AnimatorComponent self)
		{
			if (self.isStop)
			{
				return;
			}
			self.isStop = true;

			if (self.Animator == null)
			{
				return;
			}
			self.stopSpeed = self.Animator.speed;
			self.Animator.speed = 0;
		}

		public static void RunAnimator(this AnimatorComponent self)
		{
			if (!self.isStop)
			{
				return;
			}

			self.isStop = false;

			if (self.Animator == null)
			{
				return;
			}
			self.Animator.speed = self.stopSpeed;
		}

		public static void SetBoolValue(this AnimatorComponent self, string name, bool state)
		{
			if (self.Animator == null)
			{
				return;
			}
			if (!self.HasParameter(name))
            {
                return;
            }
			self.Animator.SetBool(name, state);
		}

		public static void SetFloatValue(this AnimatorComponent self, string name, float state)
		{
			//if (!self.HasParameter(name))
			//{
			//	return;
			//}
			self.Animator.SetFloat(name, state);
		}

		public static void SetIntValue(this AnimatorComponent self, string name, int value)
		{
			//if (!self.HasParameter(name))
			//{
			//	return;
			//}
			self.Animator.SetInteger(name, value);
		}

		public static void SetTrigger(this AnimatorComponent self, string name)
		{
			//if (!self.HasParameter(name))
			//{
			//	return;
			//}
			self.Animator.SetTrigger(name);
		}

		public static void SetAnimatorSpeed(this AnimatorComponent self, float speed)
		{
			self.stopSpeed = self.Animator.speed;
			self.Animator.speed = speed;
		}

		public static void ResetAnimatorSpeed(this AnimatorComponent self)
		{
			self.Animator.speed = self.stopSpeed;
		}
	}
}