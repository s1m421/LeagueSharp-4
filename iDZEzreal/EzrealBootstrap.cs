﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Core;
using LeagueSharp;
using Menu = LeagueSharp.Common.Menu;
using iDZEzreal.MenuHelper;

namespace iDZEzreal
{
    class EzrealBootstrap
    {

        internal static void OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != "Ezreal")
            {
                return;
            }
            MenuGenerator.Generate();
            DZAntigapcloser.BuildMenu(Variables.Menu, "[Ez] Antigapcloser", "ezreal.antigapcloser");

            SPrediction.Prediction.Initialize(Variables.Menu);
            Ezreal.OnLoad();
        }
    }
}
