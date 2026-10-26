using AutoMapper;
using CMS.Core.Constants;

using CMS.Core.DTOs;
using CMS.Core.Exceptions;
using CMS.Core.ViewModels;
using CMS.Data;
using CMS.Data.Models;
using CMS.Infrastructure.Services.Email;
using CMS.Infrastructure.Services.Files;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CMS.Infrastructure.Services.Users
{
    public class UserService : IUserService
    {
        private readonly CMSDbContext _db;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _usermanager;
        //constructor
        public UserService(CMSDbContext db, IMapper mapper, UserManager<User> usermanager, IFileService fileService, IEmailService emailService)
        {
            _db = db;
            _mapper = mapper;
            _usermanager = usermanager;
            _fileService = fileService;
            _emailService = emailService;
        }
        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Users.Where(x => !x.IsDelete &&
                 (x.FullName.Contains(query.GeneralSearch))||
                 (x.Email.Contains(query.GeneralSearch))||
                 (x.PhoneNumber.Contains(query.GeneralSearch))||
                 string.IsNullOrWhiteSpace(query.GeneralSearch))
                .AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var users = _mapper.Map<List<UserViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = users,
                meta = new Meta
                {
                    page = pagination.Page,
                    perpage = pagination.PerPage,
                    pages = pages,
                    total = dataCount
                }
            };
            return result;
        }
        public async Task<string> Create(CreateUserDto dto)
        {
            var EmailOrPhoneisExist = _db.Users.Any(x => !x.IsDelete &&
            (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber));

            if (EmailOrPhoneisExist)
            {
                throw new DuplicatedEmailOrPhoneException();
            }
            var user =_mapper.Map<User>(dto);
            user.UserName = dto.Email;
            if(dto.Image != null)
            {
               user.ImageUrl= await _fileService.SaveFile(dto.Image, FolderNames.ImagesFolder);
            }
           
            //CreateAsync returns a Task<IdentityResult>. When you await it, you get the IdentityResult.
            //What IdentityResult contains
            //Succeeded (bool): true if the user was created, false if not.
            //Errors(a list of IdentityError): each has a Code(like DuplicateEmail)
            //and a Description(a readable message like “Email ‘a@b.com’ is already taken.”)
            var result = await _usermanager.CreateAsync(user);
            var password = GeneratePassword();
            if (!result.Succeeded)
            {
                throw new OperationFailedException();
            }
            _emailService.Send(user.Email, "new Account !", $"Username is {user.Email} and Password is {password}");
            return user.Id;        
        
        }
        public async Task<string> Update(UpdateUserDto dto)
        {
            var EmailOrPhoneisExist = _db.Users.Any(x => !x.IsDelete &&
            (x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber)
            && x.Id != dto.Id);

            if (EmailOrPhoneisExist)
            {
                throw new DuplicatedEmailOrPhoneException();
            }
            var user = await _db.Users.FindAsync(dto.Id);
            var updatedUser =_mapper.Map<UpdateUserDto,User>(dto,user);
            user.UserName = dto.Email;
            if(dto.Image != null)
            {
                updatedUser.ImageUrl= await _fileService.SaveFile(dto.Image, FolderNames.ImagesFolder);
            }
            _db.Users.Update(updatedUser);
            await _db.SaveChangesAsync();
            return user.Id;        
        
        }

    

        public async Task<string> Delete(string Id)
        {
            var user =await _db.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if(user == null)
            {
                throw new EntityNotFoundException();
            }
            user.IsDelete = true;
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user.Id;

        }
        public async Task<UpdateUserDto> Get(string Id)
        {
            var user =await _db.Users.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if(user == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateUserDto>(user);

        }
        private string GeneratePassword()
        {
            return Guid.NewGuid().ToString().Substring(1, 8);
        }



    }
}
