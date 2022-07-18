using System.Data;
using Microsoft.Data.Sqlite;

namespace Library.Model;

public static class Db
{
    private const string connectionString = "Data Source=books.db;";

    public static List<Book> GetBooks()
    {
        var books = new List<Book>();

        var db = new SqliteConnection(connectionString);
        db.Open();

        var sql = "SELECT id, title, author, genre FROM table_books";
        var command = new SqliteCommand(sql, db);
        var result = command.ExecuteReader();
        if (result.HasRows)
        {
            while (result.Read())
            {
                books.Add(new Book
                {
                    Id = result.GetInt32("id"),
                    Title = result.GetString("title"),
                    Author = result.GetString("author"),
                    Genre = result.GetString("genre")
                });
            }
        }
        
        db.Close();
        return books;
    }

    public static void AddBook(Book book)
    {
        var db = new SqliteConnection(connectionString);
        db.Open();

        var sql = $"INSERT INTO table_books (title, author, genre) VALUE ('{book.Title}', '{book.Author}', '{book.Genre}')";
        var command = new SqliteCommand(sql, db);
        command.ExecuteNonQuery();
        
        db.Close();
    }
    
    public static void UpdateBook(Book book)
    {
        var db = new SqliteConnection(connectionString);
        db.Open();

        var sql = $"UPDATE table_books SET title='{book.Title}', author='{book.Author}', genre='{book.Genre}' WHERE id = {book.Id}";
        var command = new SqliteCommand(sql, db);
        command.ExecuteNonQuery();
        
        db.Close();
    }

    public static void DeleteBook(int id)
    {
        var db = new SqliteConnection(connectionString);
        db.Open();

        var sql = $"DELETE FROM table_books WHERE id = {id}";
        var command = new SqliteCommand(sql, db);
        command.ExecuteNonQuery();
        
        db.Close();
    }
}
