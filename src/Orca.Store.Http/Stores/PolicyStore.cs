using System.Net.Http.Json;

using Microsoft.AspNetCore.Http;

using Orca.AspNetCore.Endpoints;

namespace Orca.Store.Http;

/// <inheritdoc />
public class PolicyStore : IPolicyStore
{
    private const string endpoint = "policies";

    private readonly PolicyRequestMapper _requestMapper = new();
    private readonly PolicyResponseMapper _responseMapper = new();

    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="PolicyStore"/> class.
    /// </summary>
    /// The <paramref name="httpClientFactory"/> is used to create an <see cref="HttpClient"/> configured with the name specified in <see cref="HttpStoreDefaults.HttpClientName"/>.
    public PolicyStore(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient(HttpStoreDefaults.HttpClientName);
    }

    /// <inheritdoc />
    public async Task<Policy> FindByIdAsync(string policyId, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/{policyId}";
        var response = await _httpClient.GetFromJsonAsync<PolicyResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    /// <inheritdoc />
    public async Task<Policy> FindByNameAsync(string policyName, CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}/name/{policyName}";
        var response = await _httpClient.GetFromJsonAsync<PolicyResponse>(uri, cancellationToken);
        return _responseMapper.ToEntity(response);
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
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

    /// <inheritdoc />
    public async Task<IList<Policy>> ListAsync(CancellationToken cancellationToken)
    {
        var uri = $"{endpoint}";

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PolicyResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<PolicyResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return entities.ToList();
#endif
    }

    /// <inheritdoc />
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

#if NET8_0_OR_GREATER
        var response = _httpClient.GetFromJsonAsAsyncEnumerable<PolicyResponse>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return await entities.ToListAsync(cancellationToken);
#else
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<PolicyResponse>>(uri, cancellationToken);
        var entities = response.Select(item => _responseMapper.ToEntity(item));

        return entities.ToList();
#endif
    }
}
