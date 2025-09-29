
using Infrastructure.Services.Groq;
using Quartz;

namespace Infrastructure.Jobs
{
    public class GetMissingAccomodationsJob : IJob
    {
        private readonly IGroqService groqService;
        public GetMissingAccomodationsJob(IGroqService groqService)
        {
            this.groqService = groqService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.MergedJobDataMap; 

            string place = dataMap.GetString("place") ?? string.Empty;

            await groqService.GetAccomodationAsync(place);
        }
    }
}
