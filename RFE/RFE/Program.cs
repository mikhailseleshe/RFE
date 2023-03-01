using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static System.Formats.Asn1.AsnWriter;

namespace RFE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MessageDbContext>(opt => opt.UseInMemoryDatabase("Messages"));
            var app = builder.Build();

            app.MapPost("/v1/diff/{id}/left", async (int id, HttpContext context, MessageDbContext db) =>
            {
                if (context.Request.ContentType != "application/custom")
                {
                    return Results.Ok("Incorrect Content-type");
                }

                var input = await ProcessBody(context.Request.Body);

                var message = await db.Messages.FindAsync(id);

                if (message == null)
                {
                    db.Messages.Add(new Message { Id = id, Left = input });
                }
                else
                {
                    message.Left = input;
                }
                
                await db.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapPost("/v1/diff/{id}/right", async (int id, HttpContext context, MessageDbContext db) =>
            {
                if (context.Request.ContentType != "application/custom")
                {
                    return Results.Ok("Incorrect Content-type");
                }

                var input = await ProcessBody(context.Request.Body);

                var message = await db.Messages.FindAsync(id);

                if (message == null)
                {
                    db.Messages.Add(new Message { Id = id, Right = input });
                }
                else
                {
                    message.Right = input;
                }

                await db.SaveChangesAsync();

                return Results.Ok();
            });

            app.MapGet("/v1/diff/{id}", async (int id, MessageDbContext db) =>
            {
                var message = await db.Messages.FindAsync(id);

                if (message == null || message.Left == null || message.Right == null)
                {
                    return Results.Ok("Inputs not ready");
                }
                else if (message.Left == message.Right)
                {
                    return Results.Ok("inputs were equal");
                }
                else if (message.Left.Length != message.Right.Length)
                {
                    return Results.Ok("inputs are of different size");
                }
                else
                {
                    var diff = CalculationHelper.GetStringDiff(message.Left, message.Right);

                    return Results.Ok(diff);
                }
            });

            app.Run();
        }

        public static async Task<string> ProcessBody(Stream stream)
        {
            string body = await SerializationHelper.ReadStreamAsyncToString(stream);
            body = EncodingHelper.Escape(body);
            body = EncodingHelper.Base64Decode(body);
            body = SerializationHelper.GetInputValue(body);
            return body;
        }
    }
}