namespace Lua.Standard.Bitwise;

public sealed class ReplaceFunction : LuaFunction
{
    public override string Name => "replace";
    public static readonly ReplaceFunction Instance = new();

    protected override ValueTask<int> InvokeAsyncCore(LuaFunctionExecutionContext context, Memory<LuaValue> buffer, CancellationToken cancellationToken)
    {
        var arg0 = context.GetArgument<double>(0);
        var arg1 = context.GetArgument<double>(1);
        var arg2 = context.GetArgument<double>(2);
        var arg3 = context.HasArgument(3)
            ? context.GetArgument<double>(3)
            : 1;

        LuaRuntimeException.ThrowBadArgumentIfNumberIsNotInteger(context.State, this, 1, arg0);
        LuaRuntimeException.ThrowBadArgumentIfNumberIsNotInteger(context.State, this, 2, arg1);
        LuaRuntimeException.ThrowBadArgumentIfNumberIsNotInteger(context.State, this, 3, arg2);
        LuaRuntimeException.ThrowBadArgumentIfNumberIsNotInteger(context.State, this, 4, arg3);

        var n = Bit32Helper.ToUInt32(arg0);
        var v = Bit32Helper.ToUInt32(arg1);
        var field = (int)arg2;
        var width = (int)arg3;

        Bit32Helper.ValidateFieldAndWidth(context.State, this, 2, field, width);
        uint mask;
        if (width == 32)
        {
            mask = 0xFFFFFFFF;
        }
        else
        {
            mask = (uint)((1 << width) - 1);
        }

        v = v & mask;
        n = (n & ~(mask << field)) | (v << field);
        buffer.Span[0] = n;
        return new(1);
    }
}