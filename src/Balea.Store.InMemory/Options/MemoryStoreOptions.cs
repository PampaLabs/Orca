namespace Balea.Store.Configuration;

public class MemoryStoreOptions
{
    public HashSet<Application> Applications { get; set; } = [];

    public MemoryStoreOptions()
    {
        Applications =
            [
                new ()
                {
                    Name = "default",
                }
            ];
    }
}
