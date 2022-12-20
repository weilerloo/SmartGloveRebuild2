using Newtonsoft.Json;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.GroupResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public class GroupServices : IGroupServices
    {
        public async Task<GroupResponse> AssignGroup(AssignGroupDTO assignGroupDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.DeleteAsync("http://172.16.12.149:7006/api/Group/AssignGroup");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GroupResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<GroupResponse> CreateGroup(CreateGroupDTO createGroupDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.DeleteAsync("http://172.16.12.149:7006/api/Group/CreateGroup");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GroupResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<GroupResponse> DeleteGroup(DeleteGroupDTO deleteGroupDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.DeleteAsync("http://172.16.12.149:7006/api/Group/DeleteGroup");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GroupResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }
    
        public async Task<List<GroupList>> DisplayGroup()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.GetAsync("http://172.16.12.149:7006/api/Group/GetGroup");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<GroupList>>(json);
                }
                else
                {
                    return null;
                }
            }
        }
    
        public async Task<GroupResponse> UpdateGroup(UpdateGroupDTO updateGroupDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.DeleteAsync("http://172.16.12.149:7006/api/Group/UpdateGroup");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GroupResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
