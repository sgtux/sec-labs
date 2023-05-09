namespace NetCoreWebGoat.Models
{
    public class CreateCommentModel
    {
        public string Text { get; set; }

        public int UserId { get; set; }

        public int PostId { get; set; }

        public CommentModel ToEntity()
        {
            return new CommentModel()
            {
                UserId = UserId,
                Text = Text,
                PostId = PostId
            };
        }
    }
}