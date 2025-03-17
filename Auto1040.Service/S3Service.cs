using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Auto1040.Core.Shared;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    private readonly AwsSettings _awsSettings;
    private const string BucketName = "auto1040forms";

    public S3Service(IAmazonS3 s3Client, IOptions<AwsSettings> awsSettings)
    {
        _s3Client = s3Client;
        _awsSettings = awsSettings.Value;
    }


    public async Task<Result<string>> GeneratePreSignedURLForPostAsync(string fileName)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = fileName,
                Verb = HttpVerb.PUT, // to do
                Expires = DateTime.UtcNow.AddMinutes(10),
                ContentType= "application/pdf"
            };

            var url = _s3Client.GetPreSignedURL(request);
            return Result<string>.Success(url);
        }
        catch (AmazonS3Exception ex)
        {
            return Result<string>.Failure("Error generating pre-signed URL for POST operation: " + ex.Message);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("An unexpected error occurred while generating the pre-signed URL for POST operation: " + ex.Message);
        }
    }

    public async Task<Result<string>> GeneratePreSignedURLForGetAsync(string fileName)
    {
        try
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = BucketName,
                Key = fileName,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(5),
                ContentType = "application/pdf"
            };

            var url = _s3Client.GetPreSignedURL(request);
            return Result<string>.Success(url);
        }
        catch (AmazonS3Exception ex)
        {
            return Result<string>.Failure("Error generating pre-signed URL for GET operation: " + ex.Message);
        }
        catch (Exception ex)
        {
            return Result<string>.Failure("An unexpected error occurred while generating the pre-signed URL for GET operation: " + ex.Message);
        }
    }
}
