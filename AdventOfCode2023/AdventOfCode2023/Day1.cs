/**
* --- Day 1: Trebuchet?! ---
* Something is wrong with global snow production, and you've been selected to take a look. The Elves have even given you a map; on it, they've used stars to mark the top fifty locations that are likely to be having problems.
* You've been doing this long enough to know that to restore snow operations, you need to check all fifty stars by December 25th.
* Collect stars by solving puzzles. Two puzzles will be made available on each day in the Advent calendar; the second puzzle is unlocked when you complete the first. Each puzzle grants one star. Good luck!
* You try to ask why they can't just use a weather machine ("not powerful enough") and where they're even sending you ("the sky") and why your map looks mostly blank ("you sure ask a lot of questions") and hang on did you just say the sky ("of course, where do you think snow comes from") when you realize that the Elves are already loading you into a trebuchet ("please hold still, we need to strap you in").
* As they're making the final adjustments, they discover that their calibration document (your puzzle input) has been amended by a very young Elf who was apparently just excited to show off her art skills. Consequently, the Elves are having trouble reading the values on the document.
* The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.
* 
* For example:
* 1abc2
* pqr3stu8vwx
* a1b2c3d4e5f
* treb7uchet
* 
* In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.
* 
* Consider your entire calibration document. What is the sum of all of the calibration values?
* 
* Your puzzle answer was 54667.
* 
* --- Part Two ---
* Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".
* Equipped with this new information, you now need to find the real first and last digit on each line. For example:
* 
* two1nine
* eightwothree
* abcone2threexyz
* xtwone3four
* 4nineeightseven2
* zoneight234
* 7pqrstsixteen
* 
* In this example, the calibration values are 29, 83, 13, 24, 42, 14, and 76. Adding these together produces 281.
* 
* What is the sum of all of the calibration values?
* 
* Your puzzle answer was 54203.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode2023
{
    public static class Day1
    {
        private static string inputPath = @"..\..\..\Assets\Day1Input.txt";
        private static Hashtable numberNames = new Hashtable();

        public static int SumCalibrationValues(bool part2)
        {
            int solution = 0;
            InitializeNumberNames();
            var lines = File.ReadLines(inputPath);

            foreach (string line in lines) // Literally O(n²) right here but it's fine... right?
            {
                List<int> numbersInLine = new List<int>();
                string lineClone = line;  // keep a copy of the line so we can chop chars off of it for part 2

                foreach (char c in line)
                {
                    if (Char.IsNumber(c))  // if the character is a number, let's store that in our list of numbers
                    {
                        numbersInLine.Add((int)Char.GetNumericValue(c));
                    }
                    else
                    {
                        if (part2)  // for part 2, we check the cloned line to see if it starts with a number in text form. If it matches one of the numbers in our Dictionary, add it
                        {
                            foreach (DictionaryEntry entry in numberNames)
                            {
                                if (lineClone.StartsWith((string)entry.Key))
                                {
                                    numbersInLine.Add((int)entry.Value);
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(lineClone) && part2)  // let's keep the cloned line in sync with what character we're currently on, makes the "lineClone.StartsWith" much easier for part 2
                    {
                        lineClone = lineClone.Remove(0,1);
                    }
                }

                int calVal = int.Parse(numbersInLine[0].ToString() + numbersInLine[numbersInLine.Count-1].ToString());
                solution += calVal;
            }

            return solution;
        }

        // build a dictionary of the possible text numbers
        private static void InitializeNumberNames()
        {
            numberNames.Clear();
            numberNames.Add("zero", 0);
            numberNames.Add("one", 1);
            numberNames.Add("two", 2);
            numberNames.Add("three", 3);
            numberNames.Add("four", 4);
            numberNames.Add("five", 5);
            numberNames.Add("six", 6);
            numberNames.Add("seven", 7);
            numberNames.Add("eight", 8);
            numberNames.Add("nine", 9);
        }
    }
}
