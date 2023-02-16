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
                string assignGroupDTOStr = JsonConvert.SerializeObject(assignGroupDTO);

                var response = await client.PutAsync($"http://172.16.12.165:7006/api/Group/AssignGroup?GroupName={assignGroupDTO.GroupName}&EmployeeNumber={assignGroupDTO.EmployeeNumber}",
                      new StringContent(assignGroupDTOStr, Encoding.UTF8,
                      "application/json"));

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
        }  //Done

        public async Task<GroupResponse> CreateGroup(CreateGroupDTO createGroupDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string createGroupDTOStr = JsonConvert.SerializeObject(createGroupDTO);

                var response = await client.PostAsync($"http://172.16.12.165:7006/api/Group/CreateGroup?GroupName={createGroupDTO.GroupName}",
                      new StringContent(createGroupDTOStr, Encoding.UTF8,
                      "application/json"));

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
        } // Done

        public async Task<GroupResponse> DeleteGroup(DeleteGroupDTO deleteGroupDTO)  //Done
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string deleteGroupDTOStr = JsonConvert.SerializeObject(deleteGroupDTO);

                var response = await client.DeleteAsync($"http://172.16.12.165:7006/api/Group/DeleteGroup?GroupName={deleteGroupDTO.GroupName}");

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
                var response = await client.GetAsync($"http://172.16.12.165:7006/api/Group/GetGroup");

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

        public async Task<List<GroupList>> DisplayGroupbyGroupName(string GroupName)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.GetAsync($"http://172.16.12.165:7006/api/Group/DisplayGroupbyGroupName?GroupName={GroupName}");

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
        
        public async Task<List<GroupList>> DisplayGroupFromUsers()
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.GetAsync("http://172.16.12.165:7006/api/Group/DisplayGroupFromUsers");

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
                string updateGroupDTOStr = JsonConvert.SerializeObject(updateGroupDTO);

                var response = await client.PutAsync($"http://172.16.12.165:7006/api/Group/UpdateGroup?GroupName={updateGroupDTO.GroupName}",
                    new StringContent(updateGroupDTOStr, Encoding.UTF8,
                      "application/json"));

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
