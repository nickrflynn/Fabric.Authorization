﻿using System;
using System.Security.Claims;
using Fabric.Authorization.API.Models;
using Nancy;
using Nancy.Responses.Negotiation;
using Fabric.Authorization.API.Constants;
using Fabric.Authorization.API.ModuleExtensions;
using Fabric.Authorization.Domain.Services;
using FluentValidation;

namespace Fabric.Authorization.API.Modules
{
    public abstract class FabricModule<T> : NancyModule
    {
        protected string ReadScope => "fabric/authorization.read";
        protected string WriteScope => "fabric/authorization.write";

        protected AbstractValidator<T> Validator;
        protected Predicate<Claim> AuthorizationReadClaim
        {
            get { return claim => claim.Type == "scope" && claim.Value == ReadScope; }
        }

        protected Predicate<Claim> AuthorizationWriteClaim
        {
            get { return claim => claim.Type == "scope" && claim.Value == WriteScope; }
        }

        protected FabricModule()
        { }

        protected FabricModule(string path, AbstractValidator<T> abstractValidator) : base(path)
        {
            Validator = abstractValidator ?? throw new ArgumentNullException(nameof(abstractValidator));
        }

        protected Negotiator CreateSuccessfulPostResponse(IIdentifiable model)
        {
            var uriBuilder = new UriBuilder(Request.Url.Scheme,
                Request.Url.HostName,
                Request.Url.Port ?? 80,
                $"{ModulePath}/{model.Id}");

            var selfLink = uriBuilder.ToString();

            return Negotiate
                .WithModel(model)
                .WithStatusCode(HttpStatusCode.Created)
                .WithHeader(HttpResponseHeaders.Location, selfLink);
        }

        protected Negotiator CreateFailureResponse(string message, HttpStatusCode statusCode)
        {
            var error = ErrorFactory.CreateError<T>(message, statusCode);
            return Negotiate.WithModel(error).WithStatusCode(statusCode);
        }

        protected void CheckAccess(IClientService clientService, dynamic grain, dynamic resource,
            params Predicate<Claim>[] requiredClaims)
        {
            string grainAsString = grain.ToString();
            string resourceAsString = resource.ToString();
            this.RequiresResourceOwnershipAndClaims<T>(clientService, grainAsString, resourceAsString, requiredClaims);
        }

        protected void Validate(T model)
        {
            var validationResults = Validator.Validate(model);
            if (!validationResults.IsValid)
            {
                this.CreateValidationFailureResponse<T>(validationResults);
            }
        }
    }
}
