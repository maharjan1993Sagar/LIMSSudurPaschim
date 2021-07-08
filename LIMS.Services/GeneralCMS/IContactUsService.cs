using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LIMS.Domain;
using LIMS.Domain.Breed;
using LIMS.Domain.Data;
using LIMS.Domain.GeneralCMS;

namespace LIMS.Services.GeneralCMS
{
    public interface IContactUsService
    {
        Task<List<ContactUs>> GetAll();
        Task<IPagedList<ContactUs>> GetContact(int pageIndex = 0, int pageSize = int.MaxValue);
        Task<IPagedList<ContactUs>> GetContactByUser(int pageIndex = 0, int pageSize = int.MaxValue);

        Task DeleteContact(ContactUs contact);

        Task InsertContact(ContactUs contact);

        Task UpdateContact(ContactUs contact);
        Task<ContactUs> GetContactById(string id);
    }
}
