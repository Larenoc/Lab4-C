using System;
using System.Threading;

class Program
{
    public static Mutex[] forks;
    static Philosopher[] philosophers;

    static void Main(string[] args)
    {
        int numberOfPhilosophers = 5;
        forks = new Mutex[numberOfPhilosophers];
        philosophers = new Philosopher[numberOfPhilosophers];

        for (int i = 0; i < numberOfPhilosophers; i++)
        {
            forks[i] = new Mutex();
        }

        for (int i = 0; i < numberOfPhilosophers; i++)
        {
            philosophers[i] = new Philosopher(i);
            Thread philosopherThread = new Thread(philosophers[i].Dine);
            philosopherThread.Start();
        }
    }
}

class Philosopher
{
    private int id;
    private static Random random = new Random();

    public Philosopher(int id)
    {
        this.id = id;
    }

    public void Dine()
    {
        while (true)
        {
            // Thinking
            Thread.Sleep(random.Next(1000, 2000));

            int fork1 = id;
            int fork2 = (id + 1) % Program.forks.Length;

            // Trying to pick up the first fork
            Program.forks[fork1].WaitOne();

            // Trying to pick up the second fork
            if (Program.forks[fork2].WaitOne(1000)) // Wait for 1 second to pick up the second fork
            {
                // Eating
                Console.WriteLine("Philosopher " + id + " is eating");

                // Release the forks
                Program.forks[fork1].ReleaseMutex();
                Program.forks[fork2].ReleaseMutex();

                // Pause between meals
                Thread.Sleep(random.Next(1000, 2000));
            }
            else
            {
                // Release the first fork if the second fork is not available
                Program.forks[fork1].ReleaseMutex();
            }
        }
    }
}







