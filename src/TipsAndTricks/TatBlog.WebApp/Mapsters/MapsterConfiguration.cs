using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Mapsters
{
	public class MapsterConfiguration : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Post, PostItem>()
				.Map(dest => dest.CategoryName, src => src.Category.Name)
				.Map(dest => dest.Tags, src => src.Tags.Select(x => x.Name));

			config.NewConfig<Comment, CommentItem>()
				.Map(dest => dest.PostName, src => src.Post.Title);

			config.NewConfig<CommentItem, Comment>()
				.Ignore(dest => dest.Id)
				.Map(dest => dest.PostId, src => src.PostId);

			config.NewConfig<PostFilterModel, PostQuery>()
				.Map(dest => dest.PublishedOnly, src => false);

			config.NewConfig<CategoryFilterModel, CategoryQuery>();

			config.NewConfig<CommentFilterModel, CommentQuery>();

			config.NewConfig<AuthorEditModel, AuthorQuery>();

			config.NewConfig<AuthorFilterModel, AuthorQuery>();

			config.NewConfig<PostEditModel, Post>()
				.Ignore(dest => dest.Id)
				.Ignore(dest => dest.ImageUrl);

			config.NewConfig<Post, PostEditModel>()
				.Map(dest => dest.SelectedTags, src => 
					string.Join("\r\n",src.Tags.Select(x => x.Name)))
				.Ignore(dest => dest.CategoryList!)
				.Ignore(dest => dest.AuthorList!)
				.Ignore(dest => dest.ImageFile!);


			config.NewConfig<Category, CategoryEditModel>()
				.Ignore(dest => dest.PostCount);
			config.NewConfig<Author, AuthorEditModel>()
				.Ignore(dest => dest.PostCount)
				.Ignore(dest => dest.ImageFile!);

		}
	}
}
