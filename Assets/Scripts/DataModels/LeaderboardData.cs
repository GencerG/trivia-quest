using System;
using System.Collections.Generic;


[Serializable]
public class LeaderboardPageData
{
    public int page;
    public bool is_last;
    public List<LeaderboardPlayerData> data;
}

[Serializable]
public class LeaderboardPlayerData
{
    public int rank;
    public string nickname;
    public int score;
}

