﻿using System;

namespace AdventOfCode2023
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 1 Part 1 Solution: " + Day1.SumCalibrationValues(false));
            Console.WriteLine("Day 1 Part 2 Solution: " + Day1.SumCalibrationValues(true));
            Console.WriteLine("Day 2 Part 1 Solution: " + Day2.SumGameIDs());
            Console.WriteLine("Day 2 Part 2 Solution: " + Day2.CalculateMinimumPower());
            Console.WriteLine("Day 3 Part 1 Solution: " + Day3.SumPartNumbers(true));
            Console.WriteLine("Day 3 Part 2 Solution: " + Day3.SumPartNumbers(false));
        }
    }
}
