public static class DataProcessor
{
    public static async Task<int> ProcessDataAsync(string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        await Task.Delay(2000);
        return input.Length;
    }

    public static async Task<string> TransformDataAsync(string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        await Task.Delay(3000);
        return input.ToUpper();
    }
}
/*class Program
{
    static async Task Main(string[] args)
    {
        var strings = new List<string> { "apple", "sea", "clouds" };
        var stopwatch = Stopwatch.StartNew();

        foreach (var item in strings)
        {
            
            var processTask = DataProcessor.ProcessDataAsync(item);
            var transformTask = DataProcessor.TransformDataAsync(item);

            await Task.WhenAll(processTask, transformTask);

            int processResult = await processTask;
            string transformResult = await transformTask;

            Console.WriteLine($"Обработка строки '{item}':");
            Console.WriteLine($" Длина: {processResult}");
            Console.WriteLine($"  Преобразование: {transformResult}");
        }

        stopwatch.Stop();
        Console.WriteLine($"Общее время: {stopwatch.ElapsedMilliseconds / 1000.0:F2} секунд");
        Console.ReadLine();
    }
}*/




public static Dictionary<string, Type> GetPropertyTypes(object obj)
{
    if (obj == null)
        throw new ArgumentNullException(nameof(obj));

    var result = new Dictionary<string, Type>();
    var properties = obj.GetType().GetProperties();

    foreach (var prop in properties)
    {
        result.Add(prop.Name, prop.PropertyType);
    }

    return result;
}


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class MaxLengthAttribute : Attribute
{
    public int MaxLength { get; }

    public MaxLengthAttribute(int maxLength)
    {
        if (maxLength <= 0)
            throw new ArgumentException("MaxLength must be positive.", nameof(maxLength));
        MaxLength = maxLength;
    }
}

public class Person
{
    [MaxLength(5)]
    public string Name { get; set; }
}

public static class Validator
{
    public static void Validate(object obj)
    {
        if (obj == null) throw new ArgumentNullException(nameof(obj));

        var properties = obj.GetType().GetProperties();
        foreach (var prop in properties)
        {
            var maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null && prop.PropertyType == typeof(string))
            {
                var value = (string)prop.GetValue(obj);
                if (value != null && value.Length > maxLengthAttr.MaxLength)
                {
                    throw new InvalidOperationException(
                        $"Property {prop.Name} exceeds max length of {maxLengthAttr.MaxLength}.");
                }
            }
        }
    }
}


public static class CollectionExtensions
{
    public static IEnumerable<T> Filter<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        return source.Where(predicate);
    }

    public static IEnumerable<TResult> Transform<T, TResult>(this IEnumerable<T> source, Func<T, TResult> transformer)
    {
        return source.Select(transformer);
    }
    //TopN
    public static IEnumerable<T> TopN<T>(this IEnumerable<T> source, int count, Func<T, IComparable> keySelector)
    {
       
        if (source == null) throw new ArgumentNullException(nameof(source));
        if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count cannot be negative");

        return source.OrderByDescending(keySelector).Take(count);
    }

}

public class CustomList<T> : IEnumerable<T>
{
    private T[] _items;
    private int _count;

    public CustomList(int capacity = 4)
    {
        _items = new T[capacity];
        _count = 0;
    }

    public void Add(T item)
    {
        if (_count == _items.Length)
        {
            Array.Resize(ref _items, _items.Length * 2);
        }
        _items[_count] = item;
        _count++;
    }

    public bool Remove(T item)
    {
        int index = Array.IndexOf(_items, item, 0, _count);
        if (index >= 0)
        {
            Array.Copy(_items, index + 1, _items, index, _count - index - 1);
            _items[_count - 1] = default(T);
            _count--;
            return true;
        }
        return false;
    }

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException();
            }
            return _items[index];
        }
        set
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException();
            }
            _items[index] = value;
        }
    }

    public int Count
    {
        get { return _count; }
    }

    public bool Contains(T item)
    {
        return Array.IndexOf(_items, item, 0, _count) >= 0;
    }

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < _count; i++)
        {
            yield return _items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public class Stack<T>
{
    protected List<T> items = new List<T>();

    public void Push(T item)
    {

        items.Add(item);
    }

    public T Pop()
    {
        if (items.Count==0)
        {
            throw new InvalidOperationException();
        }
        var first = items.Last();
        items.RemoveAt(items.Count-1);
        return first;
    }

    public T Peek()
    {
        if (items.Count==0)
        {
            throw new InvalidOperationException();
        }
        return items.Last();
    }
}

public class PriorityQueue<T>
{
    private List<KeyValuePair<int, T>> items = new List<KeyValuePair<int, T>>();

    public void Enqueue(T item, int priority)
    {
        items.Add(new KeyValuePair<int, T>(priority, item));
    }

    public T Dequeue()
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("Очередь пуста");
        }

        
        var maxPriorityItem = items.OrderBy(x => x.Key).First();
        items.Remove(maxPriorityItem);
        return maxPriorityItem.Value;
    }

    public T Peek()
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("Очередь пуста");
        }

        return items.OrderBy(x => x.Key).First().Value;
    }
}


public class SortedRepository<T> where T : IComparable<T>
{
    private List<T> items = new List<T>();

    public void Add(T item)
    {
        items.Add(item);
        items.Sort();
    }

    public IEnumerable<T> GetAll()
    {
        return items.AsReadOnly();
    }

    public T FindMax()
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Репозиторий пуст");
        return items.Max();
    }
}

class Program
{
    static void Main(string[] args)
    {
        SortedRepository<int> intRepo = new SortedRepository<int>();
        intRepo.Add(3);
        intRepo.Add(1);
        intRepo.Add(2);

        var allElements = intRepo.GetAll();
        Console.WriteLine("Элементы в репозитории чисел");
        foreach (var item in allElements)
        {
            Console.WriteLine(item);
        }
        var maxEl = intRepo.FindMax();
        Console.WriteLine($"Максимальный элемент в репозитории чисел: {maxEl}");

        SortedRepository<string> stringRepo = new SortedRepository<string>();
        stringRepo.Add("Banana");
        stringRepo.Add("Apple");
        stringRepo.Add("Cherry");

        var allElementsString = stringRepo.GetAll();
        Console.WriteLine("Элементы в репозитории строк");
        foreach (var item in allElementsString)
        {
            Console.WriteLine(item);
        }
        var maxElString = stringRepo.FindMax();
        Console.WriteLine($"Максимальный элемент в репозитории строк: {maxElString}");

        Console.ReadLine();
    }
}

/*public class Repository<T>
{
    private List<T> values = new List<T>();

    public void Add(T value)
    {
        values.Add(value);
    }

    public void Remove(T item)
    {
        values.Remove(item);
    }

    public IEnumerable<T> GetAll()
    {
        return values.AsReadOnly();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Repository<int> repositoryInt = new Repository<int>();
        repositoryInt.Add(1);
        repositoryInt.Add(2);
        Console.WriteLine("Числовой репозиторий:");
        foreach (var item in repositoryInt.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }
        repositoryInt.Remove(1);
        Console.WriteLine("После удаления:");
        foreach (var item in repositoryInt.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }

        Repository<string> repositoryString = new Repository<string>();
        repositoryString.Add("Hello");
        repositoryString.Add("World");
        Console.WriteLine("\nСтроковый репозиторий:");
        foreach (var item in repositoryString.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }
        repositoryString.Remove("World");
        Console.WriteLine("После удаления:");
        foreach (var item in repositoryString.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }
    }
}*/




/*internal class Program
{
    private static async Task Main(string[] args)
    {

//1 задание
        await AsyncWork.WaitAsync(3);


        //

        //2 задание
        var tasks = new[] { AsyncWork.PrintNumberAsync(1), AsyncWork.PrintNumberAsync(2), AsyncWork.PrintNumberAsync(3) };
        var results = await Task.WhenAll(tasks);
        foreach (var res in results) Console.WriteLine(res);
        //

        //3 задание

public class Repository<T>
{
    private List<T> values = new List<T>();

    public void Add(T value)
    {
        values.Add(value);
    }

    public void Remove(T item)
    {
        values.Remove(item);
    }

    public IEnumerable<T> GetAll()
    {
        return values;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Repository<int> repositoryInt = new Repository<int>();
        repositoryInt.Add(1);
        repositoryInt.Add(2);
        Console.WriteLine("Числовой репозиторий:");
        foreach (var item in repositoryInt.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }
        repositoryInt.Remove(1);
        Console.WriteLine("После удаления:");
        foreach (var item in repositoryInt.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }

        Repository<string> repositoryString = new Repository<string>();
        repositoryString.Add("Hello");
        repositoryString.Add("World");
        Console.WriteLine("\nСтроковый репозиторий:");
        foreach (var item in repositoryString.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }
        repositoryString.Remove("World");
        Console.WriteLine("После удаления:");
        foreach (var item in repositoryString.GetAll())
        {
            Console.WriteLine($"Элемент: {item}");
        }
    }
}        var three = AsyncWork.CalculateDivideAsync(5,10);

        Console.WriteLine(three.Result);

        //4 задание

        AsyncWork.FourthTask("Install packages");


        //5 задание
        await AsyncWork.ProcessStepAsync(1);
        await AsyncWork.ProcessStepAsync(2);
        await AsyncWork.ProcessStepAsync(3);

        //6 задание

        var cts = new CancellationTokenSource();
        cts.CancelAfter(200);

        await AsyncWork.LongRunningTaskAsync(100,cts.Token);

        //7 задание

        AsyncWork.ProcessItemAsync(new[] { 1, 2, 3, 4, 5 });

        //8 задание

        var Z8task1 = Task.Run(() => AsyncWork.RiskyOperationAsync(1));
        var Z8task2 = Task.Run(() => AsyncWork.RiskyOperationAsync(2));
        var Z8task3 = Task.Run(() => AsyncWork.RiskyOperationAsync(3));

        try
        {
            await Task.WhenAll(Z8task1, Z8task2, Z8task3);
            Console.WriteLine("Все Z8Task отработали, исключения пойманы");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            await Task.WhenAll(Z8task1, Z8task3);
            Console.WriteLine("Отработали таски");
        }

        //9 задание
        Console.WriteLine(AsyncWork.GetValueAsync(5).Result);

        Console.WriteLine(AsyncWork.GetValueAsync(0).Result);

        //10 задание


        await foreach (var num in AsyncWork.GetNums())
        {
            Console.WriteLine(num);
        }


        Console.ReadLine();
    }
}

static class AsyncWork
{
    //1 задание
    public static async Task WaitAsync(int sec)
    {
        await Task.Delay(sec * 1000);

        Console.WriteLine($"Ожидали {sec} секунд");
    }

    //2 задание

    public static async Task<int> PrintNumberAsync(int number)
    {
        await Task.Delay(2000);
        return number;
    }

    //3 задание

    public static async Task<int> CalculateDivideAsync(int a, int b)
    {
        await Task.Delay(100);
        return a + b;
    }


    //Задание 4



    public static async Task FourthTask(string fileName)
    {
        var z4Task1 = Task.Run(() => AsyncWork.RandomInt());
        var z4Task2 = Task.Run(() => AsyncWork.RandomInt());
        var z4Task3 = Task.Run(() => AsyncWork.RandomInt());

        await Task.WhenAll(z4Task1, z4Task2, z4Task3);


        Console.WriteLine($"File name : {fileName}. Size = {z4Task1.Result + z4Task2.Result + z4Task3.Result}");

    }


    private static int RandomInt()
    {
        Random random = new Random();
        return random.Next(100,1000);
    }

    //5 задание

    public static async Task ProcessStepAsync(int step)
    {
        await Task.Delay(3000);

        Console.WriteLine($"Step {step} completed");

    }

    //6 задание

    public static async Task LongRunningTaskAsync(int seconds, CancellationToken token)
    {
        for (int i = 0; i < seconds; i++)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Операция отменена внутри задачи");
                return;
            }
            Console.WriteLine($"Выполняется... {i + 1}/{seconds}");
            await Task.Delay(25);
        }
        Console.WriteLine("Задача завершена");
    }

    //7 задание

    public static async Task ProcessItemAsync(int[] ints)
    {
        foreach (var items in ints)
        {
            await Task.Delay(1000);
            Console.WriteLine(items);
        }
    }

    //8 задание

    public static async Task RiskyOperationAsync(int id)
    {
        await Task.Delay(5000);//я сделал это для удобства вывода в консоль по порядку.
        if (id%2==0)
        {
            throw new Exception(); 
        }

    }

    //9 задание

    public static ValueTask<int> GetValueAsync(int value)
    {
        if (value>0)
            return new ValueTask<int>(value);

        return new ValueTask<int>(TimerFor9z());
 
    }

    static async Task<int> TimerFor9z()
    {
        await Task.Delay(1000);
        return -1;
    }

    //10 задание

    public static async IAsyncEnumerable<int> GetNums()
    {
        for (int i = 0; i <= 5; i++)
        {
            await Task.Delay(1000);
            yield return i;
        }
    }

}*/


