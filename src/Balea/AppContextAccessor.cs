namespace Balea
{
    public class AppContextAccessor : IAppContextAccessor
    {
    	public AppContext AppContext { get; set; } = new AppContext();
    }


    public class AppContextAccessorAsyncLocal : IAppContextAccessor
    {
    	private static readonly AsyncLocal<AppContextHolder> _appContextCurrent = new();

    	public AppContext AppContext
    	{
    		get
    		{
    			return _appContextCurrent.Value?.Context;
    		}
    		set
    		{
    			var holder = _appContextCurrent.Value;
    			if (holder != null)
    			{
    				// Clear current AppContext trapped in the AsyncLocals, as its done.
    				holder.Context = null;
    			}

    			if (value != null)
    			{
    				// Use an object indirection to hold the AppContext in the AsyncLocal,
    				// so it can be cleared in all ExecutionContexts when its cleared.
    				_appContextCurrent.Value = new AppContextHolder { Context = value };
    			}
    		}
    	}

    	private sealed class AppContextHolder
    	{
    		public AppContext Context;
    	}
    }
}
