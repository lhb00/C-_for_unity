using System;
using System.Collections;
using System.Collections.Generic;
namespace Exercise5
{
    class Program
    {
        public static int count = 0;

        static void Main(string[] args)
        {

            Console.Write("입력 : ");
            string input = Console.ReadLine();

            Queue qu = new Queue();
            Stack st = new Stack();

            for (int i = 0; i < input.Length; i++)
            {
                qu.Enqueue(input[i]);
                st.Push(input[i]);
            }

            ArrayList al1 = new ArrayList();
            ArrayList al2 = new ArrayList();

            while (qu.Count > 0 && st.Count > 0)
            {
                al1.Add(qu.Dequeue());
                al2.Add(st.Pop());
            }

            for(int j = 0; j < input.Length; j++)
            {
                if (al1[j].Equals(al2[j]))
                    count++;
            }

            if (count == input.Length)
                Console.WriteLine("출력 : 참(True)");
            else
                Console.WriteLine("출력 : 거짓(False)");

        }
    }
}
