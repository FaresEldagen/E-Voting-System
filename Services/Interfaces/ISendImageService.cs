namespace E_Voting_System.Services.Interfaces;

public interface ISendImageService
{
    Task<string> SendImageAsync(IFormFile IdImage, IFormFile IdFaceImage, double threshold = 0.45);
}
