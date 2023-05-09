using System;
using System.Collections.Generic;
using NetCoreWebGoat.Config;
using NetCoreWebGoat.Models;

namespace NetCoreWebGoat.Repositories
{
    public class CommentRepository : BaseRepository
    {
        public CommentRepository(AppConfig appConfig) : base(appConfig) { }

        public List<CommentModel> GetAll()
        {
            List<CommentModel> list = new List<CommentModel>();
            var sql = "SELECT Id, Text, CreatedAt, UserId, PostId FROM \"Comment\"";
            var result = Query(sql);
            if (result.HasRows)
                while (result.Read())
                    list.Add(new CommentModel(result));
            return list;
        }

        public void Add(CommentModel comment)
        {
            var sql = $"INSERT INTO \"Comment\" (Text, CreatedAt, UserId, PostId) VALUES (@Text, @CreatedAt, @UserId, @PostId)";
            comment.CreatedAt = DateTime.Now;
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@Text", comment.Text);
            dic.Add("@CreatedAt", comment.CreatedAt);
            dic.Add("@UserId", comment.UserId);
            dic.Add("@PostId", comment.PostId);
            ExecuteNonQuery(sql, dic);
        }

        public void Delete(int id)
        {
            var sql = $"DELETE FROM \"Comment\" WHERE Id = @Id";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("@Id", id);
            ExecuteNonQuery(sql, dic);
        }
    }
}