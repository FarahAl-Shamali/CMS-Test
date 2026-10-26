using CMS.Core.DTOs;
using CMS.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Infrastructure.Services.Advertisments
{
    public interface IAdvertismentService
    {
        Task<int> Create(CreateAdvertismentDto dto);
        Task<int> Delete(int Id);
        Task<ResponseDto> GetAll(Pagination pagination, Query query);
        Task<List<UserViewModel>> GetAdvertismentOwners();

    }
}
