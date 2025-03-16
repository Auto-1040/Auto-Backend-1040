using System;
using System.Threading.Tasks;

public interface IS3Service
{
    Task<Result<string>> GeneratePreSignedURLForPostAsync(string fileName);
    Task<Result<string>> GeneratePreSignedURLForGetAsync(string fileName);
}
