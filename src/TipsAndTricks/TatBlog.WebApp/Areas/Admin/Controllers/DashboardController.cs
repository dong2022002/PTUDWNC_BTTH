using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
using TatBlog.Services.Media;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;


        public DashboardController(
            IBlogRepository blogRepository,
             IMapper mapper,
            ICommentRepository commentRepository
            )
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _blogRepository.GetStatistical();
            return View(list);
        }
    }
}





