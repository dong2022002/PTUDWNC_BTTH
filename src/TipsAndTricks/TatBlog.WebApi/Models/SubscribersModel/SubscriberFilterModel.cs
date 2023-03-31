namespace TatBlog.WebApi.Models.SubscribersModel
{
	public class SubscriberFilterModel :PagingModel
	{
		public string Email { get; set; } = null;
    }
}
