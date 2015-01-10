//Now, lets talk about a single scenario that I hope it will make you understand the situations where to use what. 
//Lets Assume you need to make three classes, first is CAR, second is MAN, third is WOMAN. Now you need a function in 
//each of them to define how they Move. Now all three can move but CAR moves entirely in different way than MAN and WOMAN. 
//So here we use an Interface IMOVEMENT and declare a function MOVE in it. Now all three classes can inherit this interface. 
//So the classes goes like this. 

public interface IMovement 
{ 
void Move(); 
} 

public class Car : IMovement 
{ 
public void Move() 
{ 
//Provide Implementation 
} 
} 

public class Man : IMovement 
{ 
public void Move() 
{ 
//Provide Implementation 
} 
} 

public class Woman : IMovement 
{ 
public void Move() 
{ 
//Provide Implementation 
} 
} 

//But, since MAN and WOMAN walk in similar way, so providing same behavior in two different methods will be code 
//redundancy, in simpler words code is not re-used. So we can now define a Abstract Class for Human Beings movements, 
//so this class can be HUMANBEINGMOVEMENT. Also the same can be applied to CAR class, since there are lot of 
//manufactures for cars and all cars move in similar way so we can also define a abstract class for Cars movement 
//which can be CARSMOVEMENT. So our refactored code will be . 

public interface IMovement 
{ 
void Move(); 
} 

public abstract class CarsMovement : IMovement 
{ 

public virtual void Move() 
{ 
//default behavior for cars movement 
} 
} 

public class SuzukiCar : CarsMovement 
{ 
public override void Move() 
{ 
//Provide Implementation 
} 
} 

public abstract class HumanBeingMovement : IMovement 
{ 

public virtual void Move() 
{ 
//default behavior for human being movement 
} 
} 

public class Man : HumanBeingMovement 
{ 
public override void Move() 
{ 
//Provide Implementation 
} 
} 

public class Woman : HumanBeingMovement 
{ 
public override void Move() 
{ 
//Provide Implementation 
} 
} 

//Now as you can see, an hierarchy is appearing now every human being can inherit from our HUMANBEINGMOVEMENT class for 
//the move method and all types of cars can inherit from our CARSMOVEMENT class for the move method. 
