
using UnityEngine;
using UnityEngine.UI;

namespace ET
{


    public class UICreateRoleListComponentAwake : AwakeSystem<UICreateRoleListComponent, GameObject>
    {
        public override void Awake(UICreateRoleListComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.ObjRoleName = rc.Get<GameObject>("RoleName");
            self.ObjRoleLv = rc.Get<GameObject>("RoleLv");
            self.ObjImgSelect = rc.Get<GameObject>("ImgSelect");
            self.ObjImgOccHeadIcon = rc.Get<GameObject>("Img_RoleHeadIcon");
            self.ImageDi = rc.Get<GameObject>("ImageDi");
            self.ObjBtnSelectRole = rc.Get<GameObject>("BtnSelectRole");
            self.RoleOcc = rc.Get<GameObject>("RoleOcc");
            self.NoRole = rc.Get<GameObject>("NoRole");
            self.Role = rc.Get<GameObject>("Role");
            self.ObjBtnSelectRole.GetComponent<Button>().onClick.AddListener(() => { self.ClickOnSeleRoleList().Coroutine(); });
        }
    }

    
    public static class UICreateRoleListComponentSystem
    {

        //展示角色列表
        public static void ShowRoleList(this UICreateRoleListComponent self, CreateRoleInfo createRoleList)
        {
            self.CreateRoleInfo = createRoleList;
            if (self.CreateRoleInfo != null)
            {
                self.ObjRoleName.GetComponent<Text>().text = self.CreateRoleInfo.PlayerName;
                self.ObjRoleLv.GetComponent<Text>().text = GameSettingLanguge.LoadLocalization("等级:") + self.CreateRoleInfo.PlayerLv.ToString();
                self.ObjRoleLv.SetActive(true);
                if (self.CreateRoleInfo.OccTwo > 0)
                {
                    OccupationTwoConfig occupationTwo = OccupationTwoConfigCategory.Instance.Get(self.CreateRoleInfo.OccTwo);
                    self.RoleOcc.GetComponent<Text>().text = $"职业:{occupationTwo.OccupationName}";
                }
                else
                {
                    OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(self.CreateRoleInfo.PlayerOcc);
                    self.RoleOcc.GetComponent<Text>().text = $"职业:{occupationConfig.OccupationName}";
                }
                UICommonHelper.ShowOccIcon(self.ObjImgOccHeadIcon, self.CreateRoleInfo.PlayerOcc);
                self.ObjImgOccHeadIcon.SetActive(true);
                self.NoRole.SetActive(false);
                self.Role.SetActive(true);
            }
            else 
            {
                AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
                if (accountInfoComponent.CreateRoleList.Count >= 4)
                {
                    self.NoRole.SetActive(false);
                    self.Role.SetActive(false);
                    self.ObjRoleLv.SetActive(false);
                    self.ObjImgOccHeadIcon.SetActive(false);
                }
                else
                {
                    //显示为空
                    self.NoRole.SetActive(true);
                    self.Role.SetActive(false);
                    self.ObjRoleName.GetComponent<Text>().text = GameSettingLanguge.LoadLocalization("点击创建角色");
                    self.ObjRoleLv.SetActive(false);
                    self.ObjImgOccHeadIcon.SetActive(false);
                    self.RoleOcc.GetComponent<Text>().text = "职业:战士/法师";
                }
            }
            self.ImageDi.SetActive(self.CreateRoleInfo == null);
        }

        //选择角色界面
        public static async ETTask ClickOnSeleRoleList(this UICreateRoleListComponent self)
        {
            if (self.CreateRoleInfo != null)
            {
                //选择进入游戏的角色
                //ui.GetComponent<UILobbyComponent>().SeletXuHaoID = self.SelectXuHaoID;
            }
            else
            {
                //点击开始创建,显示创建角色界面
                //ui.GetComponent<UILobbyComponent>().SeletXuHaoID = -1;           // -1表示创建一个新的角色

                //打开创建界面
                //ui.GetComponent<UILobbyComponent>().OpenCreateRoleShow();
                UI createRole = await UIHelper.Create( self.DomainScene(), UIType.UICreateRole );
                createRole.GetComponent<UICreateRoleComponent>().ShowHeroSelect(1);
                UIHelper.Remove( self.DomainScene(), UIType.UILobby);
                return;
            }
            //Log.Info("提示啦提示啦！！！！");
            //更新选中提示
            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            if (self.CreateRoleInfo == null && accountInfoComponent.CreateRoleList.Count >= 4)
            {
                FloatTipManager.Instance.ShowFloatTip("角色列表已达上限！");
                return;
            }
            
            UI ui = UIHelper.GetUI(self.DomainScene(), UIType.UILobby);
            ui.GetComponent<UILobbyComponent>().SeletRoleInfo = self.CreateRoleInfo;
            ui.GetComponent<UILobbyComponent>().UpdateSelectShow().Coroutine();
        }

        //更新选中状态
        public static void UpdateSelectStatus(this UICreateRoleListComponent self, CreateRoleInfo createRoleListInfo)
        {

            //Log.Info("self.SelectXuHaoID = " + self.SelectXuHaoID);
            if (createRoleListInfo != null && createRoleListInfo == self.CreateRoleInfo)
            {
                self.ObjImgSelect.SetActive(true);
                self.GameObject.transform.localScale = Vector3.one * 1.2f;
            }
            else
            {
                self.ObjImgSelect.SetActive(false);
                self.GameObject.transform.localScale = Vector3.one;
            }
        }
    }

}
