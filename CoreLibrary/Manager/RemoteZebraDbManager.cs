using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Zebra.Library.PdfHandling;

namespace Zebra.Library
{
    public class RemoteZebraDbManager : IZebraDBManager
    {
        #region Properties


        private static HttpClient _httpClient = new HttpClient() { Timeout = TimeSpan.FromSeconds(15) };

        private HttpClient HttpClient { get { return RemoteZebraDbManager._httpClient; } }

        public ZebraConfig ZebraConfig { get; set; }

        private string IPAdress { get; set; }

        private DirectoryInfo CacheFolder { get; set; }


        #endregion

        #region Constructors

        public RemoteZebraDbManager(ZebraConfig conf)
        {
            ZebraConfig = conf;

            //Load IP Address and Cache folder from Configuration

            IPAdress = conf.ServerIPAddress + ':' + conf.ServerPort;
            CacheFolder = new DirectoryInfo(Path.Combine(conf.RepositoryDirectory, "archive"));

        }

        #endregion

        #region Methods       

        private async Task<List<T>> GetDatabaseListAsync<T>()
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var apiPath = $"https://{IPAdress}/api/{GetApiPath(typeof(T))}";
            var streamTask = HttpClient.GetStreamAsync(apiPath);

            var list = await JsonSerializer.DeserializeAsync<List<T>>(await streamTask);

            return list;
        }

        private async Task<T> GetDatabaseEntityAsync<T>(int id)
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var streamTask = HttpClient.GetStreamAsync($"https://{IPAdress}/api/{GetApiPath(typeof(T))}/{id}");

            var entity = await JsonSerializer.DeserializeAsync<T>(await streamTask);

            return entity;
        }
        private async Task<T> PostDatabaseEntityAsync<T>(T entity)
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/problem+json"));

            var json = JsonSerializer.Serialize<T>(entity);

            var streamTask = await HttpClient.PostAsync($"https://{IPAdress}/api/{GetApiPath(typeof(T))}", new StringContent( json, Encoding.UTF8, "application/json"));

            var newEntity = await JsonSerializer.DeserializeAsync<T>(await streamTask.Content.ReadAsStreamAsync());
            return newEntity;
            
        }

        public async Task<PartDTO> PostPartAsync(PartDTO newPart) => await PostDatabaseEntityAsync<PartDTO>(newPart);

        private async Task<byte[]> GetFileAsync(int id)
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/pdf"));

            var streamTask = HttpClient.GetByteArrayAsync($"https://{IPAdress}/api/{GetApiPath(typeof(FileInfo))}/{id}");

            return await streamTask;
        }

        public async Task<PartDTO> GetPartAsync(int id) => await GetDatabaseEntityAsync<PartDTO>(id);

        public async Task<PieceDTO> GetPieceAsync(int id) => await GetDatabaseEntityAsync<PieceDTO>(id);

        public async Task<SetlistDTO> GetSetlistAsync(int id) => await GetDatabaseEntityAsync<SetlistDTO>(id);

        public async Task<SheetDTO> GetSheetAsync(int id) => await GetDatabaseEntityAsync<SheetDTO>(id);

        public async Task<List<SetlistDTO>> GetAllSetlistsAsync() => await GetDatabaseListAsync<SetlistDTO>();
        public async Task<List<PieceDTO>> GetAllPiecesAsync() => await GetDatabaseListAsync<PieceDTO>();
        public async Task<List<PartDTO>> GetAllPartsAsync() => await GetDatabaseListAsync<PartDTO>();
        public async Task<List<SheetDTO>> GetAllSheetsAsync() => await GetDatabaseListAsync<SheetDTO>();

        private string GetApiPath(Type t)
        {
            

            if (t == typeof(SetlistDTO)) return "Setlists";
            if (t == typeof(PieceDTO)) return "Pieces";
            if (t == typeof(PartDTO)) return "Parts";
            if (t == typeof(FileInfo)) return "Files";
            if (t == typeof(ImportCandidate)) return "ImportCandidates";


            throw new ArgumentException($"Type {nameof(t)} has no API Path");
                    
            }

        public async Task<string> GetPDFPathAsync(int id)
        {
            if (!File.Exists(Path.Combine(CacheFolder.FullName, FileNameResolver.GetFileName(id))))
            {
                var bytes = await GetFileAsync(id);
                await File.WriteAllBytesAsync(Path.Combine(CacheFolder.FullName , FileNameResolver.GetFileName(id)), bytes);                
            }

            return Path.Combine(CacheFolder.FullName, FileNameResolver.GetFileName(id));
        }

        public async Task<ImportCandidate> GetImportCandidateAsync(string filepath)
        {
            var bytes = await File.ReadAllBytesAsync(filepath);

            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/problem+json"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            


            // https://stackoverflow.com/questions/56847199/upload-zip-files-to-asp-net-core-webapi-service-with-httpclient-but-iformdata-ar
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(bytes);

            content.Add(fileContent, "file", filepath);


            var apiPath = $"https://{IPAdress}/api/{GetApiPath(typeof(ImportCandidate))}";
            var streamTask = await HttpClient.PostAsync(apiPath, content);

            if (streamTask.IsSuccessStatusCode == false)
            {
                throw new Exception(streamTask.ReasonPhrase +" "+ await streamTask.Content.ReadAsStringAsync());
            }

            var json = await streamTask.Content.ReadAsStreamAsync();

            var ic= await JsonSerializer.DeserializeAsync<ImportCandidate>(json);

            foreach (var page in ic.Pages)
            {
                page.ImportCandidate = ic;
            }
            return ic;


        }

        public async Task ImportImportCandidateAsync(ImportCandidate ic)
        {
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var json = JsonSerializer.Serialize<ImportCandidate>(ic);

            //var streamTask = await HttpClient.PostAsync($"https://{IPAdress}/api/{GetApiPath(typeof(ImportCandidate))}", new StringContent(json, Encoding.UTF8, "application/json"));
            var streamTask = await HttpClient.PutAsync($"https://{IPAdress}/api/{GetApiPath(typeof(ImportCandidate))}/{ic.DocumentId}", new StringContent(json, Encoding.UTF8, "application/json"));

            if (streamTask.IsSuccessStatusCode == false)
            {
                throw new Exception(await streamTask.Content.ReadAsStringAsync());
            }

        }
    }

        #endregion
    
}
