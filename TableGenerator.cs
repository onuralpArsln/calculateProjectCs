using System;
using System.Data;
using System.IO;

class TableGenerator
{
    string filePath;
    DataTable table = new DataTable();


    public TableGenerator(string filePath)
    {
        // File path to your .txt file
        this.filePath = filePath;



        // Column names
        string[] columnNames = new string[]
        {
            "Trim", "Draft", "Displt", "LCB", "TCB", "VCB", "WPA", "LCF", "KML", "KMT", "BML", "BMT", "IL", "IT", "TPC", "MTC", "WSA"
        };

        // Add columns to DataTable
        foreach (var column in columnNames)
        {
            this.table.Columns.Add(column);
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
                        // Try to parse each value as a double
                        if (!double.TryParse(values[i], out _))
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
                            row[i] = values[i];
                        }
                        this.table.Rows.Add(row);
                        lineCount++;
                    }
                }
            }
        }


    }


    public void testTable()
    {
        Console.WriteLine(this.table.Rows.Count);
        // Display every 80th row in the DataTable
        for (int i = 200; i < this.table.Rows.Count; i += 200)
        {
            DataRow row = table.Rows[i];
            foreach (var item in row.ItemArray)
            {
                Console.Write(item + "\t");
            }
            Console.WriteLine();
        }
    }


    public void lookForTrim(double givenTrim)
    {

        DataRow trimLowerBound = null;
        DataRow trimUpperBound = null;

        trimLowerBound = table.AsEnumerable()
                        .Where(r => r.Field<double>("Trim") <= givenTrim)
                        .OrderByDescending(r => r.Field<double>("Trim"))
                        .FirstOrDefault();


        trimUpperBound = this.table.AsEnumerable()
                        .Where(r => r.Field<double>("Trim") >= givenTrim)
                        .OrderBy(r => r.Field<double>("Trim"))
                        .FirstOrDefault();

        Console.WriteLine("lower bound is " + trimLowerBound + "upper" + trimUpperBound);
    }

}
