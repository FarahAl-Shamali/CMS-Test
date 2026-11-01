using CMS.Core.ViewModels;
using CMS.Data;
using CMS.Infrastructure.Services.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using CMS.Core.Enums;
namespace CMS.Infrastructure.Services
{
    public class DashBoardService: IDashBoardService
    {
        private readonly CMSDbContext _db;
        public DashBoardService(CMSDbContext db)
        {
            _db = db;
        }

        public async Task<DashBoardViewModel> GetData()
        {
            var data = new DashBoardViewModel();
            data.NoOfUsers =await _db.Users.CountAsync(x=>!x.IsDelete);
            data.NoOfTrack =await _db.Tracks.CountAsync(x=>!x.IsDelete);
            data.NoOfPost =await _db.Posts.CountAsync(x=>!x.IsDelete);
            data.NoOfAdvertisement =await _db.Advertisments.CountAsync(x=>!x.IsDelete);
            return data;
        }
        public async Task<List<PieChartViewModel>> GetUserTypeChart()
        {
            var data = new List<PieChartViewModel>();
            data.Add(new PieChartViewModel() 
            { 
                Key = "Administrator",
                Value =await _db.Users.CountAsync(x=> !x.IsDelete && x.UserType == UserType.Administrator),
                Color= "#6B552F"
            });
            data.Add(new PieChartViewModel() 
            { 
                Key = "ArticleAuthor",
                Value = await _db.Users.CountAsync(x => !x.IsDelete && x.UserType == UserType.ArticleAuthor),
                Color = "#456B2F"
            });
            data.Add(new PieChartViewModel() 
            { 
                Key = "TrackAdministrator",
                Value = await _db.Users.CountAsync(x => !x.IsDelete && x.UserType == UserType.TrackAdministrator),
                Color = "#636B2F"
            });
            data.Add(new PieChartViewModel() 
            { 
                Key = "AdvertisementOwner",
                Value = await _db.Users.CountAsync(x => !x.IsDelete && x.UserType == UserType.AdvertisementOwner),
                Color = "#BAC095"
            });
           
            return data;
        }
        public async Task<List<PieChartViewModel>> GetContentTypeChart()
        {
            var data = new List<PieChartViewModel>();
            data.Add(new PieChartViewModel() 
            { 
                Key = "Track",
                Value =await _db.Tracks.CountAsync(x=> !x.IsDelete),
                Color= "#BAC095"
            });
            data.Add(new PieChartViewModel() 
            { 
                Key = "Post",
                Value = await _db.Posts.CountAsync(x => !x.IsDelete),
                Color = "#636B2F"
            });
           
           
            return data;
        }
        public async Task<List<PieChartViewModel>> GetContentByMonthChart()
        {

            var data = new List<PieChartViewModel>();
            data.Add(new PieChartViewModel()
            {
                Key = "Jan",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 1).Count(),
                Color = "#A8D8EA"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Feb",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 2).Count(),
                Color = "#AA96DA"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Mar",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 3).Count(),
                Color = "#FCBAD3"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Apr",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 4).Count(),
                Color = "#FFFFD2"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "May",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 5).Count(),
                Color = "#B5EAD7"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Jun",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 6).Count(),
                Color = "#C7CEEA"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Jul",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 7).Count(),
                Color = "#FFDAC1"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Aug",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 8).Count(),
                Color = "#E2F0CB"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Sep",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 9).Count(),
                Color = "#9BDEAC"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Oct",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 10).Count(),
                Color = "#F6C6EA"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Nov",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 11).Count(),
                Color = "#A0CED9"
            });
            data.Add(new PieChartViewModel()
            {
                Key = "Dec",
                Value = _db.Posts.Where(x => !x.IsDelete && x.CreatedAt.Date.Month == 12).Count(),
                Color = "#EAC4D5"
            });

            return data;
        }
    }
}
