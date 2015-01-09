Link: http://programmers.stackexchange.com/questions/177649/what-is-constructor-injection


//Let’s examine the idea of dependency injection by walking through a simple example. Let’s say you’re writing the next blockbuster game, where noble warriors do battle for great glory. First, we’ll need a weapon suitable for arming our warriors.

class Sword 
{
    public void Hit(string target)
    {
        Console.WriteLine("Chopped {0} clean in half", target);
    }
}
//Then, let’s create a class to represent our warriors themselves. In order to attack its foes, the warrior will need an Attack() method. When this method is called, it should use its Sword to strike its opponent.

class Samurai
{
    readonly Sword sword;
    public Samurai() 
    {
        this.sword = new Sword();
    }

    public void Attack(string target)
    {
        this.sword.Hit(target);
    }
}

//Now, we can create our Samurai and do battle!

class Program
{
    public static void Main() 
    {
        var warrior = new Samurai();
        warrior.Attack("the evildoers");
    }
}
//As you might imagine, this will print Chopped the evildoers clean in half to the console. This works just fine, but what if we wanted to arm our Samurai with another weapon? Since the Sword is created inside the Samurai class’s constructor, we have to modify the implementation of the class in order to make this change.
//When a class is dependent on a concrete dependency, it is said to be tightly coupled to that class. 
//In this example, the Samurai class is tightly coupled to the Sword class. When classes are tightly coupled, they cannot be interchanged without altering their implementation. In order to avoid tightly coupling classes, we can use interfaces to provide a level of indirection. Let’s create an interface to represent a weapon in our game.


interface IWeapon
{
    void Hit(string target);
}
Then, our Sword class can implement this interface:

class Sword : IWeapon
{
    public void Hit(string target) 
    {
        Console.WriteLine("Chopped {0} clean in half", target);
    }
}
And we can alter our Samurai class:

class Samurai
{
    readonly IWeapon weapon;
    public Samurai() 
    {
        this.weapon = new Sword();
    }
    public void Attack(string target) 
    {
        this.weapon.Hit(target);
    }
}
//Now our Samurai can be armed with different weapons. But wait! The Sword is still created inside the constructor of Samurai. 
//Since we still need to alter the implementation of Samurai in order to give our warrior another weapon, Samurai is still tightly coupled to Sword.
//Fortunately, there is an easy solution. Rather than creating the Sword from within the constructor of Samurai, 
//we can expose it as a parameter of the constructor instead. Also known as Constructor Injection.

class Samurai
{
    readonly IWeapon weapon;
    public Samurai(IWeapon weapon) 
    {
        this.weapon = weapon;
    }
    public void Attack(string target) 
    {
        this.weapon.Hit(target);
    }
}
//As Giorgio pointed out, there's also property injection. That would be something like:

class Samurai
{
    IWeapon weapon;

    public Samurai() { }


    public void SetWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }

    public void Attack(string target) 
    {
        this.weapon.Hit(target);
    }

}
//Hope this helps.
