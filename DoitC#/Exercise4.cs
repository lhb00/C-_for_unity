using System;
// 미니 퀴즈
class Cat
{
    public String Name = null;

    public Cat(string name)
    {
        Name = name;
        Console.WriteLine("고양이의 이름은 " + Name + "입니다.");
    }
}
// 도전 코딩
class Car
{
    public string car = "자동차";
    private string name = null;
    private int perHour = 0;

    public void SetName(string name)
    {
        this.name = name;
    }

    public string GetName()
    {
        return this.name;
    }

    public void SetperHour(int perHour)
    {
        this.perHour = perHour;
    }

    public int GetperHour()
    {
        return this.perHour;
    }

    public void forward()
    {
        Console.WriteLine(car + "가 전진합니다.");
    }

    public void rear()
    {
        Console.WriteLine(car + "가 후진합니다.");
    }

    public void rightTurn()
    {
        Console.WriteLine(car + "가 우회전합니다.");
    }

    public void leftTurn()
    {
        Console.WriteLine(car + "가 좌회전합니다.");
    }

    public void stop()
    {
        Console.WriteLine(car + "가 멈춥니다.");
    }
}

class Exercise4
{
    public static void Main(String[] args)
    {
        Cat lynx = new Cat("시라소니");

        Car sonata = new Car();
        sonata.SetName("소나타");
        sonata.SetperHour(80);
        Console.WriteLine(sonata.car + "의 이름은 " + sonata.GetName() + "입니다.");
        sonata.forward();
        Console.WriteLine(sonata.car + "의 속도는 시속 " + sonata.GetperHour() + "km입니다.");
        sonata.rightTurn();
        sonata.SetperHour(20);
        Console.WriteLine(sonata.car + "의 속도는 시속 " + sonata.GetperHour() + "km입니다.");
        sonata.leftTurn();
        sonata.rear();
        sonata.stop();
    }
}
