using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var post = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors()
        {
            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jason Mouth",
                    UrlSlug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@gmail.com",
                    JoinedDate = new DateTime(2022, 4, 19)
                },
                new()
                {
                    FullName = "Tran Dieu Dong",
                    UrlSlug = "tran-dieu-dong",
                    Email = "dongtdd@gmail.com",
                    JoinedDate = new DateTime(2023, 2, 28)
                },
                   new()
                {
                    FullName = "Nguyen Van A",
                    UrlSlug = "nguyen-van-a",
                    Email = "nguyenvana@gmail.com",
                    JoinedDate = new DateTime(2022, 2, 9)
                },
                      new()
                {
                    FullName = "Duong My Loc",
                    UrlSlug = "duong-my-loc",
                    Email = "duongmyloc@gmail.com",
                    JoinedDate = new DateTime(2022, 2, 27)
                },
                        


            };
            _dbContext.AddRange(authors);
            _dbContext.SaveChanges();
            return authors;
        }
        private IList<Category> AddCategories()
        {
            var categories = new List<Category>()
            {
                new() { Name = ".NET Core", Description = ".NET Core", UrlSlug = "net-core", ShowOnMenu = true },
                new() { Name = "Architeture", Description = "Architecture", UrlSlug = "architecture", ShowOnMenu = true },
                new() { Name = "Messaging", Description = "Messaging", UrlSlug = "messaging", ShowOnMenu = true },
                new() { Name = "OOP", Description = "Object-Oriented Program", UrlSlug = "oop", ShowOnMenu = true },
                new() { Name = "Design Patters", Description = "Design Patters", UrlSlug = "design-patters", ShowOnMenu = true },
                new() { Name = "Java", Description = "Java", UrlSlug = "java" },
                new() { Name = "C#", Description = "C#", UrlSlug = "c-sharp"  },
                new() { Name = "Javascript", Description = "Javascript", UrlSlug = "javascript" },
                new() { Name = "React", Description = "React", UrlSlug = "react" },
                new() { Name = "Vuejs", Description = "Vuejs", UrlSlug = "vuejs"},

            };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();
            return categories;
        }
        private IList<Tag> AddTags()
        {
            var tags = new List<Tag>()
            {
                new() { Name = "Google", Description = "Google Applications", UrlSlug = "google-application"},
                new() { Name = "ASP .NET MVC", Description = "ASP .NET MVC", UrlSlug = "asp-net-mvc" },
                new() { Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page"},
                new() { Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
                new() { Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning" },
                new() { Name = "Netural Network", Description = "Netural Network", UrlSlug = "netural-network" },
                new() { Name = "Adapter design pattern", Description = "Adapter design pattern", UrlSlug = "Adapter-design-pattern" },
                new() { Name = "Bridge design pattern", Description = "Bridge-design-pattern", UrlSlug = "Bridge-design-pattern" },
                new() { Name = "Microsoft Edge", Description = "Microsoft Edge", UrlSlug = "microsoft-edge" },
                new() { Name = "Abstraction OOP", Description = "Abstraction OOP", UrlSlug = "abstraction-oop" },
                new() { Name = "Inheritance OOP", Description = "Inheritance OOP", UrlSlug = "inheritance-oop" },

            };
            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();
            return tags;
        }

        private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "ASP .NET Core Diagnostic Scenarios",
                    ShortDescription = "David And Friends has great repository filled",
                    Description = "Here's a few great DON'T and DO examples ",
                    Meta="David And Friends has great repository filled",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },
				  new()
				{
					Title = "2.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "2-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2021, 9, 3, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[3],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
					new()
				{
					Title = "3.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "3-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2021, 1, 30, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
					  new()
				{
					Title = "4.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "4-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2022, 9, 30, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[0],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
						new()
				{
					Title = "5.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "5-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2021, 12, 30, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
						  new()
				{
					Title = "6.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "6-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2023, 2, 3, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
							new()
				{
					Title = "7.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "7-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2023, 1, 30, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
							  new()
				{
					Title = "8.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "8-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2023, 3, 9, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
								new()
				{
					Title = "9.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "9-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2023, 3, 9, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
								  new()
				{
					Title = "10.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "10-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2023, 3, 9, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
									new()
				{
					Title = "11.ASP .NET Core Diagnostic Scenarios",
					ShortDescription = "David And Friends has great repository filled",
					Description = "Here's a few great DON'T and DO examples ",
					Meta="David And Friends has great repository filled",
					UrlSlug = "11-aspnet-core-diagnostic-scenarios",
					Published = true,
					PostedDate = new DateTime(2023, 3, 9, 10, 20, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[0],
					Tags = new List<Tag>()
					{
						tags[0]
					}
				},
				new()
                {
                    Title = "Abstraction OOP",
                    ShortDescription = "Objects only reveal internal mechanisms that are relevant for the use of other objects, hiding any unnecessary implementation code.",
                    Description = " Objects only reveal internal mechanisms that are relevant for the use of other objects, hiding any unnecessary implementation code. The derived class can have its functionality extended. This concept can help developers more easily make additional changes or additions over time.",
                    UrlSlug = "abstraction-oop-post",
                    Meta="Objects only reveal internal mechanisms that are relevant for the use of other objects, hiding any unnecessary implementation code.",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 28, 21, 0, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[2],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                        tags[9]
                    }
                },
                new()
                {
                    Title = "Inheritance OOP",
                    ShortDescription = "Classes can reuse code from other classes. ",
                    Description = "Classes can reuse code from other classes. Relationships and subclasses between objects can be assigned, enabling developers to reuse common logic while still maintaining a unique hierarchy. This property of OOP forces a more thorough data analysis, reduces development time and ensures a higher level of accuracy.",
                    UrlSlug = "inheritance-oop-post",
                    Meta="Classes can reuse code from other classes. ",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 28, 19, 0, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[4],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new()
                {
                    Title = "Java",
                    ShortDescription = "Java is an object-oriented programming language that produces software for multiple platforms.  ",
                    Description = "When a programmer writes a Java application, the compiled code (known as bytecode) runs on most operating systems (OS), including Windows, Linux and Mac OS. Java derives much of its syntax from the C and C++ programming languages.",
                    UrlSlug = "java-post",
                    Meta="Java is an object-oriented programming language that produces software for multiple platforms.",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 28, 22, 0, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[2],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[4],
                        tags[9],
                        tags[10]
                    }
                },
				 new()
				{
					Title = "Java2",
					ShortDescription = "Java is an object-oriented programming language that produces software for multiple platforms.  ",
					Description = "When a programmer writes a Java application, the compiled code (known as bytecode) runs on most operating systems (OS), including Windows, Linux and Mac OS. Java derives much of its syntax from the C and C++ programming languages.",
					UrlSlug = "java-post2",
					Meta="Java is an object-oriented programming language that produces software for multiple platforms.",
					Published = true,
					PostedDate = new DateTime(2023, 2, 1, 22, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[3],
					Category = categories[5],
					Tags = new List<Tag>()
					{
						tags[4],
						tags[10]
					}
				},
				  new()
				{
					Title = "Java3",
					ShortDescription = "Java is an object-oriented programming language that produces software for multiple platforms.  ",
					Description = "When a programmer writes a Java application, the compiled code (known as bytecode) runs on most operating systems (OS), including Windows, Linux and Mac OS. Java derives much of its syntax from the C and C++ programming languages.",
					UrlSlug = "java-post3",
					Meta="Java is an object-oriented programming language that produces software for multiple platforms.",
					Published = true,
					PostedDate = new DateTime(2023, 3, 6, 22, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[3],
					Category = categories[5],
					Tags = new List<Tag>()
					{
						tags[4],
						tags[9],
						tags[10]
					}
				},
				   new()
				{
					Title = "Java4",
					ShortDescription = "Java is an object-oriented programming language that produces software for multiple platforms.  ",
					Description = "When a programmer writes a Java application, the compiled code (known as bytecode) runs on most operating systems (OS), including Windows, Linux and Mac OS. Java derives much of its syntax from the C and C++ programming languages.",
					UrlSlug = "java-post4",
					Meta="Java is an object-oriented programming language that produces software for multiple platforms.",
					Published = true,
					PostedDate = new DateTime(2023, 2, 4, 22, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[4],
					Category = categories[5],
					Tags = new List<Tag>()
					{
						tags[4],
						tags[9],
						tags[10]
					}
				},
					new()
				{
					Title = "Java5",
					ShortDescription = "Java is an object-oriented programming language that produces software for multiple platforms.  ",
					Description = "When a programmer writes a Java application, the compiled code (known as bytecode) runs on most operating systems (OS), including Windows, Linux and Mac OS. Java derives much of its syntax from the C and C++ programming languages.",
					UrlSlug = "java-post5",
					Meta="Java is an object-oriented programming language that produces software for multiple platforms.",
					Published = true,
					PostedDate = new DateTime(2023, 2, 8, 22, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[0],
					Category = categories[5],
					Tags = new List<Tag>()
					{
						tags[4],
						tags[9],
						tags[10]
					}
				},
					 new()
				{
					Title = "Java6",
					ShortDescription = "Java is an object-oriented programming language that produces software for multiple platforms.  ",
					Description = "When a programmer writes a Java application, the compiled code (known as bytecode) runs on most operating systems (OS), including Windows, Linux and Mac OS. Java derives much of its syntax from the C and C++ programming languages.",
					UrlSlug = "java-post6",
					Meta="Java is an object-oriented programming language that produces software for multiple platforms.",
					Published = true,
					PostedDate = new DateTime(2023, 3, 4, 22, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[5],
					Tags = new List<Tag>()
					{
						tags[4],
						tags[9],
						tags[10]
					}
				},
				new()
                {
                    Title = "Design Patterns",
                    ShortDescription = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. ",
                    Description = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. A design pattern isn't a finished design that can be transformed directly into code. It is a description or template for how to solve a problem that can be used in many different situations.",
                    UrlSlug = "design-pattern-post",
                    Meta="In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design.",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 2, 9, 0, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[3],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                        tags[6],
                        tags[7],
                    }

                },
				new()
                {
                    Title = "Design Patterns3",
                    ShortDescription = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. ",
                    Description = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. A design pattern isn't a finished design that can be transformed directly into code. It is a description or template for how to solve a problem that can be used in many different situations.",
                    UrlSlug = "design-pattern3-post",
                    Meta="In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design.",
                    Published = true,
                    PostedDate = new DateTime(2023, 2, 1, 9, 0, 0),
                    ModifiedDate = null,
                    ViewCount = 0,
                    Author = authors[4],
                    Category = categories[4],
                    Tags = new List<Tag>()
                    {
                        tags[7],
                    }
                },

				new()
				{
					Title = "Design Patterns2",
					ShortDescription = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. ",
					Description = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. A design pattern isn't a finished design that can be transformed directly into code. It is a description or template for how to solve a problem that can be used in many different situations.",
					UrlSlug = "design-pattern2-post",
					Meta="In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design.",
					Published = true,
					PostedDate = new DateTime(2023, 2, 8, 9, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[3],
					Category = categories[4],
					Tags = new List<Tag>()
					{
						tags[6],
						tags[7],
					}
				},

				new()
				{
					Title = "Design Patterns4",
					ShortDescription = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. ",
					Description = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. A design pattern isn't a finished design that can be transformed directly into code. It is a description or template for how to solve a problem that can be used in many different situations.",
					UrlSlug = "design-pattern4-post",
					Meta="In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design.",
					Published = true,
					PostedDate = new DateTime(2023, 2, 8, 9, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[2],
					Category = categories[4],
					Tags = new List<Tag>()
					{
						tags[6],
						tags[7],
					}
				},

				new()
				{
					Title = "Design Patterns5",
					ShortDescription = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. ",
					Description = "In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design. A design pattern isn't a finished design that can be transformed directly into code. It is a description or template for how to solve a problem that can be used in many different situations.",
					UrlSlug = "design-pattern5-post",
					Meta="In software engineering, a design pattern is a general repeatable solution to a commonly occurring problem in software design.",
					Published = true,
					PostedDate = new DateTime(2023, 2, 8, 9, 0, 0),
					ModifiedDate = null,
					ViewCount = 0,
					Author = authors[0],
					Category = categories[4],
					Tags = new List<Tag>()
					{
						tags[6],
						tags[7],
					}
				}
			};
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();
            return posts;
        }

    }
}