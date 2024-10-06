namespace Lua.Standard.IO;

public sealed class ReadFunction : LuaFunction
{
    public override string Name => "read";
    public static readonly ReadFunction Instance = new();

    protected override ValueTask<int> InvokeAsyncCore(LuaFunctionExecutionContext context, Memory<LuaValue> buffer, CancellationToken cancellationToken)
    {
        var file = context.State.Environment["io"].Read<LuaTable>()["stdio"].Read<FileHandle>();
        var resultCount = IOHelper.Read(context.State, file, Name, 0, context.Arguments, buffer, false);
        return new(resultCount);
    }
}