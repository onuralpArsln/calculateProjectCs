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

    public void lookForDraft(double givenDraft)
    {
        DataRow draftLowerBound = null;
        DataRow draftUpperBound = null;

        // Find the row with the highest "Draft" value that is <= givenDraft
        draftLowerBound = table.AsEnumerable()
                            .Where(r => r.Field<double>("Draft") <= givenDraft)
                            .OrderByDescending(r => r.Field<double>("Draft"))
                            .FirstOrDefault();

        // Find the row with the lowest "Draft" value that is >= givenDraft
        draftUpperBound = this.table.AsEnumerable()
                            .Where(r => r.Field<double>("Draft") >= givenDraft)
                            .OrderBy(r => r.Field<double>("Draft"))
                            .FirstOrDefault();

        // Display the results (draftLowerBound and draftUpperBound are DataRow objects, so we'll extract relevant data)
        if (draftLowerBound != null)
        {
            Console.WriteLine("Lower bound Draft: " + draftLowerBound["Draft"] + ", Trim: " + draftLowerBound["Trim"]);
            DisplayAllValues(draftLowerBound);  // Display all values for the draft lower bound row
        }
        else
        {
            Console.WriteLine("No lower bound found for Draft.");
        }

        if (draftUpperBound != null)
        {
            Console.WriteLine("Upper bound Draft: " + draftUpperBound["Draft"] + ", Trim: " + draftUpperBound["Trim"]);
            DisplayAllValues(draftUpperBound);  // Display all values for the draft upper bound row
        }
        else
        {
            Console.WriteLine("No upper bound found for Draft.");
        }
    }


    public void lookForTrimAndDraft(double givenTrim, double givenDraft)
    {
        // Find the lower and upper bounds for Trim
        DataRow trimLowerBound = table.AsEnumerable()
            .Where(r => r.Field<double>("Trim") <= givenTrim)
            .OrderByDescending(r => r.Field<double>("Trim"))
            .FirstOrDefault();

        DataRow trimUpperBound = table.AsEnumerable()
            .Where(r => r.Field<double>("Trim") >= givenTrim)
            .OrderBy(r => r.Field<double>("Trim"))
            .FirstOrDefault();

        // If either trim bound is not found, return
        if (trimLowerBound == null || trimUpperBound == null)
        {
            Console.WriteLine("Trim bounds not found.");
            return;
        }

        // Display the found Trim bounds along with all other values in the row
        Console.WriteLine("Lower Bound Trim: " + trimLowerBound["Trim"] + ", Draft: " + trimLowerBound["Draft"]);
        Console.WriteLine("Upper Bound Trim: " + trimUpperBound["Trim"] + ", Draft: " + trimUpperBound["Draft"]);
        DisplayAllValues(trimLowerBound);  // Display all column values for lower bound
        DisplayAllValues(trimUpperBound);  // Display all column values for upper bound

        // Cast the Trim and Draft values to double for proper comparison
        double lowerTrim = trimLowerBound.Field<double>("Trim");
        double upperTrim = trimUpperBound.Field<double>("Trim");
        double lowerDraft = trimLowerBound.Field<double>("Draft");
        double upperDraft = trimUpperBound.Field<double>("Draft");

        // Find the lower and upper bounds for Draft based on the given draft and trim values
        // For Trim Upper Bound (given upper trim), find the lower and upper Draft bounds
        DataRow draftLowerForTrimUpper = table.AsEnumerable()
            .Where(r => r.Field<double>("Trim") == upperTrim && r.Field<double>("Draft") <= givenDraft)
            .OrderByDescending(r => r.Field<double>("Draft"))
            .FirstOrDefault();

        DataRow draftUpperForTrimUpper = table.AsEnumerable()
            .Where(r => r.Field<double>("Trim") == upperTrim && r.Field<double>("Draft") >= givenDraft)
            .OrderBy(r => r.Field<double>("Draft"))
            .FirstOrDefault();

        // For Trim Lower Bound (given lower trim), find the lower and upper Draft bounds
        DataRow draftLowerForTrimLower = table.AsEnumerable()
            .Where(r => r.Field<double>("Trim") == lowerTrim && r.Field<double>("Draft") <= givenDraft)
            .OrderByDescending(r => r.Field<double>("Draft"))
            .FirstOrDefault();

        DataRow draftUpperForTrimLower = table.AsEnumerable()
            .Where(r => r.Field<double>("Trim") == lowerTrim && r.Field<double>("Draft") >= givenDraft)
            .OrderBy(r => r.Field<double>("Draft"))
            .FirstOrDefault();

        // Check if all the rows are found and print the results
        if (draftLowerForTrimUpper != null && draftUpperForTrimUpper != null &&
            draftLowerForTrimLower != null && draftUpperForTrimLower != null)
        {
            Console.WriteLine("Draft Lower Bound for Trim Upper Bound: " + draftLowerForTrimUpper["Draft"]);
            Console.WriteLine("Draft Upper Bound for Trim Upper Bound: " + draftUpperForTrimUpper["Draft"]);
            Console.WriteLine("Draft Lower Bound for Trim Lower Bound: " + draftLowerForTrimLower["Draft"]);
            Console.WriteLine("Draft Upper Bound for Trim Lower Bound: " + draftUpperForTrimLower["Draft"]);

            // Display all values for the draft rows
            DisplayAllValues(draftLowerForTrimUpper);  // Display all column values for draft lower bound for upper trim
            DisplayAllValues(draftUpperForTrimUpper);  // Display all column values for draft upper bound for upper trim
            DisplayAllValues(draftLowerForTrimLower);  // Display all column values for draft lower bound for lower trim
            DisplayAllValues(draftUpperForTrimLower);  // Display all column values for draft upper bound for lower trim
        }
        else
        {
            Console.WriteLine("One or more draft bounds not found.");
        }
    }

    // Helper method to display all values of a DataRow
    public void DisplayAllValues(DataRow row)
    {
        foreach (var column in row.ItemArray)
        {
            Console.Write(column + "\t");
        }
        Console.WriteLine();  // New line after each row for clarity
    }

}
