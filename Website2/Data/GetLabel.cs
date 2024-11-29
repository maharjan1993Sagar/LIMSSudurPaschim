using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using LIMS.Website1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Localization;
using System.Globalization;


namespace LIMS.Website1.Data
{
    public class GetLabel : IGetLabel
    {
        private IHostingEnvironment _env;
        public GetLabel(IHostingEnvironment env)
        {
            _env = env;
        }

        public List<LabelModel> GetAll()
        {
            List<LabelModel> lst = new List<LabelModel>();
            DataSet ds = new DataSet();
            ds.ReadXml(Path.Combine(_env.WebRootPath, "XML/Labels.xml"));
            DataView dvPrograms;
            dvPrograms = ds.Tables[0].DefaultView;
            dvPrograms.Sort = "Id";
            foreach (DataRowView dr in dvPrograms)
            {
                LabelModel model = new LabelModel();
                model.Id = Convert.ToString(dr[0]);
                model.Title = Convert.ToString(dr[1]);
                model.EnglishName = Convert.ToString(dr[2]);
                model.NepaliName = Convert.ToString(dr[3]);
                lst.Add(model);
            }
            return lst;
        }

        public LabelModel GetById(string Id)
        {
            var all = GetAll();
            if(all.Any())
            {
                var obj = all.FirstOrDefault(m => m.Id == Id);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }

        public LabelModel GetByTitle(string Title)
        {
            var all = GetAll();
            if (all.Any())
            {
                var obj = all.FirstOrDefault(m => m.Title == Title);
                if (obj != null)
                {
                    return obj;
                }
            }
            return null;
        }
        public string GetByName(string name, string culture)
        {
           culture = CultureInfo.CurrentCulture.ToString();

            var all = GetAll();
            if (all.Any())
            {
                if (all.Any(m => m.Title == name))
                {
                    var obj = all.FirstOrDefault(m => m.Title == name);
                    if (culture == "ne-NP")
                    {
                        return obj.NepaliName;
                    }
                    else if (culture == "en-US")
                    {
                        return obj.EnglishName;
                    }
                    else
                    {
                        return name;
                    }
                }
            }
            return name;
        }

        public string CreateUpdate(LabelModel model)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(Path.Combine(_env.WebRootPath, "Xml/labels.xml"));
                var items = (from item in xmlDoc.Descendants("Label") select item).ToList();
                XElement selected = items.Where(p => p.Element("Id").Value == model.Id.ToString()).FirstOrDefault();
                if (selected != null)
                {
                    selected.Remove();
                }
                xmlDoc.Save(Path.Combine(_env.WebRootPath, "Xml/labels.xml"));
                xmlDoc.Element("Labels").Add(new XElement("Label",
                    new XElement("Id", model.Id),
                    new XElement("Title", model.Title),
                    new XElement("EnglishName", model.EnglishName),
                    new XElement("NepaliName", model.NepaliName)
                    ));
                xmlDoc.Save(Path.Combine(_env.WebRootPath, "Xml/labels.xml"));
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public string Delete(string id)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(Path.Combine(_env.WebRootPath, "Xml/labels.xml"));
                var items = (from item in xmlDoc.Descendants("Label") select item).ToList();
                XElement selected = items.Where(p => p.Element("Id").Value == id).FirstOrDefault();
                selected.Remove();
                xmlDoc.Save(Path.Combine(_env.WebRootPath, "Xml/labels.xml"));

                return "Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
       
    }
}
