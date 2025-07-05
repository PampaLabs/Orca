namespace System;

internal static class ActionExtensions
{
	public static TOptions Build<TOptions>(this Action<TOptions> optionsAction)
		where TOptions : new()
	{
		return Build(optionsAction, new TOptions());
	}

    public static TOptions Build<TOptions>(this Action<TOptions> optionsAction, TOptions options)
    {
        optionsAction.Invoke(options);
        return options;
    }

    public static TOptions Build<T1, TOptions>(this Action<T1, TOptions> optionsAction, T1 arg)
        where TOptions : new()
    {
        return Build(optionsAction, arg, new TOptions());
    }

    public static TOptions Build<T1, TOptions>(this Action<T1, TOptions> optionsAction, T1 arg, TOptions options)
    {
        optionsAction.Invoke(arg, options);
        return options;
    }
}
