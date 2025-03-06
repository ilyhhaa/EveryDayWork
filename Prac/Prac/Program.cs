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

        var three = AsyncWork.CalculateDivideAsync(5,10);

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


