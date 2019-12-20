using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizationTester
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine($"Welcome to the optimization tester! Give a function and a size to run that function, ? to list functions, x to exit");

      var keepGoing = true;

      while(keepGoing)
      {
        var input = Console.ReadLine();
        var words = input.Split(' ');
        var steps = 0;

        switch(words[0])
        {
          case "x":
            keepGoing = false;
            break;
          case "dupes":
            var size = int.Parse(words[1]);
            var data = GenerateRandomList(size);
            var result = CountDuplicatedItems(data, ref steps);
            Console.WriteLine($"Found {result} duplicated items in {steps} steps");
            break;
          case "maxdb":
            size = int.Parse(words[1]);
            var max = FetchMaxFromDatabase(size);
            Console.WriteLine($"found maximum: {max}");
            break;
          case "matchleads":
            var people = People;
            MatchLeads(people, Leads);
            foreach(var person in people)
            {
              Console.WriteLine(person);
            }
            break;
        }
      }

      Console.WriteLine("goodbye!");
      Console.ReadKey();
    }

    private static int CountDuplicatedItems(List<int> items, ref int steps)
    {
      var duplicateItems = new List<int>();
      var totalMatches = 0;
      foreach (var i in items)
      {
        if(!duplicateItems.Contains(i))
        {
          var foundSelf = false;
          foreach (var j in items)
          {
            if(j == i)
            {
              if (!foundSelf)
              {
                foundSelf = true;
              }
              else
              {
                totalMatches++;
                if (!duplicateItems.Contains(i)) duplicateItems.Add(i);
              }
            }
            steps++;
          }
        }
      }

      return totalMatches;
    }


    private static int FetchMaxFromDatabase(int totalItems)
    {
      var results = new List<int>();
      for(var i = 0; i < totalItems; i++)
      {
        results.Add(FakeDbFetch(() => new Random().Next()).GetAwaiter().GetResult());
      }

      return results.Max();
    }

    private static void MatchLeads(List<FakePerson> people, List<FakeLead> leads)
    {
      foreach(var person in people)
      {
        person.Leads = leads.Where(l => l.Name == person.Name || l.Email == person.Email).ToList();
      }
    }

    //UTIL STUFF

    private static List<int> GenerateRandomList(int length)
    {
      var result = new List<int>();
      var random = new Random();

      for(var i = 0; i < length; i++)
      {
        result.Add(random.Next() % 10000);
      }

      return result;
    }

    //given a lambda which returns T, just await a random time between 0 and 1 seconds, then return T

    private static async Task<T> FakeDbFetch<T>(Func<T> generator)
    {
      var randomInt = new Random().Next();

      await Task.Delay(randomInt % 1000);

      return generator();
    }

    private static List<FakePerson> People
      => new List<FakePerson>
      {
        new FakePerson(0, "Sleve McDichael", "s.mcdichael@aol.com"),
        new FakePerson(1, "Darryl Archideld", "d.archideld@hotmail.com"),
        new FakePerson(2, "Dean Wesrey", "leanmeandean@verizon.net"),
        new FakePerson(3, "Todd Bonzalez", "hotrodtodd@aol.com"),
        new FakePerson(4, "Bobson Dugnutt", "bs@bs.com"),
      };

    private static List<FakeLead> Leads
      => new List<FakeLead>
      {
        new FakeLead(0, "", "wrongemail@fake.com"),
        new FakeLead(1, "Darryl Archideld", ""),
        new FakeLead(2, "Mack Jueller", null),
        new FakeLead(3, "", "hotrodtodd@aol.com"),
        new FakeLead(4, "Bobson Dugnutt", "bs@bs.com"),
        new FakeLead(5, "Todd Bonzalez", "hotrodtodd@aol.com"),
        new FakeLead(6, "Darryl Archideld", "wrongemail@fake.com"),
        new FakeLead(7, "Kim Taetzel", "xXx_the_emoest_tpot_xXx@myspace.com"),
        new FakeLead(8, "Bobson Dugnutt", "bs@bs.com"),
        new FakeLead(9, "Radam Ozanski", "bunnyman@pardioc.net"),
        new FakeLead(10, "", "s.mcdichael@aol.com"),
        new FakeLead(11, "Sleve McDichael", "sleve@gmail.com"),
        new FakeLead(12, null, "test@test.com"),
        new FakeLead(13, "Dean Wesrey", "leanmeandean@veirzon.net"),
        new FakeLead(14, "", "bs@bs.com"),
        new FakeLead(15, "Bobson Dugnutt", "wrongemail@fake.com"),
        new FakeLead(16, "Todd Bonzalez", "hotrodtodd@aol.com"),
        new FakeLead(17, "Glenallen Mixon", ""),
        new FakeLead(18, "Darryl Archideld", "d.archideld@hotmail.com"),
        new FakeLead(19, "Tony Smehrik", "tony@smehrikplumbing.net"),
      };
  }


  public class FakePerson
  {
    public int Id;
    public string Name;
    public string Email;

    public FakePerson(int id, string name, string email)
    {
      Id = id;
      Name = name;
      Email = email;
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.AppendLine($"{Id} | {Name} | {Email}");
      foreach(var lead in Leads)
      {
        sb.AppendLine($"- {lead}");
      }

      return sb.ToString();
    }

    public List<FakeLead> Leads;
  }

  public class FakeLead
  {
    public int LeadId;
    public string Name;
    public string Email;

    public FakeLead(int leadId, string name, string email)
    {
      LeadId = leadId;
      Name = name;
      Email = email;
    }

    public override string ToString() => $"{LeadId} | {Name} | {Email}";
  }
}
