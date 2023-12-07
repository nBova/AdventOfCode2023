using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    public static class Day1
    {
        private static string inputPath = @"..\..\..\Assets\Day1Input.txt";

        public static int SumCalibrationValues()
        {
            List<int> calValues = new List<int>();
            var lines = File.ReadLines(inputPath);

            foreach(var line in lines) // Literally O(n²) right here but it's fine... right?
            {
                List<int> intsInLine = new List<int>();

                foreach (char c in line)  // just going char by char on each line... I really hate this
                {
                    if (Char.IsNumber(c))
                    {
                        intsInLine.Add((int)Char.GetNumericValue(c));
                    }
                }

                int calVal = int.Parse(intsInLine[0].ToString() + intsInLine[intsInLine.Count-1].ToString());
                calValues.Add(calVal);
            }

            return calValues.Sum();
        }
    }
}
