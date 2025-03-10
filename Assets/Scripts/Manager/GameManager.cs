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
    #region 构造函数及其变量
    public GameManager()
    {
        configMag = new ConfigManager();
    }
    public static bool isDbugLog = true;
    public PlayerData playerData = null;                            //玩家数据（本地持久化）
    public ConfigManager configMag;
    private System.Random Random1;                                   //随机种子
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
        Application.targetFrameRate = 60;//设置帧率为60帧
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


    #region OnApplicationPause(bool pause)切屏感知
    public void OnApplicationPause(bool pause)
    {
        if (pause == true)
        {
            if (isDbugLog)
                Debug.Log("切屏感知");
            SaveGame();
        }
    }
    #endregion

    #region OnApplicationQuit() 退出游戏感知
    public void OnApplicationQuit()
    {
        if (isDbugLog)
            Debug.Log("退出感知");
        SaveGame();

    }
    #endregion

    #region 获取本地数据
    public void GetLocalPlayerData()
    {
        playerData = PlayerData.GetLocalData();//读取本地持久化玩家数据(包括本土化设置)
        configMag.InitGameCfg();//读取配置表
        playerData.InitData();//根据配置表和本地数据初始化游戏数据
    }
    #endregion

    #region SaveGame() 保存玩家数据
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
    /// 注册一个update在这里跑
    /// </summary>
    /// <param name="_action"></param>
    public void AddUpdateListener(UnityAction _action)
    {
        unityActionList.Add(_action);
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    public void SpritPropImage(string id, Image image)
    {
        string path = "Icon/" + id;
        Sprite Tab3Img = ResourcesLoad.instance.Load<Sprite>(path);
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// 加载图片--装备图标
    /// </summary>
    public void SpritPropEquipIcon(string id, Image image)
    {
        string path = "EquipIcon/" + id;
        Sprite Tab3Img = ResourcesLoad.instance.Load<Sprite>(path);
        image.sprite = Tab3Img;
    }


    /// <summary>
    /// 加载图片
    /// </summary>
    public void SpritPropImageByPath(string path, Image image)
    {
        Sprite Tab3Img = Resources.Load(path, typeof(Sprite)) as Sprite;
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    public void SpritPropImageByPath(string path, SpriteRenderer image)
    {
        Sprite Tab3Img = Resources.Load(path, typeof(Sprite)) as Sprite;
        image.sprite = Tab3Img;
    }

    /// <summary>
    /// 添加预制体
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
    /// 销毁预制体
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
    /// 销毁预制体
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(GameObject prefabObj, GameObject gameObject, string _path = null)
    {
        ObjPool.instance.Recycle(prefabObj, gameObject, "Prefab/" + _path);
        return;
    }
    /// <summary>
    /// 销毁预制体
    /// </summary>
    /// <returns></returns>
    public void DestroyPrefab(GameObject gameObject)
    {
        string name = gameObject.GetComponent<DesObj>().name;
        ObjPool.instance.Recycle(name, gameObject);
        return;
    }
    /// <summary>
    /// 播放动画并重置动画到第0帧
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
    /// 播放动画并重置动画到第0帧
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
    /// 获取对象池内对象数据
    /// </summary>
    /// <returns></returns>
    public ObjPool.PoolItem GetPoolItem(string name)
    {
        string newpath = "Prefab/" + name;
        return ObjPool.instance.GetPoolItem(newpath); ;
    }
    /// <summary>
    /// 网络拉取图片
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
    /// 清空数据
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
        //移除所有场景包含的对象
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            var objs = scene.GetRootGameObjects();
            for (var j = 0; j < objs.Length; j++)
            {
                allGameObjects.Remove(objs[j]);
            }
        }
        //移除父级不为null的对象
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

    private List<CardInfo> bossCardList = new List<CardInfo>();             //boss牌组
    private List<CardInfo> playerCardList = new List<CardInfo>();           //玩家牌组

    private List<GameObject> bossShouCardList = new List<GameObject>();         //boss手牌
    private List<GameObject> playerShouCardList = new List<GameObject>();       //玩家手牌

    private List<GameObject> bossChangCardList = new List<GameObject>();         //boss场牌
    private List<GameObject> playerChangCardList = new List<GameObject>();       //玩家场牌

    private CardPlayManager cardPlayManager = new CardPlayManager();            //牌局管理
    private GameObject attackObj = null;   //攻击者场牌
    private GameObject beAttackObj = null; //被攻击场牌
    private GameObject heroObj = null;      //英雄
    private GameObject bossObj = null;      //boss


    private GameObject content1;                //玩家手牌
    private GameObject content2;                //玩家场牌
    private GameObject content3;                //BOSS手牌
    private GameObject content4;                //BOSS场牌
    private GameObject gamePanel;               //游戏panel节点
    public GameObject xuanpaiPanel;             //选牌界面
    public GameObject beginPanel;               //游戏开始界面
    public GameObject msgPanel;                 //提示消息面板
    public GameObject jieshaoPanel;             //游戏介绍面板
    public GameObject endPanel;                 //游戏结束面板
    private int gameState = 0;                  //游戏阶段 0选牌阶段 1游戏阶段
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
        endPanel.SetActive(false);
        ChangYL();
    }
    //结束游戏
    public void EndGame(int _winType)
    {
        if (_winType == 1)
        {
            //playerData.playerLevel++;
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

        paikuPlayer.Clear();
        paikuBoss.Clear();

        gameState = 0;
        endPanel.SetActive(true);
        BeginPanel(true);
    }
    public void XuanPaiBegin(bool isBool = true)
    {
        if (isBool)
        {
            xuanpaiPanel.SetActive(true);
            BossXuanPai();
            // 实例化所有牌
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
            //随机特效
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


        // 随机取出3个元素并从原列表删除
        randomElements = RandCardObj(paichiPlayerObj, 3);
        //改变父对象
        for (int i = 0; i < randomElements.Count; i++)
        {
            randomElements[i].transform.SetParent(xuanpaiPanel.transform.Find("List1/Viewport/Content"));
            randomElements[i].transform.localScale = Vector3.one;
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
        //牌库加牌
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
            Debug.Log("开始游戏");
            BeginGame();
        }
    }
    //随机特效
    // 获取随机不重复元素的方法
    public List<GameObject> GetRandomElements(int count)
    {
        // 检查合法性
        if (paichiPlayerObj == null || paichiPlayerObj.Count < count)
        {
            Debug.LogError("列表为空或元素数量不足！");
            return new List<GameObject>();
        }

        // 创建临时列表（避免修改原列表）
        List<GameObject> tempList = new List<GameObject>(paichiPlayerObj);
        List<GameObject> result = new List<GameObject>();

        // Fisher-Yates 洗牌算法（部分洗牌，只洗到需要的数量）
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, tempList.Count);

            // 交换元素位置
            GameObject temp = tempList[i];
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;

            // 将选中的元素加入结果
            result.Add(tempList[i]);
        }

        return result;
    }
    public List<GameObject> GetRandomElementsBoss(int count)
    {
        // 检查合法性
        if (paichiBossObj == null || paichiBossObj.Count < count)
        {
            Debug.LogError("列表为空或元素数量不足！");
            return new List<GameObject>();
        }

        // 创建临时列表（避免修改原列表）
        List<GameObject> tempList = new List<GameObject>(paichiBossObj);
        List<GameObject> result = new List<GameObject>();

        // Fisher-Yates 洗牌算法（部分洗牌，只洗到需要的数量）
        for (int i = 0; i < count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, tempList.Count);

            // 交换元素位置
            GameObject temp = tempList[i];
            tempList[i] = tempList[randomIndex];
            tempList[randomIndex] = temp;

            // 将选中的元素加入结果
            result.Add(tempList[i]);
        }

        return result;
    }
    //BOSS选牌
    public void BossXuanPai()
    {
        // 实例化所有牌
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
        //随机特效
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
        //List<GameObject> random30 = GetRandomElementsBoss(30);
        //foreach (var item in random30)
        //{
        //    CardTxAndId cardTxAndId = new CardTxAndId();
        //    cardTxAndId.id = item.GetComponent<ShouCard>().GetCardInfo().id;
        //    cardTxAndId.txState = item.GetComponent<ShouCard>().GetCardInfo().txState;
        //    paikuBoss.Add(cardTxAndId);
        //}
        for (int i = 0; i < 30; i++)
        {
            randomElements = RandCardObj(paichiBossObj, 3);
            var obj = BossAiXuanPai(randomElements);

            CardTxAndId cardTxAndId = new CardTxAndId();
            cardTxAndId.id = obj.GetComponent<ShouCard>().GetCardInfo().id;
            cardTxAndId.txState = obj.GetComponent<ShouCard>().GetCardInfo().txState;
            paikuBoss.Add(cardTxAndId);
        }


    }
    //BOSS选牌AI
    public GameObject BossAiXuanPai(List<GameObject> _list)
    {
        List<CardQuanZhongInfo> qzList = new List<CardQuanZhongInfo>();
        for (int i = 1; i < 5; i++)
        {
            CardQuanZhongInfo cardQuan = new CardQuanZhongInfo();
            cardQuan.txState = i;
            if (i == 1)
            {
                cardQuan.qzNumber = 4;
            }
            else if (i == 2)
            {
                cardQuan.qzNumber = 1;
            }
            else if (i == 3)
            {
                cardQuan.qzNumber = 2;
            }
            else if (i == 4)
            {
                cardQuan.qzNumber = 3;
            }
            qzList.Add(cardQuan);
        }
        qzList[0].haveNumber = GetTxNumber(1);
        qzList[1].haveNumber = GetTxNumber(2);
        qzList[2].haveNumber = GetTxNumber(3);
        qzList[3].haveNumber = GetTxNumber(4);

        var sortedList = qzList.OrderBy(c => c.haveNumber)
                     .ThenByDescending(c => c.qzNumber)
                     .ToList();

        foreach (var item in sortedList)
        {
            foreach (var jtem in _list)
            {
                if (item.txState == jtem.GetComponent<ShouCard>().GetCardInfo().txState)
                {
                    return jtem;
                }
            }
        }
        return _list[0];
    }
    //获取选牌有多少张特效牌
    public int GetTxNumber(int _txState)
    {
        int iRet = 0;
        foreach (var item in paikuBoss)
        {
            if (item.txState == _txState)
            {
                iRet++;
            }
        }
        return iRet;
    }
    //开始游戏
    public void BeginGame()
    {
        gameState = 1;
        xuanpaiPanel.SetActive(false);
        BeginPanel(false);
        //初始化牌组
        InitCard();
        //初始化牌局管理
        cardPlayManager.InitData();
        //初始化界面
        UpdateXyShow();
        //刷新结束回合按钮
        UpdateEndHuiHeBUtton();
    }
    //初始化牌组
    public void InitCard()
    {
        InitHero();
        //读取牌组配置
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
        //打乱排序
        Util.shuffle<CardInfo>(bossCardList);
        Util.shuffle<CardInfo>(playerCardList);

        //随机牌组的心境类型
        //RandXjType();
    }
    //初始化英雄数据
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
    //随机牌组心境类型
    public void RandXjType()
    {
        //确定卡牌心境类型
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

        //随机赋值牌组的心境类型
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
    //添加玩家手牌
    public void AddPlayerShouCard(int _number)
    {
        FaCard(playerCardList, playerShouCardList, _number, content1.transform, 3);
    }
    //添加Boss手牌
    public void AddBossShouCard(int _number)
    {
        FaCard(bossCardList, bossShouCardList, _number, content3.transform, 4);
    }
    //发牌
    public void FaCard(List<CardInfo> _list1, List<GameObject> _list2, int _number, Transform _fatherTransform, int _cardState)
    {
        for (int i = 0; i < _number; i++)
        {
            if (_list1.Count <= 0)
            {
                //无牌扣血逻辑
                if (_cardState == 3)
                {
                    //玩家无牌
                    int number = cardPlayManager.GetWuPaiKouXueNumber(1);
                    heroObj.GetComponent<ShouCard>().addHpNumber(-number);
                    MsgShow("疲劳扣除血量" + number);
                }
                else
                {
                    //boss无牌
                    int number = cardPlayManager.GetWuPaiKouXueNumber(2);
                    bossObj.GetComponent<ShouCard>().addHpNumber(-number);
                    MsgShow("疲劳扣除血量" + number);
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
                Debug.Log("走多牌销毁逻辑");
                return;
            }
            if (_cardState == 4 && bossShouCardList.Count >= maxShouCardNumber)
            {
                _list2.Remove(obj);
                obj.transform.SetParent(gamePanel.transform.Find("DesTag"));
                StartCoroutine(DesDuoPai(obj));
                Debug.Log("走多牌销毁逻辑");
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
                .SetEase(Ease.OutQuad); // 设置缓动效果
        yield return new WaitForSeconds(1f);
        AddPrefab("desCard", _obj.transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(_obj);
    }
    //出牌
    public bool ChuCard(GameObject _obj, CardInfo _cardInfo)
    {
        if (cardPlayManager.GetXyNumber() < _cardInfo.xjNumber)
        {
            return false;
        }
        //判断场牌数量
        if (playerChangCardList.Count >= maxChangCardNumber)
        {
            GameManager.instance.MsgShow("场牌已满");
            return false;
        }
        //扣除心境值
        cardPlayManager.AddXjNumber(-_cardInfo.xjNumber);
        //士兵
        if (_cardInfo.type == 0)
        {
            //先把父节点设置了
            _obj.transform.SetParent(content2.transform);
            //获取卡牌数据
            _cardInfo.state = 5;
            //删除手牌数据
            playerShouCardList.Remove(_obj);
            //加入场牌数据
            playerChangCardList.Add(_obj);
            if (_cardInfo.gjNumberNow >= 7)
            {
                GameManager.instance.GjTx(_obj.transform);
            }
        }
        //抽卡
        if (_cardInfo.type == 1)
        {
            AddPlayerShouCard(3);
            //删除手牌数据
            playerShouCardList.Remove(_obj);
            //刪除牌
            Destroy(_obj);
        }

        ChuPaiTeXiao(_cardInfo.txState, 1, _obj);
        return true;
    }
    //Boss出牌
    public bool ChuPaiBoss(GameObject _obj, CardInfo _cardInfo)
    {
        if (cardPlayManager.GetXyNumber() < _cardInfo.xjNumber)
        {
            return false;
        }
        //判断场牌数量
        if (bossChangCardList.Count >= maxChangCardNumber)
        {
            return false;
        }
        _obj.GetComponent<ShouCard>().UpateTxImage();
        //扣除心境值
        cardPlayManager.AddXjNumber(-_cardInfo.xjNumber);
        if (_cardInfo.type == 0)
        {
            //先把父节点设置了
            _obj.transform.SetParent(content4.transform);
            //获取卡牌数据
            _cardInfo.state = 6;
            //删除手牌数据
            bossShouCardList.Remove(_obj);
            //加入场牌数据
            bossChangCardList.Add(_obj);
            if (_cardInfo.gjNumberNow >= 7)
            {
                GameManager.instance.GjTx(_obj.transform);
            }
        }
        //抽卡
        if (_cardInfo.type == 1)
        {
            AddBossShouCard(3);
            //删除手牌数据
            bossShouCardList.Remove(_obj);
            //刪除牌
            Destroy(_obj);
        }
        //刷新牌背
        _obj.GetComponent<ShouCard>().UpdateCardBack();

        ChuPaiTeXiao(_cardInfo.txState, 2, _obj);
        return true;
    }
    //特效牌效果
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
            //（复活）
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
    //拖拽牌设置父节点
    public void DrageCardSetFatherOut(GameObject _obj)
    {
        _obj.transform.SetParent(GameObject.Find("Canvas/Panel").transform);
    }
    //拖拽牌设置父节点
    public void DrageCardSetFatherIn(GameObject _obj)
    {
        _obj.transform.SetParent(GameObject.Find("Canvas/Panel/List1/Viewport/Content").transform);
    }
    //刷新心愿值
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
    //玩家结束回合
    public void PlayerEndHuiHe()
    {
        cardPlayManager.EndHuiHe();
        UpdateEndHuiHeBUtton();
    }
    //刷新回合按钮
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
    //获取回合是否是玩家回合
    public bool GetPlayerOperand()
    {
        return cardPlayManager.GetOperand() == 1;
    }
    //AI出牌
    public IEnumerator BossPlayerCard()
    {

        for (int i = 0; i < 5; i++)
        {
            var obj = GetTxXjCard();
            if (obj == null)
            {
                yield return new WaitForSeconds(1f);

                break;
            }
            //出牌
            ChuPaiBoss(obj, obj.GetComponent<ShouCard>().GetCardInfo());
            yield return new WaitForSeconds(1f);
        }
        //AI攻击
        foreach (var item in bossChangCardList)
        {
            if (item.GetComponent<ShouCard>().GetCardInfo().attackNumber > 0)
            {
                if (playerChangCardList.Count > 0)
                {
                    //获取未失活的牌
                    List<GameObject> list = new List<GameObject>();
                    foreach (var jtem in playerChangCardList)
                    {
                        if (jtem.activeInHierarchy)
                        {
                            list.Add(jtem);
                        }
                    }
                    //攻击
                    if (list.Count > 0)
                    {
                        GameObject obj = GetAiAttackObj(list);
                        if (obj != null)
                        {
                            StartCoroutine(AttackIEnumerator(item, obj));
                            yield return new WaitForSeconds(2f);
                        }
                        else
                        {
                            Debug.LogError("数据异常");
                        }
                    }
                    else
                    {
                        //攻击英雄
                        StartCoroutine(AttackIEnumerator(item, heroObj));
                        yield return new WaitForSeconds(2f);
                    }
                }
                else
                {
                    //攻击英雄
                    StartCoroutine(AttackIEnumerator(item, heroObj));
                    yield return new WaitForSeconds(2f);
                }
            }
        }
        //结束回合
        PlayerEndHuiHe();

        yield return new WaitForSeconds(1f);

    }
    public bool GetMaxGjAndPlayerHp()
    {
        int maxGj = 0;
        foreach (var item in bossChangCardList)
        {
            maxGj += item.GetComponent<ShouCard>().GetCardInfo().gjNumberNow;
        }
        int playerHp = heroObj.GetComponent<ShouCard>().GetCardInfo().hpNumberNow;
        if (maxGj >= playerHp)
        {
            return true;
        }
        return false;
    }
    //获取攻击目标
    public GameObject GetAiAttackObj(List<GameObject> _list)
    {
        if (_list.Count <= 0)
        {
            return null;
        }
        foreach (var item in _list)
        {
            var info = item.GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 1)
            {
                return item;
            }
        }
        if (GameManager.instance.GetMaxGjAndPlayerHp())
        {
            return heroObj;
        }
        foreach (var item in _list)
        {
            var info = item.GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 2)
            {
                return item;
            }
        }
        foreach (var item in _list)
        {
            var info = item.GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 4)
            {
                return item;
            }
        }
        foreach (var item in _list)
        {
            var info = item.GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 3)
            {
                return item;
            }
        }
        int randNumber = Util.randomInt(0, _list.Count - 1);
        return _list[randNumber];
    }
    //获取当前手牌里心境最高的牌
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
    //按特效出牌
    public GameObject GetTxXjCard()
    {
        int haveXj = cardPlayManager.GetXyNumber();
        for (int i = 0; i < bossShouCardList.Count; i++)
        {
            var info = bossShouCardList[i].GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 1 && haveXj >= info.xjNumber)
            {
                return bossShouCardList[i];
            }
        }
        for (int i = 0; i < bossShouCardList.Count; i++)
        {
            var info = bossShouCardList[i].GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 4 && haveXj >= info.xjNumber)
            {
                return bossShouCardList[i];
            }
        }
        for (int i = 0; i < bossShouCardList.Count; i++)
        {
            var info = bossShouCardList[i].GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 3 && haveXj >= info.xjNumber)
            {
                return bossShouCardList[i];
            }
        }
        for (int i = 0; i < bossShouCardList.Count; i++)
        {
            var info = bossShouCardList[i].GetComponent<ShouCard>().GetCardInfo();
            if (info.txState == 2 && haveXj >= info.xjNumber)
            {
                return bossShouCardList[i];
            }
        }
        return GetMaxXjCard();
    }
    //存在回合数增加
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
    //场牌攻击次数增加
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
    //回收攻击次数
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
    //攻击
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
                //检查嘲讽
                if (CheckChaoFeng(bossChangCardList))
                {
                    //如果有嘲讽，要先判断是否被攻击obj 是不是嘲讽
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
        _attack0bj.transform.DOMove(mubiaoVec, 0.2f)
          .SetEase(Ease.InOutQuad);// 设置缓动函数
        yield return new WaitForSeconds(0.4f);
        AttackTx(mubiaoVec);
        //扣除血量
        _attack0bj.GetComponent<ShouCard>().addHpNumber(-_beAttack0bj.GetComponent<ShouCard>().GetCardInfo().gjNumberNow);
        _beAttack0bj.GetComponent<ShouCard>().addHpNumber(-_attack0bj.GetComponent<ShouCard>().GetCardInfo().gjNumberNow);
        //扣除行动次数
        _attack0bj.GetComponent<ShouCard>().addAttackNumber(-1);
       //回到原位
       _attack0bj.transform.DOMove(oldVec, 0.3f)
        .SetEase(Ease.InOutQuad);// 设置缓动函数
        yield return new WaitForSeconds(1f);
        //检查血量
        _attack0bj.GetComponent<ShouCard>().CheckHp();
        _beAttack0bj.GetComponent<ShouCard>().CheckHp();
        yield return new WaitForSeconds(1f);
    }
    //设置层级
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
    //销毁场牌
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
    //開啓開始界面
    public void BeginPanel(bool isBool)
    {
        beginPanel.SetActive(isBool);
        beginPanel.transform.Find("Text (Legacy)").GetComponent<Text>().text = "当前关卡："+playerData.playerLevel.ToString();
    }
    //攻击特效
    public void AttackTx(Vector3 _vec3)
    {
        var obj = AddPrefab("attack", GameObject.Find("Canvas").transform);
        obj.transform.position = _vec3;
    }
    //HP特效
    public void HpTx(Transform _trs)
    {
        var obj = AddPrefab("hp", _trs);
        
    }
    //攻击特效
    public void GjTx(Transform _trs)
    {
        //var obj = AddPrefab("gj", _trs);
    }
    //圣盾特效
    public GameObject SDTx(Transform _trs)
    {
        var obj = AddPrefab("ShengDunTx", _trs);
        return obj;
    }
    //复活特效
    public GameObject FHTx(Transform _trs)
    {
        var obj = AddPrefab("fuhuo", _trs);
        return obj;
    }
    // 退出游戏的方法
    public void Quit()
    {
#if UNITY_EDITOR
        // 在 Unity 编辑器中，使用 UnityEditor.EditorApplication.isPlaying 属性来停止播放
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 在发布的应用程序中，使用 Application.Quit() 方法来退出游戏
        Application.Quit();
#endif
    }
    //检查场牌是否有嘲讽牌
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
    //通用提示
    public void MsgShow(string _msg)
    {
        msgPanel.transform.Find("Text (Legacy)").GetComponent<Text>().text = _msg;
        DOVirtual.DelayedCall(3f,MyFunction);
    }
    void MyFunction()
    {
        msgPanel.transform.Find("Text (Legacy)").GetComponent<Text>().text = "";
    }
    //刷新剩余卡牌数
    public void UpdateCardNumberShow()
    {
        gamePanel.transform.Find("Text_player_car_number").GetComponent<Text>().text = playerCardList.Count.ToString();
        gamePanel.transform.Find("Text_boss_car_number").GetComponent<Text>().text = bossCardList.Count.ToString();
    }
    //开启游戏介绍界面
    public void OpenJieShaoPanel()
    {
        jieshaoPanel.SetActive(true);
    }
    //修改音量
    public Slider volumeSlider;
    public void ChangYL()
    {
        if (volumeSlider != null)
        {
            // 为Slider的OnValueChanged事件添加监听器
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }
    void SetVolume(float volume)
    {
        // 设置AudioSource的音量为Slider的值
        musicManager.ChangeBkValue(volume);
    }
}

