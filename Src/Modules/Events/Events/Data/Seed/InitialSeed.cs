namespace Events.Data.Seed;

using System;
using System.Collections.Generic;
using System.Linq;
using global::Events.Events.Models;
using MassTransit;

public static class InitialData
{
    public static List<EventModel> Events { get; }


    static InitialData()
    {

        Events = new List<EventModel>
        {
            //EventModel.Create()
        };
    }
}