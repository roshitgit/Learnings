//Now imagine we have a church called Jubilee, which has branches world wide. Our aim is to simply get an item from each branch. Here would be the solution with DI;

//1) Create an interface IJubilee:

public interface IJubilee
{
    string GetItem(string userInput);
}
//2) Create a class JubileeDI which would take the IJubilee interface as a constructor and return an item:

public class JubileeDI
{
    readonly IJubilee jubilee;

    public JubileeDI(IJubilee jubilee)
    {
        this.jubilee = jubilee;
    }

    public string GetItem(string userInput)
    {
        return this.jubilee.GetItem(userInput);
    }
}
//3) Now create three braches of Jubilee namely JubileeGT, JubileeHOG, JubileeCOV, which all MUST inherit the IJubilee interface. For the fun of it, make one of them implement its method as virtual:

public class JubileeGT : IJubilee
{
    public virtual string GetItem(string userInput)
    {
        return string.Format("For JubileeGT, you entered {0}", userInput);
    }
}

public class JubileeHOG : IJubilee
{
    public string GetItem(string userInput)
    {
        return string.Format("For JubileeHOG, you entered {0}", userInput);
    }
}

public class JubileeCOV : IJubilee
{
    public string GetItem(string userInput)
    {
        return string.Format("For JubileCOV, you entered {0}", userInput);
    }
}

public class JubileeGTBranchA : JubileeGT
{
    public override string GetItem(string userInput)
    {
        return string.Format("For JubileeGT branch A, you entered {0}", userInput);
    }
}
//4) That is it! Now let us see DI in action:

        JubileeDI jCOV = new JubileeDI(new JubileeCOV());
        JubileeDI jHOG = new JubileeDI(new JubileeHOG());
        JubileeDI jGT = new JubileeDI(new JubileeGT());
        JubileeDI jGTA = new JubileeDI(new JubileeGTBranchA());

        var item = jCOV.GetItem("Give me some money!");
        var item2 = jHOG.GetItem("Give me a check!");
        var item3 = jGT.GetItem("I need to be fed!!!");
        var item4 = jGTA.GetItem("Thank you!");
//For each instance of the container class, we pass a new instance of the class we require, which enables loose coupling.
//Hope this explains the concept in a nutshell.
