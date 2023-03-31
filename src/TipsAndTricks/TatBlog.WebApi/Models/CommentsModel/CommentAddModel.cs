namespace TatBlog.WebApi.Models.CommentsModel
{
	public class CommentAddModel
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int PostId { get; set; }
	}
}
