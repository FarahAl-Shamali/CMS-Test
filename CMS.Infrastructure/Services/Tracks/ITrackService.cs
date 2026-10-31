using CMS.Core.DTOs;
using CMS.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Infrastructure.Services.Tracks
{
    public interface ITrackService
    {
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<UpdateTrackDto> Get(int Id);
        Task<int> Create(CreateTrackDto dto);
        Task<int> Update(UpdateTrackDto dto);
        Task<int> Delete(int Id);
        Task<int> UpdateStatus(int Id, ContentStatus status);
    }
}
