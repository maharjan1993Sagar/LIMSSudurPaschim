using LIMS.Domain.Customers;
using LIMS.Domain.Media;
using LIMS.Services.Common;
using LIMS.Services.Media;
using LIMS.Web.Features.Models.Customers;
using LIMS.Web.Models.Customer;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace LIMS.Web.Features.Handlers.Customers
{
    public class GetAvatarHandler : IRequestHandler<GetAvatar, CustomerAvatarModel>
    {
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;

        public GetAvatarHandler(IPictureService pictureService,
            MediaSettings mediaSettings)
        {
            _pictureService = pictureService;
            _mediaSettings = mediaSettings;
        }

        public async Task<CustomerAvatarModel> Handle(GetAvatar request, CancellationToken cancellationToken)
        {
            var model = new CustomerAvatarModel();
            model.AvatarUrl = await _pictureService.GetPictureUrl(request.Customer.GetAttributeFromEntity<string>(SystemCustomerAttributeNames.AvatarPictureId),
                _mediaSettings.AvatarPictureSize, false);

            return model;
        }
    }
}
