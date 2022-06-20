using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Npgsql;
using System;
// using System.DateTimeOffset;
using System.Collections.Generic;

namespace SimpleHttpFunction
{
    public class Comment
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public string Category { get; set; }
        public string CreatedAt { get; set; }
        public int AuthorId { get; set; }
    }

    public class Function : IHttpFunction
    {
        private readonly ILogger _logger;

        public Function(ILogger<Function> logger) =>
            _logger = logger;

        // async Task getData() 
        // {
            // using var cmd = new NpgsqlCommand(sql, con);
            // using NpgsqlDataReader rdr = cmd.ExecuteReader();
            // System.Console.WriteLine($"{rdr.GetName(0),-4} {rdr.GetName(1),-10} {rdr.GetName(2),10}");
            // while (rdr.Read())
            // {
            // Console.WriteLine($"{rdr.GetInt32(0),-4} {rdr.GetString(1)} {rdr.GetString(2)} {rdr.GetString(3)} {rdr.GetInt32(4)}");
            // }

            // return "";
        // }    

        public async Task HandleAsync(HttpContext context)
        {
            try
            {
                List<Comment> comments = new List<Comment>();
            // await getData();
            var connString = "Host=167.235.52.214;Username=dev;Password=PASS;Database=welt";

            await using var conn = new NpgsqlConnection(connString);
            await conn.OpenAsync();

            // Insert some data
            // await using (var cmd = new NpgsqlCommand("INSERT INTO data (some_field) VALUES ($1)", conn))
            // {
            //     cmd.Parameters.AddWithValue("Hello world");
            //     await cmd.ExecuteNonQueryAsync();
            // }

            // Retrieve all rows
            await using (var cmd = new NpgsqlCommand("SELECT * from comments limit 1000;", conn))
            await using (var rdr = await cmd.ExecuteReaderAsync())
            {
                while (await rdr.ReadAsync())
                {
                    Console.WriteLine(" >>> Getting comment from DB");
                    var comment = new Comment();
                    comment.Id = rdr.GetInt32(0);
                    comment.Body = rdr.GetString(1);
                    comment.Category = rdr.GetString(2);
                    comment.CreatedAt = "";
                    comment.AuthorId = rdr.GetInt32(4);
                    comments.Add(comment);
                }
            }
            
           /////////////////////////////////////////////////////////////
            // HttpRequest request = context.Request;
            // Check URL parameters for "message" field
            // string message = request.Query["message"];

            // using TextReader reader = new StreamReader(comments);
            // string text = await reader.ReadToEndAsync();
            // if (text.Length > 0)
            // {
                try
                {
                    // JsonElement json = JsonSerializer.Deserialize<JsonElement>(comments[0]);
                    var json = JsonSerializer.Serialize(comments);
                    // if (json.TryGetProperty("message", out JsonElement messageElement) &&
                    //     messageElement.ValueKind == JsonValueKind.String)
                    // {
                    //     message = messageElement.GetString();
                    // }
                    await context.Response.WriteAsync(json);
                }
                catch (JsonException parseException)
                {
                    _logger.LogError(parseException, "Error parsing JSON request");
                }
            // }

            // await context.Response.WriteAsync(text);
            }
            catch (System.Exception ex)
            {
                 Console.WriteLine($"Generic Exception Handler: {ex}");
            }
        }
    }
}
