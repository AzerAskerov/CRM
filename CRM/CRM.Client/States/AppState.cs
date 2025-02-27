using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Zircon.Core;

using CRM.Operation.Models;
using CRM.Operation.Models.RequestModels;
using CRM.Operation.Models.Login;

using CRM.Client.Models;
using CRM.Client.Shared.Components;
using CRM.Operation.JsonConverters;
using CRM.Operation.Models.DealOfferModels;
using Zircon.Core.Authorization;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using JsonSerializer = System.Text.Json.JsonSerializer;
using CRM.Data.Database;


namespace CRM.Client.States
{
    public class AppState
    {
        private readonly HttpClient _httpClient;
        //private readonly IODataClient _client;

        public AppState(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OperationResult<UserModel>> LoginAsync(LoginModel model)
        {
            OperationResult<UserModel> result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<LoginModel, OperationResult<UserModel>>("api/auth/login", model);
            }
            catch (Exception ex)
            {
                //here may be we need to log
                result = null;
            }

            return result;
        }

        public async Task<OperationResult<List<CommentContract>>> CommentsWithCreators(CommentsWithCreatorRequestModel model)
        {
            OperationResult<List<CommentContract>> result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<CommentsWithCreatorRequestModel, OperationResult<List<CommentContract>>>("api/commentcreators/comments", model);
            }
            catch (Exception ex)
            {
                //here may be we need to log
                result = null;
            }

            return result;
        }

        public async Task<UserModel> HasUserLoggedIn()
        {
            UserModel result = null;

            try
            {
                result = await _httpClient.GetFromJsonAsync<UserModel>("api/auth/auth1");
            }
            catch (Exception ex)
            {
                //here may be we need to log
                Console.WriteLine("errrorrrr:" + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("errrorrrr111111111:" + ex.InnerException.Message);
                }
                result = null;
            }

            return result;
        }

        public async Task<UserSummaryShort> GetUserInfoByUserGuid(Guid userGuid)
        {
            UserSummaryShort result = null;

            try
            {
                result = await _httpClient.GetFromJsonAsync<UserSummaryShort>(
                    $"api/user/GetUserInfoByUserGuid?userGuid={userGuid}");
            }
            catch
            {
                Console.WriteLine($"{nameof(GetUserInfoByUserGuid)} error");
            }

            return result;
        }

        public async Task<List<UserSummaryShort>> GetUsersWithPermission(PermissionContract contract)
        {
            IEnumerable<UserSummaryShort> result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<PermissionContract, IEnumerable<UserSummaryShort>>(
                    $"api/user/GetUsersWithPermission", contract);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{nameof(GetUsersWithPermission)} error" + e.Message);
            }

            return result?.ToList();
        }
        public async Task<DealScopeModel> GetScopeUserGuids(GetScopeUsersRequestModel request)
        {
            try
            {
                var result = 
                await _httpClient.PostJsonAsync<GetScopeUsersRequestModel, DealScopeModel>(
                    $"api/deal/GetScopeUserGuids", request);

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<DealScopeModel> GetOrgUserGuids()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<DealScopeModel>(
                  $"api/deal/GetOrgazinationUserGuids");

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public async Task<PermissionParameterModel> GetUsersWithPermission()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<PermissionParameterModel>(
                  $"api/auth/GetPermissionParameter");
                
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }


        public async Task<List<UserSummaryShort>> GetUsersWithPermissionParameterValue(GetUsersWithPermissionParameterValueContract contract)
        {
            IEnumerable<UserSummaryShort> result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<GetUsersWithPermissionParameterValueContract,IEnumerable<UserSummaryShort>>(
                    $"api/user/GetUsersWithPermissionParameterValue",contract);
            }
            catch(Exception e) 
            {
                Console.WriteLine($"{nameof(GetUsersWithPermissionParameterValue)} error" +e.Message);
            }

            return result?.ToList();
        }

        public async Task<OperationResult<List<SourceItem>>> Sourceloader(SourceLoaderModel sourceLoaderModel)
        {
            OperationResult<List<SourceItem>> result = await _httpClient.PostJsonAsync<SourceLoaderModel, OperationResult<List<SourceItem>>>("api/sourceloader/load", sourceLoaderModel);
            return result;
        } 


        public async Task LogoutAsync()
        {
            var result = await _httpClient.PostAsync("api/auth/logout", null);
            result.EnsureSuccessStatusCode();
        } 

        public async Task<OperationResult<int>> CreateClient(ClientInfoContract model)
        {
            OperationResult<int> result = await _httpClient.PostJsonAsync<ClientInfoContract, OperationResult<int>>("api/client/create", model);
            return result;
        }

        public async Task<OperationResult<int>> CreateOrUpdateClient(ClientInfoContract model)
        {
            OperationResult<int> result = await _httpClient.PostJsonAsync<ClientInfoContract, OperationResult<int>>("api/client/createorupdate", model);
            return result;
        }

        //public async Task<OperationResult<int>> UpdateClient(ClientInfoContract model)
        //{
        //        OperationResult<int> result = await _httpClient.PostJsonAsync<ClientInfoContract, OperationResult<int>>("api/client/update", model);
        //        return result;
        //}

        public async Task<List<ContactListItemModel>> PhysicalPerson(string query, MetaData metaData)
        {
           try {
                var result = await _httpClient.GetAsync($"api/PhysicalPerson?$select=inn,fullname,pinnumber,birthdate,positionname,gendertype" +
                 $"&$filter=1 eq 1{query}&" +
                 $"$orderby=inn&$count=true&$skip={metaData.Skip}&$top={metaData.PageSize}");

                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ODataModel<List<ContactListItemModel>>>(responseContent);
                    metaData.TotalPages = model.Count;
                    var clients = model.Value;
                    return clients;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<ContactListItemModel>> PhysicalPersonByDocumentNumber(string documentNumber)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/GetPhysicalPersonByDocNumber(DocNumber='{documentNumber}')?$select=inn,fullname,pinnumber,birthdate,positionname,gendertype");
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ODataModel<List<ContactListItemModel>>>(responseContent);
                    Console.WriteLine($" a: {responseContent}");
                    var clients = model.Value;
                    return clients;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<OperationResult<List<ParticipatedInsuredPersonPoliciesViewModel>>> ParticipatedInsuredPersonPolicies(ClientContract request)
        {
            try
            {
                var result = await _httpClient.PostJsonAsync<ClientContract, OperationResult<List<ParticipatedInsuredPersonPoliciesViewModel>>>("api/participatedperson/policies", request);
                

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<OperationResult<List<DealListItem>>> GetScopedUserHavingDealsOperation(GetScopeUsersRequestModel request)
        {
            try
            {
                var result = await _httpClient.PostJsonAsync<GetScopeUsersRequestModel, OperationResult<List<DealListItem>>>("api/deal/GetScopedUserHavingDealsOperation", request);


                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<ClientContract>> GetClientByDocumentNumber(string documentNumber)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/GetClientContractByDocNumber(DocNumber='{documentNumber}')?$select=firstname,lastname,fathername,companyname,INN,clienttype");
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ODataModel<List<ClientContract>>>(responseContent);
                    Console.WriteLine($" a: {responseContent}");
                    var clients = model.Value;
                    return clients;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<ClientContract>> GetCompanyByDocumentNumber(string documentNumber)
        {
            try
            {
                var result = await _httpClient.GetAsync($"api/GetCompanyContractByDocNumber(DocNumber='{documentNumber}')?$select=companyname,INN,clienttype");
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ODataModel<List<ClientContract>>>(responseContent);
                    Console.WriteLine($" a: {responseContent}");
                    var clients = model.Value;
                    return clients;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<List<CompanyListItemModel>> Companies(string query, MetaData metaData)
        {
            try
            {

                var result = await _httpClient.GetAsync($"api/Company?$select=inn,companyname,tinnumber" +
                    $"&$filter=1 eq 1{query}&" +
                    $"$orderby=inn&$count=true&$skip={metaData.Skip}&$top={metaData.PageSize}");
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ODataModel<List<CompanyListItemModel>>>(responseContent);
                    metaData.TotalPages = model.Count;
                    var companies = model.Value;
                    return companies;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<HttpResponseMessage> Deals(string query, MetaData metaData)
        {
            try
            {
                var queryString = $"api/deal?$select=DealSubject,DealGuid,DealNumber,DealStatus,DealType,CreatedTimeStamp,CreatedByUserGuid,CreatedByUserFullName,UnderwriterUserGuid,UnderwriterUserFullName,ResponsiblePersonType,ClientInnNavigation, ClientInn" +
                                                        $"&$filter=1 eq 1{query}&" +
                                                        $"$expand=discussion($select = SenderGuid,ReceiverGuid,IsRead,Content, id)&" +
                                                        $"$orderby=CreatedTimeStamp desc&$count=true&$skip={metaData.Skip}&$top={metaData.PageSize}";


                var result = await _httpClient.GetAsync(queryString);
                

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public async Task<DealModel> GetDealByDealGuid(Guid dealGuid)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/deal/GetDealByDealGuid?dealGuid={dealGuid}");
                var responseAsJson = await response.Content.ReadAsStringAsync();

                var responseModel = JsonSerializer.Deserialize<DealModel>(responseAsJson,new JsonSerializerOptions
                {
                    Converters = { new DealOfferFormJsonConverter()}
                });

                return responseModel;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<DealModel> GetDealByDealNumber(string dealNumber)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/deal/GetDealByDealNumber?dealNumber={dealNumber}");
                var responseAsJson = await response.Content.ReadAsStringAsync();

                var responseModel = JsonSerializer.Deserialize<DealModel>(responseAsJson,new JsonSerializerOptions
                {
                    Converters = { new DealOfferFormJsonConverter()}
                });

                return responseModel;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        /// <summary>
        /// can call to get client info (PhysicalPerson, Company, Document, COntact, Relation, Tag)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<T> GetClientInfo<T>(string query)
        {
            try
            {
                
                var result = await _httpClient.GetAsync(query);
                bool fg = result.IsSuccessStatusCode;
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ODataModel<T>>(responseContent);
                    if (model.Value != null)
                    {
                        return model.Value;
                    }
                    else
                    {
                        var m = JsonConvert.DeserializeObject<T>(responseContent);
                        return m;
                    }
                    
                }
                return default(T);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }


        public async Task<OperationResult<List<ProductBaseModel>>> GetPolicies(ClientContract model)
        {
            OperationResult<List<ProductBaseModel>> result = null;
            try
            {
                result = await _httpClient.PostJsonAsync<ClientContract, OperationResult<List<ProductBaseModel>>>("api/policy/products", model);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<OperationResult<List<CuratorProductModel>>> GetCuratorPolicies(CuratorProductRequest model)
        {
            OperationResult<List<CuratorProductModel>> result = null;
            try
            {
                result = await _httpClient.PostJsonAsync<CuratorProductRequest, OperationResult<List<CuratorProductModel>>>("api/policy/curatorproducts", model);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<List<AssetViewModel>> GetAssets(ClientContract model)
        {
            List<AssetViewModel> result = null;
            try
            {
              
                result = await _httpClient.PostJsonAsync<ClientContract,List<AssetViewModel>>("api/assets/clientassets", model);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }


        public async Task<OperationResult<ProductBaseModel>> GetClientPolicy(GetClientPolicyModel model)
        {
            OperationResult<ProductBaseModel> result = null;
            try
            {
                result = await _httpClient.PostJsonAsync<GetClientPolicyModel, OperationResult<ProductBaseModel>>("api/policy/GetClientPolicy", model);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<OperationResult<List<InvoiceModel>>> GetInvoices(ClientContract model)
        {
            OperationResult<List<InvoiceModel>> result = null;
            try
            {
                result = await _httpClient.PostJsonAsync<ClientContract, OperationResult<List<InvoiceModel>>>("api/invoice/invoices", model);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<OperationResult<List<MedicineModel>>> GetMedServices(int INN)
        {
            OperationResult<List<MedicineModel>> result = null;
            try
            {
                result = await _httpClient.PostJsonAsync<int, OperationResult<List<MedicineModel>>>("api/medicine/medical", INN);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<OperationResult<List<DealModel>>> GetDeals(int INN)
        {
            OperationResult<List<DealModel>> result = null;
            try
            {
                result = await _httpClient.PostJsonAsync<int, OperationResult<List<DealModel>>>("api/deal/GetClientDeals", INN);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<OperationResult<ClaimModel>> GetClaims(ClientContract model)
        {
            OperationResult<ClaimModel> result = null;
            try
            {
                result = await _httpClient.PostJsonAsync<ClientContract, OperationResult<ClaimModel>>("api/claim/claims", model);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<RoleModel> IsOwnClient(ClientContract model)
        {
            RoleModel result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<ClientContract, RoleModel>("api/auth/isownclient", model);
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<List<CodeViewModel>> AppointmentTypes()
        {
            List<CodeViewModel> result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<int,List<CodeViewModel>>("api/events/types", 0);
                return result;
            }
            catch (Exception ex)
            {
                result = null;
            }

            return result;
        }
        
        public async Task<List<ContactListItemModel>> BirthdayPersons(MetaData metaData)
        {
            try {
                var result = await _httpClient.GetAsync($"api/birthdaypersons?$select=inn,firstname,lastname,fullname,pinnumber,birthdate,gendertype" +
                                                        $"&$orderby=days_left&$count=true&$skip={metaData.Skip}&$top={metaData.PageSize}");
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<ODataModel<List<ContactListItemModel>>>(responseContent);
                    metaData.TotalPages = model.Count;
                    var clients = model.Value;
                    return clients;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<OperationResult<RemoteCountryModel>> GetRemoteCountriesAsync()
        {
            return await _httpClient.PostJsonAsync<string, OperationResult<RemoteCountryModel>>("api/remotedata/countries", null);
        }

        public async Task<OperationResult<List<string>>> GetRemoteCitiesAsync(string countryName)
        {
            return await _httpClient.PostJsonAsync<string, OperationResult<List<string>>>("api/remotedata/cities", countryName);
        }


        public async Task<OperationResult<List<InsuredVehicleModel>>> GetVehicles(Guid dealGuid)
        {
            OperationResult<List<InsuredVehicleModel>> result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<Guid, OperationResult<List<InsuredVehicleModel>>>("api/deal/GetVehiclesForInsurance", dealGuid);
            }
            catch (Exception ex)
            {
                //here may be we need to log
                result = null;
            }

            return result;
        }


        public async Task<OperationResult<List<InsuredVehicleModel>>> GetCarNEquipVehicles(Guid dealGuid)
        {
            OperationResult<List<InsuredVehicleModel>> result = null;

            try
            {
                result = await _httpClient.PostJsonAsync<Guid, OperationResult<List<InsuredVehicleModel>>>("api/deal/GetVehiclesForCarAndEquipInsurance", dealGuid);
            }
            catch (Exception ex)
            {
                //here may be we need to log
                result = null;
            }

            return result;
        }

    }
}
