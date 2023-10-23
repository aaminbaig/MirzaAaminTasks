class Calculator
{

    static void Main(string[] args)
    {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("1) Addition");
        Console.WriteLine("2) Subtraction");
        Console.WriteLine("3) Multiplication");
        Console.WriteLine("4) Division");
        Console.WriteLine("5) Exit");

        Console.Write("\r\nSelect a number: ");


        switch (Console.ReadLine())
        {
            case "1":
                Console.Write("\r\nEnter first number: ");
                int num1 = Int32.Parse(Console.ReadLine());

                Console.Write("\r\nEnter second number: ");
                int num2 = Int32.Parse(Console.ReadLine());

                int sum = Calculations.Addition(num1, num2);
                Console.WriteLine(sum);
                break;

            case "2":
                Console.Write("\r\nEnter first number: ");
                int num3 = Int32.Parse(Console.ReadLine());

                Console.Write("\r\nEnter second number: ");
                int num4 = Int32.Parse(Console.ReadLine());

                int sub = Calculations.Subtraction(num3, num4);
                Console.WriteLine(sub);
                break;

            case "3":
                Console.Write("\r\nEnter first number: ");
                int num5 = Int32.Parse(Console.ReadLine());

                Console.Write("\r\nEnter second number: ");
                int num6 = Int32.Parse(Console.ReadLine());

                int mul = Calculations.Multiplication(num5, num6);
                Console.WriteLine(mul);
                break;

            case "4":
                Console.Write("\r\nEnter first number: ");
                int num7 = Int32.Parse(Console.ReadLine());

                Console.Write("\r\nEnter second number: ");
                int num8 = Int32.Parse(Console.ReadLine());

                int div = Calculations.Division(num7, num8);
                Console.WriteLine(div);
                break;

            case "5":
                Console.WriteLine("Thank you, bye bye");
                break;

            default:
                Console.WriteLine("Wooops, not an option. Bye");
                break;


        }
    }
}

public static class Calculations
{
    static public int Addition(int num1, int num2)
    {
        int sum = num1 + num2;

        return sum;
    }

    static public int Subtraction(int num1, int num2)
    {
        int diff = num1 - num2;

        return diff;
    }

    static public int Multiplication(int num1, int num2)
    {
        int count = 0;

        for (int i = 0; i < num2; i++)
        {
            count += num1;
        }

        return count;
    }

    static public int Division(int num1, int num2)
    {
        if (num2 == 0)
        {
            throw new DivideByZeroException("Genius, what were you thinking dividing it by zero?");
        }

        num1 = Math.Abs(num1);
        num2 = Math.Abs(num2);

        int num3 = 0;

        while (num1 >= num2)
        {
            num1 -= num2;
            num3 += 1;
        }

        return num3;
    }

}