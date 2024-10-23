using LIMS.Core;
using LIMS.Domain.AInR;
using LIMS.Domain.Report;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Wkhtmltopdf.NetCore;

namespace LIMS.Services.Common
{
    /// <summary>
    /// Generate invoice  products , shipment as pdf (from html template to pdf)
    /// </summary>
    public class WkPdfService : IPdfService
    {
        private const string _earTagsTemplate = "~/Views/PdfTemplates/EarTagsPdfTemplate.cshtml";
        private const string _trainingTemplate = "~/Views/PdfTemplates/trainingPdfTemplate.cshtml";
        private const string _subsidyTemplate = "~/Views/PdfTemplates/subsidyPdfTemplate.cshtml";
        private const string _productsFooter = "pdf/footers/products.html";
        private readonly IGeneratePdf _generatePdf;
        private readonly IViewRenderService _viewRenderService;
       
        public WkPdfService(IGeneratePdf generatePdf, IViewRenderService viewRenderService)
        {
            _generatePdf = generatePdf;
            _viewRenderService = viewRenderService;
        }

        public async Task PrintEarTagsToPdf(Stream stream, IList<EarTag> earTags)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (earTags == null)
                throw new ArgumentNullException("earTags");

            _generatePdf.SetConvertOptions(new ConvertOptions() {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,
                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },
                FooterHtml = CommonHelper.WebMapPath(_productsFooter)
            });

           
            var html = await _viewRenderService.RenderToStringAsync<IList<EarTag>>(_earTagsTemplate, earTags);
            var pdfBytes = _generatePdf.GetPDF(html);
            stream.Write(pdfBytes);
        }
        public async Task PrintSubsidyPdf(Stream stream, SubsidyReportModel reportModel)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            //if (earTags == null)
            //    throw new ArgumentNullException("earTags");

            _generatePdf.SetConvertOptions(new ConvertOptions() {
                PageSize = Wkhtmltopdf.NetCore.Options.Size.A4,
                PageMargins = new Wkhtmltopdf.NetCore.Options.Margins() { Bottom = 10, Left = 10, Right = 10, Top = 10 },
                FooterHtml = CommonHelper.WebMapPath(_productsFooter)
            });

            var html = await _viewRenderService.RenderToStringAsync<SubsidyReportModel>(_subsidyTemplate, reportModel);
            var pdfBytes = _generatePdf.GetPDF(html);
            stream.Write(pdfBytes);
        }

    }

}
