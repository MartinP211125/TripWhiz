
using Infrastructure.Services.Groq;
using Quartz;

namespace Infrastructure.Jobs
{
    public class UpdateOffersJob : IJob
    {
        private readonly IGroqService groqService;
        public UpdateOffersJob(IGroqService groqService)
        {
            this.groqService = groqService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await groqService.GetOfferAsync();
            await groqService.GetTrendingOffers();
        }
    }
}
