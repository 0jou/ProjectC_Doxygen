using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class ManagementExtensions
{
    // 制作者　田内
    // 経営用の拡張クラス

    /// <summary>
    /// リストをランダムにする
    /// </summary>
    public static List<T> RandomList<T>(this List<T> _list)
    {
        // ランダムインスタンスを作成
        System.Random random = new System.Random();

        // Listの要素をランダムに並び替える
        List<T> shuffleList = _list.OrderBy(x => random.Next()).ToList();


        return shuffleList;
    }

    /// <summary>
    /// リストからランダムで値を取得
    /// </summary>
    public static T Random<T>(this List<T> _list)
    {
        if (_list == null || _list.Count == 0)
        {
            return default;
        }

        System.Random random = new System.Random();
        int index = random.Next(_list.Count);
        return _list[index];
    }


    /// <summary>
    /// ハッシュセットをランダムにする
    /// </summary>
    public static HashSet<T> RandomHashSet<T>(this HashSet<T> _hashSet)
    {
        // ランダムインスタンスを作成
        System.Random random = new System.Random();

        // HashSetの要素をランダムに並び替える
        HashSet<T> shuffleHashSet = _hashSet.OrderBy(x => random.Next()).ToHashSet();

        return shuffleHashSet;
    }
}
