using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// ��Ϸ����������
/// </summary>
public class LanguageCfg
{
    public string key="";                             //����ֵ
    public string content = "";                       //��������
}

/// <summary>
/// �������ñ�
/// </summary>
public class TaskInfoCfg
{
    public int ID;						    //ID
    public string system;					//����ϵͳ
    public string describe;				    //����
    public long targetNumber;			    //Ŀ����
    public int prepositionTaskID;		    //ǰ������ID
    public string award;					//����
    public int type;					    //���ͣ�1�ճ�2�ܳ�3�ɳ�4������
    public int open;					    //���أ�0�ر�1������
    public int jump;					    //��תĿ�ģ�0û����ת��
    public int iocn;					    //ͼ��
    public int gameID;					    //��Ӧ����Ϸid
    public int rank;					    //����
    public int frame;					    //���
    public int numberType;				    //�����ۼ����ͣ�1������2�ۻ����ͣ�
    public int taskType;				    //�������Ϳͻ��˷�����
    public string awardVip;					//ս���
    public int needUnlock;                  //�Ƿ���Ҫ������0����Ҫ1��Ҫ��
    public int vitality;                    //��Ծ��չʾ
}
//�����ñ�
public class CardInfoCfg
{
    public int ID;                          //ID
    public int xjNumber = 0;            //�����Ҫ���ĵ��ľ�ֵ
    public int hpNumber = 0;            //Ѫ��
    public int gjNumber = 0;            //����
    public string name;                 //��������
    public int type = 0;                //��������0ʿ���� 1������
    public int imageId = 0;             //��ӦͼƬid
}

//������
public class CardInfo
{
    public int id = 0;
    public int addId = 0;               //����id
    public int xjNumber = 0;            //�����Ҫ���ĵ��ľ�ֵ
    public int hpNumber = 0;            //Ѫ��
    public int gjNumber = 0;            //����
    public int hpNumberNow = 0;         //��ǰѪ��
    public int gjNumberNow = 0;         //��ǰ����
    public string name;                 //��������
    public int type = 0;                //��������0ʿ���� 1������
    public int xjType = 0;              //�ľ�����0��1ϲ2ŭ3��4��
    public int imageId = 0;             //��ӦͼƬid
    public int state = 0;               //1�������2BOSS����3�������4BOSS����5��ҳ���6BOSS����7���8BOSS
    public int liveHh = 0;              //���ڻغ��� 
    public int attackNumber = 0;        //ʣ�๥������

}

