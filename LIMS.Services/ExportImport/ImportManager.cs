using LIMS.Core;
using LIMS.Domain.Bali;
using LIMS.Domain.Directory;
using LIMS.Domain.Media;
using LIMS.Domain.RationBalance;
using LIMS.Services.Basic;
using LIMS.Services.Directory;
using LIMS.Services.ExportImport.Help;
using LIMS.Services.Media;
using LIMS.Web.Areas.Admin.Helper;
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
        private readonly IFiscalYearService _fiscalYearService;
        private readonly IWorkContext _workContext;


        #endregion

        #region Ctor

        public ImportManager(         
            IPictureService pictureService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IPujigatKharchaKharakramService pujigatKharchaKharakramService,
            IFiscalYearService fiscalYearService,
             IWorkContext workContext
            )
        {
            _pictureService = pictureService;
            _countryService = countryService;
            _stateProvinceService = stateProvinceService;
            _pujigatKharchaKharakramService = pujigatKharchaKharakramService;
            _fiscalYearService = fiscalYearService;
            _workContext = workContext;
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
                        category.Limbis_Code = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        
                        break;
                    case "kharchacode":
                        category.kharchaCode = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;
                    case "1st_quarter_budget":
                        category.PrathamChaumasikBadjet = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;
                    case "1st_quarter_qty":
                        category.PrathamChaumasikParimam = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;

                        

                    case "2nd_quarter_qty":
                        category.DorsoChaumasikParimam = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;
                    case "2nd_quater_budget":
                        category.DosroChaumasikBadjet = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;


                    case "3rd_quarter_qty":
                        category.TesroChaumasikParimam = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;
                    case "3rd_quarter_budget":
                        category.TesroChaumasikBadjet = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;
                    case "yearly_budget":
                        category.BarsikBajet = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;

                    case "yearly_qty":
                        category.BarshikParinam = NumberHelper.EnglishToNepaliNumber(property.StringValue);
                        break;

                    case "unit":
                        category.Unit = property.StringValue;
                        break;

                    case "expenses_category":
                        category.Expenses_category = property.StringValue;
                        break;
                }

            }















            
        }
        public virtual async Task ImportCategoryFromXlsx(Stream stream,string Type, string FiscalYear,string ProgramType)
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
                pujigatKharcha.CreatedBy = _workContext.CurrentCustomer.Id;
                pujigatKharcha.Type = Type;
                pujigatKharcha.FiscalYearId = FiscalYear;
                pujigatKharcha.FiscalYear =await _fiscalYearService.GetFiscalYearById(FiscalYear);
                pujigatKharcha.ProgramType = ProgramType;

                PrepareCategoryMapping(pujigatKharcha, manager);
                if (!string.IsNullOrEmpty(pujigatKharcha.Limbis_Code))
                {
                    if(await _pujigatKharchaKharakramService.GetPujigatKharchaKharakramByLmBIsCode(pujigatKharcha.Limbis_Code))
                    {
                        await _pujigatKharchaKharakramService.UpdatePujigatKharchaKharakram(pujigatKharcha);

                    }
                    else
                    {
                        await _pujigatKharchaKharakramService.InsertPujigatKharchaKharakram(pujigatKharcha);

                    }
                }
               
                
            }

        }

        #endregion
    }
}