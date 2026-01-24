namespace Vote.Data.Seed;

using global::Vote.Votes.Models;
using Google.Rpc;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;

public static class InitialData
{
    public static List<VoteModel> Votes { get; }


    static InitialData()
    {
        Votes = new List<VoteModel>
        {
            //VoteModel.Create()
        };
    }
}