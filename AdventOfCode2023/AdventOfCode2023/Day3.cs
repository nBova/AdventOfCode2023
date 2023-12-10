using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace AdventOfCode2023
{
    public static class Day3
    {
        private static string inputPath = @"..\..\..\Assets\Day3Input.txt";

        public static int SumPartNumbers()
        {
            int solution = 0;
            var engineSchematic = File.ReadLines(inputPath);
            Engine engine = new Engine();

            foreach (string engineLine in engineSchematic)
            {
                string partNum = "";
                int currIdx = 0;
                List<int> specialCharsOnLineIdxs = new List<int>();
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
                solution += engine.CheckCurrentLineParts();
                solution += engine.CheckPreviousLineParts();
            }

            return solution;
        }
    }

    public class Part
    {
        public int PartNumber { get; set; }

        public List<int> VerticalIndexes { get; set; }

        public List<int> HorizontalIndexes { get; set; }

        public bool Used { get; set; } = false;
    }

    public class Engine
    {
        public List<int> PreviousLineSpecialIdxs { get; set; }

        public List<int> CurrentLineSpecialIdxs { get; set; }

        public List<Part> PreviousLineParts { get; set; }

        public List<Part> CurrentLineParts { get; set; }

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
    }
}
