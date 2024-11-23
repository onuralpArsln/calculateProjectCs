class Program
{
	static void Main(string[] args)
	{
		TableGenerator testTable = new TableGenerator("hydrostatics.txt");
		testTable.testTable();
		testTable.lookForTrim(-3.20);


	}
}
