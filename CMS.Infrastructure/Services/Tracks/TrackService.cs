using AutoMapper;
using CMS.Core.DTOs;
using CMS.Core.Exceptions;
using CMS.Core.ViewModels;
using CMS.Data.Models;
using CMS.Data;
using CMS.Infrastructure.Services.Files;
using CMS.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Enums;

namespace CMS.Infrastructure.Services.Tracks
{
    public class TrackService: ITrackService
    {
        private readonly CMSDbContext _db;
        private readonly IMapper _mapper;
        private readonly IFileService _fileservice;

        public TrackService(CMSDbContext db, IMapper mapper, IFileService fileservice)
        {
            _db = db;
            _mapper = mapper;
            _fileservice = fileservice;
        }
        public async Task<ResponseDto> GetAll(Pagination pagination, Query query)
        {
            var queryString = _db.Tracks.Include(x=>x.Category).Include(x=>x.PublishedBy).Where(x=>!x.IsDelete);

            var dataCount = queryString.Count();
            var skipValue = pagination.GetSkipValue();
            var dataList = await queryString.Skip(skipValue).Take(pagination.PerPage).ToListAsync();
            var tracks = _mapper.Map<List<TrackViewModel>>(dataList);
            var pages = pagination.GetPages(dataCount);
            var result = new ResponseDto
            {
                data = tracks,
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
            var track = await _db.Tracks.SingleOrDefaultAsync(x=>x.Id == Id && !x.IsDelete);
            if(track == null)
            {
                throw new EntityNotFoundException();
            }
            track.IsDelete = true;
            _db.Tracks.Update(track);
            await _db.SaveChangesAsync();
            return track.Id;

        }
       public async Task<UpdateTrackDto> Get(int Id)
        {
            var track = await _db.Tracks.SingleOrDefaultAsync(x => x.Id == Id && !x.IsDelete);
            if (track == null)
            {
                throw new EntityNotFoundException();
            }
            return _mapper.Map<UpdateTrackDto>(track);
        }
       public async Task<int> Create(CreateTrackDto dto)
        {
            var track = _mapper.Map<Track>(dto);
            if(dto.Track != null)
            {
                track.TrackUrl = await _fileservice.SaveFile(dto.Track,"Tracks");
            }
            await _db.Tracks.AddAsync(track);
            await _db.SaveChangesAsync();
            return track.Id;
        }
       public async Task<int> Update(UpdateTrackDto dto)
        {
            var track = await _db.Tracks.SingleOrDefaultAsync(x => x.Id == dto.Id && !x.IsDelete);
            if (track == null)
            {
                throw new EntityNotFoundException();
            }
            var updatedTrack = _mapper.Map(dto,track);
            if(dto.Track != null)
            {
                updatedTrack.TrackUrl = await _fileservice.SaveFile(dto.Track,"Tracks");
            }
             _db.Tracks.Update(updatedTrack);
            await _db.SaveChangesAsync();
            return updatedTrack.Id;
        }
        public async Task<int> UpdateStatus(int Id, ContentStatus status)
        {
            var track = await _db.Tracks.SingleOrDefaultAsync(x => !x.IsDelete && x.Id == Id);
            if (track == null)
            {
                throw new EntityNotFoundException();
            }
            track.Status = status;
            _db.Tracks.Update(track);
            await _db.SaveChangesAsync();
            return track.Id;

        }
    }
}
