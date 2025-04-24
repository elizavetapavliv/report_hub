﻿using Exadel.ReportHub.RA.Abstract;
using FluentValidation;

namespace Exadel.ReportHub.Handlers.UserAssignment.Upsert;

public class UpsertUserAssignmentRequestValidator : AbstractValidator<UpsertUserAssignmentRequest>
{
    private readonly IUserRepository _userRepository;
    private readonly IClientRepository _clientRepository;

    public UpsertUserAssignmentRequestValidator(IUserRepository userRepository, IClientRepository clientRepository)
    {
        _userRepository = userRepository;
        _clientRepository = clientRepository;
        ConfigureRules();
    }

    private void ConfigureRules()
    {
        RuleFor(x => x.SetUserAssignmentDTO)
            .ChildRules(child =>
            {
                child.RuleLevelCascadeMode = CascadeMode.Stop;

                child.RuleFor(x => x.UserId)
                    .NotEmpty()
                    .MustAsync(_userRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Common.UserDoesNotExist);

                child.RuleFor(x => x.ClientId)
                    .NotEmpty()
                    .MustAsync(_clientRepository.ExistsAsync)
                    .WithMessage(Constants.Validation.Common.ClientDoesNotExist);

                child.RuleFor(x => x.Role)
                    .IsInEnum();
            });
    }
}
