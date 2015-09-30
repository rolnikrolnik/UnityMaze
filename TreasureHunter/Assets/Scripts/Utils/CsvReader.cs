using UnityEngine;
using System.Collections;
using System.Linq; 
using System;

namespace Treasure_Hunter.Utils
{
    public static class CsvReader
    {
        // outputs the content of a 2D array, useful for checking the importer
        static public void DebugOutputGrid(string[,] grid)
        {
            string textOutput = "";
            for (int y = 0; y < grid.GetUpperBound(1); y++)
            {
                for (int x = 0; x < grid.GetUpperBound(0); x++)
                {

                    textOutput += grid[x, y];
                    textOutput += "|";
                }
                textOutput += "<>\n";
            }
            Debug.Log(textOutput);
        }

        // splits a CSV file into a 2D string array
        static public string[,] SplitCsvGrid(string csvText)
        {
            string[] lines = csvText.Split(new string[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);

            // finds the max width of row
            int width = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                string[] row = lines[i].Split(new string[] { "," }, StringSplitOptions.None);
                width = Mathf.Max(width, row.Length);
            }

            // creates new 2D string grid to output to
            string[,] outputGrid = new string[width + 1, lines.Length + 1];
            for (int y = 0; y < lines.Length; y++)
            {
                string[] row = lines[y].Split(new string[] { "," }, StringSplitOptions.None);
                for (int x = 0; x < row.Length; x++)
                {
                    outputGrid[x, y] = row[x];

                    // This line was to replace "" with " in my output. 
                    // Include or edit it as you wish.
                    outputGrid[x, y] = outputGrid[x, y].Replace("\"\"", "\"");
                }
            }

            return outputGrid;
        }

        // splits a CSV row 
        static public string[] SplitCsvLine(string line)
        {
            return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
                                                                                                                @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
                                                                                                                System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                    select m.Groups[1].Value).ToArray();
        }
    }
}