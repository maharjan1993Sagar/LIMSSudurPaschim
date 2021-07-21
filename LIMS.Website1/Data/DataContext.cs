using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LIMS.Website1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LIMS.Website1.Data
{
    public class DataContext
    {
        private readonly IConfiguration _config;
        string BaseURL="" ;
        string userId="";
        public DataContext(IConfiguration config)
        {
            _config = config;
            BaseURL = _config.GetValue<string>("Constants:BaseURL");
            userId = _config.GetValue<string>("Constants:UserId");
        }
        public async Task<List<BannerModel>> GetBanner()
        {
            var banners = new List<BannerModel>();
            string url = "Banner/GetBanner/"+userId;

            using (var httpClient = new HttpClient())
            {
                using (var response =await httpClient.GetAsync(BaseURL+url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    banners = JsonConvert.DeserializeObject<List<BannerModel>>(apiResponse);
                }
            }
            if (banners.Any())
            {
                banners = banners.Where(m => m.UserId == userId).ToList();
            }
            return banners;
        }
        public async Task<List<MainMenuModel>> GetMainMenuModel()
        {
            var mainMenus = new List<MainMenuModel>();
            string url = "MainMenu/GetMainMenu/"+userId;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseURL + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    mainMenus = JsonConvert.DeserializeObject<List<MainMenuModel>>(apiResponse);
                }
            }
            if (mainMenus.Any())
            {
                mainMenus = mainMenus.Where(m => m.UserId == userId).ToList();
            }
            return mainMenus;
        }
        public async Task<List<EmployeeModel>> GetEmployee()
        {
            var lst = new List<EmployeeModel>();
            string url = "Employee/GetEmployee/"+userId;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseURL + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<EmployeeModel>>(apiResponse);
                }
            }
            if (lst.Any())
            {
                lst = lst.Where(m => m.UserId == userId).ToList();
            }
            return lst;
        }
        public async Task<List<NewsEventTenderModel>> GetNewsEventTender(string type)
        {
            var lst = new List<NewsEventTenderModel>();
            string url = "NewsEvent/GetNewsEvent/"+userId;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseURL + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<NewsEventTenderModel>>(apiResponse);
                }
            }
            if (lst.Any())
            {
                lst = lst.Where(m => m.UserId == userId).ToList();
                if (!String.IsNullOrEmpty(type))
                { 
                lst = lst.Where(m => m.UserId == userId).Where(m=>m.Type==type).ToList();
                }
            }
            return lst;
        }
        public async Task<PageContentModel> GetPageContent(string name)
        {
            var lst = new List<PageContentModel>();
            string url = "PageContent/GetPageContent/"+userId;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseURL + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<PageContentModel>>(apiResponse);
                }
            }
            //if (lst!=null)
            //{
            //    lst = lst.Where(m => m.UserId == userId).ToList();
            //}
            return lst.FirstOrDefault(m=>m.PageName==name);
        }
        public async Task<ContactUsModel> GetContactUsModel()
        {
            var lst = new List<ContactUsModel>();
            string url = "ContactUs/GetContactUs/"+userId;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseURL + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<ContactUsModel>>(apiResponse);
                }
            }
            //if (lst!=null)
            //{
            //    lst = lst.Where(m => m.UserId == userId).ToList();
            //}
            return lst.FirstOrDefault();
        }
        public async Task<List<ImportantLinks>> GetImportantLinks()
        {
            var lst = new List<ImportantLinks>();
            string url = "ImportantLink/GetLinks/"+userId;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseURL + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<ImportantLinks>>(apiResponse);
                }
            }
            if (lst != null)
            {
                lst = lst.Where(m => m.UserId == userId).ToList();
            }
            return lst;
        } 
        
        public async Task<List<GalleryModel>> GetGallery()
        {
            var lst = new List<GalleryModel>();
            string url = "Gallery/GetGallery/"+userId;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(BaseURL + url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lst = JsonConvert.DeserializeObject<List<GalleryModel>>(apiResponse);
                }
            }
            if (lst != null)
            {
                lst = lst.Where(m => m.UserId == userId).ToList();
            }
            return lst;
        }
    }
}
