using DPKS.Data.EF;
using DPKS.Model.Organizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IOrganizationService
    {
        Task<List<OrganizationVm>> GetAll(int? parentId = null);
    }
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly AppDbContext _context;

        public OrganizationService(AppDbContext context) : base(context)
        {
            _action = string.Empty;
            _context = context;
        }
        private List<OrganizationVm> lstOrganization;

        public async Task<List<OrganizationVm>> GetAll(int? parentId = null)
        {
            try
            {
                var query = from t in _context.Organizations
                            //where !t.IsDelete && t.IsStatus
                            select new OrganizationVm()
                            {
                                Id = t.Id,
                                Code = t.Code,
                                Name = t.Name,
                               // IsStatus = t.IsStatus,
                                //Order = t.Order,
                                //ParentName = t.ParentOrganization.Name,
                                ParentId = t.ParentId
                            };

                //var data = await query.ToListAsync();
                lstOrganization = new List<OrganizationVm>();
                //lstOrganization = GetOrganizations(data, parentId, 0);

                return lstOrganization;
            }
            catch
            {
                throw;
            }
        }
    }
}
