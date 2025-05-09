/* tslint:disable */
/* eslint-disable */
// Generated by Microsoft Kiota
// @ts-ignore
import { createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue, createMaomiAITeamSharedQueriesResponsesTeamMemberResponseFromDiscriminatorValue, type MaomiAIInfraModelsBusinessExceptionResponse, type MaomiAITeamSharedQueriesResponsesTeamMemberResponse } from '../../../../models/index.js';
// @ts-ignore
import { type BaseRequestBuilder, type Parsable, type ParsableFactory, type RequestConfiguration, type RequestInformation, type RequestsMetadata } from '@microsoft/kiota-abstractions';

/**
 * Builds and executes requests for operations under /api/team/{-id}/adminlist
 */
export interface AdminlistRequestBuilder extends BaseRequestBuilder<AdminlistRequestBuilder> {
    /**
     * 查询团队管理员列表.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {Promise<MaomiAITeamSharedQueriesResponsesTeamMemberResponse[]>}
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 400 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 401 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 403 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 409 status code
     * @throws {MaomiAIInfraModelsBusinessExceptionResponse} error when the service returns a 500 status code
     */
     get(requestConfiguration?: RequestConfiguration<object> | undefined) : Promise<MaomiAITeamSharedQueriesResponsesTeamMemberResponse[] | undefined>;
    /**
     * 查询团队管理员列表.
     * @param requestConfiguration Configuration for the request such as headers, query parameters, and middleware options.
     * @returns {RequestInformation}
     */
     toGetRequestInformation(requestConfiguration?: RequestConfiguration<object> | undefined) : RequestInformation;
}
/**
 * Uri template for the request builder.
 */
export const AdminlistRequestBuilderUriTemplate = "{+baseurl}/api/team/{%2Did}/adminlist";
/**
 * Metadata for all the requests in the request builder.
 */
export const AdminlistRequestBuilderRequestsMetadata: RequestsMetadata = {
    get: {
        uriTemplate: AdminlistRequestBuilderUriTemplate,
        responseBodyContentType: "application/json",
        errorMappings: {
            400: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            401: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            403: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            409: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
            500: createMaomiAIInfraModelsBusinessExceptionResponseFromDiscriminatorValue as ParsableFactory<Parsable>,
        },
        adapterMethodName: "sendCollection",
        responseBodyFactory:  createMaomiAITeamSharedQueriesResponsesTeamMemberResponseFromDiscriminatorValue,
    },
};
/* tslint:enable */
/* eslint-enable */
