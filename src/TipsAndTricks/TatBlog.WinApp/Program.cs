
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Core.Seeders;
using TatBlog.Data.Contexts;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

var context = new BlogDbContext();

//var seeder = new DataSeeder(context);


//seeder.Initialize();

//var authors = context.Authors.ToList();

//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", "ID", "Full Name", "Email", "Joined Data");

//foreach (var auhor in authors)
//{
//    Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}", auhor.Id, auhor.FullName, auhor.Email, auhor.JoinedDate);
//}

//var posts = context.Posts
//    .Where(p => p.Published)
//    .OrderBy(p => p.Title)
//    .Select(p => new
//    {
//        Id = p.Id,
//        Title = p.Title,
//        ViewCount = p.ViewCount,
//        PostedDate = p.PostedDate,
//        Author = p.Author.FullName,
//        Category = p.Category.Name,
//    })
//    .ToList();


IBlogRepository blogRepo = new BlogRepository(context);

//var posts = await blogRepo.GetPopularArticlesAsync(3);
var posts = await blogRepo.GetPostsRandomAsync(3);


foreach (var post in posts)
{
    Console.WriteLine("ID        : {0}", post.Id);
    Console.WriteLine("Title     : {0}", post.Title);
    Console.WriteLine("View      : {0}", post.ViewCount);
    Console.WriteLine("Date      : {0:MM/dd/yyyy}", post.PostedDate);
    Console.WriteLine("Author    : {0}", post.Author);
    Console.WriteLine("Category  : {0}", post.Category);
    Console.WriteLine("".PadRight(80, '-'));
}
//var categories = await blogRepo.GetCategoriesAsync();



//var pangingParams = new PagingParams
//{
//    PageNumber = 1,
//    PageSize = 4,
//    SortColumn = "Name",
//    SortOrder = "DESC",
//};

////var tagsList = await blogRepo.GetPagedTagsAsync(pangingParams);
//var catsList = await blogRepo.GetPagedCategoriesAsync(pangingParams);

//foreach (var item in catsList)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
//}

var tag = await blogRepo.GetTagFromSlugAsync("google-application");
//var tag = await blogRepo.GetTagFromSlugAsync("netural-network");
////var tag = await blogRepo.GetTagFromSlugAsync("123");
//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Description");
//Console.WriteLine("{0,-5}{1,-50}{2,10}", tag.Id, tag.Name, tag.Description);


//// Xoa Tag
//var tagsList = await blogRepo.GetTagItemListAsync();


//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Count");

//foreach (var item in tagsList)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
//}
//if(tag != null)
//{
//    Console.WriteLine(tag.Id);
//    var isSuccess = await blogRepo.delTagAsync(tag.Id);
//    Console.WriteLine(isSuccess);
//}
//Console.WriteLine("----------------");
//foreach (var item in tagsList)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}", item.Id, item.Name, item.PostCount);
//}



////////////Category
///
//var cat1 = await blogRepo.GetCategoryFromSlugAsync("javascript");
//var cat = await blogRepo.GetCategoryFromIDAsync(cat1.Id);
//Console.WriteLine("{0,-5}{1,-50}{2,10}", "ID", "Name", "Description");
//Console.WriteLine("{0,-5}{1,-50}{2,10}", cat.Id, cat.Name, cat.Description);

//var cat = new Category()
//{
//    Id = 34,
//    Name = "New Category",
//    UrlSlug = "new-category",
//    Description = "New Category",
//    ShowOnMenu = true,
//    Posts = new List<Post>() { },
//};
//var cat = new Category()
//{
//    Id =43,
//    Name = ".NET Core",
//    UrlSlug = "netcore123456",
//    Description = ".NET Core",
//    ShowOnMenu = false,
//};
//var isSuccess = await blogRepo.AddUpdateCategoryAsync(cat);
//Console.WriteLine(isSuccess);



//// Category Add and Update
//var categories = await blogRepo.GetCategoriesAsync();

//foreach (var item in categories)
//{
//    Console.WriteLine("{0,-5}{1,-30}{3,-20}{2,10}", item.Id, item.Name,item.UrlSlug, item.PostCount);
//}
//var ListDatePost = await blogRepo.CountPostMonth(4);
//foreach (var item in ListDatePost)
//{
//    Console.WriteLine("{0,-5}{1,-30}{2,10}", item.Year.ToString(),item.Month.ToString(),item.PostCount.ToString());
//}
