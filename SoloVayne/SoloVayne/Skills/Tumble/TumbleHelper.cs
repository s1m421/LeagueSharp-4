﻿using System.Collections.Generic;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SoloVayne.Utility;
using SoloVayne.Utility.Geometry;

namespace SoloVayne.Skills.Tumble
{
    class TumbleHelper
    {

        private static float range = 1000f;

        /// <summary>
        /// Gets the rotated q positions.
        /// </summary>
        /// <returns></returns>
        public static List<Vector3> GetRotatedQPositions()
        {
            const int currentStep = 30;
           // var direction = ObjectManager.Player.Direction.To2D().Perpendicular();
            var direction = (Game.CursorPos - ObjectManager.Player.ServerPosition).Normalized().To2D();

            var list = new List<Vector3>();
            for (var i = -95; i <= 95; i += currentStep)
            {
                var angleRad = Geometry.DegreeToRadian(i);
                var rotatedPosition = ObjectManager.Player.Position.To2D() + (300f * direction.Rotated(angleRad));
                list.Add(rotatedPosition.To3D());
            }
            return list;
        }

        /// <summary>
        /// Gets the rotated q positions.
        /// </summary>
        /// <returns></returns>
        public static List<Vector3> GetCompleteRotatedQPositions()
        {
            const int currentStep = 30;
            // var direction = ObjectManager.Player.Direction.To2D().Perpendicular();
            var direction = (Game.CursorPos - ObjectManager.Player.ServerPosition).Normalized().To2D();

            var list = new List<Vector3>();
            for (var i = -0; i <= 360; i += currentStep)
            {
                var angleRad = Geometry.DegreeToRadian(i);
                var rotatedPosition = ObjectManager.Player.Position.To2D() + (300f * direction.Rotated(angleRad));
                list.Add(rotatedPosition.To3D());
            }
            return list;
        }

        /// <summary>
        /// Gets the closest enemy.
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns></returns>
        public static Obj_AI_Hero GetClosestEnemy(Vector3 from)
        {
            if (Variables.Orbwalker.GetTarget() is Obj_AI_Hero)
            {
                var owAI = Variables.Orbwalker.GetTarget() as Obj_AI_Hero;
                if (owAI.IsValidTarget(Orbwalking.GetRealAutoAttackRange(null) + 120f, true, from))
                {
                    return owAI;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether the specified position is Safe using AA ranges logic.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static bool IsSafeEx(Vector3 position)
        {
            var closeEnemies =
                    HeroManager.Enemies.FindAll(en => en.IsValidTarget(range) && !(en.Distance(ObjectManager.Player.ServerPosition) < en.AttackRange + 65f))
                    .OrderBy(en => en.Distance(position));

            return closeEnemies.All(
                                enemy =>
                                    position.CountEnemiesInRange(enemy.AttackRange) <= 1);
        }

        /// <summary>
        /// Gets the average distance of a specified position to the enemies.
        /// </summary>
        /// <param name="from">From.</param>
        /// <returns></returns>
        public static float GetAvgDistance(Vector3 from)
        {
            var numberOfEnemies = from.CountEnemiesInRange(range);
            if (numberOfEnemies != 0)
            {
                var enemies = HeroManager.Enemies.Where(en => en.IsValidTarget(range, true, from)
                                                    &&
                                                    en.Health >
                                                    ObjectManager.Player.GetAutoAttackDamage(en) * 3 +
                                                    Variables.spells[SpellSlot.W].GetDamage(en) +
                                                    Variables.spells[SpellSlot.Q].GetDamage(en)).ToList();
                var enemiesEx = HeroManager.Enemies.Where(en => en.IsValidTarget(range, true, from)).ToList();
                var LHEnemies = enemiesEx.Count() - enemies.Count();

                var totalDistance = (LHEnemies > 1 && enemiesEx.Count() > 2) ?
                    enemiesEx.Sum(en => en.Distance(ObjectManager.Player.ServerPosition)) :
                    enemies.Sum(en => en.Distance(ObjectManager.Player.ServerPosition));

                return totalDistance / numberOfEnemies;
            }
            return -1;
        }

        /// <summary>
        /// Gets the enemy points.
        /// </summary>
        /// <param name="dynamic">if set to <c>true</c> [dynamic].</param>
        /// <returns></returns>
        public static List<Vector2> GetEnemyPoints(bool dynamic = true)
        {
            //Static Melee range
            var staticRange = 380f;
            var polygonsList = TumbleVariables.EnemiesClose.Select(enemy => new SOLOGeometry.Circle(enemy.ServerPosition.To2D(), (dynamic ? (enemy.IsMelee ? enemy.AttackRange * 1.5f : enemy.AttackRange) : staticRange) + enemy.BoundingRadius + 20).ToPolygon()).ToList();
            var pathList = SOLOGeometry.ClipPolygons(polygonsList);
            var pointList = pathList.SelectMany(path => path, (path, point) => new Vector2(point.X, point.Y)).Where(currentPoint => !currentPoint.IsWall()).ToList();
            return pointList;
        }

        /// <summary>
        /// Gets the Q burst mode position.
        /// </summary>
        /// <returns></returns>
        public static Vector3? GetQBurstModePosition()
        {
            var positions = GetWallQPositions(ObjectManager.Player.BoundingRadius - 2.5f).ToList();

            return positions.FirstOrDefault(position => position.IsWall() && position.IsSafe());
        }

        /// <summary>
        /// Gets the wall Q positions.
        /// </summary>
        /// <param name="Range">The range.</param>
        /// <returns></returns>
        public static Vector3[] GetWallQPositions(float Range)
        {
            //Gets the position at the left and right of the players
            Vector3[] vList =
            {
                (ObjectManager.Player.ServerPosition.To2D() + Range * ObjectManager.Player.Direction.To2D()).To3D(),
                (ObjectManager.Player.ServerPosition.To2D() - Range * ObjectManager.Player.Direction.To2D()).To3D()
            };

            return vList;
        }

    }
}
