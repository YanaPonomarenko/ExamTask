using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Exam;

public class ArticleService
{
    private readonly HttpClient _client;
    private readonly string _baseUrl;

    public ArticleService(string baseUrl)
    {
        _client = new HttpClient();
        _baseUrl = baseUrl;
    }

    public async Task<List<Article>> GetAllAsync() =>
        await _client.GetFromJsonAsync<List<Article>>(_baseUrl);

    public async Task<Article> GetByIdAsync(string id) =>
        await _client.GetFromJsonAsync<Article>($"{_baseUrl}/{id}");

    public async Task<List<Article>> SearchByTitleAsync(string title) =>
        await _client.GetFromJsonAsync<List<Article>>($"{_baseUrl}?title={title}");

    public async Task<string?> AddArticleAsync(Article article)
    {
        var response = await _client.PostAsJsonAsync(_baseUrl, article);

        if (response.IsSuccessStatusCode)
        {
            var added = await response.Content.ReadFromJsonAsync<Article>();
            return added?.id;
        }
        return null;
    }
}