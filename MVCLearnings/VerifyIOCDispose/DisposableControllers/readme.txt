If what you are asking is how to dispose of disposable controllers, you need to implement this method in your factory:

public override void ReleaseController(IController controller)
{
    var disposableController = controller as IDisposable;

    if (disposableController != null)
    {
        disposableController.Dispose();
    }
}
