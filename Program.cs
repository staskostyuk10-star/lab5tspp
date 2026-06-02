using System;
using System.Collections.Generic;

// 1 (Singleton)
class CacheManager {

    private static readonly CacheManager _inst = new CacheManager(); 
    public Dictionary<string, string> Cache = new();
    private CacheManager() {}
    public static CacheManager Instance => _inst;
}


// 2 (Adapter)
interface IDatabase { void Connect(); }

class MySQL { public void ConnMySQL() => Console.WriteLine("MySQL підключено"); }
class PGSQL { public void ConnPG() => Console.WriteLine("PostgreSQL підключено"); }
class SQLite { public void ConnSQ() => Console.WriteLine("SQLite підключено"); }

class MySQLAdapter : IDatabase {
    MySQL db = new MySQL();
    public void Connect() => db.ConnMySQL();
}
class PGAdapter : IDatabase {
    PGSQL db = new PGSQL();
    public void Connect() => db.ConnPG();
}
class SQLiteAdapter : IDatabase {
    SQLite db = new SQLite();
    public void Connect() => db.ConnSQ();
}
// 3 (Observer)
interface IObserver { void Update(string title); }

class User : IObserver {
    string name;
    public User(string n) => name = n;
    public void Update(string t) => Console.WriteLine($"{name} отримав сповіщення: {t}");
}

class Blog {
    List<IObserver> subs = new();
    public void Subscribe(IObserver o) => subs.Add(o);
    public void Publish(string title) {
        Console.WriteLine($"\nНова стаття: {title}");
        foreach (var s in subs) s.Update(title);
    }
}


class Program {
    static void Main() {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        
        var cache1 = CacheManager.Instance; 
        cache1.Cache["user"] = "admin";
        var cache2 = CacheManager.Instance; 
        Console.WriteLine($"З кешу: {cache2.Cache["user"]}\n");

      
        IDatabase[] dbs = { new MySQLAdapter(), new PGAdapter(), new SQLiteAdapter() };
        foreach(var db in dbs) db.Connect();

        
        var blog = new Blog();
        blog.Subscribe(new User("Іван"));
        blog.Subscribe(new User("Марія"));
        blog.Publish("Як вивчити C# за ніч");
    }
}
