using Microsoft.Xna.Framework;
using StardewValley;
using System;
using System.Collections.Generic;

namespace Find_Item
{
    public class Path_Finding
    {
        public static void FindPaths(GameLocation location, Vector2 playerPos, List<Vector2> targets)
        {
            ModEntry.paths.Clear();
            ModEntry.pathColors.Clear();
            ModEntry.shouldDraw = false;

            float hue = 0f;
            float hueIncrement = 1f / Math.Max(1, targets.Count);

            foreach (Vector2 target in targets)
            {
                List<Vector2> path = FindPath(location, playerPos, target);
                if (path.Count > 0)
                {
                    ModEntry.paths.Add(path);
                    Color pathColor = ColorFromHSV(hue, 1f, 1f);
                    ModEntry.pathColors[path] = pathColor;
                    hue = (hue + hueIncrement) % 1f;
                }
            }

            ModEntry.shouldDraw = ModEntry.paths.Count > 0;
        }

        private static List<Vector2> FindPath(GameLocation location, Vector2 start, Vector2 end)
        {
            var openSet = new List<Vector2> { start };
            var closedSet = new HashSet<Vector2>();
            var cameFrom = new Dictionary<Vector2, Vector2>();
            var gScore = new Dictionary<Vector2, float>();
            var fScore = new Dictionary<Vector2, float>();

            gScore[start] = 0;
            fScore[start] = ManhattanDistance(start, end);

            while (openSet.Count > 0)
            {
                Vector2 current = GetLowestFScore(openSet, fScore);
                if (current == end)
                    return ReconstructPath(cameFrom, end);

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Vector2 neighbor in GetValidNeighbors(location, current))
                {
                    if (closedSet.Contains(neighbor))
                        continue;

                    float tentativeGScore = gScore[current] + 1;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                    else if (tentativeGScore >= gScore[neighbor])
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + ManhattanDistance(neighbor, end);
                }
            }

            return new List<Vector2>();
        }

        private static Vector2 GetLowestFScore(List<Vector2> openSet, Dictionary<Vector2, float> fScore)
        {
            float lowestF = float.MaxValue;
            Vector2 lowestNode = openSet[0];

            foreach (Vector2 node in openSet)
            {
                if (fScore.ContainsKey(node) && fScore[node] < lowestF)
                {
                    lowestF = fScore[node];
                    lowestNode = node;
                }
            }

            return lowestNode;
        }

        private static List<Vector2> GetValidNeighbors(GameLocation location, Vector2 pos)
        {
            var neighbors = new List<Vector2>();
            Vector2[] directions = new[]
            {
                new Vector2(-1, 0),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(0, 1)
            };

            foreach (Vector2 dir in directions)
            {
                Vector2 newPos = pos + dir;
                if (IsWalkable(location, newPos))
                    neighbors.Add(newPos);
            }

            return neighbors;
        }

        private static bool IsWalkable(GameLocation location, Vector2 pos)
        {
            return location.isTilePassable(new xTile.Dimensions.Location((int)pos.X, (int)pos.Y), Game1.viewport);
        }

        private static float ManhattanDistance(Vector2 a, Vector2 b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        private static List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current)
        {
            var path = new List<Vector2> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Add(current);
            }
            path.Reverse();
            return path;
        }

        private static Color ColorFromHSV(float hue, float saturation, float value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue * 6));
            float f = hue * 6 - hi;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);

            float r, g, b;
            switch (hi)
            {
                case 0: r = value; g = t; b = p; break;
                case 1: r = q; g = value; b = p; break;
                case 2: r = p; g = value; b = t; break;
                case 3: r = p; g = q; b = value; break;
                case 4: r = t; g = p; b = value; break;
                default: r = value; g = p; b = q; break;
            }

            return new Color(r, g, b);
        }
    }
}