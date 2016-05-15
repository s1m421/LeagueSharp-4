﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZAIO_Reborn.Plugins.Interface;
using LeagueSharp;
using LeagueSharp.Common;

namespace DZAIO_Reborn.Plugins.Champions.Trundle
{
    class Trundle : IChampion
    {
        public void OnLoad(Menu menu)
        {
             
        }

        public void RegisterEvents(){  }

        public Dictionary<SpellSlot, Spell> GetSpells()
        {
           return new Dictionary<SpellSlot, Spell>
                      {
                                    { SpellSlot.Q, new Spell(SpellSlot.Q, 550f) },
                                    { SpellSlot.W, new Spell(SpellSlot.W, 900f) },
                                    { SpellSlot.E, new Spell(SpellSlot.E, 1000f) },
                                    { SpellSlot.R, new Spell(SpellSlot.R, 700f) }
                      };
        }

        public void OnTick()
        {
            throw new NotImplementedException();
        }

        public void OnCombo()
        {
            throw new NotImplementedException();
        }

        public void OnMixed()
        {
            throw new NotImplementedException();
        }

        public void OnLastHit()
        {
            throw new NotImplementedException();
        }

        public void OnLaneclear()
        {
            throw new NotImplementedException();
        }
    }
}
