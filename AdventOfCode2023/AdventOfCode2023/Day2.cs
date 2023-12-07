using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2023
{
    public static class Day2
    {
        private static string inputPath = @"..\..\..\Assets\Day2Input.txt";
        private static readonly string gameIDRegexString = @"Game (\d+)";
        private static readonly string gameResultsRegexString = @"(\d+) (red|green|blue)";
        private const int REDCUBESMAX = 12;
        private const int BLUECUBESMAX = 14;
        private const int GREENCUBESMAX = 13;

        public static int SumGameIDs()
        {
            int solution = 0;
            var games = File.ReadLines(inputPath);

            foreach(string game in games)
            {
                int gameID = -1;
                bool validGame = true;
                Regex gameIDRegex = new Regex(gameIDRegexString);
                Regex gameResultsRegex = new Regex(gameResultsRegexString);

                Match gameIDMatch = gameIDRegex.Match(game);
                if (gameIDMatch.Success)
                {
                    gameID = Int32.Parse(gameIDMatch.Groups[1].Value);
                }

                foreach(Match match in gameResultsRegex.Matches(game))
                {
                    switch (match.Groups[2].Value.ToString())
                    {
                        case "blue":
                            if (Int32.Parse(match.Groups[1].Value) > BLUECUBESMAX)
                            {
                                validGame = false;
                            }
                            break;
                        case "red":
                            if (Int32.Parse(match.Groups[1].Value) > REDCUBESMAX)
                            {
                                validGame = false;
                            }
                            break;
                        case "green":
                            if (Int32.Parse(match.Groups[1].Value) > GREENCUBESMAX)
                            {
                                validGame = false;
                            }
                            break;
                    }
                    if (!validGame)
                    {
                        break;
                    }
                }

                if (validGame && gameID > 0)
                {
                    solution += gameID;
                }
            }

            return solution;
        }
    }
}
