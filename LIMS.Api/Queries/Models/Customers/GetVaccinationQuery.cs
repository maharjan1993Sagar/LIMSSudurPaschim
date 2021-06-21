using LIMS.Api.DTOs.AnimalHealth;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIMS.Api.Queries.Models.Customers
{
    public class GetVaccinationQuery: IRequest<IList<VaccineDto>>
    {
    }

}
