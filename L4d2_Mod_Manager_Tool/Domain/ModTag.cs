using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L4d2_Mod_Manager_Tool.Domain
{
    public static class ModTag
    {
        public static string[] SurvivorsTags =>
            new string[]
            {
                "Survivors"
                ,"Bill"
                ,"Francis"
                ,"Louis"
                ,"Zoey"
                ,"Coach"
                ,"Ellis"
                ,"Nick"
                ,"Rochelle"
            };

        public static string[] InfectedTags =>
            new string[]
            {
                "Common Infected"
                ,"Special Infected"
                ,"Boomer"
                ,"Charger"
                ,"Hunter"
                ,"Jockey"
                ,"Smoker"
                ,"Spitter"
                ,"Tank"
                ,"Witch"
            };

        public static string[] GameContentTags =>
            new string[]
            {
                "Campaigns"
                ,"Weapons"
                ,"Items"
                ,"Sounds"
                ,"Scripts"
                ,"UI"
                ,"Miscellaneous"
                ,"Models"
                ,"Textures"
            };

        public static string[] GameModesTags =>
            new string[]
            {
                "Single Player"
                ,"Co-op"
                ,"Versus"
                ,"Scavenge"
                ,"Survival"
                ,"Realism"
                ,"Realism Versus"
                ,"Mutations"
            };

        public static string[] WeaponsTags =>
            new string[]
            {
                "Grenade Launcher"
                ,"M60"
                ,"Melee"
                ,"Pistol"
                ,"Rifle"
                ,"Shotgun"
                ,"SMG"
                ,"Sniper"
                ,"Throwable"
            };

        public static string[] ItemsTags =>
            new string[]
            {
                "Adrenaline"
                ,"Defibrillator"
                ,"Medkit"
                ,"Pills"
                ,"Other"
            };
    }
}
