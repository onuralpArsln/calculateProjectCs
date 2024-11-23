using System;

class Program
{

	static void Main(string[] args)
	{

		string filePath = "hydrostatics.txt";
		string searchTerm = "Trim Draft Displt LCB TCB VCB WPA LCF KML KMT BML BMT IL IT TPC MTC WSA"; // Replace with your search term

		try
		{
			if (File.Exists(filePath))
			{
				using (StreamReader reader = new StreamReader(filePath))
				{
					string line;
					int lineNumber = 0;

					while ((line = reader.ReadLine()) != null)
					{
						lineNumber++;

						// Check if the line contains the search term
						if (line.Contains(searchTerm))
						{
							Console.WriteLine($"Line {lineNumber}: {line}");
						}
					}
				}
			}
			else
			{
				Console.WriteLine("File not found.");
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"An error occurred: {ex.Message}");
		}


	}
}
