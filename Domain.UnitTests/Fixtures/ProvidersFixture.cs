using Domain.Entities;
using Domain.ValueObjects;

namespace Domain.UnitTests.Fixtures;

public static class ProvidersFixture
{
   public static Provider GetTestProvider()
   {
      var id = Guid.NewGuid();
      var companyName = CompanyName.From("BBC").Value;
      var phoneNumber = "+3806666665";
      var email = Email.From("bbc@bbc.co.uk").Value;
      
      return Provider.Create(id, companyName,phoneNumber,email).Value;
   }
}