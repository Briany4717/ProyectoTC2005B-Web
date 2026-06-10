using System.Text.Json;
using WhirlpoolPromptWeb.Models;
namespace WhirlpoolPromptWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq; 

public class LibraryServices : ILibraryServices
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://10.14.255.43:6747";

    public LibraryServices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<string>> GetCategories()
    {
        try
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/PromptCategories");
            if (!response.IsSuccessStatusCode) return new List<string>();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<PromptCategory> listaDeObjetos = JsonSerializer.Deserialize<List<PromptCategory>>(jsonResponse, options) 
            ?? new List<PromptCategory>();
            List<string> categories = listaDeObjetos.Select(objeto => objeto.Nombre).ToList();
            return categories;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetCategories error: {ex.Message}");
            return new List<string>();
        }
    }
}