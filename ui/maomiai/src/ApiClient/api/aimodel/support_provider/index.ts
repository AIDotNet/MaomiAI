/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createMaomiAIAiModelSharedQueriesResponesQuerySupportModelProviderCommandResponseFromDiscriminatorValue, createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue, type MaomiAIAiModelSharedQueriesResponesQuerySupportModelProviderCommandResponse, type MaomiAIInfraModelsBusinessExceptionResponse } from '../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/aimodel/support_provider
 */
export interface Support_providerRequestBuilder extends BaseRequestBuilder<Support_providerRequestBuilder> {
    /**
     * 查询支持的供应商列表和配置.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<MaomiAIAiModelSharedQueriesResponesQuerySupportModelProviderCommandResponse>}
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 400 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 401 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 403 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 409 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 500 status code
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<MaomiAIAiModelSharedQueriesResponesQuerySupportModelProviderCommandResponse | undefined>;
    /**
     * 查询支持的供应商列表和配置.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toGetRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const Support_providerRequestBuilderUriTemplate = "{+baseurl}/api/aimodel/support_provider";
/**
 * Metadata for all the requests in the request builder.
 */
export const Support_providerRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: Support_providerRequestBuilderUriTemplate,
        responseBodyContentType: "application/json",
        errorMappings: {
            400: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            403: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            409: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            500: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "send",
        responseBodyFactory:  createMaomiAIAiModelSharedQueriesResponesQuerySupportModelProviderCommandResponseFromDiscriminatorValue,
    },
};
/* tslint:enable */
/* eslint-enable */
