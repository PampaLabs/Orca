using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Balea.AspNetCore.Endpoints;

namespace Balea.Store.Http;

public class PolicyStore : IPolicyStore
{
    private const string endpoint = "policies";

    private readonly PolicyRequestMapper _requestMapper = new();
    private readonly PolicyResponseMapper _responseMapper = new();

    private readonly HttpClient _httpClient;

    public PolicyStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    public async Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{policyId}";
        var response = await _httpClient.GetFromJsonAsync<PolicyResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/name/{policyName}";
        var response = await _httpClient.GetFromJsonAsync<PolicyResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    public async Task<AccessControlResult> CreateAsync(Policy policy, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(policy);

        var uri = $"{endpoint}";
        var response = await _httpClient.PostAsJsonAsync(uri, data, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<AccessControlResult> UpdateAsync(Policy policy, CancellationToken cancellationToken)
    {
        var data = _requestMapper.FromEntity(policy);

        var uri = $"{endpoint}/{policy.Id}";
        var response = await _httpClient.PutAsJsonAsync(uri, data, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<AccessControlResult> DeleteAsync(Policy policy, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{policy.Id}";
        var response = await _httpClient.DeleteAsync(uri, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return AccessControlResult.Success;
        }
        else
        {
            return AccessControlResult.Failed(new AccessControlError { Code = response.StatusCode.ToString() });
        }
    }

    public async Task<IList<Policy>> ListAsync(CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PolicyResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
    }

    public async Task<IList<Policy>> SearchAsync(PolicyFilter filter, CancellationToken cancellationToken = default)
    {
        var query = QueryString.Empty;

        if (!string.IsNullOrEmpty(filter.Name))
        {
            query.Add(nameof(filter.Name), filter.Name);
        }

        if (!string.IsNullOrEmpty(filter.Description))
        {
            query.Add(nameof(filter.Description), filter.Description);
        }

        var uri = $"{endpoint}{query.ToUriComponent()}";
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PolicyResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
    }
}
