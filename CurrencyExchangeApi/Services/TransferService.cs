using CurrencyExchangeApi.Common;
using CurrencyExchangeApi.Infrastructure;
using CurrencyExchangeApi.Models.Requests;
using CurrencyExchangeApi.Models.Responses;

namespace CurrencyExchangeApi.Services;

public interface ITransferService
{
    Result<CreateTransferResponse> CreateTransfer(CreateTransferRequest request);
    Result<CreateTransferResponse>  GetTransferById(Guid transferId);
}

public class TransferService(IStore<CreateQuoteResponse> quoteStore, IStore<CreateTransferResponse> transferStore) : ITransferService
{
    public Result<CreateTransferResponse> CreateTransfer(CreateTransferRequest request)
    {
        var existingQuote = quoteStore.Get(request.QuoteId);

        if (existingQuote is null)
        {
            return Result<CreateTransferResponse>.Fail($"Quote with ID '{request.QuoteId}' was not found.");
        }

        var transfer = MapToCreateTransferResponse(request);
        transferStore.Save(transfer);

        return Result<CreateTransferResponse>.Ok(transfer);
    }

    public Result<CreateTransferResponse> GetTransferById(Guid transferId)
    {
        var transfer = transferStore.Get(transferId);

        return transfer is null
            ? Result<CreateTransferResponse>.Fail($"Transfer with ID '{transferId}' was not found.")
            : Result<CreateTransferResponse>.Ok(transfer);
    }

    private static CreateTransferResponse MapToCreateTransferResponse(CreateTransferRequest request)
    {
        return new CreateTransferResponse
        {
            TransferId = Guid.NewGuid(),
            Status = TransferStatus.Processing,
            EstimatedDeliveryDate = DateTime.UtcNow.AddDays(1),
            TransferDetails = new TransferDetails
            {
                QuoteId = request.QuoteId,
                Payer = new PayerDetailsResponse
                {
                    Id = request.Payer.Id,
                    Name = request.Payer.Name,
                    TransferReason = request.Payer.TransferReason
                },
                Recipient = new RecipientDetailsResponse
                {
                    Name = request.Recipient.Name,
                    AccountNumber = request.Recipient.AccountNumber,
                    BankCode = request.Recipient.BankCode,
                    BankName = request.Recipient.BankName
                }
            }
        };
    }
}
