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
    [McpServerTool, Description("Adicione dois números entre eles")]
    public async Task<string> SomarNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        return await Task.FromResult((a + b).ToString());
    }

    [McpServerTool, Description("Subtraia dois números entre eles")]
    public async Task<string> SubtraiaNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        return await Task.FromResult((a + b).ToString());
    }

    [McpServerTool, Description("Multiplique dois números entre eles")]
    public async Task<string> MultipliqueNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        return await Task.FromResult((a * b).ToString());
    }

    ///Agora gere outro para dividir dois números entre eles
    [McpServerTool, Description("Divida dois números entre eles")]
    public async Task<string> DividaNumeros(
        [Description("o primeiro número")] int a,
        [Description("o segundo número")] int b)
    {
        return await Task.FromResult((a / b).ToString());
    }

    ///Agora gere outro para extrair a raiz quadrada de um número
    [McpServerTool, Description("Extrai a raiz quadrada de um número")]
    public async Task<string> RaizQuadrada(
        [Description("o número")] int a)
    {
        return await Task.FromResult(Math.Sqrt(a).ToString());
    }

    ///agora gere outro para elevar a potencia de um número
    [McpServerTool, Description("Eleva um número a uma potência")]
    public async Task<string> Potencia(
        [Description("o número base")] int baseNum,
        [Description("o expoente")] int expoente)
    {
        return await Task.FromResult(Math.Pow(baseNum, expoente).ToString());
    }
}