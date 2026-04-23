// public class MyCustomMiddleware
// {
//     private readonly RequestDelegate _next;

//     public MyCustomMiddleware(RequestDelegate next)
//     {
//         _next = next;
//     }

//     // public async Task InvokeAsync(HttpContext context)
//     // {
//     //     Console.WriteLine("--> Request ဝင်လာပါပြီ"); // စမ်းသပ်ရန် Log

//     //     await _next(context); // နောက် Middleware ကို ဆက်သွားခိုင်းခြင်း

//     //     Console.WriteLine("<-- Response ပြန်ထွက်သွားပါပြီ"); // စမ်းသပ်ရန် Log
//     // }
//     public async Task InvokeAsync(HttpContext context)
//     {
//         Console.WriteLine("Request ကို ဒီမှာပဲ ရပ်လိုက်မယ်");
//         // await _next(context); // ဒီလိုင်းကို ဖျက်လိုက်ရင် နောက် Middleware တွေနဲ့ Controller တွေဆီ မရောက်တော့ဘူး
//         await context.Response.WriteAsync("Middleware stopped the request!");
//     }
// }

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // ဝင်လာတဲ့ request ကို log ထုတ်မယ်
        Console.WriteLine($"[POS LOG] {DateTime.Now}: {context.Request.Method} {context.Request.Path}");
        
        await _next(context); // နောက် pipeline ကို ဆက်သွားခိုင်းမယ်
    }
}