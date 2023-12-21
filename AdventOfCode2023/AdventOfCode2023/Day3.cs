using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2023
{
    /**
     * --- Day 3: Gear Ratios ---
     * 
     * You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source, but this is as far as he can bring you. You go inside.
     * It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.
     * "Aaah!"
     * You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone! The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.
     * The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one. If you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.
     * The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of numbers and symbols you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)
     * 
     * Here is an example engine schematic:
     * 
     * 467..114..
     * ...*......
     * ..35..633.
     * ......#...
     * 617*......
     * .....+.58.
     * ..592.....
     * ......755.
     * ...$.*....
     * .664.598..
     * 
     * In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114 (top right) and 58 (middle right). Every other number is adjacent to a symbol and so is a part number; their sum is 4361.
     * Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?
     * Your puzzle answer was 532445.
     * 
     * --- Part Two ---
     * 
     * The engineer finds the missing part and installs it in the engine! As the engine springs to life, you jump in the closest gondola, finally ready to ascend to the water source.
     * You don't seem to be going very fast, though. Maybe something is still wrong? Fortunately, the gondola has a phone labeled "help", so you pick it up and the engineer answers.
     * Before you can explain the situation, she suggests that you look out the window. There stands the engineer, holding a phone in one hand and waving with the other. You're going so slowly that you haven't even left the station. You exit the gondola.
     * The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.
     * This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out which gear needs to be replaced.
     * 
     * Consider the same engine schematic again:
     * 
     * 467..114..
     * ...*......
     * ..35..633.
     * ......#...
     * 617*......
     * .....+.58.
     * ..592.....
     * ......755.
     * ...$.*....
     * .664.598..
     * 
     * In this schematic, there are two gears. The first is in the top left; it has part numbers 467 and 35, so its gear ratio is 16345. The second gear is in the lower right; its gear ratio is 451490. (The * adjacent to 617 is not a gear because it is only adjacent to one part number.) Adding up all of the gear ratios produces 467835.
     * 
     * What is the sum of all of the gear ratios in your engine schematic?
     * Your puzzle answer was 79842967.
     * 
     */
    public static class Day3
    {
        private static string inputPath = @"..\..\..\Assets\Day3Input.txt";

        public static int SumPartNumbers(bool part1)
        {
            int solution1 = 0;
            int solution2 = 0;
            var engineSchematic = File.ReadLines(inputPath);
            Engine engine = new Engine();

            foreach (string engineLine in engineSchematic)
            {
                string partNum = "";
                int currIdx = 0;
                List<int> specialCharsOnLineIdxs = new List<int>();
                List<Gear> gearsOnLine = new List<Gear>();
                List<Part> partsOnLine = new List<Part>();

                foreach (char engineChar in engineLine)
                {
                    if (char.IsNumber(engineChar))
                    {
                        partNum = string.Concat(partNum, engineChar);
                    }
                    else
                    {
                        if(engineChar.ToString() != ".")  // found special character
                        {
                            specialCharsOnLineIdxs.Add(currIdx);

                            if (engineChar.ToString() == "*")  // found gear character
                            {
                                gearsOnLine.Add(new Gear() { idx = currIdx});
                            }
                        }

                        if (!string.IsNullOrEmpty(partNum))  // end of part number
                        {
                            Part part = new Part();
                            part.PartNumber = int.Parse(partNum);
                            part.VerticalIndexes = new List<int>();
                            part.HorizontalIndexes = new List<int>();
                            for (int i = 0; i <= partNum.Length + 1; i++)
                            {
                                int addIdx = currIdx - i;
                                if(addIdx >= 0)
                                {
                                    part.VerticalIndexes.Add(addIdx);
                                }
                            }
                            int len = part.VerticalIndexes.Count;
                            part.HorizontalIndexes.Add(part.VerticalIndexes[0]);
                            part.HorizontalIndexes.Add(part.VerticalIndexes[len - 1]);
                            partsOnLine.Add(part);
                            partNum = "";
                        }
                    }
                    currIdx++;
                }
                if (!string.IsNullOrEmpty(partNum))
                {
                    Part part = new Part();
                    part.PartNumber = int.Parse(partNum);
                    part.VerticalIndexes = new List<int>();
                    part.HorizontalIndexes = new List<int>();
                    for (int i = 0; i <= partNum.Length + 1; i++)
                    {
                        int addIdx = currIdx - i;
                        if (addIdx >= 0)
                        {
                            part.VerticalIndexes.Add(addIdx);
                        }
                    }
                    int len = part.VerticalIndexes.Count;
                    part.HorizontalIndexes.Add(part.VerticalIndexes[0]);
                    part.HorizontalIndexes.Add(part.VerticalIndexes[len - 1]);
                    partsOnLine.Add(part);
                    partNum = "";
                }

                engine.AddSpecialIdxLine(specialCharsOnLineIdxs);
                engine.AddPartsLine(partsOnLine);
                engine.AddGearLine(gearsOnLine);
                solution1 += engine.CheckCurrentLineParts();
                solution1 += engine.CheckPreviousLineParts();
                engine.UpdatePreviosLineGears();
                engine.UpdateCurrentLineGears();
                solution2 += engine.GearRatioSum();
            }

            if (part1)
            {
                return solution1;
            }
            else
            {
                return solution2;
            }
        }
    }

    public class Part
    {
        public int PartNumber { get; set; }

        public List<int> VerticalIndexes { get; set; }

        public List<int> HorizontalIndexes { get; set; }

        public bool Used { get; set; } = false;
    }

    public class Gear
    {
        public int idx { get; set; }

        public Part adjacentPart1 { get; set; }

        public Part adjacentPart2 { get; set; }

        public bool Used { get; set; } = false;

        public int GearRatio
        { 
            get
            {
                if(adjacentPart1 != null && adjacentPart2 != null && !Used)
                {
                    Used = true;
                    return adjacentPart1.PartNumber * adjacentPart2.PartNumber;
                }
                else
                {
                    return 0;
                }
            } 
        }

        public void AddPart(Part part)
        {
            if(adjacentPart1 == null)
            {
                adjacentPart1 = part;
            }
            else if (adjacentPart2 == null)
            {
                adjacentPart2 = part;
            }
        }
    }

    public class Engine
    {
        public List<int> PreviousLineSpecialIdxs { get; set; }

        public List<int> CurrentLineSpecialIdxs { get; set; }

        public List<Part> PreviousLineParts { get; set; }

        public List<Part> CurrentLineParts { get; set; }

        public List<Gear> PreviousLineGearIdx { get; set; }

        public List<Gear> CurrentLineGearIdx { get; set; }

        public int CheckCurrentLineParts()
        {
            int total = 0;

            foreach(Part part in CurrentLineParts)
            {
                bool totalChanged = false;
                foreach(int idx in part.HorizontalIndexes)
                {
                    if (CurrentLineSpecialIdxs.Contains(idx))
                    {
                        if (!part.Used)
                        {
                            total += part.PartNumber;
                            part.Used = true;
                            totalChanged = true;
                            break;
                        }
                    }
                }

                if (totalChanged)
                {
                    continue;
                }

                if (PreviousLineSpecialIdxs != null)
                {
                    foreach (int idx in part.VerticalIndexes)
                    {
                        if (PreviousLineSpecialIdxs.Contains(idx))
                        {
                            if (!part.Used)
                            {
                                total += part.PartNumber;
                                part.Used = true;
                                break;
                            }
                        }
                    }
                }
            }

            return total;
        }

        public int CheckPreviousLineParts()
        {
            int total = 0;

            if(PreviousLineSpecialIdxs == null || PreviousLineParts == null)
            {
                return total;
            }

            foreach(Part part in PreviousLineParts)
            {
                foreach(int idx in part.VerticalIndexes)
                {
                    if (CurrentLineSpecialIdxs.Contains(idx))
                    {
                        if (!part.Used)
                        {
                            total += part.PartNumber;
                            part.Used = true;
                            break;
                        }
                    }
                }
            }

            return total;
        }

        public void UpdateCurrentLineGears()
        {
            foreach(Gear gear in CurrentLineGearIdx)
            {
                if(PreviousLineParts != null)
                {
                    foreach(Part part in PreviousLineParts)
                    {
                        if (part.VerticalIndexes.Contains(gear.idx))
                        {
                            gear.AddPart(part);
                        }
                    }
                }

                foreach(Part part in CurrentLineParts)
                {
                    if (part.VerticalIndexes.Contains(gear.idx))
                    {
                        gear.AddPart(part);
                    }
                }
            }
        }

        public void UpdatePreviosLineGears()
        {
            if(PreviousLineGearIdx == null)
            {
                return;
            }

            foreach(Gear gear in PreviousLineGearIdx)
            {
                foreach(Part part in CurrentLineParts)
                {
                    if (part.VerticalIndexes.Contains(gear.idx))
                    {
                        gear.AddPart(part);
                    }
                }
            }
        }

        public int GearRatioSum()
        {
            int total = 0;

            if(PreviousLineGearIdx != null)
            {
                foreach (Gear gear in PreviousLineGearIdx)
                {
                    total += gear.GearRatio;
                }
            }

            foreach (Gear gear in CurrentLineGearIdx)
            {
                total += gear.GearRatio;
            }

            return total;
        }

        public void AddSpecialIdxLine(List<int> specialIdxs)
        {
            if(CurrentLineSpecialIdxs == null)
            {
                CurrentLineSpecialIdxs = specialIdxs;
            }
            else
            {
                PreviousLineSpecialIdxs = CurrentLineSpecialIdxs;
                CurrentLineSpecialIdxs = specialIdxs;
            }
        }

        public void AddPartsLine(List<Part> parts)
        {
            if(CurrentLineParts == null)
            {
                CurrentLineParts = parts;
            }
            else
            {
                PreviousLineParts = CurrentLineParts;
                CurrentLineParts = parts;
            }
        }

        public void AddGearLine(List<Gear> gears)
        {
            if (CurrentLineGearIdx == null)
            {
                CurrentLineGearIdx = gears;
            }
            else
            {
                PreviousLineGearIdx = CurrentLineGearIdx;
                CurrentLineGearIdx = gears;
            }
        }
    }
}
