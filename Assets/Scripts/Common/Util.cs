using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Spine.Unity;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Util
{
    // 求指定范围内的一个随机数
    public static int randomInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    public static float randomFloat(float min,float max)
    {
        return Random.Range(min, max);
    }

    //根据概率获得结果，例：1,100,60（60是爆率）
    public static bool getResultRandom(float probability)
    {
        int random = Util.randomInt(1, 100);
        bool result = probability >= random ? true : false;
        return result;
    }
    /**
    * describe: 根据权重来随机
    * 从一个数组中进行随机选择元素, 需要其元素为一个obj类型, 包含名为weight的key
    * 返回下标
    * @param array 
    */
    public static int randByWeight(List<(int id, int weight)> list)
    {
        float totalWeight = 0;
        int randIndex = -1;
        foreach (var element in list)
        {
            totalWeight += element.weight;
        }

        if (totalWeight <= 0) {
            return randIndex;
        }
        else
        {
            float randVal = Util.randomFloat(1, totalWeight);
            for (int index = 0; index < list.Count; index++)
            {
                var element = list[index];
                if (randVal <= element.weight) 
                {
                    randIndex = index;
                    break;
                }
                else 
                {
                    randVal -= element.weight;
                }
            }
        }
        return randIndex;
    }
    /**
    * describe: 根据权重来随机
    * 从一个数组中进行随机选择元素, 需要其元素为一个obj类型, 包含名为weight的key
    * 返回下标
    * @param array 
    */
    public static int randByWeight(List<(int id, float time, int weight)> list)
    {
        float totalWeight = 0;
        int randIndex = -1;
        foreach (var element in list)
        {
            totalWeight += element.weight;
        }

        if (totalWeight <= 0) {
            return randIndex;
        }
        else
        {
            float randVal = Util.randomFloat(1, totalWeight);
            for (int index = 0; index < list.Count; index++)
            {
                var element = list[index];
                if (randVal <= element.weight) 
                {
                    randIndex = index;
                    break;
                }
                else 
                {
                    randVal -= element.weight;
                }
            }
        }
        return randIndex;
    }

    /**
     * 获取距离最近的n个节点
     * 以节点坐标为准
     */
    //public static List<(BaseTarget targetScript, float distance)> getDistanceNode(int n, Vector3 stratPos, float offset, List<BaseTarget> targetArray)
    //{
    //    List<(BaseTarget targetScript, float distance)> tempList = new List<(BaseTarget targetScript, float distance)>();
    //    for (int i = 0; i < targetArray.Count; i++)
    //    {
    //        Vector3 dest = targetArray[i].gameObject.transform.position;
    //        float distance = (stratPos - dest).magnitude;
    //        distance -= offset;
    //        tempList.Add((targetArray[i], distance));
    //    }
    //    tempList.Sort((item1, item2) => item1.distance.CompareTo(item2.distance));
    //    List<(BaseTarget targetScript, float distance)> array = new List<(BaseTarget targetScript, float distance)>();
    //    for (int i = 0; i < n; i++)
    //    {
    //        array.Add(tempList[i]);
    //    }
    //    return array;
    //}

    /**打乱数组排序 */
    public static void shuffle<T>(List<T> arr)
    {
        for (int i = 0; i < arr.Count; i++)
        { 
            var index = Random.Range(i, arr.Count);
            var tmp = arr[i];
            var ran = arr[index];
            arr[i] = ran;
            arr[index] = tmp;
        }
    }
    /**计算2个时间相差天数*/
    public static int getTimeSubDay(DateTime d1, DateTime d2)
    {
        DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));
        DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));
        int days = (d4 - d3).Days;
        return days;
    }

    public static string GetStringWithNewLine(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        return input.Replace('@'.ToString(), Environment.NewLine);
    }

    public static Rect InitRect(Vector2 center, Vector2 size)
    {
        Rect rect = new Rect(center - size / 2, size);
        return rect;
    }
    public static void DrawRect(Rect rect, Color color)
    {
        #if UNITY_EDITOR
            Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x + rect.width, rect.y ),color);
            Debug.DrawLine(new Vector3(rect.x, rect.y), new Vector3(rect.x , rect.y + rect.height), color);
            Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x + rect.width, rect.y), color);
            Debug.DrawLine(new Vector3(rect.x + rect.width, rect.y + rect.height), new Vector3(rect.x, rect.y + rect.height), color);
        #endif
    }


    /// <summary>
    /// 从源列表中随机取出指定数量的元素，并从源列表中删除这些元素
    /// </summary>
    /// <param name="sourceList">源列表（将被修改）</param>
    /// <param name="n">要取出的元素数量</param>
    /// <returns>随机取出的元素列表</returns>
    public static List<int> TakeAndRemoveRandomElements(List<int> sourceList, int n)
    {
        List<int> result = new List<int>();
        if (sourceList == null || sourceList.Count == 0 || n <= 0)
            return result;

        // 计算实际要取的数量
        int takeCount = Mathf.Min(n, sourceList.Count);

        // 生成索引列表并洗牌
        List<int> indexes = Enumerable.Range(0, sourceList.Count).ToList();
        FisherYatesShuffle(indexes);

        // 取出前N个洗牌后的索引
        List<int> selectedIndexes = indexes.Take(takeCount).ToList();

        // 按原始顺序收集结果
        foreach (int index in selectedIndexes.OrderBy(x => x))
        {
            result.Add(sourceList[index]);
        }

        // 按倒序删除元素（避免索引变化问题）
        foreach (int index in selectedIndexes.OrderByDescending(x => x))
        {
            sourceList.RemoveAt(index);
        }

        return result;
    }

    /// <summary>
    /// Fisher-Yates洗牌算法
    /// </summary>
    private static void FisherYatesShuffle<T>(IList<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }


    #region 大额数值转换Numdispose(float tempNum_, int digits = 2)
    /// <summary>
    /// 大额数值转换Numdispose(float tempNum_, int digits = 2)
    /// </summary>
    /// <param name="tempNum_">被转换的数据对象</param>
    /// <param name="digits">保留位数</param>
    /// <returns></returns>
    public static string Numdispose(float tempNum_, int digits = 2)
    {
        string num = "";
        string[] symbol = { "", "K", "M", "B", "T", "aa", "ab", "ac", "ad" };
        float tempNum = tempNum_;
        long v = 1000;
        int unitIndex = 0;
        while (tempNum >= v)
        {
            unitIndex++;
            tempNum /= v;
        }
        if (unitIndex >= symbol.Length)
        {
            num = tempNum_.ToString();
        }
        else
        {
            tempNum = Round(tempNum, digits);
            num = $"{tempNum}{symbol[unitIndex]}";
        }
        return num;
    }
    public static float Round(float value, int digits)
    {
        float multiple = Mathf.Pow(10, digits);
        float tempValue = value * multiple + 0.5f;
        tempValue = Mathf.FloorToInt(tempValue);
        return tempValue / multiple;
    }
    #endregion
}