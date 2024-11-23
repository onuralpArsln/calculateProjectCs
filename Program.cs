class Program
{
	static void Main(string[] args)
	{
		var parser = new HydrostaticTableParser("hydrostatics.txt");
		parser.detectLines();
		Console.WriteLine("getrting lines 28 31 ");
		parser.retrieveLinesByNumber(28);
		parser.retrieveLinesByNumber(31);
		Console.WriteLine("getrting lines 299 300 301 302");

		parser.retrieveLinesByNumber(299);
		parser.retrieveLinesByNumber(300);
		parser.retrieveLinesByNumber(301);
		parser.retrieveLinesByNumber(302);




	}
}
