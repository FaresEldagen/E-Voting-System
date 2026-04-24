
using E_Voting_System.Responces;
using E_Voting_System.Services.Interfaces;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text.Json;

namespace E_Voting_System.Services.Implementation
{
    public class SendImageService : ISendImageService
    {
        private readonly HttpClient _client;

        public SendImageService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> SendImageAsync(
            IFormFile IdCardImage,
            IFormFile PersonWithIdImage,
            double threshold = 0.45)
            {
                using var content = new MultipartFormDataContent();

                var url = $"http://127.0.0.1:8000/verify-identity?threshold={threshold}";

            

                // ID Card
                using var idStream = IdCardImage.OpenReadStream();
                using var idContent = new StreamContent(idStream);
                idContent.Headers.ContentType = new MediaTypeHeaderValue(IdCardImage.ContentType);
                content.Add(idContent, "id_card_image", IdCardImage.FileName);

                // Person Image
                using var personStream = PersonWithIdImage.OpenReadStream();
                using var personContent = new StreamContent(personStream);
                personContent.Headers.ContentType = new MediaTypeHeaderValue(PersonWithIdImage.ContentType);
                content.Add(personContent, "person_with_id_image", PersonWithIdImage.FileName);

                // Call API
                var response = await _client.PostAsync(url, content);

                var json = await response.Content.ReadAsStringAsync();

                // Deserialize
                var result = JsonSerializer.Deserialize<VerifyResponse>(json);

                // return only English ID
                return result?.id_number_english ?? "";
        }
    }
}
