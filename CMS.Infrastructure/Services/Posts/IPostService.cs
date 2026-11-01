using CMS.Core.DTOs;
using CMS.Core.Enums;
using CMS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Infrastructure.Services.Posts
{
    public interface IPostService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<UpdatePostDto> Get(int Id);
        Task<int> Create(CreatePostDto dto);
        Task<int> Delete(int Id);
        Task<List<ContentChangeLogViewModel>> GetLog(int Id);
        Task<int> UpdateStatus(int Id, ContentStatus status);
    }
}
