using Auth_Module.src.Application.Services;
using MediatR;

namespace Auth_Module.src.Application.UseCases.SignUp
{
    public class SignUpHandle : IRequestHandler<SignUpCommand, SignUpCommandResponse>
    {
        private readonly IExecuteAtomically _executeAtomically;
        public SignUpHandle(IExecuteAtomically executeAtomically)
        {
            _executeAtomically = executeAtomically;
        }
        public async Task<SignUpCommandResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            return await _executeAtomically.ExecuteAtomicallyAsync<SignUpCommandResponse>(() => MainTask(request), cancellationToken);
        }

        private async Task<SignUpCommandResponse> MainTask(SignUpCommand request)
        {
            await Task.CompletedTask;
            return new SignUpCommandResponse();
        }
    }
}