using System.Diagnostics.CodeAnalysis;

namespace NatLib.Core.Utils;

public static class DelegateUtils
{
    /// <summary>
    /// Presents simple validation cycle without exit option. <br/> <br/>
    /// <b>Pipeline:</b> beforeInput -> input -> return value if validation(input) is true -> onError -> repeat
    /// </summary>
    /// <param name="beforeInput">Function that runs before input. </param>
    /// <param name="input">Function what gets input.</param>
    /// <param name="validate">Validates value got by input.</param>
    /// <param name="onError">Runs when validation failed.</param>
    /// <typeparam name="T">Requested type in input.</typeparam>
    /// <returns>Validated input.</returns>
    public static T ValidateCycle<T>(
        Action? beforeInput, 
        Func<T> input, 
        Func<T, bool> validate, 
        Action? onError)
    {
        CYCLE_START:
        beforeInput?.Invoke();
        var value = input.Invoke();
        
        if (validate(value)) return value;
        
        onError?.Invoke();
        goto CYCLE_START;
    }
    
    /// <summary>
    /// Presents simple validation cycle with exit option. <br/> <br/>
    /// <b>Pipeline:</b> beforeInput -> input -> return true and sets result if validation(input) is true
    /// -> shouldExit -> return false if shouldExit is false -> repeat
    /// </summary>
    /// <param name="beforeInput">Function that runs before input.</param>
    /// <param name="input">Function what gets input.</param>
    /// <param name="validate">Validates value got by input.</param>
    /// <param name="shouldExit">Decides should cycle continue or not.</param>
    /// <param name="result">Validated input.</param>
    /// <typeparam name="T">Type requested in input.</typeparam>
    /// <returns><b>true</b> - when got input successfully validated,<br/><b>false</b> - when exit used.</returns>
    public static bool ValidateCycleWithExit<T>(
        Action? beforeInput, 
        Func<T> input,
        Func<T, bool> validate,
        Func<bool> shouldExit, 
        out T? result)
    {
        result = default;
        CYCLE_START:
        beforeInput?.Invoke();
        var value = input.Invoke();
        if (!validate(value))
        {
            if (shouldExit.Invoke())
                goto CYCLE_START;
            return false;
        }

        result = value;
        return true;
    }
    
    /// <summary>
    /// Presents simple validation cycle without exit option and type-linking. <br/> <br/>
    /// <b>Pipeline:</b> beforeInput -> validate -> return when validate is true -> onError -> repeat 
    /// </summary>
    /// <param name="beforeInput">Function that runs before input.</param>
    /// <param name="validate">Performs validation.</param>
    /// <param name="onError">Runs when validation failed.</param>
    public static void ValidateCycle(
        Action? beforeInput,
        Func<bool> validate,
        Action? onError)
    {
        CYCLE_START:
        beforeInput?.Invoke();
        if (validate()) return;
        
        onError?.Invoke();
        goto CYCLE_START;
    }

    /// <summary>
    /// Presents simple validation cycle with exit option without type-linking. <br/> <br/>
    /// <b>Pipeline:</b> beforeInput -> validate -> return true when validate is true ->
    /// shouldExit -> return false if shouldExit is false -> repeat  
    /// </summary>
    /// <param name="beforeInput">Function that runs before input.</param>
    /// <param name="validate">Performs validation.</param>
    /// <param name="shouldExit">Decides should cycle continue or not.</param>
    /// <returns><b>true</b> - when got input successfully validated,<br/><b>false</b> - when exit used.</returns>
    public static bool ValidateCycleWithExit(
        Action? beforeInput,
        Func<bool> validate,
        Func<bool> shouldExit)
    {
        CYCLE_START:
        beforeInput?.Invoke();
        if (validate()) return true;
        
        if (shouldExit()) 
            goto CYCLE_START;
        
        return false;
    }
}