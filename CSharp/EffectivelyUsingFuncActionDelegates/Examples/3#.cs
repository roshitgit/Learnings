public static int Count<T>(T[] arr, Predicate<T> condition)
{
    int counter = 0;
    for (int i = 0; i < arr.Length; i++)
        if (condition(arr[i]))
            counter++;
    return counter;
}

Predicate<string> longWords = delegate(string word) { return word.Length > 10; };
int numberOfBooksWithLongNames = Count(words, longWords);
int numberOfCheapbooks = Count(books, delegate(Book b) { return b.Price< 20; });
int numberOfNegativeNumbers = Count(numbers, x => x < 0);
int numberOfEmptyBookTitles = Count(words, String.IsNullOrEmpty);


You can use functional programming to replace some standard C# constructions. One typical example is using block shown in the following listing: 

Hide   Copy Code
using (obj)
{
     obj.DoAction();
}
Using block is applied on the disposable objects. In the using block you can work with the object, call some of the methods etc. When using  block ends, object will be disposed. Instead of the using block you can create your own function that will warp the action that will be applied on the object.

Hide   Copy Code
public static void Use<T>(this T obj, Action<T> action) where T : IDisposable
{
       using (obj)
       {
                action(obj);
       }
}  
Here is created one extension method that accepts action that should be executed and wrap call in the using block. Example of usage is shown in the following code:

Hide   Copy Code
obj.Use(  x=> x.DoAction(); );
