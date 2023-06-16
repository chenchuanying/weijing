﻿namespace ET
{

    [Event]
    public class Unit_OnHpUpdate : AEventClass<EventType.UnitHpUpdate>
    {
        protected override  void Run(object upchange)
        {
            EventType.UnitHpUpdate args = upchange as EventType.UnitHpUpdate;
            //更新当前血量
            HeroHeadBarComponent heroHeadBarComponent = args.Unit.GetComponent<HeroHeadBarComponent>();
            if (heroHeadBarComponent!= null)
            {
                heroHeadBarComponent.UpdateBlood();
            }

            Unit attack = args.Attack;
            if (args.Unit.MainHero || (attack != null && UnitTypeHelper.GetMasterId(attack) == UnitHelper.GetMyUnitId(args.Unit.ZoneScene())))
            {
                FallingFontComponent fallingFontComponent = args.Unit.DomainScene().GetComponent<FallingFontComponent>();
                if (fallingFontComponent != null && UISettingHelper.ShowBlood)
                {
                    //触发飘字
                    fallingFontComponent.Play(args.ChangeHpValue, args.Unit, args.DamgeType);
                }
                //触发受击特效
                if (args.SkillID != 0)
                {
                    FunctionEffect.GetInstance().PlayHitEffect(args.Unit, args.SkillID);
                }
            }

            if (args.Unit.MainHero)
            {
                args.Unit.GetComponent<SingingComponent>().BeAttacking();
            }
            MapComponent mapComponent = args.Unit.ZoneScene().GetComponent<MapComponent>();
            //主界面血條
            UI mainui = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIMain);
            mainui?.GetComponent<UIMainComponent>().OnUpdateHP(args.Unit, mapComponent.SceneTypeEnum);

            if (mapComponent.SceneTypeEnum == SceneTypeEnum.PetDungeon
             || mapComponent.SceneTypeEnum == SceneTypeEnum.PetTianTi)
            {
                UI petmain = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIPetMain);
                petmain?.GetComponent<UIPetMainComponent>().OnUnitHpUpdate(args.Unit);
            }
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.TrialDungeon && args.Unit.Type == UnitType.Monster)
            {
                UI trialmain = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UITrialMain);
                trialmain.GetComponent<UITrialMainComponent>().OnUpdateHurt(args.ChangeHpValue * -1);
            }
        }
    }
}