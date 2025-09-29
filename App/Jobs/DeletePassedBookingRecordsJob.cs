
using Core.Interfaces.Repositories;
using Quartz;

namespace Infrastructure.Jobs
{
    public class DeletePassedBookingRecordsJob : IJob
    {
        private readonly IAccomodationRepository accomodationRepository;
        private readonly ITransportationRepository transportationRepository;
        public DeletePassedBookingRecordsJob(IAccomodationRepository accomodationRepository, ITransportationRepository transportationRepository)
        {
            this.accomodationRepository = accomodationRepository;
            this.transportationRepository = transportationRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await accomodationRepository.DeleteAccomodationAcailabilitiesWithPassedDates();
            await transportationRepository.DeleteTransportationAcailabilitiesWithPassedDates();
        }
    }
}
