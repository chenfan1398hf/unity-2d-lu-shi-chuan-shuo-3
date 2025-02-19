using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using Spine.Unity;
using UnityEngine.SceneManagement;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoSingleton<GameManager>
{
    #region ���캯���������
    public GameManager()
    {
        configMag = new ConfigManager();
    }
    public static bool isDbugLog = true;
    public PlayerData playerData = null;                            //������ݣ����س־û���
    public ConfigManager configMag;
    private System.Random Random1;                                   //�������
    private int TimeNumber = 0;
    private List<UnityAction> unityActionList = new List<UnityAction>();
    public bool isBattle = true;


    public static int TI_LI_MAX_NUMBER = 100;
    public static int TI_LI_CD_NUMBER = 600;

    private int maxChangCardNumber = 5;
    private int maxShouCardNumber = 8;

    #endregion

    private void Update()
    {
        foreach (var item in unityActionList)
        {
            item.Invoke();
        }
    }
    #region Awake()
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;//����֡��Ϊ60֡
        GetLocalPlayerData();
        Random1 = new System.Random(Guid.NewGuid().GetHashCode());
    }
    #endregion



    private void Start()
    {
        InitGame();
        this.InvokeRepeating("CheckTime", 0, 0.1f);
    }

    void CheckTime()
    {
        TimeNumber++;

        if (TimeNumber % 10 == 0)
        {

        }
        if (TimeNumber % 20 == 0)
        {

        }


    }


    #region OnApplicationPause(bool pause)������֪
    public void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            if (isDbugLog)
                Debug.Log("������֪");
            SaveGame();
        }
    }
    #endregion

    #region OnApplicationQuit() �˳���Ϸ��֪
    public void OnApplicationQuit()
    {
        if (isDbugLog)
            Debug.Log("�˳���֪");
        SaveGame();

    }
    #endregion

    #region ��ȡ��������
    public void GetLocalPlayerData()
    {
        playerData = PlayerData.GetLocalData();//��ȡ���س־û��������(��������������)
        configMag.InitGameCfg();//��ȡ���ñ�
        playerData.InitData();//�������ñ�ͱ������ݳ�ʼ����Ϸ����
    }
    #endregion

    #region SaveGame() �����������
    public void SaveGame()
    {
        //if(SocketManager.instance.socket!=null)
        //SocketManager.instance.socket.Disconnect();
        playerData.Save();
    }
    #endregion

    #region OnDestroy()
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    #endregion

    /// <summary>
    /// ע��һ��update��������
    /// </summary>
    /// <param name="_action"></param>
    public void AddUpdateListener(UnityAction _action)
    {
        unityActionList.Add(_action);
    }

    /// <summary>
    /// ����ͼƬ
    /// </summary>
    public void SpritPropImage(string id, Image image)
    {
        string path = "Icon/" + id;
        Sprite Tab3Img = ResourcesLoad.instance.Load<Sprite>(path);
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// ����ͼƬ--װ��ͼ��
    /// </summary>
    public void SpritPropEquipIcon(string id, Image image)
    {
        string path = "EquipIcon/" + id;
        Sprite Tab3Img = ResourcesLoad.instance.Load<Sprite>(path);
        image.sprite = Tab3Img;
    }


    /// <summary>
    /// ����ͼƬ
    /// </summary>
    public void SpritPropImageByPath(string path, Image image)
    {
        Sprite Tab3Img = Resources.Load(path, typeof(Sprite)) as Sprite;
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// ����ͼƬ
    /// </summary>
    public void SpritPropImageByPath(string path, SpriteRenderer image)
    {
        Sprite Tab3Img = Resources.Load(path, typeof(Sprite)) as Sprite;
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// ���Ԥ����
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fatherTransform"></param>
    /// <returns></returns>
    public GameObject AddPrefab(string name, Transform fatherTransform)
    {
        string newpath = "Prefab/" + name;
        GameObject obj = ObjPool.instance.GetObj(newpath, fatherTransform);
        obj.AddComponent<DesObj>();
        obj.GetComponent<DesObj>().InitDes(newpath);
        return obj;
    }
    /// <summary>
    /// ����Ԥ����
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(string name, GameObject gameObject)
    {
        string[] list = name.Split(new char[] { '(' });
        if (list.Length != 2)
        {
            string newpath = "Prefab/" + name;
            ObjPool.instance.Recycle(newpath, gameObject);
        }
        else
        {
            string newpath = "Prefab/" + list[0];
            ObjPool.instance.Recycle(newpath, gameObject);
        }
        return;
    }
    /// <summary>
    /// ����Ԥ����
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(GameObject prefabObj, GameObject gameObject, string _path = null)
    {
        ObjPool.instance.Recycle(prefabObj, gameObject, "Prefab/" + _path);
        return;
    }
    /// <summary>
    /// ����Ԥ����
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(GameObject gameObject)
    {
        string name = gameObject.GetComponent<DesObj>().name;
        ObjPool.instance.Recycle(name, gameObject);
        return;
    }
    /// <summary>
    /// ���Ŷ��������ö�������0֡
    /// </summary>
    public void PlaySpine(SkeletonGraphic _skeletonGraphic, bool isLoop, string _spineName, bool isRest)
    {
        if (isRest)
        {
            _skeletonGraphic.AnimationState.ClearTracks();
            _skeletonGraphic.AnimationState.Update(0);
        }
        _skeletonGraphic.AnimationState.SetAnimation(0, _spineName, isLoop);

        return;
    }
    /// <summary>
    /// ���Ŷ��������ö�������0֡
    /// </summary>
    public void PlaySpine(Animator _animator, string _spineName, bool isRest)
    {
        //_animator.Play(_spineName, 0 ,0f);
        if (isRest)
        {
            //_animator.Update(0);
            _animator.Play(_spineName, 0, 0f);
        }
        else
        {
            _animator.Play(_spineName);
        }
        return;
    }
    /// <summary>
    /// ��ȡ������ڶ�������
    /// </summary>
    /// <returns></returns>
    public ObjPool.PoolItem GetPoolItem(string name)
    {
        string newpath = "Prefab/" + name;
        return ObjPool.instance.GetPoolItem(newpath); ;
    }
    /// <summary>
    /// ������ȡͼƬ
    /// </summary>
    /// <param name="_url"></param>
    /// <param name="_image"></param>
    /// <returns></returns>
    public IEnumerator GetHead(string _url, Image _image)
    {
        if (_url == string.Empty || _url == "")
        {
            _url = "https://p11.douyinpic.com/aweme/100x100/aweme-avatar/mosaic-legacy_3797_2889309425.jpeg?from=3067671334";
        }

        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(1f, 1f));
                _image.sprite = sprite;
                //Renderer renderer = plane.GetComponent<Renderer>();
                //renderer.material.mainTexture = texture;
            }
        }
    }

    /// <summary>
    /// �������
    /// </summary>
    public void CleraPlayerData()
    {
        PlayerData.ClearLocalData();
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Editor/Tools/Clear")]
    static void CleraPlayerData1()
    {
        PlayerData.ClearLocalData();
    }
#endif
    private GameObject[] GetDontDestroyOnLoadGameObjects()
    {
        var allGameObjects = new List<GameObject>();
        allGameObjects.AddRange(FindObjectsOfType<GameObject>());
        //�Ƴ����г��������Ķ���
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }
        //�Ƴ�������Ϊnull�Ķ���
        int k = allGameObjects.Count;
        while (--k >= 0)
        {
            if (allGameObjects[k].transform.parent != null)
            {
                allGameObjects.RemoveAt(k);
            }
        }
        return allGameObjects.ToArray();
    }

    public MusicManager musicManager;
    private List<int> paiChiPlayer = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15 };
    private List<int> paikuChiBoss = new List<int> { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14, 15, 15 };
    private List<GameObject> paichiPlayerObj = new List<GameObject>();
    private List<GameObject> paichiBossObj = new List<GameObject>();
    private List<GameObject> randomElements;



    private List<CardTxAndId> paikuPlayer = new List<CardTxAndId>();
    private List<CardTxAndId> paikuBoss = new List<CardTxAndId>();

    private List<CardInfo> bossCardList = new List<CardInfo>();             //boss����
    private List<CardInfo> playerCardList = new List<CardInfo>();           //�������

    private List<GameObject> bossShouCardList = new List<GameObject>();         //boss����
    private List<GameObject> playerShouCardList = new List<GameObject>();       //�������

    private List<GameObject> bossChangCardList = new List<GameObject>();         //boss����
    private List<GameObject> playerChangCardList = new List<GameObject>();       //��ҳ���

    private CardPlayManager cardPlayManager = new CardPlayManager();            //�ƾֹ���
    private GameObject attackObj = null;   //�����߳���
    private GameObject beAttackObj = null; //����������
    private GameObject heroObj = null;      //Ӣ��
    private GameObject bossObj = null;      //boss


    private GameObject content1;                //�������
    private GameObject content2;                //��ҳ���
    private GameObject content3;                //BOSS����
    private GameObject content4;                //BOSS����
    private GameObject gamePanel;               //��Ϸpanel�ڵ�
    public GameObject xuanpaiPanel;             //ѡ�ƽ���
    public GameObject beginPanel;               //��Ϸ��ʼ����
    public GameObject msgPanel;                 //��ʾ��Ϣ���
    private int gameState = 0;                  //��Ϸ�׶� 0ѡ�ƽ׶� 1��Ϸ�׶�
    public void InitGame()
    {
        BeginPanel(true);
        xuanpaiPanel.SetActive(false);
        musicManager = new MusicManager();
        musicManager.PlayBkMusic("123");
        content1 = GameObject.Find("Canvas/Panel/List1/Viewport/Content").gameObject;
        content2 = GameObject.Find("Canvas/Panel/List2/Viewport/Content").gameObject;
        content3 = GameObject.Find("Canvas/Panel/List3/Viewport/Content").gameObject;
        content4 = GameObject.Find("Canvas/Panel/List4/Viewport/Content").gameObject;
        gamePanel = GameObject.Find("Canvas/Panel").gameObject;

        heroObj = GameObject.Find("Canvas/Panel/ZhuJiao").gameObject;
        bossObj = GameObject.Find("Canvas/Panel/Boss").gameObject;

        msgPanel.SetActive(true);
    }
    //������Ϸ
    public void EndGame(int _winType)
    {
        if (_winType == 1)
        {
            playerData.playerLevel++;
        }
        bossCardList.Clear();
        playerCardList.Clear();
        foreach (var item in bossShouCardList)
        {
            Destroy(item);
        }
        foreach (var item in playerShouCardList)
        {
            Destroy(item);
        }
        bossShouCardList.Clear();
        playerShouCardList.Clear();

        foreach (var item in bossChangCardList)
        {
            Destroy(item);
        }
        foreach (var item in playerChangCardList)
        {
            Destroy(item);
        }
        bossChangCardList.Clear();
        playerChangCardList.Clear();

        BeginPanel(true);
    }
    public void XuanPaiBegin(bool isBool = true)
    {
        if (isBool)
        {
            xuanpaiPanel.SetActive(true);
            BossXuanPai();
            // ʵ����������
            for (int i = 0; i < paiChiPlayer.Count; i++)
            {
                CardInfoCfg cfg = configMag.GetCardInfoCfgByKey(paiChiPlayer[i]);
                CardInfo cardInfo = new CardInfo();
                cardInfo.addId = i;
                cardInfo.id = cfg.ID;
                cardInfo.xjNumber = cfg.xjNumber;
                cardInfo.hpNumber = cfg.hpNumber;
                cardInfo.gjNumber = cfg.gjNumber;
                cardInfo.hpNumberNow = cfg.hpNumber;
                cardInfo.gjNumberNow = cfg.gjNumber;
                cardInfo.name = cfg.name;
                cardInfo.type = cfg.type;
                cardInfo.imageId = cfg.imageId;
                cardInfo.state = 0;
                var obj = AddPrefab("ShouCard", xuanpaiPanel.transform.Find("List2/Viewport/Content"));
                obj.GetComponent<ShouCard>().InitCardInfo(cardInfo);
                paichiPlayerObj.Add(obj);
            }
            //�����Ч
            List<GameObject> random60 = GetRandomElements(60);
            for (int i = 0; i < random60.Count; i++)
            {
                if (i < 15)
                {
                    random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 1;
                    Debug.Log(random60[i].GetComponent<ShouCard>().GetCardInfo().addId);
                }
                else if (i < 30)
                {
                    random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 2;
                }
                else if (i < 45)
                {
                    random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 3;
                }
                else if (i < 60)
                {
                    random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 4;
                }
                random60[i].GetComponent<ShouCard>().UpateTxImage();
            }
        }

       
        // ���ȡ��3��Ԫ�ز���ԭ�б�ɾ��
        randomElements = RandCardObj(paichiPlayerObj, 3);
        //�ı丸����
        for (int i = 0; i < randomElements.Count; i++)
        {
            randomElements[i].transform.SetParent(xuanpaiPanel.transform.Find("List1/Viewport/Content"));
        }

    }
    public List<GameObject> RandCardObj(List<GameObject> _list, int _number)
    {
        List<GameObject> result = new List<GameObject>();
        if (_list.Count <= 0)
        {
            return result;
        }

        for (int i = 0; i < _number; i++)
        {
            int randIndex = Util.randomInt(0, _list.Count - 1);
            result.Add(_list[randIndex]);
            _list.RemoveAt(randIndex);
            if (_list.Count <= 0)
            {
                return result;
            }
        }

        return result;
    }
    public void XuanPaiCard(GameObject _obj)
    {
        //�ƿ����
        CardTxAndId cardTxAndId = new CardTxAndId();
        cardTxAndId.id = _obj.GetComponent<ShouCard>().GetCardInfo().id;
        cardTxAndId.txState = _obj.GetComponent<ShouCard>().GetCardInfo().txState;
        paikuPlayer.Add(cardTxAndId);
        foreach (var item in randomElements)
        {
            Destroy(item);
        }
        if (paichiPlayerObj.Count > 0)
        {
            XuanPaiBegin(false);
        }
        else
        {
            Debug.Log("��ʼ��Ϸ");
            BeginGame();
        }
    }
    //�����Ч
    // ��ȡ������ظ�Ԫ�صķ���
    public List<GameObject> GetRandomElements(int count)
    {
        // ���Ϸ���
        if (paichiPlayerObj == null || paichiPlayerObj.Count < count)
        {
            Debug.LogError("�б�Ϊ�ջ�Ԫ���������㣡");
            return new List<GameObject>();
        }

        // ������ʱ�б������޸�ԭ�б�
        List<GameObject> tempList = new List<GameObject>(paichiPlayerObj);
        List<GameObject> result = new List<GameObject>();

        // Fisher-Yates ϴ���㷨������ϴ�ƣ�ֻϴ����Ҫ��������
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, tempList.Count);

            // ����Ԫ��λ��
            GameObject temp = tempList[i];
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;

            // ��ѡ�е�Ԫ�ؼ�����
            result.Add(tempList[i]);
        }

        return result;
    }
    public List<GameObject> GetRandomElementsBoss(int count)
    {
        // ���Ϸ���
        if (paichiBossObj == null || paichiBossObj.Count < count)
        {
            Debug.LogError("�б�Ϊ�ջ�Ԫ���������㣡");
            return new List<GameObject>();
        }

        // ������ʱ�б������޸�ԭ�б�
        List<GameObject> tempList = new List<GameObject>(paichiBossObj);
        List<GameObject> result = new List<GameObject>();

        // Fisher-Yates ϴ���㷨������ϴ�ƣ�ֻϴ����Ҫ��������
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, tempList.Count);

            // ����Ԫ��λ��
            GameObject temp = tempList[i];
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;

            // ��ѡ�е�Ԫ�ؼ�����
            result.Add(tempList[i]);
        }

        return result;
    }
    //BOSSѡ��
    public void BossXuanPai()
    {
        // ʵ����������
        for (int i = 0; i < paikuChiBoss.Count; i++)
        {
            CardInfoCfg cfg = configMag.GetCardInfoCfgByKey(paikuChiBoss[i]);
            CardInfo cardInfo = new CardInfo();
            cardInfo.addId = i;
            cardInfo.id = cfg.ID;
            cardInfo.xjNumber = cfg.xjNumber;
            cardInfo.hpNumber = cfg.hpNumber;
            cardInfo.gjNumber = cfg.gjNumber;
            cardInfo.hpNumberNow = cfg.hpNumber;
            cardInfo.gjNumberNow = cfg.gjNumber;
            cardInfo.name = cfg.name;
            cardInfo.type = cfg.type;
            cardInfo.imageId = cfg.imageId;
            cardInfo.state = 0;
            var obj = AddPrefab("ShouCard", xuanpaiPanel.transform.Find("List3/Viewport/Content"));
            obj.GetComponent<ShouCard>().InitCardInfo(cardInfo);
            paichiBossObj.Add(obj);
        }
        //�����Ч
        List<GameObject> random60 = GetRandomElementsBoss(60);
        for (int i = 0; i < random60.Count; i++)
        {
            if (i < 15)
            {
                random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 1;
                Debug.Log(random60[i].GetComponent<ShouCard>().GetCardInfo().addId);
            }
            else if (i < 30)
            {
                random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 2;
            }
            else if (i < 45)
            {
                random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 3;
            }
            else if (i < 60)
            {
                random60[i].GetComponent<ShouCard>().GetCardInfo().txState = 4;
            }
        }
        for (int i = 0; i < paichiBossObj.Count; i++)
        {
            paichiBossObj[i].GetComponent<ShouCard>().UpateTxImage();
        }
        List<GameObject> random30 = GetRandomElementsBoss(30);
        foreach (var item in random30)
        {
            CardTxAndId cardTxAndId = new CardTxAndId();
            cardTxAndId.id = item.GetComponent<ShouCard>().GetCardInfo().id;
            cardTxAndId.txState = item.GetComponent<ShouCard>().GetCardInfo().txState;
            paikuBoss.Add(cardTxAndId);
        }
      
    }
    //��ʼ��Ϸ
    public void BeginGame()
    {
        gameState = 1;
        xuanpaiPanel.SetActive(false);
        BeginPanel(false);
        //��ʼ������
        InitCard();
        //��ʼ���ƾֹ���
        cardPlayManager.InitData();
        //��ʼ������
        UpdateXyShow();
        //ˢ�½����غϰ�ť
        UpdateEndHuiHeBUtton();
    }
    //��ʼ������
    public void InitCard()
    {
        InitHero();
        //��ȡ��������
        for (int i = 0; i < paikuPlayer.Count; i++)
        {
            CardInfoCfg cfg = configMag.GetCardInfoCfgByKey(paikuPlayer[i].id);
            CardInfo cardInfo2 = new CardInfo();
            cardInfo2.addId = 100 + i;
            cardInfo2.id = cfg.ID;
            cardInfo2.xjNumber = cfg.xjNumber;
            cardInfo2.hpNumber = cfg.hpNumber;
            cardInfo2.gjNumber = cfg.gjNumber;
            cardInfo2.hpNumberNow = cfg.hpNumber;
            cardInfo2.gjNumberNow = cfg.gjNumber;
            cardInfo2.name = cfg.name;
            cardInfo2.type = cfg.type;
            cardInfo2.imageId = cfg.imageId;
            cardInfo2.state = 1;
            cardInfo2.txState = paikuPlayer[i].txState;
            playerCardList.Add(cardInfo2);
        }
        for (int i = 0; i < paikuBoss.Count; i++)
        {
            CardInfoCfg cfg = configMag.GetCardInfoCfgByKey(paikuBoss[i].id);
            CardInfo cardInfo = new CardInfo();
            cardInfo.addId = i;
            cardInfo.id = cfg.ID;
            cardInfo.xjNumber = cfg.xjNumber;
            cardInfo.hpNumber = cfg.hpNumber;
            cardInfo.gjNumber = cfg.gjNumber;
            cardInfo.hpNumberNow = cfg.hpNumber;
            cardInfo.gjNumberNow = cfg.gjNumber;
            cardInfo.name = cfg.name;
            cardInfo.type = cfg.type;
            cardInfo.imageId = cfg.imageId;
            cardInfo.state = 2;
            cardInfo.txState = paikuBoss[i].txState;
            bossCardList.Add(cardInfo);
        }
        //��������
        Util.shuffle<CardInfo>(bossCardList);
        Util.shuffle<CardInfo>(playerCardList);

        //���������ľ�����
        RandXjType();
    }
    //��ʼ��Ӣ������
    public void InitHero()
    {
        CardInfo info = new CardInfo();
        info.hpNumberNow = 30;
        info.state = 7;
        info.imageId = 1;
        heroObj.GetComponent<ShouCard>().InitCardInfo(info);

        CardInfo info2 = new CardInfo();
        info2.hpNumberNow = 30;
        info2.state = 8;
        info2.imageId = 2;
        bossObj.GetComponent<ShouCard>().InitCardInfo(info2);
    }
    //��������ľ�����
    public void RandXjType()
    {
        //ȷ�������ľ�����
        List<int> bossRand = new List<int>();
        List<int> playerRand = new List<int>();
        if (playerData.playerLevel == 1)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(1);
                playerRand.Add(1);
            }
        }
        else if (playerData.playerLevel == 2)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(2);
            }
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(2);
        }
        else if (playerData.playerLevel == 3)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(3);
            }
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(3);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(3);
            playerRand.Add(3);
        }
        else if (playerData.playerLevel == 4)
        {
            for (int i = 0; i < 8; i++)
            {
                bossRand.Add(4);
            }
            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(3);
            playerRand.Add(3);
            playerRand.Add(4);
            playerRand.Add(4);
        }
        else if (playerData.playerLevel == 5)
        {
            bossRand.Add(1);
            bossRand.Add(1);
            bossRand.Add(2);
            bossRand.Add(2);
            bossRand.Add(3);
            bossRand.Add(3);
            bossRand.Add(4);
            bossRand.Add(4);

            playerRand.Add(1);
            playerRand.Add(1);
            playerRand.Add(2);
            playerRand.Add(2);
            playerRand.Add(3);
            playerRand.Add(3);
            playerRand.Add(4);
            playerRand.Add(4);
        }

        //�����ֵ������ľ�����
        for (int i = 0; i < 8; i++)
        {
            int randNumber = Util.randomInt(1, 29);
            bossCardList[randNumber].xjType = bossRand[i];
        }
        for (int i = 0; i < 8; i++)
        {
            int randNumber = Util.randomInt(1, 29);
            playerCardList[randNumber].xjType = playerRand[i];
        }
    }
    //����������
    public void AddPlayerShouCard(int _number)
    {
        FaCard(playerCardList, playerShouCardList, _number, content1.transform, 3);
    }
    //���Boss����
    public void AddBossShouCard(int _number)
    {
        FaCard(bossCardList, bossShouCardList, _number, content3.transform, 4);
    }
    //����
    public void FaCard(List<CardInfo> _list1, List<GameObject> _list2, int _number, Transform _fatherTransform, int _cardState)
    {
        for (int i = 0; i < _number; i++)
        {
            if (_list1.Count <= 0)
            {
                //���ƿ�Ѫ�߼�
                if (_cardState == 3)
                {
                    //�������
                    int number = cardPlayManager.GetWuPaiKouXueNumber(1);
                    heroObj.GetComponent<ShouCard>().addHpNumber(-number);
                    MsgShow("ƣ�Ϳ۳�Ѫ��"+ number);
                }
                else
                {
                    //boss����
                    int number = cardPlayManager.GetWuPaiKouXueNumber(2);
                    bossObj.GetComponent<ShouCard>().addHpNumber(-number);
                    MsgShow("ƣ�Ϳ۳�Ѫ��" + number);
                }
                continue;
            }
            CardInfo info = _list1.First();
            info.state = _cardState;
            _list1.RemoveAt(0);
            var obj = AddPrefab("ShouCard", _fatherTransform);
            obj.GetComponent<ShouCard>().InitCardInfo(info);
            _list2.Add(obj);

            if (_cardState == 3 && playerShouCardList.Count >= maxShouCardNumber)
            {
                _list2.Remove(obj);
                obj.transform.SetParent(gamePanel.transform.Find("DesTag"));
                StartCoroutine(DesDuoPai(obj));
                Debug.Log("�߶��������߼�");
                return;
            }
            if (_cardState == 4 && bossShouCardList.Count >= maxShouCardNumber)
            {
                _list2.Remove(obj);
                obj.transform.SetParent(gamePanel.transform.Find("DesTag"));
                StartCoroutine(DesDuoPai(obj));
                Debug.Log("�߶��������߼�");
                return;
            }
        }
        UpdateCardNumberShow();
    }
    public IEnumerator DesDuoPai(GameObject _obj)
    {
        gamePanel.transform.Find("DesTag").GetComponent<RectTransform>().SetAsLastSibling();
        RectTransform uiElement = _obj.GetComponent<RectTransform>();
        Vector2 vec2 = new Vector2(900, 0);
        _obj.transform.localPosition = vec2;
        Vector2 vec2New = new Vector2(0, 0);
        uiElement.DOAnchorPos(vec2New, 1f)
                .SetEase(Ease.OutQuad); // ���û���Ч��
        yield return new WaitForSeconds(1f);
        AddPrefab("desCard", _obj.transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(_obj);
    }
    //����
    public bool ChuCard(GameObject _obj, CardInfo _cardInfo)
    {
        if (cardPlayManager.GetXyNumber() < _cardInfo.xjNumber)
        {
            return false;
        }
        //�жϳ�������
        if (playerChangCardList.Count >= maxChangCardNumber)
        {
            GameManager.instance.MsgShow("��������");
            return false;
        }
        //�۳��ľ�ֵ
        cardPlayManager.AddXjNumber(-_cardInfo.xjNumber);
        //ʿ��
        if (_cardInfo.type == 0)
        {
            //�ȰѸ��ڵ�������
            _obj.transform.SetParent(content2.transform);
            //��ȡ��������
            _cardInfo.state = 5;
            //ɾ����������
            playerShouCardList.Remove(_obj);
            //���볡������
            playerChangCardList.Add(_obj);
            if (_cardInfo.gjNumberNow >= 7)
            {
                GameManager.instance.GjTx(_obj.transform);
            }
        }
        //�鿨
        if (_cardInfo.type == 1)
        {
            AddPlayerShouCard(3);
            //ɾ����������
            playerShouCardList.Remove(_obj);
            //�h����
            Destroy(_obj);
        }

        ChuPaiTeXiao(_cardInfo.txState, 1, _obj);
        return true;
    }
    //Boss����
    public bool ChuPaiBoss(GameObject _obj, CardInfo _cardInfo)
    {
        if (cardPlayManager.GetXyNumber() < _cardInfo.xjNumber)
        {
            return false;
        }
        //�жϳ�������
        if (bossChangCardList.Count >= maxChangCardNumber)
        {
            return false;
        }
        _obj.GetComponent<ShouCard>().UpateTxImage();
        //�۳��ľ�ֵ
        cardPlayManager.AddXjNumber(-_cardInfo.xjNumber);
        if (_cardInfo.type == 0)
        {
            //�ȰѸ��ڵ�������
            _obj.transform.SetParent(content4.transform);
            //��ȡ��������
            _cardInfo.state = 6;
            //ɾ����������
            bossShouCardList.Remove(_obj);
            //���볡������
            bossChangCardList.Add(_obj);
            if (_cardInfo.gjNumberNow >= 7)
            {
                GameManager.instance.GjTx(_obj.transform);
            }
        }
        //�鿨
        if (_cardInfo.type == 1)
        {
            AddBossShouCard(3);
            //ɾ����������
            bossShouCardList.Remove(_obj);
            //�h����
            Destroy(_obj);
        }
        //ˢ���Ʊ�
        _obj.GetComponent<ShouCard>().UpdateCardBack();

        ChuPaiTeXiao(_cardInfo.txState, 2, _obj);
        return true;
    }
    //��Ч��Ч��
    public void ChuPaiTeXiao(int _xjType, int _operand, GameObject _obj)
    {
        List<GameObject> list;
        if (_operand == 1)
        {
            list = playerChangCardList;
        }
        else
        {
            list = bossChangCardList;
        }
        if (_xjType == 1)
        {
         
        }
        else if (_xjType == 2)
        {
            //�����
           _obj.GetComponent<ShouCard>().AiType();
        }
        else if (_xjType == 3)
        {
         
        }
        else if (_xjType == 4)
        {
           _obj.GetComponent<ShouCard>().SetSdType();
        }
    }
    //��ק�����ø��ڵ�
    public void DrageCardSetFatherOut(GameObject _obj)
    {
        _obj.transform.SetParent(GameObject.Find("Canvas/Panel").transform);
    }
    //��ק�����ø��ڵ�
    public void DrageCardSetFatherIn(GameObject _obj)
    {
        _obj.transform.SetParent(GameObject.Find("Canvas/Panel/List1/Viewport/Content").transform);
    }
    //ˢ����Ըֵ
    public void UpdateXyShow()
    {
        GameObject trs;
        if (cardPlayManager.GetOperand() == 1)
        {
            trs = GameObject.Find("Canvas/Panel/ShuiJingPlayer");
        }
        else
        {
            trs = GameObject.Find("Canvas/Panel/ShuiJingBoss");
        }

        int xyNumber = cardPlayManager.GetXyNumber();
        for (int i = 1; i < 11; i++)
        {
            var obj = trs.transform.Find("Image" + i).gameObject;
            if (i <= xyNumber)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }
    //��ҽ����غ�
    public void PlayerEndHuiHe()
    {
        cardPlayManager.EndHuiHe();
        UpdateEndHuiHeBUtton();
    }
    //ˢ�»غϰ�ť
    public void UpdateEndHuiHeBUtton()
    {
        if (cardPlayManager.GetOperand() == 1)
        {
            gamePanel.transform.Find("Button_end").gameObject.SetActive(true);
        }
        else
        {
            gamePanel.transform.Find("Button_end").gameObject.SetActive(false);
        }
    }
    //��ȡ�غ��Ƿ�����һغ�
    public bool GetPlayerOperand()
    {
        return cardPlayManager.GetOperand() == 1;
    }
    //AI����
    public IEnumerator BossPlayerCard()
    {

        for (int i = 0; i < 10; i++)
        {
            var obj = GetMaxXjCard();
            if (obj == null)
            {
                yield return new WaitForSeconds(1f);

                break;
            }
            //����
            ChuPaiBoss(obj, obj.GetComponent<ShouCard>().GetCardInfo());
            yield return new WaitForSeconds(1f);
        }
        //AI����
        foreach (var item in bossChangCardList)
        {
            if (item.GetComponent<ShouCard>().GetCardInfo().attackNumber > 0)
            {
                if (playerChangCardList.Count > 0)
                {
                    //��ȡδʧ�����
                    List<GameObject> list = new List<GameObject>();
                    foreach (var jtem in playerChangCardList)
                    {
                        if (jtem.activeInHierarchy)
                        {
                            list.Add(jtem);
                        }
                    }
                    //�������
                    if (list.Count > 0)
                    {
                        int randNumber = Util.randomInt(0, list.Count - 1);
                        StartCoroutine(AttackIEnumerator(item, list[randNumber]));
                        yield return new WaitForSeconds(2f);
                    }
                    else
                    {
                        //����Ӣ��
                        StartCoroutine(AttackIEnumerator(item, heroObj));
                        yield return new WaitForSeconds(2f);
                    }
                }
                else
                {
                    //����Ӣ��
                    StartCoroutine(AttackIEnumerator(item, heroObj));
                    yield return new WaitForSeconds(2f);
                }
            }
        }
        //�����غ�
        PlayerEndHuiHe();

        yield return new WaitForSeconds(1f);

    }
    //��ȡ��ǰ�������ľ���ߵ���
    public GameObject GetMaxXjCard()
    {
        int haveXj = cardPlayManager.GetXyNumber();
        int maxXj = -1;
        int maxIndex = -1;
        for (int i = 0; i < bossShouCardList.Count; i++)
        {
            var info = bossShouCardList[i].GetComponent<ShouCard>().GetCardInfo();
            if (haveXj >= info.xjNumber && info.xjNumber > maxXj)
            {
                maxXj = info.xjNumber;
                maxIndex = i;
            }
        }
        if (maxIndex < 0)
        {
            return null;
        }
        else
        {
            return bossShouCardList[maxIndex];
        }
    }
    //���ڻغ�������
    public void AddLiveHh(int _type)
    {
        List<GameObject> list;
        if (_type == 1)
        {
            list = playerChangCardList;
        }
        else
        {
            list = bossChangCardList;
        }
        foreach (var item in list)
        {
            var info = item.GetComponent<ShouCard>().GetCardInfo();
            info.liveHh++;
        }
    }
    //���ƹ�����������
    public void AddAttackNumnber(int _type)
    {
        List<GameObject> list;
        if (_type == 1)
        {
            list = playerChangCardList;
        }
        else
        {
            list = bossChangCardList;
        }
        foreach (var item in list)
        {
            var info = item.GetComponent<ShouCard>().GetCardInfo();
            if (info.liveHh > 0)
            {
                info.attackNumber = 1;
                item.GetComponent<ShouCard>().ShowAttack();
            }
        }
    }
    //���չ�������
    public void RecycleAttackNumber(int _type)
    {
        List<GameObject> list;
        if (_type == 1)
        {
            list = playerChangCardList;
        }
        else
        {
            list = bossChangCardList;
        }
        foreach (var item in list)
        {
            var info = item.GetComponent<ShouCard>().GetCardInfo();
            if (info.liveHh > 0)
            {
                info.attackNumber = 0;
                item.GetComponent<ShouCard>().ShowAttack();
            }
        }
    }
    //����
    public void Attack(GameObject _obj)
    {
        if (gameState == 0)
        {
            XuanPaiCard(_obj);
            return;
        }
        var info = _obj.GetComponent<ShouCard>().GetCardInfo();
        if (info.state == 5)
        {
            if (info.attackNumber > 0)
            {
                attackObj = _obj;
            }
        }
        if (info.state == 6 || info.state == 8)
        {
            beAttackObj = _obj;
            if (attackObj != null)
            {
                //��鳰��
                if (CheckChaoFeng(bossChangCardList))
                {
                    //����г���Ҫ���ж��Ƿ񱻹���obj �ǲ��ǳ���
                    if (beAttackObj.GetComponent<ShouCard>().CheckCf())
                    {
                        StartCoroutine(AttackIEnumerator(attackObj, beAttackObj));
                    }
                }
                else
                {
                    StartCoroutine(AttackIEnumerator(attackObj, beAttackObj));
                }
            }
        }
    }
    public IEnumerator AttackIEnumerator(GameObject _attack0bj, GameObject _beAttack0bj)
    {
        Vector3 oldVec = _attack0bj.transform.position;
        Vector3 mubiaoVec = _beAttack0bj.transform.Find("BiaoJi").transform.position;
        _attack0bj.transform.DOMove(mubiaoVec, 0.3f)
          .SetEase(Ease.InOutQuad);// ���û�������
        yield return new WaitForSeconds(0.4f);
        AttackTx(mubiaoVec);
        //�۳�Ѫ��
        _attack0bj.GetComponent<ShouCard>().addHpNumber(-_beAttack0bj.GetComponent<ShouCard>().GetCardInfo().gjNumberNow);
        _beAttack0bj.GetComponent<ShouCard>().addHpNumber(-_attack0bj.GetComponent<ShouCard>().GetCardInfo().gjNumberNow);
        //�۳��ж�����
        _attack0bj.GetComponent<ShouCard>().addAttackNumber(-1);
       //�ص�ԭλ
       _attack0bj.transform.DOMove(oldVec, 0.3f)
        .SetEase(Ease.InOutQuad);// ���û�������
        yield return new WaitForSeconds(1f);
        //���Ѫ��
        _attack0bj.GetComponent<ShouCard>().CheckHp();
        _beAttack0bj.GetComponent<ShouCard>().CheckHp();
        yield return new WaitForSeconds(1f);
    }
    //���ò㼶
    public void SetCj(int _type)
    {
        if (_type == 1)
        {
            gamePanel.transform.Find("List2").GetComponent<RectTransform>().SetAsLastSibling();
        }
        else
        {
            gamePanel.transform.Find("List4").GetComponent<RectTransform>().SetAsLastSibling();
        }
       
    }
    //���ٳ���
    public void DesChangCard()
    {
        for (int i = playerChangCardList.Count - 1; i >= 0; i--)
        {
            if (playerChangCardList[i].activeSelf == false) 
            {
                Destroy(playerChangCardList[i]);
                playerChangCardList.RemoveAt(i);
            }
        }

        for (int i = bossChangCardList.Count - 1; i >= 0; i--)
        {
            if (bossChangCardList[i].activeSelf == false)
            {
                Destroy(bossChangCardList[i]);
                bossChangCardList.RemoveAt(i);
            }
        }
    }
    //�_���_ʼ����
    public void BeginPanel(bool isBool)
    {
        beginPanel.SetActive(isBool);
        beginPanel.transform.Find("Text (Legacy)").GetComponent<Text>().text = "��ǰ�ؿ���"+playerData.playerLevel.ToString();
    }
    //������Ч
    public void AttackTx(Vector3 _vec3)
    {
        var obj = AddPrefab("attack", GameObject.Find("Canvas").transform);
        obj.transform.position = _vec3;
    }
    //HP��Ч
    public void HpTx(Transform _trs)
    {
        var obj = AddPrefab("hp", _trs);
        
    }
    //������Ч
    public void GjTx(Transform _trs)
    {
        //var obj = AddPrefab("gj", _trs);
    }
    //ʥ����Ч
    public GameObject SDTx(Transform _trs)
    {
        var obj = AddPrefab("ShengDunTx", _trs);
        return obj;
    }
    //������Ч
    public GameObject FHTx(Transform _trs)
    {
        var obj = AddPrefab("fuhuo", _trs);
        return obj;
    }
    // �˳���Ϸ�ķ���
    public void Quit()
    {
#if UNITY_EDITOR
        // �� Unity �༭���У�ʹ�� UnityEditor.EditorApplication.isPlaying ������ֹͣ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �ڷ�����Ӧ�ó����У�ʹ�� Application.Quit() �������˳���Ϸ
        Application.Quit();
#endif
    }
    //��鳡���Ƿ��г�����
    public bool CheckChaoFeng(List<GameObject> _list)
    {
        bool isBool = false;
        if (_list.Count <= 0)
        {
            return isBool;
        }
        foreach (var item in _list)
        {
            if (item.GetComponent<ShouCard>().CheckCf() && item.GetComponent<ShouCard>().GetCardInfo().hpNumberNow > 0)
            {
                return true;
            }
        }
        return false;
    }
    //ͨ����ʾ
    public void MsgShow(string _msg)
    {
        msgPanel.transform.Find("Text (Legacy)").GetComponent<Text>().text = _msg;
        DOVirtual.DelayedCall(3f,MyFunction);
    }
    void MyFunction()
    {
        msgPanel.transform.Find("Text (Legacy)").GetComponent<Text>().text = "";
    }
    //ˢ��ʣ�࿨����
    public void UpdateCardNumberShow()
    {
        gamePanel.transform.Find("Text_player_car_number").GetComponent<Text>().text = playerCardList.Count.ToString();
        gamePanel.transform.Find("Text_boss_car_number").GetComponent<Text>().text = bossCardList.Count.ToString();
    }
}

