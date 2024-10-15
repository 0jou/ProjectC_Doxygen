
// 制作者(田内)

namespace FoodInfo
{

    // 初期値入れとかないと全部ズレるんで消さないでください

    #region 名前空間説明
    /// -------------------------------------
    /// <summary>
    /// 料理のIDを示す列挙型
    /// </summary>
    /// -------------------------------------
    #endregion
    // 料理のIDを示す列挙型
    public enum FoodID
    {
        // キノコ料理
        PoisonMushroomSoup = 103,
        ParalysisMushroomSoup = 105,
        SleepMushroomSoup = 106,
        FireMushroomSoup = 107,

        // オムレツ料理
        Omelette = 40,
        MushroomOmelette = 41,
        ParalysisMushroomOmelette = 42,
        SpicyMushroomOmelette = 43,

        // パイ料理
        SleepApplePie = 60,

        // 肉
        Steak = 80,


        // ボス
        GoldenPudding = 1000,
        WolfRoastBeef = 1001,

    }
}
