using AutoMapper;
using CMS.Core.DTOs;
using CMS.Core.Exceptions;
using CMS.Core.ViewModels;
using CMS.Data;
using CMS.Data.Models;
using CMS.Infrastructure.Services.Files;
using CMS.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Enums;
using CMS.Infrastructure.Services.Email;

namespace CMS.Infrastructure.Services.Posts
{
    public class PostService : IPostService
    {

        private readonly CMSDbContext _db;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailservice;
        private readonly IFileService _fileservice;

        public PostService(CMSDbContext db, IMapper mapper, IFileService fileservice, IEmailService emailservice)
        {
            _db = db;
            _mapper = mapper;
            _fileservice = fileservice;
            _emailservice = emailservice;
        }
        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Posts.Include(x => x.Category)
                .Include(x => x.Attachments)
                .Include(x => x.Author)
                .Where(x => !x.IsDelete)
                .AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var posts = _mapper.Map<List<PostViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = posts,
                meta = new Meta
                {
                    page = pagination.Page,
                    perpage = pagination.PerPage,
                    pages = pages,
                    total = dataCount,
                }
            };
            return result;
        }

        public async Task<int> Delete(int Id)
        {
            var post = await _db.Posts.SingleOrDefaultAsync(x => !x.IsDelete && x.Id == Id);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            post.IsDelete = true;
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
            return post.Id;
        }
        public async Task<UpdatePostDto> Get(int Id)
        {
            var post = await _db.Posts.SingleOrDefaultAsync(x => !x.IsDelete && x.Id == Id);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdatePostDto>(post);
        }
        public async Task<int> Create(CreatePostDto dto)
        {
            var post = _mapper.Map<Post>(dto);
            await _db.Posts.AddAsync(post);
            await _db.SaveChangesAsync();
            if (dto.Attachments != null)
            {
                foreach (var attachment in dto.Attachments)
                {
                    var postAttachment = new PostAttachment();
                    postAttachment.AttachmenUrl = await _fileservice.SaveFile(attachment, "Images");
                    postAttachment.PostId = post.Id;
                    await _db.PostAttachments.AddAsync(postAttachment);
                    await _db.SaveChangesAsync();
                }
            }
            return post.Id;
        }
        public async Task<List<ContentChangeLogViewModel>> GetLog(int Id)
        {
            var changes =await _db.ContentChangeLogs.Where(x=>x.ContentId == Id && x.Type ==ContentType.Post).ToListAsync();
            return _mapper.Map<List<ContentChangeLogViewModel>>(changes);

        
        }
        public async Task<int> UpdateStatus(int Id, ContentStatus status)
        {
            var post = await _db.Posts.Include(x=>x.Author).SingleOrDefaultAsync(x => !x.IsDelete && x.Id == Id);
            if (post == null)
            {
                throw new EntityNotFoundException();
            }
            var changeLog = new ContentChangeLog();
            changeLog.ContentId = post.Id;
            changeLog.Type = ContentType.Post;
            changeLog.Old = post.Status;
            changeLog.New = status;
            changeLog.ChangeAt = DateTime.Now;
            await _db.ContentChangeLogs.AddAsync(changeLog);
            await _db.SaveChangesAsync();

            post.Status = status;
            _db.Posts.Update(post);
            await _db.SaveChangesAsync();
             

            await _emailservice.Send(post.Author.Email, "UPDATE POST STATUS !", $"YOUR POST NOW IS {status.ToString()}");
            return post.Id;

        }
    }
}
