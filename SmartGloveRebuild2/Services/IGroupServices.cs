using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.GroupResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public interface IGroupServices
    {
        Task<GroupResponse> CreateGroup(CreateGroupDTO createGroupDTO);
        Task<GroupResponse> DeleteGroup(DeleteGroupDTO deleteGroupDTO);
        Task<List<GroupList>> DisplayGroup();
        Task<GroupResponse> UpdateGroup(UpdateGroupDTO updateGroupDTO);
        Task<GroupResponse> AssignGroup(AssignGroupDTO assignGroupDTO);
    }
}
