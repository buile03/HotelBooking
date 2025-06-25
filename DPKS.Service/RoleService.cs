using DPKS.Common;
using DPKS.Common.Helper;
using DPKS.Common.Helper;
using DPKS.Common.Result;
using DPKS.Data.EF;
using DPKS.Data.Entites;
using DPKS.Model.Role;
using DPKS.Model.Role.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPKS.Service
{
    public interface IRoleService
    {
        Task<Result<PagedResult<DanhSachRoleVm>>> GetPaging(RoleSearchRequest request);
        Task<Result<int>> Create(RoleCreateRequest request);
        Task<Result<int>> Update(RoleUpdateRequest request);
        Task<Result<int>> Delete(DeleteRequest request);
        Task<RoleUpdateRequest> GetById(int id);
    }

    public class RoleService : BaseService, IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly AppDbContext _context;

        public RoleService(RoleManager<ApplicationRole> roleManager ,AppDbContext context, IStorageService storageService = null) : base(context, storageService)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<Result<int>> Create(RoleCreateRequest request)
        {
            try
            {
                var role = new ApplicationRole
                {
                    Name = request.Name,
                    Descritption = request.Description,
                    IsActive = true
                };

                var result = await _roleManager.CreateAsync(role);
                return result.Succeeded 
                    ? Result<int>.Success("Tạo vai trò thành công", role.Id)
                    : Result<int>.Error(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Delete(DeleteRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return Result<int>.Error("Không tìm thấy vai trò!");

                role.IsDeleted = true;
                _context.Roles.Update(role);
                var result = await SaveChange();

                return result > 0
                    ? Result<int>.Success("Xóa vai trò thành công", role.Id)
                    : Result<int>.Error("Xóa thất bại");
            }
            catch
            {
                throw;
            }
        }

        public async Task<RoleUpdateRequest> GetById(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null) return null;

            return new RoleUpdateRequest
            {
                Id = role.Id.EncodeId1(),
                Name = role.Name,
                Description = role.Descritption
            };
        }

        public async Task<Result<PagedResult<DanhSachRoleVm>>> GetPaging(RoleSearchRequest request)
        {
            try
            {
                var query = _context.Roles
                    .Where(p => p.IsActive && !p.IsDeleted)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(request.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(request.Keyword));
                }

                var totalRecords = await query.CountAsync();
                var roles = await query
                    .OrderBy(x => x.Id)
                    .Skip((request.PageIndex - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x => new DanhSachRoleVm
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Descritption,
                        IsActive = x.IsActive
                    }).ToListAsync();

                return Result<PagedResult<DanhSachRoleVm>>.Success("Lấy danh sách vai trò thành công", new PagedResult<DanhSachRoleVm>
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = totalRecords,
                    Items = roles
                });
            }
            catch
            {
                throw;
            }
        }

        public async Task<Result<int>> Update(RoleUpdateRequest request)
        {
            try
            {
                int id = request.Id.DecodeId();
                var role = await _context.Roles.FindAsync(id);
                if (role == null)
                    return Result<int>.Error("Không tìm thấy vai trò!");

                role.Name = request.Name;
                role.Descritption = request.Description;

                _context.Roles.Update(role);
                var result = await SaveChange();

                return result > 0
                    ? Result<int>.Success("Cập nhật vai trò thành công", role.Id)
                    : Result<int>.Error("Cập nhật thất bại");
            }
            catch
            {
                throw;
            }
        }
    }
}
