
using MediatR;

namespace SFA.DAS.ProviderFeedback.Domain.Messaging
{
    public sealed class MediatrMessaging : IMessaging
    {
        private readonly IMediator _mediator;

        public MediatrMessaging(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> SendStatusCommandAsync(ICommand command)
        {
            var request = command as IRequest<bool>;

            return await _mediator.Send(request);
        }

        public async Task SendCommandAsync(ICommand command)
        {
            var request = command as IRequest<Unit>;

            await _mediator.Send(request);
        }

        public async Task PublishEvent(IEvent @event)
        {
            var notification = @event as INotification;

            await _mediator.Publish(notification);
        }
    }
}
