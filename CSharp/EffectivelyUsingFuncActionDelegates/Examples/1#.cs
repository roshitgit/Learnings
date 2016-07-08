To implement a similar map method in C#, which operates on a List:

delegate T GenericDelegate< T >( T args );

static List< T > Map< T >( List< T > inList, GenericDelegate f )
{   
  List< T > outList = new List< T >();
  for ( int i = 0; i < inList.Count(); i++ ) {
    outList.Add( f( inList[i] ) );
  }
  return outList;
}

List< int > test = new List< int >(){1, 2, 3, 4, 5};
List< int > doubleTest = Map( test, ( i ) => { return 2 * i; } );
doubleTest.ForEach( ( i ) => { Console.Write( i + " " ); } );
//2 4 6 8 10 
Action and Func
These are just syntactic sugar in C# for generic delegates.
We could use this in place of the generic delegate we used in the previous section:

delegate TResult Func< in T, out TResult >(T arg);
So we could implement the map method using a Func overload as:

static List< T > MapWithFunc< T >( List< T > inList, Func< T, T > f )
{
  List< T > outList = new List< T >();
  for ( int i = 0; i < inList.Count(); i++ ) {
    outList.Add( f( inList[i] ) );
  }
  return outList;
}

List< int > test = new List< int >(){1, 2, 3, 4, 5};
List< int > doubleTest = MapWithFunc< int >( test, ( i ) => { return 2 * i; } );
doubleTest.ForEach( ( i ) => { Console.Write( i + " " ); } );
//2 4 6 8 10 
