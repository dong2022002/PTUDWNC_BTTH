using Mapster;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.CommentsModel;
using TatBlog.WebApi.Models.PostsModel;
using TatBlog.WebApi.Models.TagsModel;

namespace TatBlog.WebApi.Mapsters
{
	public class MapsterConfiguration : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<Author, AuthorDto>();
			config.NewConfig<Author, AuthorItem>()
				.Map(dest => dest.PostCount,
					src => src.Posts == null ? 0 : src.Posts.Count);

			config.NewConfig<AuthorEditModel, Author>();

			config.NewConfig<Category, CategoryDto>();
			config.NewConfig<Category, CategoryItem>()
				.Map(dest => dest.PostCount,
					src => src.Posts == null ? 0 : src.Posts.Count);

			config.NewConfig<Comment, CommentItem>();
			config.NewConfig<CommentAddModel, Comment>();
			

			config.NewConfig<Post, PostDto>();
			config.NewConfig<Post, PostDetail>();
			config.NewConfig<PostEditModel, Post>()
					.Ignore(dest => dest.Id)
				.Ignore(dest => dest.ImageUrl);


			config.NewConfig<PostFilterModel, PostQuery>();



			config.NewConfig<Tag, TagItem>()
				.Map(dest => dest.PostCount,
					src => src.Posts == null ? 0 : src.Posts.Count);
			config.NewConfig<TagEditModel, Tag>();



		}
	}
}
