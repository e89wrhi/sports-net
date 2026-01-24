namespace Matches.Data.Seed;

using global::Matches.Matches.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public static class InitialData
{
    public static List<MatchModel> Matchs { get; }


    static InitialData()
    {
        Matchs = new List<MatchModel>
        {
            //MatchModel.Create()
        };
    }
}