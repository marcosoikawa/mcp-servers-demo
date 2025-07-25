using System.ComponentModel;
using ModelContextProtocol.Server;
using ModelContextProtocol.Protocol;

var builder = WebApplication.CreateBuilder(args);
builder.Services
       .AddMcpServer()
       .WithHttpTransport()
       .WithTools<Tools>();

builder.Services.AddHttpClient();

var app = builder.Build();


app.MapMcp();

app.Run();


[McpServerToolType]
public sealed class Tools
{
    [McpServerTool, Description("Adicione dois n�meros entre eles")]
    public async Task<string> SomarNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        return await Task.FromResult((a + b).ToString());
    }

    [McpServerTool, Description("Subtraia dois n�meros entre eles")]
    public async Task<string> SubtraiaNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        return await Task.FromResult((a + b).ToString());
    }

    [McpServerTool, Description("Multiplique dois n�meros entre eles")]
    public async Task<string> MultipliqueNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        return await Task.FromResult((a * b).ToString());
    }

    ///Agora gere outro para dividir dois n�meros entre eles
    [McpServerTool, Description("Divida dois n�meros entre eles")]
    public async Task<string> DividaNumeros(
        [Description("o primeiro n�mero")] int a,
        [Description("o segundo n�mero")] int b)
    {
        return await Task.FromResult((a / b).ToString());
    }

    ///Agora gere outro para extrair a raiz quadrada de um n�mero
    [McpServerTool, Description("Extrai a raiz quadrada de um n�mero")]
    public async Task<string> RaizQuadrada(
        [Description("o n�mero")] int a)
    {
        return await Task.FromResult(Math.Sqrt(a).ToString());
    }

    ///agora gere outro para elevar a potencia de um n�mero
    [McpServerTool, Description("Eleva um n�mero a uma pot�ncia")]
    public async Task<string> Potencia(
        [Description("o n�mero base")] int baseNum,
        [Description("o expoente")] int expoente)
    {
        return await Task.FromResult(Math.Pow(baseNum, expoente).ToString());
    }
}