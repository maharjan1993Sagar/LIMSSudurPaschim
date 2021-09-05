using LIMS.Website1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Website1.Data
{
    public interface IGetLabel
    {
        string GetByName(string title,string culture);
        LabelModel GetById(string id);
        LabelModel GetByTitle(string title);
        List<LabelModel> GetAll();
        string CreateUpdate(LabelModel obj);
        string Delete(string id);

    }
}
