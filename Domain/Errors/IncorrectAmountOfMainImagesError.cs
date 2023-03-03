using FluentResults;

namespace Domain.Errors;

public class IncorrectAmountOfMainImagesError : Error
{
   public IncorrectAmountOfMainImagesError(int amount) 
      : base($"There should be exactly {amount} main images")
   {
      ;
   }
}