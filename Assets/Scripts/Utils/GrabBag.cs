using System.Collections.Generic;

// Grab bag randomizer class

// A bag that holds one of each integer from 0 to 99 <default>
// Numbers can then be drawn from the bag
// The min and max numbers can be set
// Multiple sets and ranges of numbers can be added to the bag

public class GrabBag  
{
	// Bag to hold numbers
	List<int> bagList = new List<int> ();

	// Default values to fill the bag with
	int _Min = 0;
	int _Max = 99;
	int _Dups = 1;

	// ENCAPSULATION
    public int Min  { get => _Min;  set => _Min  = value; }
	public int Max  { get => _Max;  set => _Max  = value; }
    public int Dups { get => _Dups; set => _Dups = value; }

    //public int 	getMin()		{ return Min; }
	//public void 	setMin(int x) 	{ Min = x; }
	//public int 	getMax()		{ return Max; }
	//public void 	setMax(int x) 	{ Max = x; }
	//public int 	getDups()		{ return Dups; }
	//public void 	setDups(int x)	{ Dups = x; }
	public int 		getSize()		{ return bagList.Count;	}
	public void 	emptyBag()		{ bagList.Clear ();	}

	public string	showBag()
	{
		string retVal = "";

		if (bagList.Count == 0)	
			return "EMPTY";

		for (int i = 0; i < bagList.Count; i++) 
		{
			if (i > 0) retVal += ",";

			retVal += bagList [i].ToString ();
		}

		return retVal;
	}

	// Bag manipulation methods

	// Default method to fill an empty bag with numbers using the defaults
	public void fillBag()
	{
		emptyBag ();
		addRange ();
	}

	// Routine to shuffle the bag contents
	public void shakeBag()
	{
		RandomizeList (bagList);
	}

	// POLYMORPHISM - overloading addRange()
	// Default method to add the default range to the existing contents of the bag
	public void addRange()
	{
		addRange (_Min, _Max, _Dups);
	}

	// Method to add to the bag the range from 'min' to 'max' in multiples of 'duplicates'
	// Do nothing if 'min' is greater than 'max'
	public void addRange(int min, int max, int duplicates)
	{
		if (min > max) return;

		for (int i = 0; i < duplicates; i++) 
			for (int j = min; j <= max; j++) 
				bagList.Insert(UnityEngine.Random.Range(0,bagList.Count), j);
	}

	// Method to add the specified number 'n' to the bag 'count' number of times
	public void addNumber(int n, int count)
	{
		for (int i = 0; i < count; i++) 
			bagList.Insert(UnityEngine.Random.Range(0,bagList.Count), n);	
	}

	// Method to remove from the bag all accounts of the specified number 'n'
	// Returns the count of 'n's removed
	public int removeAllofNumber(int n)
	{
		int initialCount = bagList.Count;

		bagList.RemoveAll (i => i == n);

		return initialCount - bagList.Count;
	}

	// Method to remove a random number from the bag and return it.
	// If the bag is empty first fill it with numbers using the default values.
	public int getRndNumber()
	{
		if (bagList.Count == 0) 
			addRange ();

		int i = UnityEngine.Random.Range (0, bagList.Count);
		int retVal = bagList [i];

		bagList.RemoveAt(i);

		return retVal;
	}


	public List<T> RandomizeList<T>(List<T> list)
	{
		List<T> randomizedList = new List<T> ();
		while (list.Count > 0) 
		{

			int index = UnityEngine.Random.Range (0, list.Count);
			randomizedList.Add (list [index]);
			list.RemoveAt (index);

		}
		return randomizedList;
	}
}
