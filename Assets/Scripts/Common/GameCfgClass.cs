using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 游戏内语言文字
/// </summary>
public class LanguageCfg
{
    public string key="";                             //索引值
    public string content = "";                       //文字内容
}

/// <summary>
/// 任务配置表
/// </summary>
public class TaskInfoCfg
{
    public int ID;						    //ID
    public string system;					//所属系统
    public string describe;				    //描述
    public long targetNumber;			    //目标数
    public int prepositionTaskID;		    //前置任务ID
    public string award;					//奖励
    public int type;					    //类型（1日常2周长3成长4线索）
    public int open;					    //开关（0关闭1开启）
    public int jump;					    //跳转目的（0没有跳转）
    public int iocn;					    //图标
    public int gameID;					    //对应的游戏id
    public int rank;					    //排序
    public int frame;					    //外框
    public int numberType;				    //任务累计类型（1限制类2累积类型）
    public int taskType;				    //任务类型客户端分类用
    public string awardVip;					//战令奖励
    public int needUnlock;                  //是否需要解锁（0不需要1需要）
    public int vitality;                    //活跃度展示
}
//牌配置表
public class CardInfoCfg
{
    public int ID;                          //ID
    public int xjNumber = 0;            //打出需要消耗的心境值
    public int hpNumber = 0;            //血量
    public int gjNumber = 0;            //攻击
    public string name;                 //卡牌名称
    public int type = 0;                //卡牌类型0士兵牌 1法术牌
    public int imageId = 0;             //对应图片id
}

//牌数据
public class CardInfo
{
    public int id = 0;
    public int addId = 0;               //自增id
    public int xjNumber = 0;            //打出需要消耗的心境值
    public int hpNumber = 0;            //血量
    public int gjNumber = 0;            //攻击
    public int hpNumberNow = 0;         //当前血量
    public int gjNumberNow = 0;         //当前攻击
    public string name;                 //卡牌名称
    public int type = 0;                //卡牌类型0士兵牌 1法术牌
    public int xjType = 0;              //心境类型0无1喜2怒3哀4乐
    public int imageId = 0;             //对应图片id
    public int state = 0;               //1玩家牌组2BOSS牌组3玩家手牌4BOSS手牌5玩家场牌6BOSS场牌7玩家8BOSS
    public int liveHh = 0;              //存在回合数 
    public int attackNumber = 0;        //剩余攻击次数

}

