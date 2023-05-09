using System;
using NetCoreWebGoat.Helpers;
using Npgsql;

namespace NetCoreWebGoat.Models
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public UserModel Author { get; set; }

        public bool Owner { get; set; }

        public CommentModel() { }

        public CommentModel(NpgsqlDataReader dataReader) => FillFromDataReader(dataReader);

        public void FillFromDataReader(NpgsqlDataReader dataReader)
        {
            Id = DatabaseHelper.GetValueOrNull<int>(dataReader, "Id");
            Text = DatabaseHelper.GetValueOrNull<string>(dataReader, "Text");
            CreatedAt = DatabaseHelper.GetValueOrNull<DateTime>(dataReader, "CreatedAt");
            UserId = DatabaseHelper.GetValueOrNull<int>(dataReader, "UserId");
            PostId = DatabaseHelper.GetValueOrNull<int>(dataReader, "PostId");
        }
    }
}