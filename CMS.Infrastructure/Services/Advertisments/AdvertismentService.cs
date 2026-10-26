using AutoMapper;
using CMS.Core.DTOs;
using CMS.Core.Exceptions;
using CMS.Core.ViewModels;
using CMS.Data;
using CMS.Data.Models;
using CMS.Infrastructure.Services.Advertisments;
using CMS.Infrastructure.Services.Files;
using CMS.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Infrastructure.Services
{
    public class AdvertismentService: IAdvertismentService
    {
        private readonly CMSDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userservice;
        private readonly IFileService _fileservice;

        public AdvertismentService(CMSDbContext db, IMapper mapper, IUserService userservice, IFileService fileservice) 
        {
            _db = db;
            _mapper = mapper;
            _userservice = userservice;
            _fileservice = fileservice;
        }
        public async Task<List<UserViewModel>> GetAdvertismentOwners() 
        {
            var users =await _db.Users.Where(x => !x.IsDelete && x.UserType == Core.Enums.UserType.AdvertisementOwner).ToListAsync();
            return _mapper.Map<List<UserViewModel>>(users);
        
        }
        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Advertisments.Include(x => x.Owner).Where(x => !x.IsDelete).AsQueryable();

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var advertisements = _mapper.Map<List<AdvertismentViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = advertisements,
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
    
        public async Task<int> Create(CreateAdvertismentDto dto)
        {

            if (dto.StartDate >= dto.EndDate)
            {
                throw new InvalidDateException();
            }

            var advertisement = _mapper.Map<Advertisment>(dto);
            if (dto.Image != null)
            {
                advertisement.ImageUrl = await _fileservice.SaveFile(dto.Image, "Images");
            }

            if (!string.IsNullOrWhiteSpace(dto.OwnerId))
            {
                advertisement.OwnerId = dto.OwnerId;
            }

            await _db.Advertisments.AddAsync(advertisement);
            await _db.SaveChangesAsync();

            if (advertisement.OwnerId == null)
            {
                var userId = await _userservice.Create(dto.Owner);
                advertisement.OwnerId = userId;

                _db.Advertisments.Update(advertisement);
                await _db.SaveChangesAsync();

            }

            return advertisement.Id;
        }
        public async Task<int> Delete(int Id)
        {
            var advertisment = await _db.Advertisments.SingleOrDefaultAsync(x=>!x.IsDelete && x.Id == Id);
            if(advertisment == null)
            {
                throw new EntityNotFoundException();
            }
            advertisment.IsDelete = true;
            _db.Advertisments.Update(advertisment);
            await _db.SaveChangesAsync();
            return advertisment.Id;


        }


    }
}
