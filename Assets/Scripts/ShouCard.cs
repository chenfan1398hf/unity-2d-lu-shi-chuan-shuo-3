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
    private Image txImage;
    private Text xText;
    private bool isNuType = false;
    private bool isAiType = false;
    public bool isLeType = false;
    private bool isShengDun = false;
    private GameObject sdObj = null;
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
        txImage = this.transform.Find("Image_tx").GetComponent<Image>();
        huiObj = this.transform.Find("ImageB").gameObject;
        bg = this.gameObject.GetComponent<Image>();
        info = _info;
     
        //��
        //if (info.xjType == 4)
        //{
        //    info.xjNumber = 0;
        //}
      
        ShowCrad();
    }
    private void ShowCrad()
    {
        gjText.text = info.gjNumberNow.ToString();
        hpText.text = info.hpNumberNow.ToString();
        xjText.text = info.xjNumber.ToString();
        GameManager.instance.SpritPropImageByPath("KaPai/"+ info.imageId, cardImage);
        UpateTxImage();
        UpdateXjType();
        UpdateCardBack();

    }
    public void UpdateCardBack()
    {
        //�����BOSS�������û�
        if (info.state == 4)
        {
            huiObj.SetActive(true);

        }
        else
        {
            huiObj.SetActive(false);

        }
        if (info.state == 4)
        {
            this.transform.Find("Image_tx").gameObject.SetActive(false);
        }
        else if (info.state == 6)
        {
            UpateTxImage();
        }
    }
    //����϶���
    public void DragMethod()
    {
        //��������϶�����
        if (info.state == 3 && GameManager.instance.GetPlayerOperand())
        {
            GameManager.instance.DrageCardSetFatherOut(this.gameObject);
            Vector3 mouseScreenPosition = Input.mousePosition;
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPosition.x, mouseScreenPosition.y, 100f));
            transform.position = mouseWorldPosition;
        }
        
    }
    //�����϶�
    public void EndDragMethod()
    {
        Debug.Log(this.transform.position.y);
        if (info.state == 3)
        {
            if (this.transform.position.y >= -2f)
            {
                //�ɹ�
                bool isBool = GameManager.instance.ChuCard(this.gameObject, info);
                if (isBool == false)
                {
                    //ʧ��
                    GameManager.instance.DrageCardSetFatherIn(this.gameObject);
                }
            }
            else
            {
                //ʧ��
                GameManager.instance.DrageCardSetFatherIn(this.gameObject);
            }
        }
       
    }
    //��ҵ����
    public void OnClickCard()
    { 

    }
    //��ȡ������
    public CardInfo GetCardInfo()
    {
        return info;
    }
    //�ɹ�����ɫ
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
    //���ƹ���
    public void CardAttack()
    {
        GameManager.instance.Attack(this.gameObject);
    }
    //����Ѫ��
    public void addHpNumber(int _number, bool isColor =false)
    {
        if (isShengDun && _number < 0)
        {
            isShengDun = false;
            if (sdObj != null)
            {
                Destroy(sdObj);
            }
            return;
        }
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
        if (_number < 0 && info.hpNumberNow > 0)
        {
            CheckXiXue(-_number);
        }
        CheckHp();
    }
    //���ӹ���
    public void addGjNumber(int _number, bool isColor = false)
    {
        info.gjNumberNow += _number;
        ShowCrad();
        if (isColor)
        {
            gjText.color = Color.red;
        }
    }
    //�۳��ж�����
    public void addAttackNumber(int _number)
    {
        info.attackNumber += _number;
        ShowAttack();
    }
    //Ѫ�����
    public void CheckHp()
    {
        if (info.hpNumberNow <= 0 && info.state < 7)
        {
            if (isAiType)
            {
                info.hpNumberNow = 1;
                isAiType = false;
                GameManager.instance.FHTx(this.gameObject.transform);
                ShowCrad();
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        if (info.hpNumberNow <= 0 && info.state == 7)
        {
            //BOSSʤ��
            GameManager.instance.EndGame(0);
        }
        if (info.hpNumberNow <= 0 && info.state == 8)
        {
            //���ʤ��
            GameManager.instance.EndGame(1);
        }
    }
    //ˢ��ϲŭ������ʾ
    public void UpdateXjType()
    {
        if (info.xjType == 0)
        {
            xText.text = "";
        }
        else if (info.xjType == 1)
        {
            xText.text = "ϲ";
        }
        else if (info.xjType == 2)
        {
            xText.text = "ŭ";
        }
        else if (info.xjType == 3)
        {
            xText.text = "��";
        }
        else if (info.xjType == 4)
        {
            xText.text = "��";
        }
    }
    //ŭ
    public void NuType()
    {
        addGjNumber(info.gjNumberNow,true);
        isNuType = true;
    }
    //��
    public void AiType()
    {
        isAiType = true;
    }
    //��
    public void LeType()
    {
        isLeType = true;

    }
    //ʥ��
    public void SetSdType()
    {
        isShengDun = true;
        sdObj = GameManager.instance.SDTx(this.gameObject.transform);
    }
    //ˢ����Ч
    public void UpateTxImage()
    {
        if (info.txState == 0)
        {
            this.transform.Find("Image_tx").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("Image_tx").gameObject.SetActive(true);
            GameManager.instance.SpritPropImageByPath("Tex/" + info.txState.ToString(), txImage);
        }

    }
    //��鳰��
    public bool CheckCf()
    {
        if (info.txState == 1)
        {
            return true;
        }
        return false;
    }
    //��Ѫ�߼����
    public void CheckXiXue(int _number)
    {
        if (info.txState == 3)
        {
            info.hpNumberNow += _number;
            if (info.hpNumberNow > info.hpNumber)
            {
                info.hpNumberNow = info.hpNumber;
            }
            ShowCrad();
            if (_number > 0)
            {
                GameManager.instance.HpTx(this.transform);
            }
        }
    }
}
