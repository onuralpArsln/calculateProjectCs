using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class HydrostaticTableParser
{

    string filePath = "";
    string[] lines;

    public HydrostaticTableParser(string filePath)
    {
        this.filePath = filePath;
        this.lines = File.ReadAllLines(filePath);
    }

    public void detectLines()
    {
        Console.WriteLine("Lines containing header:");

        // Loop through each line and check if it contains the word "Tables"
        for (int i = 0; i < this.lines.Length; i++)
        {
            if (this.lines[i].Contains("Trim Draft Displt LCB TCB VCB WPA LCF KML KMT BML BMT IL IT TPC MTC WSA "))
            {
                Console.WriteLine($"Line {i + 1}"); // Output line number (i + 1 to start from 1)
            }
        }
    }

    

    public void retrieveLinesByNumber(int num)
    {
        string line = this.lines[num - 1];

        // Print the retrieved line
        Console.WriteLine($"Line {num}: {line}");
    }


}
