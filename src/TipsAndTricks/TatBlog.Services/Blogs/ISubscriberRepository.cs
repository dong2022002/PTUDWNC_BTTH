﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs
{
    public interface ISubscriberRepository
    {

        Task<bool> SubscriberAsync(
            string email,
            CancellationToken cancellationToken =default);
        Task UnSubscriberAsync(
        string email,
        string reason,
        bool isVoluntary,
        CancellationToken cancellationToken = default);

        Task BlockSubscriberAsync(
           int id,
       string notes,
       string reason,
       CancellationToken cancellationToken = default);
        Task<bool> DeleteSubscriberAsync(
           int id,
       CancellationToken cancellationToken = default);
        Task<Subscriber>  GetSubscriberByIdAsync(
          int id,
      CancellationToken cancellationToken = default);
        Task<Subscriber> GetSubscriberByEmailAsync(
      string email,
        CancellationToken cancellationToken = default);

		Task<IPagedList<Subscriber>> GetPagedSubcriberAsync(
		 SubcriberQuery condition,
		  int pageNumber = 1,
		  int pageSize = 5,
		  CancellationToken cancellationToken = default);
        Task<IPagedList<Subscriber>> GetPagedSubcriberAsync(
         IPagingParams pagingParams,
         string email = null,
         CancellationToken cancellationToken = default);
        Task<bool> IsEmailExitedAsync(
            int id,
            string email,
            CancellationToken cancellationToken = default);
		
		}
}
