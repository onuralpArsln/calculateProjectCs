class Program
{
	static void Main(string[] args)
	{
		TableGenerator testTable = new TableGenerator("hydrostatics.txt");
		//testTable.testTable();
		testTable.lookForTrimAndDraft(-3.20, 2.10);


	}
}
