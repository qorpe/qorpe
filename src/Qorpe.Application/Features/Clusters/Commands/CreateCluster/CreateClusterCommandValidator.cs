using FluentValidation;

namespace Qorpe.Application.Features.Clusters.Commands.CreateCluster;

public class CreateClusterCommandValidator : AbstractValidator<CreateClusterCommand>
{
    public CreateClusterCommandValidator()
    {
        //RuleFor(v => v.Cluster.Id)
        //    .MaximumLength(3)
        //    .NotEmpty();

        //RuleFor(v => v.Cluster.ClusterId)
        //    .MaximumLength(3)
        //    .NotEmpty();
    }

    private bool BeAValidUri(Uri uri)
    {
        return uri == null || uri.IsAbsoluteUri; // Null veya geçerli bir mutlak URI olmalı
    }
}
