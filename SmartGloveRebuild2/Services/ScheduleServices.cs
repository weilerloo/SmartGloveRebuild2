using Newtonsoft.Json;
using SmartGloveRebuild2.Models.ClerkDTO;
using SmartGloveRebuild2.Models.Group;
using SmartGloveRebuild2.Models.Schedule;
using SmartGloveRebuild2.Models.ScheduleResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGloveRebuild2.Services
{
    public class ScheduleServices : IScheduleServices
    {
        public async Task<EmployeeAddScheduleResponse> EmployeeAddSchedule(EmployeeAddScheduleDTO addScheduleDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string addScheduleDTOStr = JsonConvert.SerializeObject(addScheduleDTO);

                var response = await client.PostAsync("http://172.16.12.158:7006/api/Schedule/EmployeeAddSchedule",
                      new StringContent(addScheduleDTOStr, Encoding.UTF8,
                      "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<EmployeeAddScheduleResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }  //Done, not test yet

        public async Task<List<GetScheduleByGroupResponse>> GetSchedulebyGroup(CreateGroupDTO createGroupDTO) //Done, not test yet, API Get group not done yet
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Group/GetGroupSchedule?GroupName={createGroupDTO.GroupName}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<GetScheduleByGroupResponse>>(json);
                }
                else
                {
                    return null;
                }
            }
        }  //Done, IP not change yet

        public async Task<List<GetScheduleByGroupandDateResponse>> GetSchedulebyGroupandDate(GetSchedulebyGroupandDateDTO getSchedulebyGroupandDateDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string getSchedulebyGroupandDateDTOStr = JsonConvert.SerializeObject(getSchedulebyGroupandDateDTO);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Schedule/GetSchedulebyGroupandDate?GruopName={getSchedulebyGroupandDateDTO.GroupName}&ScheduleDate={getSchedulebyGroupandDateDTO.ScheduleDate}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<GetScheduleByGroupandDateResponse>>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<ScheduleLogResponses>> GetSchedulebyDate(GetSchedulebyDateDTO getSchedulebyDateDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string getSchedulebyDateDTOStr = JsonConvert.SerializeObject(getSchedulebyDateDTO);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Schedule/GetSchedulebyDate?ScheduleDate={getSchedulebyDateDTO.ScheduleDate}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ScheduleLogResponses>>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<AddGroupScheduleResponse> GroupAddSchedule(AddGroupScheduleDTO addGroupScheduleDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string addGroupScheduleDTOStr = JsonConvert.SerializeObject(addGroupScheduleDTO);
                var response = await client.PostAsync("http://172.16.12.158:7006/api/Schedule/GroupAddSchedule",
                      new StringContent(addGroupScheduleDTOStr, Encoding.UTF8,
                      "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AddGroupScheduleResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }  //Done,not test yet

        public async Task<RejectScheduleResponse> RejectSchedule(RejectScheduleDTO rejectScheduleDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string RejectRequestStr = JsonConvert.SerializeObject(rejectScheduleDTO);
                var response = await client.PutAsync("http://172.16.12.158:7006/api/Schedule/RejectSchedule",
                      new StringContent(RejectRequestStr, Encoding.UTF8,
                      "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RejectScheduleResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        } //Done, not test yet

        public async Task<UpdateScheduleStatusByEmployeeNumberResponse> updateScheduleStatusByEmployeeName(UpdateScheduleStatusByEmployeeNumberDTO updateScheduleStatusByEmployeeNumberDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string updateScheduleStatusByEmployeeNumberStr = JsonConvert.SerializeObject(updateScheduleStatusByEmployeeNumberDTO);
                var response = await client.PutAsync($"http://172.16.12.158:7006/api/Schedule/UpdateScheduleStatusByEmployeeName/{updateScheduleStatusByEmployeeNumberDTO.EmployeeNumber}",
                      new StringContent(updateScheduleStatusByEmployeeNumberStr, Encoding.UTF8,
                      "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UpdateScheduleStatusByEmployeeNumberResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }  //Done, not test yet

        public async Task<UpdateScheduleStatusByGroupNameResponse> updateScheduleStatusByGroupName(UpdateScheduleStatusByGroupNameDTO updateScheduleStatusByGroupNameDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string updateScheduleStatusByGroupNameStr = JsonConvert.SerializeObject(updateScheduleStatusByGroupNameDTO);
                var response = await client.PutAsync($"http://172.16.12.158:7006/api/Schedule/UpdateScheduleStatusByGroupName?GroupName=" +
                    $"{updateScheduleStatusByGroupNameDTO.GroupName}&DayMonthYear={updateScheduleStatusByGroupNameDTO.DayMonthYear}" +
                    $"&Hours={updateScheduleStatusByGroupNameDTO.Hours}" +
                    $"&Remarks={updateScheduleStatusByGroupNameDTO.Remarks}" + 
                    $"&Paxs={updateScheduleStatusByGroupNameDTO.Paxs}" +
                    $"&Status={updateScheduleStatusByGroupNameDTO.Status}",
                      new StringContent(updateScheduleStatusByGroupNameStr, Encoding.UTF8,
                      "application/json"));

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UpdateScheduleStatusByGroupNameResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        } //Done, not test yet, Function not done

        public async Task<GetScheduleByGroupResponse> GetScheduleByEmployeeNumberandDate(GetScheduleByEmployeeNumberandDateDTO getScheduleByEmployeeNumberandDateDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Schedule/GetScheduleByEmployeeNumberandDate?EmployeeNumber={getScheduleByEmployeeNumberandDateDTO.EmployeeNumber}&DayMonthYear={getScheduleByEmployeeNumberandDateDTO.DayMonthYear}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetScheduleByGroupResponse>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<GetScheduleByGroupResponse>> GetRejectedScheduleLog(GetScheduleByEmployeeNumberandDateDTO getScheduleByEmployeeNumberandDateDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Schedule/GetRejectedScheduleLog?EmployeeNumber={getScheduleByEmployeeNumberandDateDTO.EmployeeNumber}&DayMonthYear={getScheduleByEmployeeNumberandDateDTO.DayMonthYear}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<GetScheduleByGroupResponse>>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<ScheduleLogResponses>> GetScheduleLogsByGroupandDate(GetSchedulebyGroupandDateDTO getSchedulebyGroupandDateDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string getScheduleLogsbyGroupandDateDTOStr = JsonConvert.SerializeObject(getSchedulebyGroupandDateDTO);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Schedule/GetScheduleLogbyGroupandDate?GruopName={getSchedulebyGroupandDateDTO.GroupName}&ScheduleDate={getSchedulebyGroupandDateDTO.ScheduleDate}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ScheduleLogResponses>>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<ScheduleLogResponses>> GetScheduleLogsByDepartmentGroupDate(GetScheduleLogsByDepartmentGroupDateDTO getScheduleLogsByDepartmentGroupDateDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string getScheduleLogsByDepartmentGroupDateDTOStr = JsonConvert.SerializeObject(getScheduleLogsByDepartmentGroupDateDTO);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Schedule/GetScheduleLogsByDepartmentGroupDate" +
                    $"?GruopName={getScheduleLogsByDepartmentGroupDateDTO.GruopName}" +
                    $"&ScheduleDate={getScheduleLogsByDepartmentGroupDateDTO.ScheduleDate}" +
                    $"&Department={getScheduleLogsByDepartmentGroupDateDTO.Department}"
                    );

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ScheduleLogResponses>>(json);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<List<ScheduleLogResponses>> GetScheduleLogsByDepartmentandDate(GetSchedulebyGroupandDateDTO getSchedulebyGroupandDateDTO)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                string getScheduleLogsByDepartmentGroupDateDTOStr = JsonConvert.SerializeObject(getSchedulebyGroupandDateDTO);
                var response = await client.GetAsync($"http://172.16.12.158:7006/api/Schedule/GetScheduleLogsByDepartmentandDate" +
                    $"?GruopName={getSchedulebyGroupandDateDTO.GroupName}" +
                    $"&ScheduleDate={getSchedulebyGroupandDateDTO.ScheduleDate}");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ScheduleLogResponses>>(json);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
