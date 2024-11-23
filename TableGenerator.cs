using System;
using System.Data;
using System.IO;
using System.Globalization;

class TableGenerator
{
    string filePath;
    DataTable table = new DataTable();

    // Constructor to initialize the TableGenerator with the file path
    public TableGenerator(string filePath)
    {
        // File path to your .txt file
        this.filePath = filePath;

        // Column names
        string[] columnNames = new string[]
        {
            "Trim", "Draft", "Displt", "LCB", "TCB", "VCB", "WPA", "LCF", "KML", "KMT", "BML", "BMT", "IL", "IT", "TPC", "MTC", "WSA"
        };

        // Add columns to DataTable and set their type to double
        foreach (var column in columnNames)
        {
            this.table.Columns.Add(column, typeof(double));  // Ensure each column stores doubles
        }

        // Read the file line by line
        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            int lineCount = 0;

            while ((line = sr.ReadLine()) != null)
            {
                // Split the line by spaces or tabs
                string[] values = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // Check if the number of columns is correct
                if (values.Length == columnNames.Length)
                {
                    // Validate if all values are valid doubles
                    bool isValidLine = true;
                    for (int i = 0; i < values.Length; i++)
                    {
                        // Try to parse each value as a double using InvariantCulture to ensure dot separator
                        if (!double.TryParse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                        {
                            isValidLine = false;
                            break;
                        }
                    }

                    // If valid, add it to the DataTable
                    if (isValidLine)
                    {
                        DataRow row = table.NewRow();
                        for (int i = 0; i < values.Length; i++)
                        {
                            // Parse each value as a double before adding it
                            row[i] = double.Parse(values[i], NumberStyles.Any, CultureInfo.InvariantCulture);  // Enforce invariant culture
                        }
                        this.table.Rows.Add(row);
                        lineCount++;
                    }
                }
            }
        }

        // Optional: Output line count for debugging
        Console.WriteLine($"Total valid lines: {this.table.Rows.Count}");
    }

    // Method to display every 80th row in the DataTable
    public void testTable()
    {
        Console.WriteLine("Total rows: " + this.table.Rows.Count);

        // Display every 80th row in the DataTable (adjusted for clarity)
        for (int i = 79; i < this.table.Rows.Count; i += 80) // Start at index 79 for the first 80th row
        {
            DataRow row = table.Rows[i];
            foreach (var item in row.ItemArray)
            {
                // Ensure the value is a double, then format it as a string
                if (item is double)
                {
                    Console.Write(((double)item).ToString("0.##") + "\t");  // Formats doubles to show up to 2 decimal places
                }
                else
                {
                    // If it's not a double, just print it as is
                    Console.Write(item.ToString() + "\t");
                }
            }
            Console.WriteLine();
        }
    }

    // Method to find the closest Trim values (lower and upper bounds)
    public void lookForTrim(double givenTrim)
    {
        DataRow trimLowerBound = null;
        DataRow trimUpperBound = null;

        // Find the row with the highest "Trim" value that is <= givenTrim
        trimLowerBound = table.AsEnumerable()
                        .Where(r => r.Field<double>("Trim") <= givenTrim)
                        .OrderByDescending(r => r.Field<double>("Trim"))
                        .FirstOrDefault();

        // Find the row with the lowest "Trim" value that is >= givenTrim
        trimUpperBound = this.table.AsEnumerable()
                        .Where(r => r.Field<double>("Trim") >= givenTrim)
                        .OrderBy(r => r.Field<double>("Trim"))
                        .FirstOrDefault();

        // Display the results (trimLowerBound and trimUpperBound are DataRow objects, so we'll extract relevant data)
        if (trimLowerBound != null)
        {
            Console.WriteLine("Lower bound Trim: " + trimLowerBound["Trim"] + ", Draft: " + trimLowerBound["Draft"]);
        }
        else
        {
            Console.WriteLine("No lower bound found.");
        }

        if (trimUpperBound != null)
        {
            Console.WriteLine("Upper bound Trim: " + trimUpperBound["Trim"] + ", Draft: " + trimUpperBound["Draft"]);
        }
        else
        {
            Console.WriteLine("No upper bound found.");
        }
    }
}
