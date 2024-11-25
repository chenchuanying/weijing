using System;
using DG.Tweening;
using UnityEngine;

namespace ET
{

	public class CameraComponentAwakeSystem : AwakeSystem<CameraComponent>
	{
		public override void Awake(CameraComponent self)
		{
			self.Awake();
		}
	}

	public class CameraComponentLateUpdateSystem : LateUpdateSystem<CameraComponent>
	{
		public override void LateUpdate(CameraComponent self)
		{
			self.LateUpdate();
		}
	}

	public class CameraComponentDestroySystem : DestroySystem<CameraComponent>
	{
		public override void Destroy(CameraComponent self)
		{
			UIHelper.CurrentNpcId = 0;
			UIHelper.CurrentNpcUI = "";
		}
	}

	public static class CameraComponentSystem 
	{
		
		public static void Awake(this CameraComponent self)
		{
			self.MainCamera = GameObject.Find("Global/Main Camera").GetComponent<Camera>();

            // $"{offsetposition.x}_{offsetposition.y}_{offsetposition.z}
            string camerainfo =  PlayerPrefsHelp.GetString(PlayerPrefsHelp.CameraParams);

			if (string.IsNullOrEmpty(camerainfo))
			{
                self.OffsetPostion = new Vector3(0, 10f, -6f);
            }
			else
			{
				string[] infolist = camerainfo.Split('_');
                self.OffsetPostion = new Vector3(float.Parse(infolist[0]), float.Parse(infolist[1]), float.Parse(infolist[2]));
            }

			float lendepth = PlayerPrefsHelp.GetFloat(PlayerPrefsHelp.LenDepth);
            if (lendepth == 0f)
			{
				self.LenDepth = 1f;
			}
			else
			{
				self.LenDepth = lendepth;
			}


            self.PullRate = 1f;
			self.CameraMoveType = CameraMoveType.Normal;
			self.MainUnit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
			
			self.OnEnterScene(self.ZoneScene().GetComponent<MapComponent>().SceneTypeEnum );
		}

		//== SceneTypeEnum.MainCityScene
		public static  void OnEnterScene(this CameraComponent self, int sceneTypeEnum)
		{
			switch (sceneTypeEnum)
			{
				case SceneTypeEnum.PetTianTi:
					self.CameraMoveType = CameraMoveType.PetFuben;
					self.MainCamera.transform.position = AIHelp.FuBenCameraPosition;
					self.MainCamera.transform.localRotation = AIHelp.FuBenCameraRotation;
					break;
				case SceneTypeEnum.PetDungeon:
					self.CameraMoveType = CameraMoveType.PetFuben;
					self.MainCamera.transform.position = AIHelp.FuBenCameraPosition;
					self.MainCamera.transform.localRotation = AIHelp.FuBenCameraRotation;
					break;
				case SceneTypeEnum.PetMing:
                    self.CameraMoveType = CameraMoveType.PetFuben;
                    self.MainCamera.transform.position = AIHelp.FuBenCameraPosition;
                    self.MainCamera.transform.localRotation = AIHelp.FuBenCameraRotation;
                    break;
				default:
					self.CameraMoveType = CameraMoveType.Normal;
					break;
			}
		}
		
		/// <summary>
		/// 拉远摄像机
		/// </summary>
		/// <param name="self"></param>
		public static void SetPullCamera(this CameraComponent self)
		{
            self.PullRate = 1f;
            self.CameraMoveType = CameraMoveType.Pull;
		}

		public static void PullCameraMove(this CameraComponent self)
		{
			if (self.PullRate >= 1.5f) // 最大距离
			{
				return;
			}

			self.PullRate += Time.deltaTime * 0.08f; // 速度
			self.MainCamera.transform.position = self.MainUnit.Position + self.OffsetPostion * self.PullRate;
			self.MainCamera.transform.LookAt(self.MainUnit.Position); 
		}

		/// <summary>
		/// 回位摄像机
		/// </summary>
		/// <param name="self"></param>
		public static void SetRollbackCamera(this CameraComponent self)
		{
			self.CameraMoveType = CameraMoveType.Rollback;
		}

		public static void RollbackCamera(this CameraComponent self)
		{
			if (self.PullRate <= 1f)
			{
				self.CameraMoveType = CameraMoveType.Normal;
				return;
			}
			self.PullRate -= Time.deltaTime * 0.3f;
			self.MainCamera.transform.position = self.MainUnit.Position + self.OffsetPostion * self.PullRate;
			self.MainCamera.transform.LookAt(self.MainUnit.Position); 
		}
		
		//摄像机镜头处理
		public static void SetBuildEnter(this CameraComponent self, Unit npc, Action action)
		{
			self.NpcUnit = npc;
			self.CameraMoveTime = 0f;
			self.CameraMoveType = (npc != null) ? CameraMoveType.NpcEnter : CameraMoveType.Normal;

			self.TargetPosition = npc.Position + npc.Forward * 4f;
			self.TargetPosition.y += 2f;

			Vector3 temp1 = npc.Position + new Vector3(0f, 1f, 0f);
			//Vector3 forward = temp1 - self.TargetPosition;
			self.OldCameraPostion = self.MainCamera.transform.position;
			self.OnBuildEnter = action;

			UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene()).GetComponent<UIUnitHpComponent>()?.ShowHearBar(false);
		}

		public static void SetBuildExit(this CameraComponent self)
		{
			Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
			self.CameraMoveTime = 0f;
			self.CameraMoveType = CameraMoveType.NpcExit;
			self.OldCameraPostion = self.MainCamera.transform.localPosition;
			self.TargetPosition = unit.Position + self.OffsetPostion;
		}

		public static void BuildEnterMove(this CameraComponent self)
		{
			if (self.CameraMoveTime > 0.8f && self.OnBuildEnter != null)
			{
				self.OnBuildEnter();
				self.OnBuildEnter = null;
			}
			if (self.CameraMoveTime > 1f)
			{
				return;
			}

			Vector3 chaV3 = self.OldCameraPostion + (self.TargetPosition - self.OldCameraPostion) * self.CameraMoveTime;
			self.MainCamera.transform.localPosition = chaV3;

			if (self.NpcUnit.Type == UnitType.Monster)
			{
				self.MainCamera.transform.LookAt(self.NpcUnit.Position + Vector3.up * 0.5f);
			}
			else
			{
				self.MainCamera.transform.LookAt(self.NpcUnit.Position + Vector3.up);
			}
		}

		public static void BuildExitMove(this CameraComponent self)
		{
			Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
			if (unit == null)
			{
				return;
			}
			if (self.CameraMoveTime > 1f)
			{
				self.CameraMoveType = CameraMoveType.Normal;
				unit.GetComponent<UIUnitHpComponent>()?.ShowHearBar(true);
				return;
			}
			Vector3 chaV3 = self.OldCameraPostion + (self.TargetPosition - self.OldCameraPostion) * self.CameraMoveTime;
			self.MainCamera.transform.position = chaV3;
			Vector3 lookPosition = self.NpcUnit.Position + (unit.Position - self.NpcUnit.Position) * self.CameraMoveTime;
			self.MainCamera.transform.LookAt(lookPosition);
		}

		public static void LateUpdate(this CameraComponent self)
		{
			if (self.CameraMoveType == CameraMoveType.PetFuben)
			{
				return;
			}
			if (self.CameraMoveType == CameraMoveType.NpcEnter)
			{
				self.CameraMoveTime += Time.deltaTime * 2f;
				self.BuildEnterMove();
				return;
			}
			if (self.CameraMoveType == CameraMoveType.NpcExit)
			{
				self.CameraMoveTime += Time.deltaTime * 2f;
				self.BuildExitMove();
				return;
			}
			if(self.CameraMoveType == CameraMoveType.Pull)
			{
				self.PullCameraMove();
				return;
			}

			if (self.CameraMoveType == CameraMoveType.Rollback)
			{
				self.RollbackCamera();
				return;
			}
			//if (self.MainCamera.GetComponent<CameraFollow>() != null)
			//{
			//	self.OffsetPostion = self.MainCamera.GetComponent<CameraFollow>().OffsetPostion;
			//}
			if (self.IsRotateing)
			{
				self.CalculateOffset();
			}
			
			self.MainCamera.transform.position = self.MainUnit.Position + self.OffsetPostion * self.LenDepth;
			self.MainCamera.transform.LookAt(self.MainUnit.Position); 
		}

		public static void CalculateOffset(this CameraComponent self)
		{
			self.OffsetPostion.y = self.Distance * Mathf.Sin(self.OffsetAngleY * self.ANGLE_CONVERTER);
			float newRadius = self.Distance * Mathf.Cos(self.OffsetAngleY * self.ANGLE_CONVERTER);
			self.OffsetPostion.x = newRadius * Mathf.Sin(self.OffsetAngleX * self.ANGLE_CONVERTER);
			self.OffsetPostion.z = -newRadius * Mathf.Cos(self.OffsetAngleX * self.ANGLE_CONVERTER);
		}
		
		
		//开始旋转，纪录当前偏移角度，用于复原
		public static void StartRotate(this CameraComponent self)
		{
			self.IsRotateing = true;
 
			self.RecordAngleX = self.OffsetAngleX;
			self.RecordAngleY = self.OffsetAngleY;
		}
 
		//旋转，修改偏移角度的值，屏幕左右滑动即修改m_offsetAngleX，上下滑动修改m_offsetAngleY
		public static void Rotate(this CameraComponent self, float x, float y)
		{
			if (x != 0)
			{
				self.OffsetAngleX += x;
			}

			if (y != 0)
			{
				self.OffsetAngleY += y;
				self.OffsetAngleY = self.OffsetAngleY > self.MAX_ANGLE_Y? self.MAX_ANGLE_Y : self.OffsetAngleY;
				self.OffsetAngleY = self.OffsetAngleY < self.MIN_ANGLE_Y? self.MIN_ANGLE_Y : self.OffsetAngleY;
			}
		}

		//旋转结束，如需要复原镜头则，偏移角度还原并计算偏移坐标
		public static void EndRotate(this CameraComponent self,bool isNeedReset = false)
		{
			self.IsRotateing = false;

			if (isNeedReset)
			{
				self.OffsetAngleY = self.RecordAngleY;
				self.OffsetAngleX = self.RecordAngleX;
				self.CalculateOffset();
			}
		}
	}
}
