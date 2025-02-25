using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace com.mob.mobpush
{
	public class MobPush : MonoBehaviour
	{
		public static string version = "3.0.1";

		public MobPushImpl mobPushImpl;
		public OnNotifyCallback onNotifyCallback;
		public OnTagsCallback onTagsCallback;
		public OnAliasCallback onAliasCallback;
		public OnRegIdCallback onRegIdCallback;
		public OnDemoReqCallback onDemoReqCallback;
		public OnBindPhoneNumCallback onBindPhoneNumCallback;
		public OnPrivacyPolicyCallback onPrivacyPolicyCallback;

		public delegate void OnNotifyCallback(int action, Hashtable resulte);

		public delegate void OnTagsCallback(int action, string[] tags, int operation, int errorCode);

		public delegate void OnAliasCallback(int action, string alias, int operation, int errorCode);

		public delegate void OnRegIdCallback(string regId);

		public delegate void OnDemoReqCallback(bool isSuccess);

		public delegate void OnBindPhoneNumCallback(bool isSuccess);

		public delegate void OnPrivacyPolicyCallback(string content, int errorCode);

		void Awake() {
			#if UNITY_IPHONE
				mobPushImpl = new iOSMobPushImpl(gameObject);
			#elif UNITY_ANDROID
				mobPushImpl = new AndroidImpl(gameObject);
			#endif

			mobPushImpl.addPushReceiver();
		}

		private void _MobPushCallback(string data) {
			if (data == null) {
				return;
			}

			Hashtable res = (Hashtable)MiniJSON.jsonDecode(data);

			if (res == null || res.Count <=0 ) {
				return;
			}

			int action = Convert.ToInt32(res["action"]);

			if (action == ResponseState.CoutomMessage || action == ResponseState.MessageRecvice || action == ResponseState.MessageOpened) {
				if (onNotifyCallback != null) {
					Hashtable result = (Hashtable) res["result"];
					onNotifyCallback(action, result);
				}
			} else if (action == 3) {
				if (onTagsCallback != null) {
					string stringTags = Convert.ToString(res["tags"]);
					ArrayList array = new ArrayList(stringTags.Split(','));
					string[] tags = (string[])array.ToArray(typeof(string));
					int operation = Convert.ToInt32(res["operation"]);
					int errorCode = Convert.ToInt32(res["errorCode"]);
					onTagsCallback(action, tags, operation, errorCode);
				}
			} else if (action == 4) {
				if (onAliasCallback != null) {
					string alias = Convert.ToString(res ["alias"]);
					int operation = Convert.ToInt32(res["operation"]);
					int errorCode = Convert.ToInt32(res["errorCode"]);
					onAliasCallback(action, alias, operation, errorCode);
				}
			}
		}

		private void _MobPushDemoCallback(string data) {
			if (data == null) {
				return;
			}
			Debug.Log ("_MobPushDemoCallback:" + data);
			Hashtable res = (Hashtable)MiniJSON.jsonDecode (data);
			if(res == null || res.Count <= 0){
				return;
			}
			int action = Convert.ToInt32(res["action"]);
			bool isSuccess;
			if (action == 1) {
				isSuccess = true;
			} else {
				isSuccess = false;
			}
			if (onDemoReqCallback != null) {
				onDemoReqCallback(isSuccess);
			}
		}

		private void _MobPushRegIdCallback(string regId) {
			if (regId == null) {
				return;
			}
			Debug.Log("_MobPushRegIdCallback-regId:" + regId);
			if (onRegIdCallback != null) {
				onRegIdCallback(regId);
			}
		}

		private void _MobPushBindPhoneNumCallback(string result) {
			if (result == null) {
                return;
            }
            Debug.Log ("_MobPushBindPhoneNumCallback-result:" + result);

            Hashtable res = (Hashtable)MiniJSON.jsonDecode(result);
            if (res == null || res.Count <= 0) {
                return;
            }
            int isSuccess = Convert.ToInt32(res["result"]);
            if (onBindPhoneNumCallback != null) {
                onBindPhoneNumCallback(isSuccess==1?true:false);
			}
		}

		private void _MobPushPrivacyPolicyCallback(string result) {
			if (result == null) {
				return;
			}
			Debug.Log("_MobPushPrivacyPolicyCallback-result:" + result);
			Hashtable res = (Hashtable)MiniJSON.jsonDecode(result);

			if (res == null || res.Count <= 0) {
				return;
			}
			String content = Convert.ToString(res["data"]);
			int errorCode = Convert.ToInt32(res["errorCode"]);
			if (onPrivacyPolicyCallback != null) {
				onPrivacyPolicyCallback(content,errorCode);
			}
		}

		public void initPushSDK(string appKey, string appSecret) {
			mobPushImpl.initPushSDK(appKey, appSecret);
		}

		public void updatePrivacyPermissionStatus(bool agree) {
			mobPushImpl.updatePrivacyPermissionStatus(agree);
		}

		public void stopPush() {
			mobPushImpl.stopPush();
		}

		public void restartPush() {
			mobPushImpl.restartPush();
		}

		public bool isPushStopped() {
			return mobPushImpl.isPushStopped();
		}

		public void addPushReceiver() {
			mobPushImpl.addPushReceiver();
		}

		public void setAppForegroundHiddenNotification(bool hidden) {
			mobPushImpl.setAppForegroundHiddenNotification(hidden);
		}

		/// <summary>
		/// 获取 regId
		/// </summary>
		public void getRegistrationId() {
			mobPushImpl.getRegistrationId();
		}

		/// <summary>
		/// 添加标签（数组）
		/// </summary>
		/// <param name="tags"></param>
		public void addTags(string[] tags) {
			mobPushImpl.addTags(tags);
		}

		public void getTags() {
			mobPushImpl.getTags();
		}

		/// <summary>
		/// 删除标签 （数组）
		/// </summary>
		/// <param name="tags"></param>
		public void deleteTags(string[] tags) {
			mobPushImpl.deleteTags(tags);
		}

		/// <summary>
		/// 删除所有标签
		/// </summary>
		public void cleanAllTags() {
			mobPushImpl.cleanAllTags();
		}

		/// <summary>
		/// 添加别名（别名只有一个，再添加覆盖）
		/// </summary>
		/// <param name="alias"></param>
		public void addAlias(string alias) {
			mobPushImpl.addAlias(alias);
		}

		/// <summary>
		/// 获取别名
		/// </summary>
		public void getAlias() {
			mobPushImpl.getAlias();
		}

		/// <summary>
		/// 删除别名
		/// </summary>
		public void cleanAllAlias() {
			mobPushImpl.cleanAllAlias();
		}

		public void setMobPushLocalNotification(LocalNotifyStyle style) {
			mobPushImpl.setMobPushLocalNotification(style);
		}

		public void setCustomNotification(CustomNotifyStyle style) {
			mobPushImpl.setCustomNotification(style);
		}

		public void bindPhoneNum(string phoneNum) {
			mobPushImpl.bindPhoneNum(phoneNum);
		}

		public void req(int type, string content, int space, Hashtable extras) {
			String jsonStr = MiniJSON.jsonEncode (extras);
			mobPushImpl.req(type, content, space, jsonStr);
		}

		public void deleteLocalNotification(string[] ids) {
			mobPushImpl.deleteLocalNotification(ids);
		}

		public void getPrivacyPolicy(string type, string language) {
			mobPushImpl.getPrivacyPolicy(type, language);
		}
#if UNITY_IPHONE || UNITY_IOS
		public void setAPNsForProduction(bool isPro) {
			mobPushImpl.setAPNsForProduction(isPro);
		}

		public void setBadge(int badge) {
			mobPushImpl.setBadge(badge);
		}

		public void clearBadge() {
			mobPushImpl.clearBadge();
		}
#endif

#if UNITY_ANDROID
		public void setClickNotificationToLaunchPage(bool isOpen) {
			mobPushImpl.setClickNotificationToLaunchPage(isOpen);
		}

		public void setNotifyIcon(string resIcon) {
			mobPushImpl.setNotifyIcon(resIcon);
		}

		public void setShowBadge(bool show) {
			mobPushImpl.setShowBadge(show);
		}
#endif

	}
}