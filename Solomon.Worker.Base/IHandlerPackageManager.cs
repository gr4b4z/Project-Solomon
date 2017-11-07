namespace Solomon.Worker
{
    public interface IHandlerPackageManager
    {
        string GetHandlerAssembly(string handlerName);
        System.Threading.Tasks.Task UpdateHandlerAsync(string handlerName);
    }
}