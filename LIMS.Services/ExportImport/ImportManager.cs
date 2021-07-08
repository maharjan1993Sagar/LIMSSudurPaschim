using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Domain.Directory;
using LIMS.Domain.Media;
using LIMS.Domain.RationBalance;
using LIMS.Services.Basic;
using LIMS.Services.Directory;
using LIMS.Services.ExportImport.Help;
using LIMS.Services.Media;

using Microsoft.AspNetCore.StaticFiles;
using NPOI.XSSF.UserModel;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LIMS.Services.ExportImport
{
    /// <summary>
    /// Import manager
    /// </summary>
    public partial class ImportManager : IImportManager
    {
        #region Fields

        private readonly IPictureService _pictureService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IPujigatKharchaKharakramService _pujigatKharchaKharakramService;

        #endregion

        #region Ctor

        public ImportManager(         
            IPictureService pictureService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService)
        {
            _pictureService = pictureService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
        }

        #endregion

        #region Utilities

        protected virtual PropertyManager<T> GetPropertyManager<T>(NPOI.SS.UserModel.ISheet worksheet)
        {
            var properties = new List<PropertyByName<T>>();
            var poz = 0;
            while (true)
            {
                try
                {
                    var cell = worksheet.GetRow(0).Cells[poz];

                    if (cell == null || string.IsNullOrEmpty(cell.StringCellValue))
                        break;

                    poz += 1;
                    properties.Add(new PropertyByName<T>(cell.StringCellValue.ToLower()));
                }
                catch
                {
                    break;
                }
            }
            return new PropertyManager<T>(properties.ToArray());
        }
        
        protected virtual string GetMimeTypeFromFilePath(string filePath)
        {
            new FileExtensionContentTypeProvider().TryGetContentType(filePath, out string mimeType);
            //set to jpeg in case mime type cannot be found
            if (mimeType == null)
                mimeType = "image/jpeg";
            return mimeType;
        }

        /// <summary>
        /// Creates or loads the image
        /// </summary>
        /// <param name="picturePath">The path to the image file</param>
        /// <param name="name">The name of the object</param>
        /// <param name="picId">Image identifier, may be null</param>
        /// <returns>The image or null if the image has not changed</returns>
        protected virtual async Task<Picture> LoadPicture(string picturePath, string name, string picId = "")
        {
            if (String.IsNullOrEmpty(picturePath) || !File.Exists(picturePath))
                return null;

            var mimeType = GetMimeTypeFromFilePath(picturePath);
            var newPictureBinary = File.ReadAllBytes(picturePath);
            var pictureAlreadyExists = false;
            if (!String.IsNullOrEmpty(picId))
            {
                //compare with existing product pictures
                var existingPicture = await _pictureService.GetPictureById(picId);

                var existingBinary = await _pictureService.LoadPictureBinary(existingPicture);
                //picture binary after validation (like in database)
                var validatedPictureBinary = _pictureService.ValidatePicture(newPictureBinary, mimeType);
                if (existingBinary.SequenceEqual(validatedPictureBinary) ||
                    existingBinary.SequenceEqual(newPictureBinary))
                {
                    pictureAlreadyExists = true;
                }
            }

            if (pictureAlreadyExists) return null;

            var newPicture = await _pictureService.InsertPicture(newPictureBinary, mimeType,
                _pictureService.GetPictureSeName(name));
            return newPicture;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Import states from TXT file
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>Number of imported states</returns>
        public virtual async Task<int> ImportStatesFromTxt(Stream stream)
        {
            int count = 0;
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (String.IsNullOrWhiteSpace(line))
                        continue;
                    string[] tmp = line.Split(',');

                    if (tmp.Length != 5)
                        throw new LIMSException("Wrong file format");

                    //parse
                    var countryTwoLetterIsoCode = tmp[0].Trim();
                    var name = tmp[1].Trim();
                    var abbreviation = tmp[2].Trim();
                    bool published = Boolean.Parse(tmp[3].Trim());
                    int displayOrder = Int32.Parse(tmp[4].Trim());

                    var country = await _countryService.GetCountryByTwoLetterIsoCode(countryTwoLetterIsoCode);
                    if (country == null)
                    {
                        //country cannot be loaded. skip
                        continue;
                    }

                    //import
                    var states = await _stateProvinceService.GetStateProvincesByCountryId(country.Id, showHidden: true);
                    var state = states.FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

                    if (state != null)
                    {
                        state.Abbreviation = abbreviation;
                        state.Published = published;
                        state.DisplayOrder = displayOrder;
                        await _stateProvinceService.UpdateStateProvince(state);
                    }
                    else
                    {
                        state = new StateProvince {
                            CountryId = country.Id,
                            Name = name,
                            Abbreviation = abbreviation,
                            Published = published,
                            DisplayOrder = displayOrder,
                        };
                        await _stateProvinceService.InsertStateProvince(state);
                    }
                    count++;
                }
            }

            return count;
        }





        protected virtual void PrepareCategoryMapping(PujigatKharchaKharakram category, PropertyManager<PujigatKharchaKharakram> manager)
        {
            foreach (var property in manager.GetProperties)
            {
                switch (property.PropertyName.ToLower())
                {
                    case "programsummery":
                        category.ProgramSummery = property.StringValue;
                        break;
                    case "program":
                        category.Program = property.StringValue;
                        break;
                    case "remarks":
                        category.Remarks = property.StringValue;
                        break;
                    case "limbis_code":
                        category.Limbis_Code = property.StringValue;
                        break;
                    case "kharchacode":
                        category.kharchaCode = property.StringValue;
                        break;
                    case "crathamchaumasikbadjet":
                        category.PrathamChaumasikBadjet = property.StringValue;
                        break;
                    case "prathamchaumasikparimam":
                        category.PrathamChaumasikParimam = property.StringValue;
                        break;
                    case "prathamchaumasikvar":
                        category.PrathamChaumasikVar = property.StringValue;
                        break;
                    case "dosrochaumasikvar":
                        category.DosroChaumasikVar = property.StringValue;
                        break;
                    case "dosroChaumasikparimam":
                        category.DorsoChaumasikParimam = property.StringValue;
                        break;
                    case "dosroChaumasikbadjet":
                        category.DosroChaumasikBadjet = property.StringValue;
                        break;

                    case "tesrochaumasikvar":
                        category.TesroChaumasikVar = property.StringValue;
                        break;
                    case "tesrochaumasikparimam":
                        category.TesroChaumasikParimam = property.StringValue;
                        break;
                    case "tesrochaumasikbadjet":
                        category.TesroChaumasikBadjet = property.StringValue;
                        break;
                    case "barshikbadjet":
                        category.BarsikBajet = property.StringValue;
                        break;
                    case "barshikbhar":
                        category.BarshikBhar = property.StringValue;
                        break;
                    case "barshikparinam":
                        category.BarshikParinam = property.StringValue;
                        break;

                    case "unit":
                        category.Unit = property.StringValue;
                        break;
                        //    case "pagesize":
                        //        category.PageSize = property.IntValue > 0 ? property.IntValue : 10;
                        //        break;
                        //    case "allowcustomerstoselectpageSize":
                        //        category.AllowCustomersToSelectPageSize = property.BooleanValue;
                        //        break;
                        //    case "pagesizeoptions":
                        //        category.PageSizeOptions = property.StringValue;
                        //        break;
                        //    case "priceranges":
                        //        category.PriceRanges = property.StringValue;
                        //        break;
                        //    case "published":
                        //        category.Published = property.BooleanValue;
                        //        break;
                        //    case "displayorder":
                        //        category.DisplayOrder = property.IntValue;
                        //        break;
                        //    case "showonhomepage":
                        //        category.ShowOnHomePage = property.BooleanValue;
                        //        break;
                        //    case "includeintopmenu":
                        //        category.IncludeInTopMenu = property.BooleanValue;
                        //        break;
                        //    case "showonsearchbox":
                        //        category.ShowOnSearchBox = property.BooleanValue;
                        //        break;
                        //    case "searchboxdisplayorder":
                        //        category.SearchBoxDisplayOrder = property.IntValue;
                        //        break;
                        //    case "flag":
                        //        category.Flag = property.StringValue;
                        //        break;
                        //    case "flagstyle":
                        //        category.FlagStyle = property.StringValue;
                        //        break;
                        //    case "icon":
                        //        category.Icon = property.StringValue;
                        //        break;
                        //    case "parentcategoryid":
                        //        if (!string.IsNullOrEmpty(property.StringValue) && property.StringValue != "0")
                        //            category.ParentCategoryId = property.StringValue;
                        //        break;
                        //}
                }
            }















            //public virtual async Task ImportFeedFromXlsx(Stream stream)
            //{
            //    var workbook = new XSSFWorkbook(stream);
            //    var worksheet = workbook.GetSheetAt(0);
            //    if (worksheet == null)
            //        throw new LIMSException("No worksheet found");

            //    var manager = GetPropertyManager<FeedLibrary>(worksheet);

            //    var templates = await _manufacturerTemplateService.GetAllManufacturerTemplates();

            //    for (var iRow = 1; iRow < worksheet.PhysicalNumberOfRows; iRow++)
            //    {

            //        manager.ReadFromXlsx(worksheet, iRow);
            //        var manufacturerid = manager.GetProperty("id") != null ? manager.GetProperty("id").StringValue : string.Empty;
            //        var manufacturer = string.IsNullOrEmpty(manufacturerid) ? null : await _manufacturerService.GetManufacturerById(manufacturerid);

            //        var isNew = manufacturer == null;

            //        manufacturer ??= new Manufacturer();

            //        if (isNew)
            //        {
            //            manufacturer.CreatedOnUtc = DateTime.UtcNow;
            //            manufacturer.ManufacturerTemplateId = templates.FirstOrDefault()?.Id;
            //            if (!string.IsNullOrEmpty(manufacturerid))
            //                manufacturer.Id = manufacturerid;
            //        }

            //        PrepareManufacturerMapping(manufacturer, manager, templates);


            //        var picture = manager.GetProperty("picture") != null ? manager.GetProperty("sename").StringValue : "";
            //        if (!string.IsNullOrEmpty(picture))
            //        {
            //            var _picture = await LoadPicture(picture, manufacturer.Name,
            //                isNew ? "" : manufacturer.PictureId);
            //            if (_picture != null)
            //                manufacturer.PictureId = _picture.Id;
            //        }
            //        manufacturer.UpdatedOnUtc = DateTime.UtcNow;

            //        if (isNew)
            //            await _manufacturerService.InsertManufacturer(manufacturer);
            //        else
            //            await _manufacturerService.UpdateManufacturer(manufacturer);

            //        var sename = manager.GetProperty("sename") != null ? manager.GetProperty("sename").StringValue : manufacturer.Name;
            //        sename = await manufacturer.ValidateSeName(sename, manufacturer.Name, true, _seoSetting, _urlRecordService, _languageService);
            //        manufacturer.SeName = sename;
            //        await _manufacturerService.UpdateManufacturer(manufacturer);
            //        await _urlRecordService.SaveSlug(manufacturer, manufacturer.SeName, "");

            //    }

            //}
        }
        public virtual async Task ImportCategoryFromXlsx(Stream stream)
        {
            var workbook = new XSSFWorkbook(stream);
            var worksheet = workbook.GetSheetAt(0);
            if (worksheet == null)
                throw new CmsException("No worksheet found");

            var manager = GetPropertyManager<PujigatKharchaKharakram>(worksheet);


            for (var iRow = 1; iRow < worksheet.PhysicalNumberOfRows; iRow++)
            {
                manager.ReadFromXlsx(worksheet, iRow);

                // var category = string.IsNullOrEmpty(categoryid) ? null : await _categoryService.GetCategoryById(categoryid);

                //var isNew = category == null;

                //category ??= new Category();

                var pujigatKharcha = new PujigatKharchaKharakram();
                    pujigatKharcha.CreatedAt = DateTime.UtcNow;
                    
                

                PrepareCategoryMapping(pujigatKharcha, manager);
              
                 await _pujigatKharchaKharakramService.InsertPujigatKharchaKharakram(pujigatKharcha);
               
                
            }

        }

        #endregion
    }
}