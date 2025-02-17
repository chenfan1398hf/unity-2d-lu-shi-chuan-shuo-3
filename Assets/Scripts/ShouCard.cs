using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShouCard : MonoBehaviour
{
    private CardInfo info = new CardInfo();
    private Text gjText;
    private Text hpText;
    private Image cardImage;
    private Text xjText;
    private GameObject huiObj;
    private Image bg;
    private Text xText;
    private bool isNuType = false;
    private bool isAiType = false;
    public bool isLeType = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitCardInfo(CardInfo _info)
    {
        gjText = this.transform.Find("Image_gj/Text (Legacy)").GetComponent<Text>();
        hpText = this.transform.Find("Image_hp/Text (Legacy)").GetComponent<Text>();
        xjText = this.transform.Find("Image_xj/Text (Legacy)").GetComponent<Text>();
        xText = this.transform.Find("Text").GetComponent<Text>();
        cardImage = this.transform.Find("Image_pai").GetComponent<Image>();
        huiObj = this.transform.Find("ImageB").gameObject;
        bg = this.gameObject.GetComponent<Image>();
        info = _info;
     
        //乐
        if (info.xjType == 4)
        {
            info.xjNumber = 0;
        }
      
        ShowCrad();
    }
    private void ShowCrad()
    {
        gjText.text = info.gjNumberNow.ToString();
        hpText.text = info.hpNumberNow.ToString();
        xjText.text = info.xjNumber.ToString();
        GameManager.instance.SpritPropImageByPath("KaPai/"+ info.imageId, cardImage);
        UpdateXjType();
        UpdateCardBack();

    }
    public void UpdateCardBack()
    {
        //如果是BOSS手牌则置灰
        if (info.state == 4)
        {
            huiObj.SetActive(true);
        }
        else
        {
            huiObj.SetActive(false);
        }
    }
    //玩家拖动牌
    public void DragMethod()
    {
        //玩家手牌拖动出牌
        if (info.state == 3 && GameManager.instance.GetPlayerOperand())
        {
            GameManager.instance.DrageCardSetFatherOut(this.gameObject);
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 100f));
            transform.position = mouseWorldPosition;
        }
        
    }
    //结束拖动
    public void EndDragMethod()
    {
        Debug.Log(this.transform.position.y);
        if (info.state == 3)
        {
            if (this.transform.position.y >= 0f)
            {
                //成功
                bool isBool = GameManager.instance.ChuCard(this.gameObject, info);
                if (isBool == false)
                {
                    //失败
                    GameManager.instance.DrageCardSetFatherIn(this.gameObject);
                }
            }
            else
            {
                //失败
                GameManager.instance.DrageCardSetFatherIn(this.gameObject);
            }
        }
       
    }
    //玩家点击牌
    public void OnClickCard()
    { 

    }
    //获取牌数据
    public CardInfo GetCardInfo()
    {
        return info;
    }
    //可攻击变色
    public void ShowAttack()
    {
        if (info.attackNumber > 0)
        {
            bg.color = Color.green;
        }
        else
        {
            bg.color = Color.white;
        }
    }
    //卡牌攻击
    public void CardAttack()
    {
        GameManager.instance.Attack(this.gameObject);
    }
    //增加血量
    public void addHpNumber(int _number, bool isColor =false)
    {
        info.hpNumberNow += _number;
        ShowCrad();
        if (isColor)
        {
            hpText.color = Color.green;
        }
        if (_number > 0)
        {
            GameManager.instance.HpTx(this.transform);
        }
    }
    //增加攻击
    public void addGjNumber(int _number, bool isColor = false)
    {
        info.gjNumberNow += _number;
        ShowCrad();
        if (isColor)
        {
            gjText.color = Color.red;
        }
    }
    //扣除行动次数
    public void addAttackNumber(int _number)
    {
        info.attackNumber += _number;
        ShowAttack();
    }
    //血量检查
    public void CheckHp()
    {
        if (info.hpNumberNow <= 0 && info.state < 7)
        {
            if (isAiType)
            {
                info.hpNumberNow = info.hpNumber;
                isAiType = false;
                ShowCrad();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        if (info.hpNumberNow <= 0 && info.state == 7)
        {
            //BOSS胜利
            GameManager.instance.EndGame(0);
        }
        if (info.hpNumberNow <= 0 && info.state == 8)
        {
            //玩家胜利
            GameManager.instance.EndGame(1);
        }
    }
    //刷新喜怒哀乐显示
    public void UpdateXjType()
    {
        if (info.xjType == 0)
        {
            xText.text = "";
        }
        else if (info.xjType == 1)
        {
            xText.text = "喜";
        }
        else if (info.xjType == 2)
        {
            xText.text = "怒";
        }
        else if (info.xjType == 3)
        {
            xText.text = "哀";
        }
        else if (info.xjType == 4)
        {
            xText.text = "乐";
        }
    }
    //怒
    public void NuType()
    {
        addGjNumber(info.gjNumberNow,true);
        isNuType = true;
    }
    //哀
    public void AiType()
    {
        isAiType = true;
    }
    //乐
    public void LeType()
    {
        isLeType = true;

    }

}
