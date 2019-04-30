using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.StartupConfig
{
    public static class ServiceMvc
    {
        public static void AddServiceMVC(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                var mdp = options.ModelBindingMessageProvider;

                mdp.SetMissingBindRequiredValueAccessor((x) => "Um valor para a propriedade '{0}' não foi fornecido.");
                mdp.SetMissingKeyOrValueAccessor(() => "Um valor é necessário.");
                mdp.SetValueMustNotBeNullAccessor((x) => $"O valor '{x}' é inválido.");
                mdp.SetAttemptedValueIsInvalidAccessor((x, y) => $"O valor '{x}' não é válido para {y}.");
                mdp.SetUnknownValueIsInvalidAccessor((x) => $"O valor fornecido é inválido para {x}.");
                mdp.SetValueIsInvalidAccessor((x) => $"O valor '{x}' é inválido.");
                mdp.SetValueMustBeANumberAccessor((x) => $"O campo {x} deve ser um número.");
                mdp.SetMissingRequestBodyRequiredValueAccessor(() => "Um corpo de solicitação não vazio é necessário.");
                mdp.SetNonPropertyAttemptedValueIsInvalidAccessor((x) => $"O valor '{x}' não é válido.");
                mdp.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor fornecido é inválido.");
                mdp.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser um número.");

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);            
        }
    }

}
