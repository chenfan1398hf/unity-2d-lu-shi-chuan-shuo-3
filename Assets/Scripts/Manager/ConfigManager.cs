using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using System.Linq;
using System.IO;

public class ConfigManager
{

    public List<CardInfoCfg> CardInfoCfg = new List<CardInfoCfg>();                             //任务配置
    //读取游戏配置表
    public void InitGameCfg()  //初始化自定义的游戏配置表
    {
        //故意未Try 这部分加载 如果配置表异常会直接关闭程序
        string txt = FileTool.Read_Txt("cardCfg");
        CardInfoCfg = JsonMapper.ToObject<List<CardInfoCfg>>(txt);

        return;
    }

    public CardInfoCfg GetCardInfoCfgByKey(int key)
    {
        return CardInfoCfg.Find((item) => item.ID == key);
    }
}

public class CardPlayManager
{
    private int operand = 1;        //本回合操作对象 1玩家2BOSS
    private int hhNumber = 0;       //当前回合数
    private int endTime = 30;       //回合结束时间
    private int xyNumber = 0;       //本局剩余心愿数量


    //初始化数据
    public void InitData()
    {
        operand = 1;
        hhNumber = 0;
        endTime = 30;
        xyNumber = GetHhXyNumber();
        FaPaiFirst();
        BeginHuiHe();
    }
    //获取回合心愿值
    public int GetHhXyNumber()
    {
        int iRet = 0;
        iRet = hhNumber / 2 + 1;
        if (iRet > 10)
        {
            iRet = 10;
        }
        return iRet;
    }
    //回合倒计时
    public void EndTime(int _number)
    {
        endTime += _number;
    }
    //获取回合倒计时
    public int GetEndTiemNumber()
    {
        return endTime;
    }
    //回合结束
    public void EndHuiHe()
    {
        hhNumber++;
        endTime = 30;
        GameManager.instance.AddLiveHh(operand);
        GameManager.instance.RecycleAttackNumber(operand);
        if (operand == 1)
        {
            operand = 2;
        }
        else
        {
            operand = 1;
        }
        GameManager.instance.DesChangCard();
        BeginHuiHe();
    }
    //回合开始
    public void BeginHuiHe()
    {
        xyNumber = GetHhXyNumber();
        GameManager.instance.UpdateXyShow();
        GameManager.instance.SetCj(operand);
        GameManager.instance.AddAttackNumnber(operand);
        if (operand == 1)
        {
            GameManager.instance.AddPlayerShouCard(1);
        }
        else
        {
            GameManager.instance.AddBossShouCard(1);
            GameManager.instance.StartCoroutine(GameManager.instance.BossPlayerCard());
        }
    }
    //获取回合操作者
    public int GetOperand()
    {
        return operand;
    }
    //获取剩余心愿数量
    public int GetXyNumber()
    {
        return xyNumber;
    }
    //修改心境值
    public void AddXjNumber(int _number)
    {
        xyNumber += _number;
        //刷新
        GameManager.instance.UpdateXyShow();
    }
    //发牌
    public void FaPaiFirst()
    {
        int number = 3;
        GameManager.instance.AddBossShouCard(number);
        GameManager.instance.AddPlayerShouCard(number);
    }
  
}