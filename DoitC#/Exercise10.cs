using System;

class MainClass
{
    static void PrintSum<T>(T[] values)
    {
        T sum = default(T);
        foreach (T item in values)
        {
            sum += (dynamic)item;
        }

        Console.WriteLine(sum);
    }

    public static void Main(string[] args)
    {
        Console.Write("정수(1)/실수(2) 선택 : ");
        int num = int.Parse(Console.ReadLine());

        switch (num)
        {
            case 1:
                Console.Write("정수 배열의 입력 : ");
                string tempArray1 = Console.ReadLine();
                tempArray1 = tempArray1.Replace("[", "");
                tempArray1 = tempArray1.Replace("]", "");
                string[] tArray = tempArray1.Split(",");
                int[] intArr = Array.ConvertAll(tArray, int.Parse);
                Console.Write("출력 : ");
                PrintSum <int> (intArr);
                break;
            case 2:
                Console.Write("실수 배열의 입력 : ");
                string tempArray2 = Console.ReadLine();
                tempArray2 = tempArray2.Replace("[", "");
                tempArray2 = tempArray2.Replace("]", "");
                string[] tArray2 = tempArray2.Split(",");
                double[] doubleArr = Array.ConvertAll(tArray2, double.Parse);
                Console.Write("출력 : ");
                PrintSum<double>(doubleArr);
                break;
        }

    }

}