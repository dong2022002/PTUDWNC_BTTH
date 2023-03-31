namespace TatBlog.WebApi.Models.SubscribersModel
{
	public class BlockSubscriber
	{
        public int Id { get; set; }
        public string Reason { get; set; } = null;
		public string Notes { get; set; } = null;
    }
}
